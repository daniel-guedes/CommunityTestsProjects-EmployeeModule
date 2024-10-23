using Funcionarios.Application.Commands.AddFuncionarioCommand;
using Funcionarios.Infra.Contexts;
using Funcionarios.Infra.Repositories.FuncionariosRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Funcionarios.Api.Middlewares;
using Funcionarios.Application.FailFastPipeLineBehaviors;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Funcionario API", Version = "v1" });
});


//Configuração do DbContext em SQL Server
builder.Services.AddDbContext<SqlServerDbContext>(options =>
	options.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServerConn"]));

//Configuração do Mongo
builder.Services.AddSingleton<IMongoClient>(sp =>
	new MongoClient(builder.Configuration["MongoDbSettings:MongoDbConn"]));
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddSingleton(sp =>
{
	var client = sp.GetRequiredService<IMongoClient>();
	var databaseName = builder.Configuration["MongoDbSettings:DatabaseName"];
	return client.GetDatabase(databaseName);
});


// Configuração dos repositórios
builder.Services.AddScoped<IFuncionariosRepository, FuncionariosRepository>();
builder.Services.AddScoped<IFuncionarioMongoRepository, FuncionarioMongoRepository>();

// Configuração do MediatR
builder.Services.AddMediatR(cfg =>
{
	cfg.RegisterServicesFromAssembly(typeof(AddFuncionarioCommandHandler).Assembly);
});


// Validações utilizando Fluent
builder.Services.AddValidatorsFromAssemblyContaining<AddFuncionarioCommandValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FailFastPipeLineBehaviors<,>));

// Configuração do CORS permitindo todas as origens
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader();
	});
});

var app = builder.Build();


using(var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<SqlServerDbContext>();
	var loggerFactory = LoggerFactory.Create(builder =>
	{
		builder.AddConsole();
	});
	var logger = loggerFactory.CreateLogger<Program>();
	try
	{
		dbContext.Database.Migrate();
		logger.LogInformation("Banco de dados migrado com sucesso.");
	}
	catch (Exception ex)
	{
		logger.LogError(ex, "Ocorreu um erro ao migrar o banco de dados.");
	}
}

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use the CORS policy
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

await app.RunAsync(new CancellationToken());
