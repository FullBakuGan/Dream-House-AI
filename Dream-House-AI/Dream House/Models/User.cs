using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using hackaton_asp_project.Models;

namespace Dream_House.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("surname")]
        public string? Surname { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; } = null!;

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Column("hash_password")]
        public string HashPassword { get; set; } = null!;

        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<favorite> favorites { get; set; } = new List<favorite>();
        public virtual ICollection<AdUserCreated> Ads { get; set; } = new List<AdUserCreated>();

        [Column(TypeName = "timestamp without time zone")]
        public DateTime RegistrationDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
    }
}