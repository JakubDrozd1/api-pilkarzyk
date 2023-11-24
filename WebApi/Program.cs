using BLLLibrary.IService;
using BLLLibrary.Service;
using DataLibrary.ConnectionProvider;
using DataLibrary.UoW;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionProvider, ConnectionProvider>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IMeetingsService, MeetingsService>();
builder.Services.AddScoped<IMessagesService, MessagesService>();
builder.Services.AddScoped<IRankingsService, RankingsService>();
builder.Services.AddScoped<IGroupsUsersService, GroupsUsersService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Aimer API", Version = "v1" });
        options.DocInclusionPredicate((docName, description) => true);
        options.AddSecurityDefinition("bearerAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.OAuth2,
            Flows = new Microsoft.OpenApi.Models.OpenApiOAuthFlows()
            {
                Password = new Microsoft.OpenApi.Models.OpenApiOAuthFlow()
                {
                    TokenUrl = new Uri("/api/Token", UriKind.Relative),
                    Scopes = new Dictionary<string, string>()
                    {
                        { "api", "Access Aimer API" }
                    }
                }
            }
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
    .WithExposedHeaders(["Content-Disposition", "x-current-count", "x-current-page", "x-total-count", "x-page-count"]));
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
