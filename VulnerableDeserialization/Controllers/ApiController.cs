﻿using Microsoft.AspNetCore.Mvc;
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
        ///  And it is related to NewtonsoftJson    
        ///  Works only if options.SerializerSettings.TypeNameHandling = TypeNameHandling.All added to startup.cs  
        /// </remarks>
        /// <param name="param"></param>
        /// <response code="200">Just returns Ok</response>
        [HttpPost]
        public IActionResult Deserialize([FromBody] SomeDummyObject param)
        {

            return Ok();
        }


        /*
        <?xml version = "1.0" ?>
        <!DOCTYPE foo[<!ELEMENT foo ANY>
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

        /// <summary>
        /// XXE demo. By default .NET Core is not vulnerable to this attack
        /// </summary>
        [HttpPost]
        [SwaggerTextBody]
        public async Task<IActionResult> ReadXML()
        {
            var xml = "";
            using (StreamReader streamReader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                xml = await streamReader.ReadToEndAsync();
            }

            XmlReaderSettings settings = new XmlReaderSettings();
            // If you have DtdProcessing set to Parse then you can get DoS attack
            settings.DtdProcessing = DtdProcessing.Parse;

            // If you need parsing and want to be protected from DoS change MaxCharactersFromEntities
            // by default MaxCharactersFromEntities is set to 10000000 - it's too much
            //settings.MaxCharactersFromEntities = 1000; 

            // That's really very dangerous because allows to get any file content
            settings.XmlResolver = new CustomXmlResolver();

            XmlReader reader = XmlReader.Create(new StringReader(xml), settings);

            var result = new StringBuilder();

            while (reader.Read())
            {
                result.Append(reader.Value);
            }

            return Ok(result.ToString());
        }

    }
}
