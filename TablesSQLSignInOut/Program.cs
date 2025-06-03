using TablesSQLSignInOut.Components;
using Microsoft.EntityFrameworkCore;
using TablesSQLSignInOut.Database;
var builder = WebApplication.CreateBuilder(args);

// Register AuditInterceptor as singleton
builder.Services.AddSingleton<AuditInterceptor>();

// Register IDbContextFactory<YourDbContext> with the interceptor
builder.Services.AddDbContextFactory<YourDbContext>((serviceProvider, options) =>
{
    var interceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(interceptor);
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddKeyedScoped<List<AuditEntry>>("Audit",(_, _) => new());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
;