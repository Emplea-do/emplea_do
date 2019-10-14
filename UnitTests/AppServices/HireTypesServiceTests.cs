using AppServices.Services;
using Data.Repositories;
using Domain.Entities;
using NSubstitute;
using NUnit.Framework;
using System;
using UnitTests.Framework;

namespace UnitTests.AppServices
{
    [TestFixture]
    public class HireTypesServiceTests : BaseTest<IHireTypesService>
    {
        private IHireTypesRepository _hireTypeRepositoryMock;

        public HireTypesServiceTests()
        {
            _hireTypeRepositoryMock = Substitute.For<IHireTypesRepository>();
        }

        public override void Setup()
        {
            _sut = new HireTypesService(_hireTypeRepositoryMock);

        }
        [Test]
        public void Create_WhenPassedNullParameter_ThrowsException()
        {
            HireType hireType = null;

            Assert.Throws<ArgumentNullException>(() => _sut.Create(hireType));
        }

        [Test]
        public void Create_WhenPassedValidParameter_CallsInsertOnRepository()
        {
            HireType hireType = new HireType { Name = string.Empty, Description = string.Empty };

            _sut.Create(hireType);

            _hireTypeRepositoryMock.Received(1).Insert(hireType);
        }

        [Test]
        public void Create_WhenPassedValidParameter_AddsSuccessfulMessageToResult()
        {
            HireType hireType = new HireType { Name = string.Empty, Description = string.Empty };

            var result = _sut.Create(hireType);

            StringAssert.Contains("agregado", result.Messages);
        }

    }
}