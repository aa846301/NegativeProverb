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
        Description = "�b�U�ؤ��A��JJWT Token�GBearer Token",
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
//Scoped�G�`�J������b�P�@Request���A�ѦҪ����O�ۦP����(�A�bController�BView���`�J��IDbConnection���V�ۦP�Ѧ�)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProjectContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddBusiness();
builder.Services.AddSingleton<JwtHelpers>();
builder.Services.AddSingleton<ClaimsPrincipal>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // �����ҥ��ѮɡA�^�����Y�|�]�t WWW-Authenticate ���Y�A�o�̷|��ܥ��Ѫ��Բӿ��~��]
        options.IncludeErrorDetails = true; // �w�]�Ȭ� true�A���ɷ|�S�O����

        options.TokenValidationParameters = new TokenValidationParameters
        {
            // �z�L�o���ŧi�A�N�i�H�q "sub" ���Ȩó]�w�� User.Identity.Name
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            // �z�L�o���ŧi�A�N�i�H�q "roles" ���ȡA�åi�� [Authorize] �P�_����
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

            // �@��ڭ̳��|���� Issuer
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

            // �q�`���ӻݭn���� Audience
            ValidateAudience = false,
            //ValidAudience = "JwtAuthDemo", // �����ҴN���ݭn��g

            // �@��ڭ̳��|���� Token �����Ĵ���
            ValidateLifetime = true,

            // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
            ValidateIssuerSigningKey = false,

            // "1234567890123456" ���ӱq IConfiguration ���o
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
        };
    });

builder.Services.AddAuthorization();
var app = builder.Build();

// �]�w���쪺API�^���榡Middleware
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



// ���o JWT Token �����Ҧ� Claims
app.MapGet("/claims", (ClaimsPrincipal user) =>
{
    return Results.Ok(user.Claims.Select(p => new { p.Type, p.Value }));
})
    .WithName("Claims")
    .RequireAuthorization();

// ���o JWT Token �����ϥΪ̦W��
app.MapGet("/username", (ClaimsPrincipal user) =>
{
    return Results.Ok(user.Identity?.Name);
})
    .WithName("Username")
    .RequireAuthorization();

// ���o�ϥΪ̬O�_�֦��S�w����
app.MapGet("/isInRole", (ClaimsPrincipal user, string name) =>
{
    return Results.Ok(user.IsInRole(name));
})
    .WithName("IsInRole")
    .RequireAuthorization();

// ���o JWT Token ���� JWT ID
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
