using System.ComponentModel.DataAnnotations;

namespace ContratosCet95.Web.Data.Entities
{
    public class Jogador : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;


        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateOnly Birthdate { get; set; }


        [Required]
        public User User { get; set; } = null!;


        public string FullName => $"{FirstName}  {LastName}";
    }
}
