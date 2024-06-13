using Application;
using Infrastructure.Persistence;
using UserPermissionsApi;
using UserPermissionsApi.Extentions;
using UserPermissionsApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentations()
	.AddInfrastructure(builder.Configuration)
	.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	// Se ingresa en un ambiente de desarrollo
	app.ApplyMigrations();
}

app.UseExceptionHandler("/error");

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlerMiddlewares>();

app.MapControllers();

app.Run();
