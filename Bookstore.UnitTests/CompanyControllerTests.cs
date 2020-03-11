using System.Collections.Generic;
using Bookstore.Areas.Admin.Controllers;
using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Bookstore.UnitTests
{
    public class CompanyControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Company _company;
        private CompanyController _companyController;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _company = new Company
            {
                Id = 1,
                Name = "Mockito"
            };
            _companyController = new CompanyController(_unitOfWork.Object);
        }

        [Test]
        public void UpsertPost_AddsCompanyAndRedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            _unitOfWork.Setup(ouw => ouw.Company.Add(It.IsAny<Company>())).Verifiable();
            // Act
            var result = _companyController.Upsert(_company);
            // Assert
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public void UpsertPost_ReturnsModelToUpsertView_WhenModelStateIsInvalid()
        {
            // Arrange
            _companyController.ViewData.ModelState.AddModelError("State", "State is required");
            // Act
            var result = _companyController.Upsert(_company) as ViewResult;
            var model = result.Model as Company;
            // Assert
            Assert.IsNotNull(model);
        }

        [Test]
        public void UpsertGet_ReturnsNotFound_WhenCompanyIsNull()
        {
            // Arrange
            _unitOfWork.Setup(ouw => ouw.Company.Get(It.IsAny<int>())).Returns((Company)null);
            // Act
            var result = _companyController.Upsert(id: It.IsAny<int>());
            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Delete_WhenCalled_DeleteTheCompanyFromDb()
        {
            _unitOfWork.Setup(ouw => ouw.Company.Get(1)).Returns(_company);

            _companyController.Delete(1);

            _unitOfWork.Verify(uow => uow.Company.Remove(_company));
        }

    }
}