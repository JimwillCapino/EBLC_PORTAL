﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basecode.Data.ViewModels
{
    public class RomanNumeralAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check if the value is null or empty
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Return success for null or empty values
            }

            // Validate the input value against the Roman numeral pattern
            string romanNumeralPattern = @"^(M{0,3})(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(value.ToString(), romanNumeralPattern))
            {
                return new ValidationResult("The value must be in Roman numeral form.");
            }

            return ValidationResult.Success;
        }
    }
    public class SettingsViewModel
    {
        public int Id { get; set; }
        public DateTime? StartofClass { get; set; }
        public DateTime? EndofClass { get; set; }
        [Display(Name ="school name")]
        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? School_Name { get; set; }
        [Display(Name = "address")]
        [Required]
        [StringLength(60, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? Address { get; set; }
        [Display(Name = "region")]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? Region { get; set; }
        [Display(Name = "division")]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? Division { get; set; }
        [Display(Name = "district")]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at most {1} characters long.")]
        [RomanNumeral(ErrorMessage = "The value must be in Roman numeral form.")]
        public string? District { get; set; }
        public int? SchoolId { get; set; }
        public byte[]? SchoolLogo { get; set; }
        public byte[]? DepEdLogo { get; set; }
        public IFormFile? SchoolLogoRecieve { get; set; }
        public IFormFile? DepEdLogoRecieve { get; set; }
        [Display(Name = "administrator")]
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? Administrator { get; set; }
        [Display(Name = "passing grade")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "The field must be a whole number.")]
        public int? PassingGrade { get; set; }
        [Display(Name = "with highest honor")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "The field must be a whole number.")]
        public int? WithHighestHonor { get; set; }
        [Display(Name = "with high honor")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "The field must be a whole number.")]
        public int? WithHighHonor { get; set; }
        [Display(Name = "with honor")]
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "The field must be a whole number.")]
        public int? WithHonor { get; set; }
        [Display(Name = "email")]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email Address")]
        public string? SchoolEmail { get; set; }
        [Display(Name = "password")]
        [Required]
        [StringLength(60, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string? twoFPassword { get; set; }


    }
}
