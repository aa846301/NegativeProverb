using Common.Attributes;
using Common.Model;
using DataAccess.BusinessModel.Login;
using DataAccess.ProjectContext;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Security.Claims;

namespace BusinessRule.Service;

[Service()]
[AllowAnonymous()]
public class LoginService
{
    public readonly ProjectContext _db;
    public readonly JwtHelpers _jwt;
    public LoginService(ProjectContext projectContext, JwtHelpers jwt)
    {
        _db = projectContext;
        _jwt = jwt;

    }

    public async Task<BaseModel<string>> Login(LoginViewModel input)
    {
        var result = new BaseModel<string>();
        if (ValidateUser(input))
        {
            var token = _jwt.GenerateToken(input.Username);
            result.Code = ((int)HttpStatusCode.OK).ToString();
            result.Success = true;
            result.Data = token;
            return result;
        }
        else
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Data = string.Empty;
            return result;
        }
    }




    public static bool ValidateUser(LoginViewModel input)
    {
        return true;
    }
}
