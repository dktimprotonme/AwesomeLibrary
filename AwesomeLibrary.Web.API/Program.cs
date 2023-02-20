using AwesomeLibrary.BLL.Services.Concretes;
using AwesomeLibrary.BLL.Services.Interfaces;
using AwesomeLibrary.DAL;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

string appSettingsFileName = "appsettings.json";
if (builder.Environment.IsDevelopment())
{
    appSettingsFileName = "appsettings.Development.json";
}
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile(appSettingsFileName, optional: true, reloadOnChange: true)
    .Build();

builder.Services.AddDbContext<AwesomeLibraryDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("AwesomeLibrarySqlServer"));
});

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookTransactionService, BookTransactionService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IPenaltyCalculatorService, PenaltyCalculatorService>();
builder.Services.AddScoped<ITurkeyNextWorkingDayCalculatorService, TurkeyNextWorkingDayCalculatorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var corsAllowedUrls = configuration.GetSection("CorsSettings:AllowOrigins").Get<string[]>();
app.UseCors(builder => builder.WithOrigins(corsAllowedUrls).AllowAnyHeader().AllowAnyMethod());

using (var asyncScope = app.Services.CreateAsyncScope())
{
    var dbContext = asyncScope.ServiceProvider.GetRequiredService<AwesomeLibraryDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    if (app.Environment.IsDevelopment())
    {
        var bookService = asyncScope.ServiceProvider.GetRequiredService<IBookService>();
        await bookService.CreateTemporaryDataForTestAsync();

        var memberService = asyncScope.ServiceProvider.GetRequiredService<IMemberService>();
        await memberService.CreateTemporaryDataForTestAsync();

        var bookTransactionService = asyncScope.ServiceProvider.GetRequiredService<IBookTransactionService>();
        await bookTransactionService.CreateTemporaryDataForTestAsync();
    }
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
    var response = new { errorMessage = exception.Message, isError = true };
    await context.Response.WriteAsJsonAsync(response);
}));

app.Run();
