using Common.Extension;
using Common.Model;
using DataAccess.ProjectContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "在下框中，輸入JWT Token：Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
            Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"}
            },new string[] { }
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "NegativeProverb API",
        Description = "An ASP.NET Core Web API for NegativeProverb",
        //TermsOfService = new Uri("https://example.com/terms"),
        //Contact = new OpenApiContact
        //{
        //    Name = "Example Contact",
        //    Url = new Uri("https://example.com/contact")
        //},
        //License = new OpenApiLicense
        //{
        //    Name = "Example License",
        //    Url = new Uri("https://example.com/license")
        //}
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
//Scoped：注入的物件在同一Request中，參考的都是相同物件(你在Controller、View中注入的IDbConnection指向相同參考)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProjectContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddBusiness();
builder.Services.AddSingleton<JwtHelpers>();
builder.Services.AddSingleton<ClaimsPrincipal>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
        options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉

        options.TokenValidationParameters = new TokenValidationParameters
        {
            // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

            // 一般我們都會驗證 Issuer
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

            // 通常不太需要驗證 Audience
            ValidateAudience = false,
            //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

            // 一般我們都會驗證 Token 的有效期間
            ValidateLifetime = true,

            // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
            ValidateIssuerSigningKey = false,

            // "1234567890123456" 應該從 IConfiguration 取得
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();

// 設定全域的API回應格式Middleware
app.Use(async (context, next) =>
{
    try
    {
        if (context.Request.ContentLength == 0)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 400;


            var errorMessage = new BaseModel
            {
                Success = false,
                Code = context.Response.StatusCode.ToString(),
                Exception = "A non-empty request body is required.",
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessage));
        }
        else
        {
            await next();
        }
    }
    catch (Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var errorMessage = new BaseModel
        {
            Success = false,
            Code = context.Response.StatusCode.ToString(),
            Exception = ex.Message,
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessage));
    }
});



// 取得 JWT Token 中的所有 Claims
app.MapGet("/claims", (ClaimsPrincipal user) =>
{
    return Results.Ok(user.Claims.Select(p => new { p.Type, p.Value }));
})
    .WithName("Claims")
    .RequireAuthorization();

// 取得 JWT Token 中的使用者名稱
app.MapGet("/username", (ClaimsPrincipal user) =>
{
    return Results.Ok(user.Identity?.Name);
})
    .WithName("Username")
    .RequireAuthorization();

// 取得使用者是否擁有特定角色
app.MapGet("/isInRole", (ClaimsPrincipal user, string name) =>
{
    return Results.Ok(user.IsInRole(name));
})
    .WithName("IsInRole")
    .RequireAuthorization();

// 取得 JWT Token 中的 JWT ID
app.MapGet("/jwtid", (ClaimsPrincipal user) =>
{
    return Results.Ok(user.Claims.FirstOrDefault(p => p.Type == "jti")?.Value);
})
    .WithName("JwtId")
    .RequireAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
