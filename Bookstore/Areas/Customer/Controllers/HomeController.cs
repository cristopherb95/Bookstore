﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookstore.Models.ViewModels;
using Bookstore.DataAccess.Repository;
using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models;
using Microsoft.AspNetCore.Authorization;
using Bookstore.Utility;
using Microsoft.AspNetCore.Http;

namespace Bookstore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart
                    .GetAll(c => c.ApplicationUserId == claim.Value)
                    .ToList().Count();

                HttpContext.Session.SetInt32(SD.ShoppingCartSession, count);
            }

            return View(productList);
        }

        public IActionResult Details(int id)
        {
            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,CoverType");
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = productFromDb,
                ProductId = productFromDb.Id
            };
            return View(shoppingCart);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0; // Because ProductId is taken as Id from view
            if (ModelState.IsValid)
            {
                // Add to cart
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    u => u.ApplicationUserId == shoppingCart.ApplicationUserId && u.ProductId == shoppingCart.ProductId,
                    includeProperties: "Product"
                );
                if (cartFromDb == null)
                {
                    // No records in DB for that user
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                }
                else
                {
                    cartFromDb.Count += shoppingCart.Count;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                }
                _unitOfWork.Save();

                var count = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == shoppingCart.ApplicationUserId).ToList().Count();

                HttpContext.Session.SetInt32(SD.ShoppingCartSession, count);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == shoppingCart.ProductId, includeProperties: "Category,CoverType");
                ShoppingCart cartToReturn = new ShoppingCart()
                {
                    Product = productFromDb,
                    ProductId = productFromDb.Id
                };
                return View(cartToReturn);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
