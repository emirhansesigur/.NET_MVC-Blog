using BlogNET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BlogNET.Controllers
{
    public class LoginController : Controller
    {
        private readonly BlogDbContext _context;
        private IConfiguration _configuration;

        public LoginController(BlogDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        //[AllowAnonymous]
        // authorization  - yetki
        // authentication - kimlik kanıtlama
        //[Authorize(Roles = "SuperAdmin")]
        //public IActionResult Super()
        //{
        //    string token = HttpContext.Session.GetString("ali");
        //    return View();
        //}

        [HttpPost]  
        public async Task<IActionResult> Index(Author model) // home login e gidiyor. o yüzden düzeltmeler yappp.
        {
            using (SHA256 sha256 = SHA256.Create())
            {

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                byte[] passwordBytes = Encoding.UTF8.GetBytes(model.Password); // Parolayı byte dizisine dönüştür
                byte[] hashBytes = sha256.ComputeHash(passwordBytes); // Byte dizisini SHA-256 ile hashle

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Her byte'ı hexadecimal formata dönüştürerek string olarak birleştir
                }

                string hashedPassword = sb.ToString();

                var emailParam = new SqlParameter("@Email", model.Email.ToLower());
                var passwordParam = new SqlParameter("@Password", hashedPassword);

                var query = "SELECT TOP 1 * FROM Author WHERE Email = @Email AND Password = @Password";
                //TOP 1 İstenen sonucun sadece ilk kaydı olduğu biliniyorsa, bu ifade kullanılarak sorgunun performansı artırılabilir.

                var author = await _context.Author.FromSqlRaw(query, emailParam, passwordParam).FirstOrDefaultAsync();

                if (author != null)
                {
                    //var token = Generate(author);
                    //HttpContext.Session.SetString("token", token);
                    return RedirectToAction("Index");
                }

                return NotFound("User Not Found");
            }
        }

        //private string Generate(Author user)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Name),
        //        new Claim(ClaimTypes.Email, user.Email),
        //        new Claim(ClaimTypes.Surname, user.Surname),
        //        new Claim(ClaimTypes.Role, user.Role)
        //    };


        //    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
        //      _configuration["Jwt:Audience"],
        //      claims,
        //      expires: DateTime.Now.AddMinutes(15),
        //      signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}



        //private Author Authenticate(Author userLogin)
        //{
        //    var currentUser = _context.Author.FirstOrDefault(o => o.Email.ToLower() == userLogin.Email.ToLower() && o.Password == userLogin.Password);

        //    if (currentUser != null)
        //    {
        //        return currentUser;
        //    }

        //    return null;
        //}



    }
}
