using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using UserService.DTOs;
using UserService.Entities;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        /// <summary>
        /// Rolların yaradılması
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            result = await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            return Ok("Identity role created");
        }
        /// <summary>
        /// Qeydiyyat sistemi
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            AppUser user = await _userManager.FindByNameAsync(registerDto.Username);
            if (user != null)
            {
                return BadRequest("Username is already");
            }

            user = new AppUser
            {
                UserName = registerDto.Username,
                Surname = registerDto.Surname,
                PhoneNumber = registerDto.PhoneNumber,
                Email = registerDto.Email,
                Cins = registerDto.Cins,
                Countries = registerDto.Countries,
                UserCreateTime = DateTime.Now,
                Cif = registerDto.Username + registerDto.Password

            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            //var resultCif = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest("Password or email wrong");
            }
            result = await _userManager.AddToRoleAsync(user, "Admin");
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok("User yaradildi");
        }

        /// <summary>
        /// İstifadəçi girişi
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            AppUser user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return NotFound();
            }
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return NotFound();
            }

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim("surname", user.Surname));
            claims.Add(new Claim("cif", user.Cif));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList());

            string secretKey = "2e31871a-3b97-4eef-8e12-cad67e301067";
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(3),
                SigningCredentials = credentials,
                Audience = "https://localhost:44369/",
                Issuer = "https://localhost:44369/",
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);


            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

    }
}

