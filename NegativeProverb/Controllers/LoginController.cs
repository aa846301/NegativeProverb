using BusinessRule.Service;
using Common.Model;
using DataAccess.BusinessModel.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NegativeProverb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("Login")]
        public async Task<BaseModel<string>> Login(LoginViewModel input) => await _loginService.Login(input);

        [HttpGet("Claims")]
        [Authorize]
        public IActionResult GetClaims()
        {
            var userClaims = User.Claims.Select(p => new { p.Type, p.Value });
            return Ok(userClaims);
        }

        [HttpGet("Username")]
        [Authorize]
        public IActionResult GetLoginUserName()
        {
            var userName = User.Identity?.Name;
            return Ok(userName);
        }

        // 取得使用者是否擁有特定角色
        [HttpGet("IsInRole")]
        [Authorize]
        public IActionResult IsInRole(string name)
        {
            var isInRole = User.IsInRole(name);
            return Ok(isInRole);
        }

        // 取得 JWT Token 中的 JWT ID
        [HttpGet("Jwtid")]
        [Authorize]
        public IActionResult GetJwtId()
        {
            var jwtId = User.Claims.FirstOrDefault(p => p.Type == "jti")?.Value;
            return Ok(jwtId);
        }
    }
}
