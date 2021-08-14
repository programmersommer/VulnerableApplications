
namespace VulnerableDeserialization.Models
{
    public class SomeDummyObject
    {
        /// <example>
        ///     {
        ///       "$type": "System.IO.FileInfo, System.IO.FileSystem",
        ///       "fileName": "appsettings.json",
        ///       "IsReadOnly": true
        ///      }
        /// </example>
        public dynamic obj { get; set; }
    }
}
