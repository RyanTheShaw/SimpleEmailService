using Microsoft.EntityFrameworkCore;
using SimpleEmailService.Core;
using SimpleEmailService.DataAccess.DbContexts;
using SimpleEmailService.DataAccess.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
    .AddJsonOptions(options =>
    {
        options.UseDateOnlyTimeOnlyStringConverters();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

builder.Services.AddDbContext<EmailDbContext>(options => options.UseInMemoryDatabase("EmailDb"));

builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if(bool.Parse(app.Configuration.GetSection("SeedDataOnStartup").Value))
{
    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<EmailDbContext>();
    await NonProdDataSeeder.Seed(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
