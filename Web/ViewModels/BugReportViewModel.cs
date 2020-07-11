using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class BugReportViewModel
    {
        [Display(Name = "Titulo del error *")]
        [Required(ErrorMessage = "El campo título es requerido."), StringLength(int.MaxValue)]
        public string Title { get; set; }

        [Display(Name = "Descripción del error *")]
        [Required(ErrorMessage = "Debe escribir una descripción del error."), StringLength(int.MaxValue)]
        public string Description { get; set; }

        [Display(Name = "Pasos para reproducir el error *")]
        [Required(ErrorMessage = "Debe decirnos como reproducir el error."), StringLength(int.MaxValue)]
        public string ToReproduce { get; set; }

        [Display(Name = "Comportamiento esperado *")]
        [Required(ErrorMessage = "Debe escribir el comportamiento esperado."), StringLength(int.MaxValue)]
        public string ExpectedBehavior { get; set; }

        [StringLength(int.MaxValue), Url(ErrorMessage = "La captura de pantalla debe ser un Url válido.")]
        [Display(Name = "Captura de pantalla")]
        public string UrlScreeshot { get; set; }

        [Display(Name = "Especificaciones")]
        public string Specification { get; set; }

        [Display(Name = "Contexto adicional")]
        public string AdditionalContext { get; set; }
        [Display(Name = "Otra información")]
        public string OtherInformation { get; set; }

    }
}