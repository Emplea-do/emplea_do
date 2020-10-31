using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Web.ViewModels
{
    public class BannerViewModel
    {
         public int Id { get; set; }

        [StringLength(int.MaxValue), Url(ErrorMessage = "La URL del Banner debe ser un Url válido.")]
        [Display(Name = "Sitio Web (Destino del banner)")]
        public string DestinationUrl { get; set; }

        [StringLength(int.MaxValue), Url(ErrorMessage = "La Url debe ser un Url válido.")]
        [Display(Name = "Banner")]
        public FormFile ImageUrl { get; set; }

        public string DisplayImageUrl { get; set; }

        public int? UserId { get; set; }
    }
}