using BulkyBook.DataAccess.DBInitializer;
using Stripe;

namespace BulkyBookWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add DbContext to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add StripeSettings to the container.
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            // Add Identity to the container.
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            // Add Facebook authentication.
            builder.Services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = builder.Configuration["FacebookAuthentication:AppId"]!;
                options.AppSecret = builder.Configuration["FacebookAuthentication:AppSecret"]!;
            });

            // Add Microsoft authentication.
            builder.Services.AddAuthentication().AddMicrosoftAccount(options =>
            {
                options.ClientId = builder.Configuration["MicrosoftAuthentication:ClientId"]!;
                options.ClientSecret = builder.Configuration["MicrosoftAuthentication:ClientSecret"]!;
            });

            // Add Google authentication.
            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["GoogleAuthentication:ClientId"]!;
                options.ClientSecret = builder.Configuration["GoogleAuthentication:ClientSecret"]!;
            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            // Register repositories.
            builder.Services.AddScoped<IDBInitializer, DBInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Add Stripe configuration.
            StripeConfiguration.ApiKey = app.Configuration
                .GetSection("Stripe:SecretKey").Get<String>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            SeedDatabase();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();

            void SeedDatabase()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
                    dbInitializer.Initialize();
                }
            }
        }
    }
}