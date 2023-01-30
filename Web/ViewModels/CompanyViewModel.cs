using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la empresa es requerido."), StringLength(50)]
        [Display(Name = "Nombre de la empresa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo correo electrónico es requerido"), StringLength(int.MaxValue), EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Sitio Web (opcional)")]
        [StringLength(int.MaxValue), Url(ErrorMessage = "La dirección Web de la compañía debe ser un Url válido.")]
        public string Url { get; set; }

        [Display(Name = "Logo de la empresa (.jpg, .png) (opcional)")]
        [StringLength(int.MaxValue), Url(ErrorMessage = "El logo de la compañía debe ser un Url válido.")]
        public string LogoUrl { get; set; }

        public int? UserId { get; set; }
    }
}