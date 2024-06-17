using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NET1717_Lab01_ProductManagement.API.Extentions;
using NET1717_Lab01_ProductManagement.API.Middleware;
using NET1717_Lab01_ProductManagement.Repository;
using NET1717_Lab01_ProductManagement.Repository.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddIdentityApiEndpoints<IdentityUser>()
//    .AddEntityFrameworkStores<MyDbContext>();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new KebabCaseQueryModelBinderProvider());
}).AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = new KebabCaseNamingPolicy();
    opts.JsonSerializerOptions.DictionaryKeyPolicy = new KebabCaseNamingPolicy();
});

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options => {
//    options.SchemaFilter<EnumSchemaFilter>();
//});

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbContext"));
});

builder.Services.AddTransient<UnitOfWork>();
builder.Services.AddSingleton<ResponseModelMiddleware>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab01", Version = "v1" });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseMiddleware<ResponseModelMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
