using AppServices.Services;
using Data.Repositories;
using Domain.Entities;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Empleado.Unit.Tests.AppServices
{
    [TestFixture]
    public class HireTypesServiceTests
    {
        [Test]
        public void Create_WhenPassedNullParameter_ThrowsException()
        {
            var service = BuildService(out IHireTypesRepository hireTypesRepository);
            HireType hireType = null;

            Assert.Throws<ArgumentNullException>(() => service.Create(hireType));
        }

        [Test]
        public void Create_WhenPassedValidParameter_CallsInsertOnRepository()
        {
            var service = BuildService(out IHireTypesRepository hireTypesRepository);
            HireType hireType = new HireType { Name = string.Empty, Description = string.Empty };

            service.Create(hireType);

            hireTypesRepository.Received(1).Insert(hireType);
        }

        [Test]
        public void Create_WhenPassedValidParameter_AddsSuccessfulMessageToResult()
        {
            var service = BuildService(out IHireTypesRepository hireTypesRepository);
            HireType hireType = new HireType { Name = string.Empty, Description = string.Empty };

            var result = service.Create(hireType);

            StringAssert.Contains("agregado", result.Messages);
        }

        private HireTypesService BuildService(out IHireTypesRepository hireTypesRepository)
        {
            hireTypesRepository = Substitute.For<IHireTypesRepository>();

            return new HireTypesService(hireTypesRepository);
        }
    }
}