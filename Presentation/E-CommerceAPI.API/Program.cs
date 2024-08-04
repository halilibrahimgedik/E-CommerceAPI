using E_CommerceAPI.Persistence;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();



//  1-) Servislerin eklenmesi
builder.Services.AddPersistenceServices(builder.Configuration);

// 2*) CORS POLICY Yapýlandýrmasý
builder.Services.AddCors(optioms =>
{
    optioms.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader().AllowAnyMethod();
    });
});




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS Miidleware'ini çaðýrýyoruz
app.UseCors();

app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
