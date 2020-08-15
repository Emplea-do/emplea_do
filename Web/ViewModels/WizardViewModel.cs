using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;
using Domain.Entities;

namespace Web.ViewModels
{
    public class WizardViewModel: BaseViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Localidad")]
        public string LocationName { get; set; }
        public string LocationLatitude { get; set; }
        public string LocationLongitude { get; set; }
        [Required(ErrorMessage = "Debes seleccionar una localidad válida")]
        public string LocationPlaceId { get; set; }


        public bool CreateNewCompany { get; set; } = true;
        [Display(Name = "Compañía")]
        public int? CompanyId { get; set; }

        [Required(ErrorMessage = "El campo título es requerido."), StringLength(int.MaxValue)]
        [Display(Name = "Título. ¿Qué estás buscando?")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Debes especificar al menos un requisito."), StringLength(int.MaxValue)]
        [Display(Name = "Requisitos para aplicar")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo como aplicar es requerido"), StringLength(int.MaxValue)]
        [Display(Name = "¿Cómo Aplicar?")]
        public string HowToApply { get; set; }

        //[Required(ErrorMessage = "El nombre de la empresa es requerido."), StringLength(50)]
        [Display(Name = "Nombre de la empresa")]
        public string CompanyName { get; set; }

        [StringLength(int.MaxValue), Url(ErrorMessage = "La dirección Web de la compañía debe ser un Url válido.")]
        [Display(Name = "Sitio Web (opcional)")]
        public string CompanyUrl { get; set; }

        //[Required(ErrorMessage = "El campo correo electrónico es requerido"), StringLength(int.MaxValue), EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        [Display(Name = "Correo electrónico"),]
        [DataType(DataType.EmailAddress)]
        public string CompanyEmail { get; set; }


        [StringLength(int.MaxValue), Url(ErrorMessage = "El logo de la compañía debe ser un Url válido.")]
        [Display(Name = "Logo de la empresa (.jpg, .png) (opcional)")]
        public string CompanyLogoUrl { get; set; }

        [Display(Name = "¿Es un puesto remoto?")]
        public bool IsRemote { get; set; }

        [Required(ErrorMessage = "Debes elegir una categoría.")]
        [Display(Name = "Categoría")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Debes elegir un tipo de jornada.")]
        [Display(Name = "Tipo de jornada")]
        public int JobTypeId { get; set; }

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<HireType> JobTypes { get; set; } = new List<HireType>();
        public List<Company> Companies { get; internal set; }
    }
}
