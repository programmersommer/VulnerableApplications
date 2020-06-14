using System;
using System.IO;
using System.Xml;

namespace VulnerableDeserialization.Helpers
{
    public class CustomXmlResolver : XmlResolver
    {
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            return new FileStream(absoluteUri.AbsolutePath, FileMode.Open, System.IO.FileAccess.Read);
        }
    }
}
