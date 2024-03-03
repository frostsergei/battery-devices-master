using NSwag;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument(options =>
    options.PostProcess = document =>
    {
        document.Info = new OpenApiInfo()
        {
            Title = "Battery devices master",
            Version = "v1",
            Description = "API for battery devices master.",
            Contact = new OpenApiContact()
            {
                Name = "Sergey Morozov",
                Url = "https://github.com/frostsergei",
            },
        };
    });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder
            .WithOrigins(builder.Configuration["ClientUrl"] ?? "*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.MapControllers();
app.Map("/", context => Task.Run(() => context.Response.Redirect("/swagger")));

app.Run();