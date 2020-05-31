using System;
using Bookstore.Areas.Admin.Controllers;
using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Bookstore.UnitTests
{
    public class CoverTypeControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private CoverType _coverType;
        private CoverTypeController _coverTypeController;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _coverType = new CoverType()
            {
                Id = 1,
                Name = "MockTest"
            };
            _coverTypeController = new CoverTypeController(_unitOfWork.Object);
        }

        [Test]
        public void UpsertPost_UpdateUser_WhenModelStateIsValid()
        {
            _unitOfWork.Setup(ouw => ouw.CoverType.Update(_coverType));

            var result = _coverTypeController.Upsert(_coverType);

            _unitOfWork.Verify(uow => uow.CoverType.Update(_coverType));
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }
        
        [Test]
        public void UpsertPost_AddUser_WhenModelStateIsValid()
        {
            _coverType.Id = 0;
            _unitOfWork.Setup(ouw => ouw.CoverType.Add(_coverType));

            var result = _coverTypeController.Upsert(_coverType);

            _unitOfWork.Verify(uow => uow.CoverType.Add(_coverType));
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public void UpsertPost_ReturnsModelToUpsertView_WhenModelStateIsInvalid()
        {
            _coverTypeController.ViewData.ModelState.AddModelError("State", "State is required");

            var result = _coverTypeController.Upsert(_coverType) as ViewResult;
            var model = result.Model as CoverType;

            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.IsNotNull(model);
        }

        [Test]
        public void UpsertGet_ReturnsNotFound_WhenCoverTypeIsNull()
        {
            // Arrange
            _unitOfWork.Setup(ouw => ouw.CoverType.Get(It.IsAny<int>())).Returns((CoverType)null);
            // Act
            var result = _coverTypeController.Upsert(id: It.IsAny<int>());
            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Delete_WhenCalled_DeleteCoverTypeFromDb()
        {
            _unitOfWork.Setup(ouw => ouw.CoverType.Get(1)).Returns(_coverType);

            _coverTypeController.Delete(1);

            _unitOfWork.Verify(uow => uow.CoverType.Remove(_coverType));
        }

    }


}
