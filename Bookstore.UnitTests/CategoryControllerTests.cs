using System;
using Bookstore.Areas.Admin.Controllers;
using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Bookstore.UnitTests
{
    public class CategoryControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Category _category;
        private CategoryController _categoryController;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _category = new Category
            {
                Id = 1,
                Name = "MockTest"
            };
            _categoryController = new CategoryController(_unitOfWork.Object);
        }

        [Test]
        public void UpsertPost_UpdateUser_WhenModelStateIsValid()
        {
            _unitOfWork.Setup(ouw => ouw.Category.Update(_category));

            var result = _categoryController.Upsert(_category);

            _unitOfWork.Verify(uow => uow.Category.Update(_category));
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }
        
        [Test]
        public void UpsertPost_AddUser_WhenModelStateIsValid()
        {
            _category.Id = 0;
            _unitOfWork.Setup(ouw => ouw.Category.Add(_category));

            var result = _categoryController.Upsert(_category);

            _unitOfWork.Verify(uow => uow.Category.Add(_category));
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        public void UpsertPost_ReturnsModelToUpsertView_WhenModelStateIsInvalid()
        {
            _categoryController.ViewData.ModelState.AddModelError("State", "State is required");

            var result = _categoryController.Upsert(_category) as ViewResult;
            var model = result.Model as Category;

            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.IsNotNull(model);
        }

        [Test]
        public void UpsertGet_ReturnsNotFound_WhenCategoryIsNull()
        {
            // Arrange
            _unitOfWork.Setup(ouw => ouw.Category.Get(It.IsAny<int>())).Returns((Category)null);
            // Act
            var result = _categoryController.Upsert(id: It.IsAny<int>());
            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public void Delete_WhenCalled_DeleteCategoryFromDb()
        {
            _unitOfWork.Setup(ouw => ouw.Category.Get(1)).Returns(_category);

            _categoryController.Delete(1);

            _unitOfWork.Verify(uow => uow.Category.Remove(_category));
        }

    }


}
