using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TruongHocHuu_2122110460.Data;
using TruongHocHuu_2122110460.Dto;
using TruongHocHuu_2122110460.Model;
using TruongHocHuu_2122110460.Util;
using Microsoft.EntityFrameworkCore;
namespace TruongHocHuu_2122110460.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthController(AppDbContext context, JwtTokenService jwtService, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Đăng ký thành công");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
            if (user == null) return Unauthorized("Tài khoản không tồn tại");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Sai mật khẩu");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }

}
