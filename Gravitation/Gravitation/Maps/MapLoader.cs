using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Gravitation.Maps
{
    class MapLoader
    {
        Map currentmap;

        public MapLoader(String fileName)
        {
            Stream mapXml= new FileStream(fileName,FileMode.Open);
            XmlSerializer serialiser = new XmlSerializer(typeof(Map));
            Map type = (Map)serialiser.Deserialize(mapXml);


        }
    }
}
