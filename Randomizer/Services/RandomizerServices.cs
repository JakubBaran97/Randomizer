﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Randomizer.Authorization;
using Randomizer.Entities;
using Randomizer.Exceptions;
using Randomizer.Models;


namespace Randomizer.Services
{
    public interface IRandomizerService
    {
        public void MenuCreate(Menu menu);
        public List<Menu> MenuSearch();
        public Menu FindMenu(int id);
        public void EditMenu(Menu editedMenu, int id);
        public void DeleteMenu(int id);
        public void ProductCreate(Product product, int menuId);
        public List<Product> ProductList(int id);
        public Product FindProduct(int id);
        public void EditProduct(Product editedProduct, int id);

        public void DeleteProduct(int id);

        public Product Random(List<Product> list);

        

        

        
    }
    public class RandomizerServices : IRandomizerService
    {
        private readonly RandomizerDbContext _dbContext;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;
        public RandomizerServices(RandomizerDbContext dbContext, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        public void MenuCreate(Menu menu)
        {
            if (menu.Name == null) throw new NotFoundException(" Name field are empty");

            var newMenu = new Menu()
            {
                Name = menu.Name
                

            };

            

            newMenu.CreatedById = _userContextService.GetUserId;

            _dbContext.Menu.Add(newMenu);
            _dbContext.SaveChanges();
        }

        public List<Menu> MenuSearch()
        {
            int? UserId = _userContextService.GetUserId;
          
            var menu = _dbContext.Menu.Where(menu => menu.CreatedById == UserId).ToList();

            if (menu is null) throw new NotFoundException("Menu not found");

            return menu;

        }

        public Menu FindMenu(int id)
        {
            var menu = _dbContext.Menu.First(menu => menu.Id == id);
            if (menu == null) throw new NotFoundException("Menu not found");
            return menu;
        }

        public void EditMenu(Menu editedMenu, int id)
        {
            var menu = _dbContext.Menu.FirstOrDefault(x => x.Id == id);
            if (menu is null) throw new NotFoundException("Menu not Found");

            var authorizationResult = _authorizationService.AuthorizeAsync
                (_userContextService.User, menu, new MenuOperationRequirement(MenuResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException("Unauthorized");

            if (editedMenu.Name == null) throw new BadRequestException("Field are empty");
            menu.Name= editedMenu.Name;

            _dbContext.SaveChanges();

        }
        public void DeleteMenu(int id)
        {
            var menu = _dbContext.Menu.FirstOrDefault(x => x.Id == id);
            var product = _dbContext.Product.Where(x => x.MenuId == id);

            if (menu is null) throw new NotFoundException("Menu not Found");

            var authorizationResult = _authorizationService.AuthorizeAsync
                (_userContextService.User, menu, new MenuOperationRequirement(MenuResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException("Unauthorized");




            _dbContext.Product.RemoveRange(product);
            _dbContext.Menu.Remove(menu);

            _dbContext.SaveChanges();



        }

        public  void ProductCreate( Product product, int menuId)
        {
            if (product.Name == null || product.Description == null) throw new NotFoundException("Fields are empty");
            
           


            var newProduct = new Product()
            {

                Name = product.Name,
                Description = product.Description,
                MenuId = menuId

            };

            newProduct.CreatedById = _userContextService.GetUserId;

            

            _dbContext.Product.Add(newProduct);
            _dbContext.SaveChanges();

            
        }

        public List<Product> ProductList(int id)
        {
            

            var product = _dbContext.Product.Where(product => product.MenuId == id).ToList();

            if (product.Count == 0)  throw new NotFoundException("Product not Found"); 

            

            return product;

        }

        public Product FindProduct(int id)
        {
            var product = _dbContext.Product.FirstOrDefault(product => product.Id == id);
            if (product is null) throw new NotFoundException("Product not found");
            return product;
        }

        public void EditProduct(Product editedProduct, int id)
        {
            var product = _dbContext.Product.FirstOrDefault(x => x.Id == id);
            if (product is null) throw new NotFoundException("Product not Found");

            var authorizationResult = _authorizationService.AuthorizeAsync
               (_userContextService.User, product, new ProductOperationRequirement(ProductResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException("Unauthorized");
            if (editedProduct.Name == null || editedProduct.Description == null) throw new BadRequestException("Fields are empty");
            product.Name = editedProduct.Name;
            product.Description = editedProduct.Description;

            _dbContext.SaveChanges();

        }






        public void DeleteProduct(int id)
        {
            var product = _dbContext.Product.FirstOrDefault(x => x.Id == id);

            if (product is null) throw new NotFoundException("Product not Found");

            var authorizationResult = _authorizationService.AuthorizeAsync
                (_userContextService.User, product, new ProductOperationRequirement(ProductResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException("Unauthorized");

            _dbContext.Product.Remove(product);

            _dbContext.SaveChanges();



        }

        public Product Random(List<Product> list)
        {
            Random rand = new Random();
            var losowyElement = list[rand.Next(list.Count)];
            return losowyElement;
        }




    }
}
