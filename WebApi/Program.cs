using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.UoW;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Net.Http.Headers;
using DataLibrary.Helper.ConnectionProvider;
using DataLibrary.Helper.Email;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionProvider, ConnectionProvider>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IMeetingsService, MeetingsService>();
builder.Services.AddScoped<IMessagesService, MessagesService>();
builder.Services.AddScoped<IRankingsService, RankingsService>();
builder.Services.AddScoped<IGroupsUsersService, GroupsUsersService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUsersMeetingsService, UsersMeetingsService>();
builder.Services.AddScoped<IGroupInviteService, GroupInviteService>();
builder.Services.AddScoped<INotificationTokenService, NotificationTokenService>();
builder.Services.AddScoped<IChatMessagesService, ChatMessagesService>();
builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
builder.Services.AddScoped<ITeamsService, TeamsService>();

builder.Services.AddControllers().
                AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "api.pilkarzyk", Version = "v1" });

        options.DocInclusionPredicate((docName, description) => true);

        options.AddSecurityDefinition("api-pilkarzyk-oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            In = ParameterLocation.Header,
            Name = HeaderNames.Authorization,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Flows = new OpenApiOAuthFlows
            {
                Password = new OpenApiOAuthFlow
                {
                    TokenUrl = new Uri("/api/token/generate", UriKind.Relative),
                    Scopes = new Dictionary<string, string>()
                }
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "api-pilkarzyk-oauth2"
                    }
                },
                Array.Empty<string>()
            }
        });

        options.AddServer(new OpenApiServer()
        {
            // Url = "http://localhost:27884"
            Url = "http://192.168.88.20:45455"
        });
    }
    );
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"] ?? string.Empty)),
            ClockSkew = TimeSpan.Zero
        };
        options.IncludeErrorDetails = true;
    });
builder.Services.AddSignalR();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
app.UseCors(builder =>
{
    builder
        .WithOrigins("http://192.168.88.20:4200", "http://localhost:27884", "http://192.168.88.224:4200", "https://jaball.manowski.pl")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
