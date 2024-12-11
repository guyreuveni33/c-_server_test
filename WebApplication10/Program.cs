using WebApplication10.Repositories;
using WebApplication10.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IPostRepository, PostRepositories>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddHttpClient<IPostRepository, PostRepositories>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
// Add services to the container
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDb");
    return new MongoClient(connectionString);
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var postService = scope.ServiceProvider.GetRequiredService<IPostService>();

    await postService.InsertFetchedPostsToMongoAsync();
}

app.UseCors(); // Applies the default CORS policy

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers(); // Map your controller routes

app.Run();