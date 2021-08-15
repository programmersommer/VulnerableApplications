using System;

namespace VulnerableDeserialization
{
    public class SwaggerTextBodyAttribute : Attribute
    {
        public SwaggerTextBodyAttribute()
        {
            MediaType = "text/plain";
        }

        public string MediaType { get; set; }
    }
}
