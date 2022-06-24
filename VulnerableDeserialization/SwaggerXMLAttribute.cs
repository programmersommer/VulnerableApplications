using System;

namespace VulnerableDeserialization
{
    public class SwaggerXMLAttribute : Attribute
    {
        public SwaggerXMLAttribute()
        {
            MediaType = "text/plain";
        }

        public string MediaType { get; set; }
    }
}
