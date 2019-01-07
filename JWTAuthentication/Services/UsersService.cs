using JWTAuthentication.IServices;
using JWTAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace JWTAuthentication.Services
{
    public class UsersService : IUsersService
    {
        private readonly List<User> users;

        public UsersService()
        {
            users = new List<User>
            {
                new User { Username = "user", Password = "P@ssw0rd" },
                new User { Username = "marcin", Password = "MyPa$$w0rd" },
            };
        }

        // PM> Install-Package System.IdentityModel.Tokens.Jwt 
        public User Authenticate(string username, string password)
        {
            var user = users.SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                string secretKey = WebConfigurationManager.AppSettings["SecretKey"];

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                ClaimsIdentity claimsIdentity = new ClaimsIdentity();
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.HomePhone, "555-555-555"));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, "marcin.sulecki@gmail.com"));

                var securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    SigningCredentials = credentials,
                    Audience = "http://www.example.com",
                    Issuer = "self",
                    Expires = DateTime.UtcNow.AddMinutes(5)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
                user.Token = tokenHandler.WriteToken(securityToken);

                user.Password = null;

            }

            return user;


        }
    }
}