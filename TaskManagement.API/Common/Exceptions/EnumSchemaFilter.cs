using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManagement.API.Common.Exceptions
{

    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum)
                return;

            schema.Enum.Clear();

            var enumValues = Enum.GetValues(context.Type);

            foreach (var value in enumValues)
            {
                schema.Enum.Add(new OpenApiInteger((int)value));
            }

            schema.Description += "<br/><br/>Allowed values:<br/>";

            foreach (var value in enumValues)
            {
                schema.Description +=
                    $"{(int)value} = {Enum.GetName(context.Type, value)}<br/>";
            }
        }
    }
}
