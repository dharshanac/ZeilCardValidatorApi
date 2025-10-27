using FluentValidation;
using FluentValidation.AspNetCore;
using ZeilCardValidatorApi.Application.Services;
using ZeilCardValidatorApi.Application.Validators;
using ZeilCardValidatorApi.CardsValidatorApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation(); // Enables auto-validation
builder.Services.AddValidatorsFromAssemblyContaining<CreditCardRequestValidator>();

// Application services
builder.Services.AddScoped<ILuhnService, LuhnService>();

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();