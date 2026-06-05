using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!))
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend",
        policy =>
        {
            policy
                .WithOrigins(builder.Configuration["CORS_ORIGINS"] ?? "http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddAuthorization();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardService, BoardService>();

builder.Services.AddScoped<IBoardRoleRepository, BoardRoleRepository>();
builder.Services.AddScoped<IBoardRoleService, BoardRoleService>();

builder.Services.AddScoped<IBoardUserRepository, BoardUserRepository>();
builder.Services.AddScoped<IBoardUserService, BoardUserService>();

builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();

builder.Services.AddScoped<IWorkspaceRoleRepository, WorkspaceRoleRepository>();
builder.Services.AddScoped<IWorkspaceRoleService, WorkspaceRoleService>();

builder.Services.AddScoped<IWorkspaceUserRepository, WorkspaceUserRepository>();
builder.Services.AddScoped<IWorkspaceUserService, WorkspaceUserService>();

builder.Services.AddScoped<IListItemRepository, ListItemRepository>();
builder.Services.AddScoped<IListItemService, ListItemService>();

builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<ICardService, CardService>();

builder.Services.AddScoped<IChecklistRepository, ChecklistRepository>();
builder.Services.AddScoped<IChecklistService, ChecklistService>();

builder.Services.AddScoped<IChecklistItemRepository, ChecklistItemRepository>();
builder.Services.AddScoped<IChecklistItemService, ChecklistItemService>();

builder.Services.AddScoped<ICardAssignmentRepository, CardAssignmentRepository>();
builder.Services.AddScoped<ICardAssignmentService, CardAssignmentService>();

builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();

builder.Services.AddScoped<ILabelRepository, LabelRepository>();
builder.Services.AddScoped<ILabelService, LabelService>();

builder.Services.AddScoped<ICardLabelRepository, CardLabelRepository>();
builder.Services.AddScoped<ICardLabelService, CardLabelService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await Seeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API");
    });
}

app.UseCors("Frontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<LavenderFlowHub>("/lavenderFlowHub");

app.Run();