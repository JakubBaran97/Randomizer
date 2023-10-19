
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
        
        public IActionResult Index()
        {
            
            return View();
        }

        [Route("login")]
        [HttpGet]
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


        [Route("logout")]
        public IActionResult Logout()
        {
            _accountServices.Logout();
            return RedirectToAction("Index");
        }

        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")]
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
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
        public IActionResult CreateProduct([FromRoute] int id)

        {
            

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateProduct(Product product , [FromRoute]int id)

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
           
        }

        
        [HttpGet]
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

        
        [HttpPost]
        public IActionResult ProductList()

        {
            return View();
        }


        [Authorize]
        [HttpGet]
       
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

        [Authorize]
        [HttpPost]
        
        public IActionResult Randomize()

        {
            return View();
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

                ViewData["ErrorMessage"] = "Forbidden";
                return View("ErrorView");
            }
           
        }

        [Authorize]
        
        public IActionResult DeleteProduct([FromRoute]int id)

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

                ViewData["ErrorMessage"] = "Forbidden";
                return View("ErrorView");
            }
            
        }
        [Authorize]
        [HttpGet]
        public IActionResult GenerateQRCodeForMenu(int menuId)
        {
            try
            {
                
                string productListUrl = Url.Link("default", new { controller = "Randomizer", action = "ProductList", id = menuId });


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
