using E_CommerceAPI.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();



//  1-) Servislerin eklenmesi
builder.Services.AddPersistenceServices(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
