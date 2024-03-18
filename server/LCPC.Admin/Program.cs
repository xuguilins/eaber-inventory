using System.Reflection;
using System.Text;
using FluentValidation;
using LCPC.Domain.Commands;
using LCPC.Domain.IRepositories;
using LCPC.Domain.Validates;
using LCPC.Domain.Validates.Validatetor;
using LCPC.Infrastructure.Cores;
using LCPC.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using LCPC.Share.Configs;
using LCPC.Domain.IServices;
using LCPC.Domain.Services;
using LCPC.Admin.extendsition;
using Microsoft.OpenApi.Models;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using LCPC.Admin;
using LCPC.Domain.Hubs;
using LCPC.Hubs;
using LCPC.Share;
using Microsoft.AspNetCore.SignalR;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var types = Assembly.GetExecutingAssembly().GetTypes();
var controls = types.Where(d => d.FullName.EndsWith("Controller"))
    .ToList();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
Dictionary<string, string> dics = new Dictionary<string, string>();
foreach (var item in controls)
{
    var attbuite =  item.GetCustomAttribute<ApiExplorerSettingsAttribute>();
    if (attbuite != null)
    {
        if (!dics.ContainsKey(attbuite.GroupName))
            dics.Add(attbuite.GroupName,attbuite.GroupName);
    }
}
builder.Services.AddSwaggerGen(action =>
{
    action.DocumentFilter<DocumentFilter>();
    foreach (var item in dics.Keys)
    {
        action.SwaggerDoc(item, new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = item
        });
    }
    var scheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {

        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Name = "Authorization",
        Description = "ex： Bearer token",
        Scheme = "Bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        },
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    };
    var Requirement = new OpenApiSecurityRequirement
    {
        { scheme, new List<string>() }
    };
    action.AddSecurityRequirement(Requirement);
    action.AddSecurityDefinition("Bearer", scheme);
});
var options = builder.Configuration.GetSection("LCPCConfig").Get<LCPCConfig>();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(action =>
{
    action.RegisterModule<AutoFacModule>();
});

builder.Services.Configure<LCPCConfig>(
    builder.Configuration.GetSection("LCPCConfig"));


#region 仓储/服务
string connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AdminDbContext>(config =>
{
    config.UseSqlServer(connectionString);
});
builder.Services.AddScoped<ISqlDapper,SqlDapper>((provider)=>  new SqlDapper(connectionString));
builder.Services.AddCors(action =>
{
    
    action.AddPolicy("cors", option =>
    {
        option
        .AllowCredentials()
        .WithOrigins(options.CORS.Split(","))
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
#endregion

#region 即时通讯

builder.Services.AddScoped<IUserIdProvider, UserProvider>();
builder.Services.AddSignalR();


#endregion

#region 授权/认证

builder.Services.AddAuthentication(action =>
{
    action.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    action.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
{
   
    option.TokenValidationParameters = new TokenValidationParameters
    {
        // 是否开启签名认证
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.JWT.Secret)),
        ValidateIssuer = true,
        ValidIssuer = options.JWT.Issuer,
        ValidateAudience = false,
        ValidAudience = options.JWT.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
    option.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
          
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hubclient")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
    option.SaveToken = true;
});

#endregion


builder.Services.AddSingleton<CacherHelper>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
     
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    
    foreach (var item in dics.Keys)
    {
        options.SwaggerEndpoint($"/swagger/{item}/swagger.json",item);
    }
 
});
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("cors");
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();