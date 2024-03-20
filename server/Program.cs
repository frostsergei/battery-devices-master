using NSwag;

const string corsPolicy = "cors";

var app = CreateBuilder().Build();

app.UseCors(corsPolicy);
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.MapControllers();
app.MapFallbackToFile("/index.html");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.Run();

return;

WebApplicationBuilder CreateBuilder()
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

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
        options.AddPolicy(
            name: corsPolicy,
            configurePolicy: corsBuilder =>
            {
                corsBuilder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    return builder;
}
