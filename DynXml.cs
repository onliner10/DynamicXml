using System;
using System.Dynamic;
using System.Xml.Linq;

namespace DynamicXml
{
    public class DynXML : DynamicObject
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

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder.Name == _content.Root.Name)
                result = new DynamicNode(_content.Root);
            else
                throw new ArgumentException("No node named " + binder.Name);
                
            return true;
        }

        public dynamic root
        {
            get { return new DynamicNode(_content.Root); }
        }
    }
}
