using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pawtopia.Data;
using Pawtopia.Models;
using Pawtopia.Client.DTOs; // Đảm bảo đã có folder DTOs bên Client

namespace pawtopia.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly PawtopiaDbContext _db;
        // PasswordHasher giúp mã hóa mật khẩu trước khi lưu vào DB riêng
        private readonly PasswordHasher<User> _hasher = new();

        public AuthController(PawtopiaDbContext db)
        {
            _db = db;
        }

        // ========= ĐĂNG KÝ (REGISTER) =========
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ");

            // 1. Kiểm tra xem email đã tồn tại trong bảng Users chưa
            var exists = await _db.Users.AnyAsync(x => x.Email == dto.Email);
            if (exists)
                return BadRequest("Tài khoản này đã tồn tại trong hệ thống");

            // 2. Tạo đối tượng User mới
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = dto.Email,
                UserName = dto.Email,
                DisplayName = dto.Name,
                ProfileImageLink = "" // Có thể bổ sung sau
            };

            // 3. Mã hóa mật khẩu (Không bao giờ lưu mật khẩu dạng chữ thuần)
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            // 4. Lưu vào Database riêng của bạn
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công!" });
        }

        // ========= ĐĂNG NHẬP (LOGIN) =========
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ");

            // 1. Tìm user theo Email
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
                return BadRequest("Tài khoản không tồn tại");

            // 2. Giải mã và kiểm tra mật khẩu
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                return BadRequest("Mật khẩu không chính xác");

            // 3. Trả về thông tin cơ bản (sau này bạn có thể trả về JWT Token ở đây)
            return Ok(new
            {
                message = "Đăng nhập thành công",
                user = new { user.Id, user.Email, user.DisplayName }
            });
        }
    }
}