using Blog.Domain.Enums;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Blog.Api.Config;

public class SwaggerConfig
{
    private readonly IServiceCollection _services;

    public SwaggerConfig(IServiceCollection services)
    {
        _services = services;
        
        AddSwagger();
    }

    private void AddSwagger()
    {
        _services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
    
            c.MapType<PostFilter>(() => new OpenApiSchema
            {
                Type = "string",
                Enum = Enum.GetNames(typeof(PostFilter))
                    .Select(n => new OpenApiString(n))
                    .Cast<IOpenApiAny>()
                    .ToList()
            });
    
            c.MapType<UserPostsFilter>(() => new OpenApiSchema
            {
                Type = "string",
                Enum = Enum.GetNames(typeof(UserPostsFilter))
                    .Select(n => new OpenApiString(n))
                    .Cast<IOpenApiAny>()
                    .ToList()
            });
        });
    }
}