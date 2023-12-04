using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<SocialMediaPostService>();
builder.Services.AddSingleton<SalePostService>();
builder.Services.AddSingleton<LikeService>();
builder.Services.AddSingleton<CommentService>();
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddTransient<ImageService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseStaticFiles();

app.Run();
