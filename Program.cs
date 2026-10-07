using Kt5RepositoryApi.Data;
using Kt5RepositoryApi.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connection = new SqliteConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
connection.DataSource = Path.GetFullPath(connection.DataSource, builder.Environment.ContentRootPath);
Directory.CreateDirectory(Path.GetDirectoryName(connection.DataSource)!);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection.ConnectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
await app.RunAsync();
