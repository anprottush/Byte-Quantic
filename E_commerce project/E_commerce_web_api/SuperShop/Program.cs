using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Irony.Ast;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuperShop;
using SuperShop.Common.Configuration;
using SuperShop.Model.Data;
using SuperShop.Repository;
using SuperShop.Services;
using SuperShop.Services.Admin;
using SuperShop.Services.Common;
using SuperShop.SwaggerConfig;
using Swashbuckle.AspNetCore.Filters;
using System.Configuration;
using System.IO.Compression;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string AllowOrigin = "AllowAllOrigin";

builder.Services.AddControllers()
	.AddNewtonsoftJson(options =>
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddRazorPages();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Compression
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
	options.Level = CompressionLevel.Optimal;
});
builder.Services.AddResponseCompression(options =>
{
	options.EnableForHttps = true;
	options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
#endregion

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BQEcommerceConnection")));

// Add services to the container
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
{
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 6;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

#region Service Injected
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
builder.Services.AddScoped<JWTServices>();
builder.Services.AddScoped<ITokenServices, TokenServices>();
builder.Services.AddScoped<IUserServices, UserServices>();

#region Register Service
builder.Services.AddScoped<ICommonServices, CommonServices>();
#endregion

//builder.Services.AddScoped<ICustomService<Resultss>, ResultService>();
//builder.Services.AddScoped<ICustomService<Departments>, DepartmentsService>();
//builder.Services.AddScoped<ICustomService<SubjectGpas>, SubjectGpasService>();
#endregion

builder.Services.AddSingleton(builder.Configuration.GetSection("ConnectionStrings").Get<ConnectionStringConfig>());
builder.Services.Configure<JWTSettingsConfig>(builder.Configuration.GetSection("JWTSettings"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowOrigin,
        builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

#region Authentication & Authorization
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWTSettings:Secret").Value);

#endregion
#region Authentication Module
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("X-Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});
#endregion

builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<SwaggerExcludeFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "REST API",
        Version = "v1"
    });
    //c.DocumentFilter<RemoveSchemasFilter>();
    c.ExampleFilters();
    // Set the comments path for the Swagger JSON and UI.

    c.OperationFilter<SwaggerAuthorizationOperationFilter>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "Enter the request header in the following box to add Jwt To grant authorization Token: Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //c.SchemaFilter<SwaggerExcludeSchemaFilter>();
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                    });
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<StartupBase>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Log the exception
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "An unhandled exception occurred.");

        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An error occurred. Please try again later.");
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(AllowOrigin);

app.UseStaticFiles();

#region Compression
app.UseResponseCompression();
#endregion

app.UseAuthentication();
app.UseAuthorization();


#region 'Initailly Creating Role and Users'

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

    // Initialize SeedDB
    SeedDB.Initialize(serviceProvider, userManager, roleManager, dbContext).Wait();
}

#endregion

app.MapControllers();

app.Run();
