using Contracts;
using Instagram;
using Instagram.Extensions;
using Instagram.Presentation.ModelBinders;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Service;
using Service.Contracts;
using Service.HubCofig;
using Service.Notification;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LoggerManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.ConfigureCors();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddSignalR();
builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new PostForCreationModelBinderProvider());
    //options.ModelBinderProviders.Insert(0, new MediaItemForCreationModelBinderProvider());
    //options.ModelBinderProviders.Insert(0, new Base53ModelBinderProvider());
}).AddApplicationPart(typeof(Instagram.Presentation.AssemblyReference).Assembly)
.AddNewtonsoftJson();

builder.Services.AddAuthentication();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRedis(builder.Configuration);
//builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();

builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<IStoryService, StoryService>();
builder.Services.AddSingleton<IPostSlugService, PostSlugService>();
builder.Services.AddScoped<SeedDatabase>();
builder.Services.AddSingleton<IFileService>(x => new FileService(builder.Environment.WebRootPath));
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISearchRepository, SearchRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
// Configure the HTTP request pipeline.
app.ConfigureExceptionHandler(logger);
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}
if (app.Environment.IsProduction())
{
    app.UseHsts();
}
app.UseHttpsRedirection();
// Seed Database
SeedDatabase();
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
});
app.UseCors("CorsPolicy").UseCors("SignalRPolicy");
app.UseAuthentication();
app.UseAuthorization();
//app.UseResponseCaching();
//app.UseHttpCacheHeaders();
app.MapControllers();
app.MapHub<NotificationHub>("/notify");
app.Use(async (context, next) =>
{
    await next.Invoke();
});
app.Run();

void SeedDatabase()
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<SeedDatabase>();
    dbInitializer.Seed();
}