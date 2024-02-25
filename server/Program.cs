using NSwag;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddOpenApiDocument(options =>
        {
            options.PostProcess = document =>
            {
                document.Info = new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Dummy API",
                    Description = "Wow. Such api. Many endpoints. Very cool.",
                    TermsOfService = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&pp=ygUXbmV2ZXIgZ29ubmEgZ2l2ZSB5b3UgdXA%3D",
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&pp=ygUXbmV2ZXIgZ29ubmEgZ2l2ZSB5b3UgdXA%3D"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&pp=ygUXbmV2ZXIgZ29ubmEgZ2l2ZSB5b3UgdXA%3D"
                    }
                };
            };
        });

        // Add CORS support
        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader();
                // For Angular development
                policy.AllowAnyMethod().WithOrigins("http://localhost:4200");
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Add OpenAPI 3.0 document serving middleware
            // Available at: http://localhost:<port>/swagger/v1/swagger.json
            app.UseOpenApi();
            // Add web UIs to interact with the document
            // Available at: http://localhost:<port>/swagger
            app.UseSwaggerUi();
        }

        app.UseStaticFiles();
        app.UseCors("CorsPolicy");

        app.MapControllers();
        app.Run();
    }
}