using E_CommerceAPI.Application.Validators.Products;
using E_CommerceAPI.Infrustructure;
using E_CommerceAPI.Infrustructure.Filters;
using E_CommerceAPI.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>());

// 3.0 -) FluentValidation Sýnfýlarý Ekleme
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<CreateProductValidator>();

// 3.1 -) Default dönen ModelState Hata nesnesini Kapama 
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);


//  1-) Servislerin eklenmesi Persistence Layer
builder.Services.AddPersistenceServices(builder.Configuration);

// 4-) Servislerin eklenmesi Infrastructure Layer
builder.Services.AddInfrastructureServices();


// 2-) CORS POLICY Yapýlandýrmasý
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



app.UseHttpsRedirection();

// wwwroot'u kullanabilmek, sunucuda depolayabilmek için ekliyoruz.
app.UseStaticFiles();

// CORS Miidleware'ini çaðýrýyoruz
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
