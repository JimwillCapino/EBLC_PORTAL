using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public DateTime? StartofClass {  get; set; }
        public DateTime? EndofClass { get; set; }
        public string? School_Name { get; set; }
        public string? Address { get; set; }
        public string? Region { get; set; }
        public string? Division { get; set; }
        public string? District { get; set; }
        public int? SchoolId { get; set; }
        public byte[]? SchoolLogo { get; set; }
        public byte[]? DepEdLogo { get; set; }
        public string? Administrator { get; set; }
        public int? PassingGrade { get; set; }
        public int? WithHighestHonor { get; set; }
        public int? WithHighHonor { get; set; }
        public int? WithHonor { get; set; }
    }
}
