using Business;
using DataAccess;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IService, Service>();
SQLitePCL.Batteries.Init();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/boms", (IService service) =>
{
    return service.GetAllBOMs();
})
.WithName("GetBillOfMaterials")
.WithOpenApi();

app.MapGet("/parts", (IService service) =>
{
    return service.GetAllParts();
})
.WithName("GetParts")
.WithOpenApi();

app.MapGet("/tree", (IService service) =>
{
    var response = service.GetTree(0);
    return response;
})
.WithName("GetTree")
.WithOpenApi();

app.MapGet("/subtree/{id}", (int id, IService service) =>
{
    var response = service.GetTree(id);
    return response;
})
.WithName("GetSubTree")
.WithOpenApi();


app.MapGet("/detparts", (IService service) =>
{
    var response =  service.GetDetailedParts();
    return response;
})
.WithName("GetDetailedParts")
.WithOpenApi();

app.MapFallbackToFile("/index.html");

app.Run();