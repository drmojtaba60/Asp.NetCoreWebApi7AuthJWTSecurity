using Asp.NetCore7AuthJwt.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.Configure<JwtKeyOptions>(builder.Configuration.GetSection("JwtKeyOptions"));
builder.Services.Configure<JwtKeyOptions>(builder.Configuration.GetSection(nameof(JwtKeyOptions)));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

addAuthhenticationJWTService(builder.Services, builder.Configuration);
ConfigureServiceSwaggerExtensions(builder.Services, builder.Configuration);

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

app.Run();

void addAuthhenticationJWTService(IServiceCollection services, IConfiguration config)
{
    const string JwtKeyOptionsNameInAppSettings = "JwtKeyOptions";
    services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {

            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateActor = true,
                ValidAudience = config[$"{JwtKeyOptionsNameInAppSettings}:ValidAudience"],
                ValidIssuer = config[$"{JwtKeyOptionsNameInAppSettings}:ValidIssuer"],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config[$"{JwtKeyOptionsNameInAppSettings}:Secret"]))
            };
        });
}

void ConfigureServiceSwaggerExtensions(IServiceCollection services, IConfiguration configuration)
{
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    });
}

