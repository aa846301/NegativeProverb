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

        [HttpPost]
        [Route("Login")]
        public async Task<BaseModel<string>> Login(LoginViewModel input) => await _loginService.Login(input);


    }
}
