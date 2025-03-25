using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sample.Commons.Abstracts;
using Sample.Configuration.Authentication;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Sample.Configuration
{
    public static class ConfigureSwaggerExtention
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Your API Title",
                    Description = "Your API Description"
                });

                c.AddSecurityDefinition(SampleTokenAuthenticationSchemeOptions.SchemeName,
                 SampleTokenAuthenticationSchemeOptions.GetSwaggerCustomTokenApiSecurityScheme());
                c.AddSecurityRequirement(SampleTokenAuthenticationSchemeOptions.GetSwaggerCustomTokenSecurityRequirement());
                c.OperationFilter<IgnorePropertyFilter>();
                c.SchemaFilter<FieldsSchemaFilter>();
            }).AddSwaggerGenNewtonsoftSupport();
        }
    }

    public class FieldsSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            var skipProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>() != null);

            foreach (var skipProperty in skipProperties)
            {
                var propertyToSkip = schema.Properties.Keys.SingleOrDefault(x => string.Equals(x, skipProperty.Name, StringComparison.OrdinalIgnoreCase));

                if (propertyToSkip != null)
                {
                    schema.Properties.Remove(propertyToSkip);
                }
            }
        }
    }

    public class IgnorePropertyFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var name = context.MethodInfo.Name;

            var ignoredProperties = context.MethodInfo.GetParameters()
                            .SelectMany(p => p.ParameterType.GetProperties()
                            .Where(prop => prop.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>() != null))
                            .ToList();

            var excludedProperties = context.ApiDescription.ParameterDescriptions.Where(p =>
                                  p.Source.Equals(BindingSource.Form));

            if (excludedProperties.Any())
            {
                excludedProperties = excludedProperties.Where(prop =>
                   prop.Name.Contains(nameof(MetaData))).ToList();

                foreach (var excludedPropertie in excludedProperties)
                {
                    for (int i = 0; i < operation.RequestBody.Content.Values.Count; i++)
                    {
                        for (int j = 0; j < operation.RequestBody.Content.Values.ElementAt(i).Encoding.Count; j++)
                        {
                            if (operation.RequestBody.Content.Values.ElementAt(i).Encoding.ElementAt(j).Key ==
                                excludedPropertie.Name)
                            {
                                operation.RequestBody.Content.Values.ElementAt(i).Encoding
                                    .Remove(operation.RequestBody.Content.Values.ElementAt(i).Encoding
                                        .ElementAt(j));
                                operation.RequestBody.Content.Values.ElementAt(i).Schema.Properties.Remove(excludedPropertie.Name);


                            }
                        }
                    }
                }

            }

            if (!ignoredProperties.Any()) return;

            foreach (var property in ignoredProperties)
            {
                operation.Parameters = operation.Parameters
                    .Where(p => (!p.Name.Contains(property.Name, StringComparison.InvariantCulture)))
                    .ToList();
            }
        }
    }
}
