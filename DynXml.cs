using System.Xml.Linq;

namespace DynamicXml
{
    public class DynXML
    {
        public static DynXML FromString(string content)
        {
            return new DynXML(XDocument.Parse(content));
        }

        public static dynamic Builder()
        {
            return new DynXmlBuilder();
        }

        private readonly XDocument _content;

        private DynXML(XDocument content)
        {
            _content = content;
        }

        public dynamic root
        {
            get { return new DynamicNode(_content.Root); }
        }
    }
}
