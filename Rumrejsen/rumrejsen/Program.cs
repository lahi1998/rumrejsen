using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Rumrejsen.Models;
using Rumrejsen;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RumrejseApi", Version = "v1" });
    c.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization,
        Scheme = "Bearer"
    });
    // Add the security requirement globally (optional)
    // c.AddSecurityRequirement(/*...*/);
    c.OperationFilter<SecureEndpointAuthRequirementFilter>();
});

// Configure the distributed cache with the in-memory cache provider
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("E2M1pS5v8y/B?E(H+MbQeThVmYq3t6w9z$C&F)J@NcRfUjXn2r5u8x/A?D(G+KbPe"))
       };
   });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
    });
}

app.UseHttpsRedirection();

// Add JWT authentication
app.UseAuthentication();

app.UseAuthorization();

app.UseSession(); // Add the session middleware

app.MapControllers();

// Configure your session settings if needed
app.UseSession(new SessionOptions
{
    // Set your desired session options here, such as IdleTimeout, Cookie, etc.
});

app.Run();
