using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace Project.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationDbContext _db;
		public AuthController(IHttpContextAccessor context, ApplicationDbContext db)
        {
            _context = context;
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var users =  _db.users.Where((u)=>u.Email == user.Email && u.Password == user.Password ).FirstOrDefault();

           

            if (users != null)
            {

                var clams = new List<Claim> {

                    new Claim(ClaimTypes.Email,user.Email)
                };

                var clamidentity = new ClaimsIdentity(clams, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(clamidentity));



                return Redirect("Dashboard/Index");


            }

            return View();


        }

       

        public async Task<IActionResult> Logout()
        { 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login","Auth"); 
 
        }





	}
}
