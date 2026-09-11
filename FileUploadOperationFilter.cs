using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi;

using Swashbuckle.AspNetCore.SwaggerGen;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.MethodInfo
            .GetParameters()
            .Where(p => IsFileUpload(p.ParameterType))
            .ToList();

        if (fileParameters.Count == 0)
            return;

        var properties = new Dictionary<string, IOpenApiSchema>();

        foreach (var parameter in fileParameters)
        {
            properties[parameter.Name ?? "files"] = new OpenApiSchema
            {
                Type = JsonSchemaType.Array,
                Items = new OpenApiSchema
                {
                    Type = JsonSchemaType.String,
                    Format = "binary"
                }
            };
        }

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = JsonSchemaType.Object,
                        Properties = properties,
                        Required = new HashSet<string>(properties.Keys)
                    }
                }
            }
        };
    }

    private static bool IsFileUpload(Type type) =>
        type == typeof(IFormFile)
        || type == typeof(IFormFileCollection)
        || typeof(IEnumerable<IFormFile>).IsAssignableFrom(type);
}