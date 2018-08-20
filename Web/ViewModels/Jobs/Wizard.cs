using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.Jobs
{
	public class Wizard
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo título es requerido."), StringLength(int.MaxValue)]
        [Display(Name = "Título. ¿Qué estás buscando?")]
        public string Title { get; set; }

        [Display(Name = "Localidad")]
        public string LocationName { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public string LocationPlaceId { get; set; }
        public string MapsApiKey { get; set; }

        [Required(ErrorMessage = "Debes elegir una categoría.")]
        public int CategoryId { get; set; }

        [Display(Name = "Categoría")]
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        public int JobTypeId { get; set; }

        [Display(Name = "Tipo")]
        public IEnumerable<HireType> JobTypes { get; set; } = new List<HireType>();

        [Required(ErrorMessage = "Debes especificar al menos un requisito."), StringLength(int.MaxValue)]
        [Display(Name = "Requisitos para aplicar")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El nombre de la empresa es requerido."), StringLength(50)]
        [Display(Name = "Nombre de la empresa")]
        public string CompanyName { get; set; }

        [StringLength(int.MaxValue), Url(ErrorMessage = "La dirección Web de la compañía debe ser un Url válido.")]
        [Display(Name = "Sitio Web (opcional)")]
        public string CompanyUrl { get; set; }

        [Required(ErrorMessage = "El campo correo electrónico es requerido"), StringLength(int.MaxValue), EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
        [Display(Name = "Correo electrónico"),]
        public string CompanyEmail { get; set; }

        [Required(ErrorMessage = "El campo como aplicar es requerido"), StringLength(int.MaxValue)]
        [Display(Name = "¿Cómo Aplicar?")]
        public string HowToApply { get; set; }

        [StringLength(int.MaxValue), Url(ErrorMessage = "El logo de la compañía debe ser un Url válido.")]
        [Display(Name = "Logo de la empresa(opcional)")]
        public string CompanyLogoUrl { get; set; }

        [Display(Name= "¿Es un puesto remoto?")]
        public bool IsRemote { get; set; }

        [Display(Name= "¿Usas algún tipo de control de versiones? (Git, Subversion)")]
        public bool HasSourceControl { get; set; }

        [Display(Name= "¿Puedes hacer pases a producción en un solo paso?")]
        public bool HasOneStepBuilds { get; set; }

        [Display(Name = "¿Compilas el producto diariamente?")]
        public bool HasDailyBuilds { get; set; }

        [Display(Name = "¿Tienen una base de datos de bugs?")]
        public bool HasBugDatabase { get; set; }

        [Display(Name = "¿Corriges los bugs antes de añadir más código?")]
        public bool HasBusFixedBeforeProceding { get; set; }

        [Display(Name = "¿Tienes una planificación actualizada?")]
        public bool HasUpToDateSchedule { get; set; }

        [Display(Name = "¿Tienes un documento de especificaciones?")]
        public bool HasSpec { get; set; }

        [Display(Name = "¿Están los programadores en un lugar tranquilo?")]
        public bool HasQuiteEnvironment { get; set; }

        [Display(Name = "¿Utilizas las mejores herramientas que puedes comprar?")]
        public bool HasBestTools { get; set; }

        [Display(Name = "¿Tienes gente para probar los productos?")]
        public bool HasTesters { get; set; }

        [Display(Name = "¿Haces escribir código a los nuevos candidatos en las entrevistas?")]
        public bool HasWrittenTest { get; set; }

        [Display(Name = "¿Haces pruebas de usabilidad 'de vestíbulo'?")]
        public bool HasHallwayTests { get; set; }


        public Domain.Job ToEntity()
        {
            var entity = new Job
            {
                Id = Id,
                Title = Title,
                CategoryId = CategoryId,
                Description = Description,
				Company = new Company{
                    Name = CompanyName,
                    Url = CompanyUrl,
                    LogoUrl = CompanyLogoUrl,
                    Email = CompanyEmail
				},
                PublishedDate = DateTime.Now,
                IsRemote = IsRemote,
                HireTypeId = JobTypeId,
                HowToApply = HowToApply,
                JoelTest = new JoelTest
                {
                    HasSourceControl = this.HasSourceControl,
                    HasOneStepBuilds = this.HasOneStepBuilds,
                    HasDailyBuilds = this.HasDailyBuilds,
                    HasBugDatabase = this.HasBugDatabase,
                    HasBusFixedBeforeProceding = this.HasBusFixedBeforeProceding,
                    HasUpToDateSchedule = this.HasUpToDateSchedule,
                    HasSpec = this.HasSpec,
                    HasQuiteEnvironment = this.HasQuiteEnvironment,
                    HasBestTools = this.HasBestTools,
                    HasTesters = this.HasTesters,
                    HasWrittenTest = this.HasWrittenTest,
                    HasHallwayTests = this.HasHallwayTests
                }
            };

            if (!string.IsNullOrWhiteSpace(LocationName) &&
                !string.IsNullOrWhiteSpace(LocationPlaceId))
            {
                entity.Location = new Location
                {
                    Latitude = LocationLatitude,
                    Longitude = LocationLongitude,
                    Name = LocationName,
                    PlaceId = LocationPlaceId
                };
            }

            return entity;
        }

        public static Wizard FromEntity(Domain.Job entity)
        {
            var wizard = new Wizard()
            {
                Id = entity.Id,
                Title = entity.Title,
                CategoryId = entity.Category.Id,
                Description = entity.Description,
                CompanyName = entity.Company.Name,
                CompanyUrl = entity.Company.Url,
                CompanyLogoUrl = entity.Company.LogoUrl,
                CompanyEmail = entity.Company.Email,
                IsRemote = entity.IsRemote,
                JobTypeId = entity.HireType.Id,
                HowToApply = entity.HowToApply,

                LocationLatitude = entity.Location.Latitude,
                LocationLongitude = entity.Location.Longitude,
                LocationName = entity.Location?.Name,
                LocationPlaceId = entity.Location?.PlaceId
            };      

            if(entity.JoelTest != null)
            {
                wizard.HasSourceControl = entity.JoelTest.HasSourceControl;
                wizard.HasOneStepBuilds = entity.JoelTest.HasOneStepBuilds;
                wizard.HasDailyBuilds = entity.JoelTest.HasDailyBuilds;
                wizard.HasBugDatabase = entity.JoelTest.HasBugDatabase;
                wizard.HasBusFixedBeforeProceding = entity.JoelTest.HasBusFixedBeforeProceding;
                wizard.HasUpToDateSchedule = entity.JoelTest.HasUpToDateSchedule;
                wizard.HasSpec = entity.JoelTest.HasSpec;
                wizard.HasQuiteEnvironment = entity.JoelTest.HasQuiteEnvironment;
                wizard.HasBestTools = entity.JoelTest.HasBestTools;
                wizard.HasTesters = entity.JoelTest.HasTesters;
                wizard.HasWrittenTest = entity.JoelTest.HasWrittenTest;
                wizard.HasHallwayTests = entity.JoelTest.HasHallwayTests;
            }

            return wizard;
        }
    }
}
