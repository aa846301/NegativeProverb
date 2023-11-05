using Common.Extension;
using Common.Model;
using DataAccess.ProjectContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
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

});
//Scoped：注入的物件在同一Request中，參考的都是相同物件(你在Controller、View中注入的IDbConnection指向相同參考)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProjectContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddBusiness();


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
                Code = context.Response.StatusCode.ToString("D3"),
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
            Code = "500",
            Exception = ex.Message,
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorMessage));
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}


app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
