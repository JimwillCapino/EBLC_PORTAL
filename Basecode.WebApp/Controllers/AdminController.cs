using Basecode.Data;
using Basecode.Data.ViewModels;
using Basecode.Services.Interfaces;
using Basecode.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Basecode.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        IAdminService _adminService;
        private ISettingsService _settingsService;
        private readonly IStudentService _studentService;
        private readonly IUsersService _usersService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(IAdminService adminService,
            ISettingsService settingsService,
            IStudentService studentService,
            IUsersService usersService,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager) 
        { 
            _adminService = adminService;
            _settingsService = settingsService;
            _studentService = studentService;
            _usersService = usersService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            if (await _usersService.IsNewUser(_userManager.GetUserId(User)))
            {
                return RedirectToAction("AdminProfileRegistration");
            }
            var adminDashboard = await _usersService.SetDashboardDashBoard();
            adminDashboard.SchoolYear = _settingsService.GetSchoolYear();
            return View(adminDashboard);
        }
        public IActionResult AdminProfileRegistration()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            return View();
        }
        public async Task<IActionResult> UserSetUpAdmin(ProfileViewModel profile)
        {
            try
            {
                profile.AspUserId = _userManager.GetUserId(User);
                await _usersService.NewUserDetailsRegistration(profile);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Admin's Profile created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                return RedirectToAction("AdminProfileRegistration");
            }
        }
        public async Task<IActionResult> Profile()
        {
            try
            {
                ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
                ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
                return View(await _usersService.GetUserPortal(_userManager.GetUserId(User)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePorfile(ProfileViewModel profile)
        {
            try
            {
                await _usersService.UpdateUserProfile(profile);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Profile Updated Successfully!";

                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("Profile");
            }
        }
        public async Task<IActionResult> ChangePassword(ProfileViewModel profile)
        {
            try
            {
                await _usersService.ChangePassword(profile);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Passoword changed successfully!";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                Console.WriteLine(ex);
                return RedirectToAction("Profile");
            }
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel r)
        {           
            if(ModelState.IsValid)
            {
                IdentityResult result = await _adminService.CreateRole(r.Role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                Console.Write("Failed");
            }           
            Console.WriteLine("Model State invalid");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RegistrarsList()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;          
            return View();
        }
        public async Task<IActionResult> Teacherslist()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ManageTeachersListDataTable()
        {
            try
            {
                var draw = int.Parse(Request.Form["draw"]);
                var start = int.Parse(Request.Form["start"]);
                var length = int.Parse(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                //Paging Size (10,25,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                var customerData = await _adminService.GetTeachersAsync();
                var asEnumcustomerData = customerData.AsEnumerable();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn.Equals("firstname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.firstname) :
                            asEnumcustomerData.OrderByDescending(p => p.firstname);
                    }
                    else if (sortColumn.Equals("middlename"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.middlename) :
                            asEnumcustomerData.OrderByDescending(p => p.middlename);
                    }
                    else if (sortColumn.Equals("lastname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.lastname) :
                            asEnumcustomerData.OrderByDescending(p => p.lastname);
                    }
                    else if (sortColumn.Equals("gender"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.gender) :
                            asEnumcustomerData.OrderByDescending(p => p.gender);
                    }
                    else if (sortColumn.Equals("email"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.email) :
                            asEnumcustomerData.OrderByDescending(p => p.email);
                    }

                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    asEnumcustomerData = customerData.Where(m => m.firstname.ToLower().Contains(searchValue.ToString().ToLower())
                     || m.middlename.ToLower().Contains(searchValue.ToString().ToLower()) || m.lastname.ToLower().Contains(searchValue.ToString().ToLower()) ||
                     m.gender.ToLower().Contains(searchValue.ToString().ToLower()) || m.email.ToLower().Contains(searchValue.ToString().ToLower()));
                }

                //total number of rows count   
                recordsTotal = asEnumcustomerData.Count();
                //Paging   
                var data = asEnumcustomerData.Skip(skip).Take(pageSize);
                //Returning Json Data  
                var test = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return test;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> ManageRegistrarsListDataTable()
        {
            try
            {
                var draw = int.Parse(Request.Form["draw"]);
                var start = int.Parse(Request.Form["start"]);
                var length = int.Parse(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                //Paging Size (10,25,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                var customerData = await _adminService.GetRegistrarsAsync();
                var asEnumcustomerData = customerData.AsEnumerable();

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn.Equals("firstname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.firstname) :
                            asEnumcustomerData.OrderByDescending(p => p.firstname);
                    }
                    else if (sortColumn.Equals("middlename"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.middlename) :
                            asEnumcustomerData.OrderByDescending(p => p.middlename);
                    }
                    else if (sortColumn.Equals("lastname"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.lastname) :
                            asEnumcustomerData.OrderByDescending(p => p.lastname);
                    }
                    else if (sortColumn.Equals("gender"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.gender) :
                            asEnumcustomerData.OrderByDescending(p => p.gender);
                    }
                    else if (sortColumn.Equals("email"))
                    {
                        asEnumcustomerData = sortColumnDirection.ToLower() == "asc" ? asEnumcustomerData.OrderBy(p => p.email) :
                            asEnumcustomerData.OrderByDescending(p => p.email);
                    }

                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    asEnumcustomerData = customerData.Where(m => m.firstname.ToLower().Contains(searchValue.ToString().ToLower())
                     || m.middlename.ToLower().Contains(searchValue.ToString().ToLower()) || m.lastname.ToLower().Contains(searchValue.ToString().ToLower()) ||
                     m.gender.ToLower().Contains(searchValue.ToString().ToLower()) || m.email.ToLower().Contains(searchValue.ToString().ToLower()));
                }

                //total number of rows count   
                recordsTotal = asEnumcustomerData.Count();
                //Paging   
                var data = asEnumcustomerData.Skip(skip).Take(pageSize);
                //Returning Json Data  
                var test = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                return test;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult RegistrarRegistration()
        {
            ViewData["Success"] = Constants.ViewDataErrorHandling.Success;
            ViewData["ErrorMessage"] = Constants.ViewDataErrorHandling.ErrorMessage;
            return View();
        }
        public async Task<IActionResult> AddRegistrar(UsersRegistration user)
        {
            try
            {
                await _adminService.AddRegistrar(user);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Successfully Added Registrar. Let the new user set up its account to view the registrar in the table";
                return RedirectToAction("RegistrarsList");
            }
            catch(Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                return RedirectToAction("RegistrarRegistration");
            }
        }
        public async Task<IActionResult> RemoveRegistrar(string user)
        {
            try
            {
                await _adminService.RemoveRegistrar(user);
                Constants.ViewDataErrorHandling.Success = 1;
                Constants.ViewDataErrorHandling.ErrorMessage = "Successfully Removed Registrar.";
                return RedirectToAction("RegistrarsList");
            }
            catch (Exception ex)
            {
                Constants.ViewDataErrorHandling.Success = 0;
                Constants.ViewDataErrorHandling.ErrorMessage = ex.Message;
                return RedirectToAction("RegistrarsList");
            }
        }
    }
}
