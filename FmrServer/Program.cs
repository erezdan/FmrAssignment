using FmrServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Register ShareService as singleton
builder.Services.AddSingleton<ShareService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
