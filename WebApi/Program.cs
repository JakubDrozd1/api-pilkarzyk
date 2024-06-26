using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.UoW;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Net.Http.Headers;
using DataLibrary.Helper.ConnectionProvider;
using WebApi.Controllers;

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
builder.Services.AddScoped<IGuestsService, GuestsService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

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
            Url = builder.Configuration["OpenApiServer"]
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
app.UseCors(builders =>
{
    builders
        .WithOrigins(builder.Configuration["Angular"] ?? string.Empty, builder.Configuration["Swagger"] ?? string.Empty)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
