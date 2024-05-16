using Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class AttendanceContainer
    {
        public int Id { get; set; }
        public int studentId { get; set; }
        [Display(Name = "Days of School")]
        [Required(ErrorMessage = "Days of School field is required.")]
        public int Days_of_School { get; set; }
        [Display(Name = "Days of Present")]
        [Required(ErrorMessage = "Days of Present field is required.")]
        public int Days_of_Present { get; set; }
        [Display(Name = "Time of Tardy")]
        [Required(ErrorMessage = "Time of Tardy field is required.")]
        public int Time_of_Tardy { get; set; }
        [Display(Name = "Month")]
        [Required(ErrorMessage = "Month is required.")]
        public string Month { get; set; }
        public List<string> Months { get; set; }
        public List<Attendance> Attendances { get; set; }
    }
}
