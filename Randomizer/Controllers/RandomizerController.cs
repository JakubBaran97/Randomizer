﻿
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Randomizer.Exceptions;
using Randomizer.Models;
using Randomizer.Services;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.EntityFrameworkCore;
using Randomizer.Entities;
using Microsoft.Identity.Client;

namespace Randomizer.Controllers
{

    public class RandomizerController : Controller
    {

        private readonly IRandomizerService _randomizerService;
        private readonly IAccountServices _accountServices;
        private readonly IQRService _qrService;

        public RandomizerController(IRandomizerService randomizerService, IAccountServices accountServices, IQRService qrService)
        {
            _randomizerService = randomizerService;
            _accountServices = accountServices;
            _qrService = qrService;

        }
        //Index
        [Authorize]
        
        public IActionResult Index()
        {

            return View();
        }
        //User
        [Route("login")]
        public IActionResult Login()
        {

            return View();
        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login(Login login)
        {
            try
            {

                _accountServices.GenerateCookie(login);
                return RedirectToAction("Index");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }

        }

        [Authorize]
        public IActionResult Logout()
        {
            _accountServices.Logout();
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserDto dto)
        {
            try
            {
                _accountServices.RegisterUser(dto);
                return RedirectToAction("Index");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
        }
        [Authorize]
        public IActionResult EditUser()
        {
            var userId = _accountServices.FindUserId();
            var user = _accountServices.FindUserToEdit(userId);

            


            return View(user);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditUser(EditUserDto user)
        {
            try
            {
                _accountServices.EditUser(user);
                return RedirectToAction("Index");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
        }

        [Authorize]
        public IActionResult EditPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditPassword(EditPasswordDto dto)
        {
            try
            {
                _accountServices.EditPassword(dto);
                return RedirectToAction("Index");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
        }

        [Authorize]
        public IActionResult DeleteUser()
        {
            return View();

        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteUser(DeleteUserDto dto)
        {
            try
            {
                var password = dto.Password;

                _accountServices.DeleteUser(password);

                return RedirectToAction("Index");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }

        }
        
        [Authorize]
        public IActionResult AccountMenu()
        {
            return View();
        }
        //Menu
        [Authorize]
        public IActionResult CreateMenu()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateMenu(Menu newMenu)
        {

            try
            {
                _randomizerService.MenuCreate(newMenu);

                return RedirectToAction("Index");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }


        }

        [Authorize]
        public IActionResult MenuList()
        {

            try
            {
                var list = _randomizerService.MenuSearch();

                return View(list);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }


        }

        [Authorize]
        public IActionResult EditMenu([FromRoute] int id )
        {
            try
            {
                var menu = _randomizerService.FindMenu(id);
                return View(menu);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditMenu(Menu menu, [FromRoute] int id)
        {
            try
            {
                _randomizerService.EditMenu(menu, id);

                return RedirectToAction("Index");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (ForbidException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
        }

        [Authorize]
        public IActionResult DeleteMenu(int id)

        {
            try
            {
                _randomizerService.DeleteMenu(id);

                return RedirectToAction("MenuList");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (ForbidException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }

        }
        //Product
        [Authorize]
        public IActionResult CreateProduct([FromRoute] int id)

        {


            return View();
        }

        [Authorize]       
        [HttpPost]
        public IActionResult CreateProduct(Product product, [FromRoute] int id)

        {
            try
            {
                _randomizerService.ProductCreate(product, id);

                return RedirectToAction("Index");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (ForbidException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }

        }
        
        [Authorize]
        public IActionResult ProductList([FromRoute] int id)

        {
            try
            {
                var list = _randomizerService.ProductList(id);

                return View(list);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }


        }

        [Authorize]
        public IActionResult EditProduct([FromRoute] int id)
        {
            try
            {
                var product = _randomizerService.FindProduct(id);
                return View(product);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
           

        }

        [Authorize]
        [HttpPost]
        public IActionResult EditProduct(Product product, [FromRoute] int id)
        {
            try
            {
                _randomizerService.EditProduct(product, id);
                return RedirectToAction("Index");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (ForbidException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
        }

        [Authorize]
        public IActionResult DeleteProduct([FromRoute] int id)

        {
            try
            {
                _randomizerService.DeleteProduct(id);

                return RedirectToAction("MenuList");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
            catch (ForbidException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }

        }
        //Randomize
        public IActionResult Randomize([FromRoute] int id)

        {
            try
            {
                var list = _randomizerService.ProductList(id);
                var random = _randomizerService.Random(list);

                return View(random);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }


        }

        [HttpPost]
        public IActionResult Randomize()

        {
            return View();
        }
        //QR Generate
        [Authorize]
        public IActionResult GenerateQRCodeForMenu(int menuId)
        {
            try
            {

                string productListUrl = Url.Link("default", new { controller = "Randomizer", action = "Randomize", id = menuId });


                string qrCode = _qrService.QRGenerate(productListUrl);


                ViewBag.QRCode = "data:image/png;base64," + qrCode;
                ViewBag.MenuId = menuId;

                return View();
            }
            catch (NotFoundException ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorView");
            }
        }










    }
}
