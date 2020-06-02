using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Bookstore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.ViewComponents
{
    public class UserNameViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserNameViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            string id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);

            return View(userFromDb);
        }
    }
}
