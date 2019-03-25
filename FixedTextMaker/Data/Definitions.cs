using FixedTextMaker.Model;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace FixedTextMaker.Data
{
    class Definitions
    {
        /// <summary>
        /// 定義ファイルを読み込みます
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FixedTextDefines Load( string path )
        {
            var xmlSerializer = new XmlSerializer(typeof(FixedTextDefines));
            var xmlSettings = new System.Xml.XmlReaderSettings();
            using (var streamReader = new StreamReader(path, Encoding.UTF8))
            using (var xmlReader = System.Xml.XmlReader.Create(streamReader, xmlSettings))
            {
                var result = xmlSerializer.Deserialize(xmlReader) as FixedTextDefines;
                return result;
            }
        }
    }
}
