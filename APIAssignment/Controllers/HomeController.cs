using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace APIAssignment.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        /*JWT is used as OAuth 2.0 Bearer Tokens to encode all relevant parts of access tokens*/
        public IActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"some_id"),
                new Claim("granny", "cookie")
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorithm);
            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audiance,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signingCredentials
                );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { access_token = tokenJson });
        }

        /*Dataset Reference - https://reqres.in/ */
        public List<Models.Users> Users = new List<Models.Users>()

        {
            new Models.Users {Id = 1, Email ="michael.lawson@reqres.in",FirstName = "Michael", LastName = "Lawson", Age = 25},
            new Models.Users {Id = 2, Email ="lindsay.ferguson@reqres.in", FirstName = "Lindsay", LastName = "Ferguson", Age = 35},
            new Models.Users {Id = 3, Email ="tobias.funke@reqres.in",FirstName = "Tobias", LastName = "Funke", Age = 42},
            new Models.Users {Id = 4, Email ="byron.fields@reqres.in",FirstName = "Byron", LastName = "Fields", Age = 41},
            new Models.Users {Id = 5, Email ="george.edwards@reqres.in",FirstName = "George", LastName = "Edwards", Age = 21},
            new Models.Users {Id = 6, Email ="rachel.howell@reqres.in",FirstName = "Rachel", LastName = "Howell", Age = 75}
        };

       /*Get All ID*/
        [Authorize]
        [HttpGet]
        [Route("API/Users")]
        public IEnumerable<Models.Users> Get()
        {
            return Users;
        }

        /*Get One ID*/
        [HttpGet]
        [Route("API/Users/{Id}")]
        public Models.Users Get(int Id)
        {
            return Users.Find(a => a.Id == Id);
        }

    }
}


