using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Name must have at least 3 letters")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool Married { get; set; }
        [MinLength(13, ErrorMessage = "Phone number must have 13 number and have format '+3800000000000'")]
        [MaxLength(13, ErrorMessage = "Phone number must have 13 number and have format '+3800000000000'")]
        public string Phone { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }



    }
}
