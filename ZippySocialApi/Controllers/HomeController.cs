using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZippySocialApi.Content;
using ZippySocialApi.IServices;
using ZippySocialApi.Models;

namespace ZippySocialApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUser _service;
        private readonly AuthService _authService;
        private readonly ApplicationDbContext _context;
        public HomeController(IUser service, ApplicationDbContext context, AuthService authService)
        {
            _service = service;
            _context = context;
            _authService = authService;
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="login">Login model object</param>
        /// <returns>true if user is found</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) // Chack model is valid or not
                return BadRequest(ModelState);


            bool res = await _service.UserLogin(login);  // Call Service

            if (res) { 
                var token = _authService.GenerateToken(login.Email);
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == login.Email);
                return Ok(new {res=res,user, token=token });
            }
            return Unauthorized();
        }

        /// <summary>
        /// To register a user 
        /// </summary>
        /// <param name="user">user model object</param>
        /// <returns>true if user register successfully</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] User user)
        {
            if (!ModelState.IsValid) // Chack model is valid or not
                return BadRequest(ModelState);

            var emailExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower()); // To check duplicate emails.
            if (emailExists)
            {
                return Ok(new { success = false, exists = "EmailExists" });
            }
            bool res = await _service.RegisterUser(user);  // Call Service
            return Ok(new { success = res, exists = "No" });
        }

        [HttpGet("GetUsers")]
        [Authorize]
        public async Task<IActionResult> GetUsers() 
        {
            var res = await _context.Users.ToListAsync();
            return Ok(res);
        }
    }
}
