using BLLLibrary;
using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.ConnectionProvider;
using DataLibrary.UoW;
using Microsoft.OpenApi.Models;

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
// Add services to the container.

builder.Services.AddControllers().
                AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "api.pilkarzyk", Version = "v1" });
        options.DocInclusionPredicate((docName, description) => true);
        options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows()
            {
                Password = new OpenApiOAuthFlow()
                {
                    TokenUrl = new Uri("/api/token/generate", UriKind.Relative),
                    Scopes = { }
                },
            }

        });
        options.AddServer(new OpenApiServer()
        {
            Url = "http://localhost:27884"
        });
    }
    );
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    /*.WithExposedHeaders(["Content-Disposition", "x-current-count", "x-current-page", "x-total-count", "x-page-count"])*/);
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
