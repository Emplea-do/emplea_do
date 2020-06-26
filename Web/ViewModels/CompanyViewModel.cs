using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre de la empresa")]
        [Required]
        public string Name { get; set; }

        [StringLength(int.MaxValue), Url(ErrorMessage = "La dirección Web de la compañía debe ser un Url válido.")]
        [Display(Name = "Sitio Web (opcional)")]
        public string Url { get; set; }

        [Display(Name = "Correo electrónico"),]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(int.MaxValue), Url(ErrorMessage = "El logo de la compañía debe ser un Url válido.")]
        [Display(Name = "Logo de la empresa (.jpg, .png) (opcional)")]
        public string LogoUrl { get; set; }

        public int? UserId { get; set; }
    }
}