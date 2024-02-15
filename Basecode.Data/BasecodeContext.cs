using Basecode.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Basecode.Data
{
    public class BasecodeContext : IdentityDbContext<IdentityUser>
    {
        public BasecodeContext (DbContextOptions<BasecodeContext> options)
            : base(options)
        {}

        public void InsertNew(RefreshToken token)
        {
            var tokenModel = RefreshToken.SingleOrDefault(i => i.Username == token.Username);
            if (tokenModel != null)
            {
                RefreshToken.Remove(tokenModel);
                SaveChanges();
            }
            RefreshToken.Add(token);
            SaveChanges();
        }

        //public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UsersPortal> UsersPortal { get; set; }
        public virtual DbSet<NewEnrollee>NewEnrollee { get; set; }
        public virtual DbSet<RTPCommons> RTPCommons { get; set; }
        public virtual DbSet<Parent> Parent { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<RTPUsers> RTPUsers { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
    }
}