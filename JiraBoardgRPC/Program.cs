using JiraBoardgRPC.FakeDataBase;
using JiraBoardgRPC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddLogging();
builder.Services.AddScoped<IDataBaseService, FakeDatabase>();
builder.Services.AddScoped<JiraBoardService>();


var app = builder.Build();
app.MapGrpcService<JiraBoardService>();


// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
