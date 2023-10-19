using Azure.Core;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Randomizer.Entities;
using Randomizer.Exceptions;
using Randomizer.Models;
using System.Security.Claims;
using NLog.Fluent;


namespace Randomizer.Services
{
    public interface IAccountServices
    {
        public void GenerateCookie(Login login);
        public void RegisterUser(RegisterUserDto dto);
        public void Logout();
    }
    public class AccountServices : IAccountServices
    {
        private readonly RandomizerDbContext _dbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPasswordHasher<User> _passwordHasher;
        public AccountServices(RandomizerDbContext dbContext, IHttpContextAccessor httpContextAccessor, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _contextAccessor = httpContextAccessor;
            _passwordHasher = passwordHasher;
        }

        public void GenerateCookie(Login login)
        {
            var user = _dbContext.User.FirstOrDefault(x => x.Email == login.Email);

            if (user is null)
            {
                throw new BadRequestException("Invaild username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invaild username or password");
            }


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name}"),
                
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")),
                    
            };
            ClaimsIdentity claimsIdentity= new ClaimsIdentity(claims,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);


            _contextAccessor.HttpContext.SignInAsync(claimsPrincipal);
            
        }
        
        public void RegisterUser(RegisterUserDto dto)
        {

            var email = _dbContext.User.FirstOrDefault(x => x.Email == dto.Email);
            if (email != null) 
            {
                throw new BadRequestException("Email already in use");
            }


            if ( dto.Password != dto.ConfirmPassword)
            {
                throw new BadRequestException("Passwords are not the same");
            }
            var newUser = new User()
            {
                Name = dto.Name,
                Email = dto.Email,
                Nationality = dto.Nationality,
                DateOfBirth = dto.DateOfBirth,
                

            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;

            _dbContext.User.Add(newUser);
            _dbContext.SaveChanges();
            
        }

        public void Logout()
        {
            _contextAccessor.HttpContext.SignOutAsync();
            
        }

    }
}
