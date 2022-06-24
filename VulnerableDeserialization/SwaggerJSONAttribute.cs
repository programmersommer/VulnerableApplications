using System;

namespace VulnerableDeserialization
{
    public class SwaggerJSONAttribute : Attribute
    {
        public SwaggerJSONAttribute()
        {
            MediaType = "text/plain";
        }

        public string MediaType { get; set; }
    }
}
