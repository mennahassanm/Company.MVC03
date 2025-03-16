﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.MVC.PL.DTOS
{
    public class CreateEmployeeDTO
    {
        [Required(ErrorMessage = "Name is Required :( ")]
        public string Name { get; set; }

        [Range(22, 60 , ErrorMessage ="Age Must Between 22 and 60 !!")]
        public int? Age { get; set; }

        [DataType(DataType.EmailAddress , ErrorMessage = "Email is Not Valid :(")]
        public string Email { get; set; }

        [RegularExpression
            (@"^\d+\s[A-Za-z]+\s[A-Za-z]+(\s#\d+)?$",
            ErrorMessage = "123 Main St :)")]
        public string Addrees { get; set; }

        [Phone]
        public string Phone { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [DisplayName("HiringData")]
        public DateTime HiringData { get; set; }

        [DisplayName("Date Of Create")]
        public DateTime CreateAt { get; set; }

    }
}
