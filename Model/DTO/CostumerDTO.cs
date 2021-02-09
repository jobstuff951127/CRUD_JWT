using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class CostumerDTO
    {

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("([1-9][0-9]*)")]
        [StringLength(10, MinimumLength = 10)]

        public string Cellphone { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string BirthDate { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}