using System;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace DynXml
{
    public class DynamicNode : DynamicObject
    {
        private readonly XElement _currentElement;

        public DynamicNode(XElement currentElement)
        {
            _currentElement = currentElement;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var nodes = _currentElement.Elements().Where(e => e.Name == binder.Name);

            if (nodes.Count() > 1)
                result = nodes.Select(n => new DynamicNode(n)).ToArray();
            else if (nodes.Count() == 1)
                result = new DynamicNode(nodes.First());
            else
                throw new ArgumentException("No node named " + binder.Name);

            return true;
        }

        public static implicit operator string(DynamicNode dynamicNode)
        {
            return dynamicNode.ToString();
        }

        public override string ToString()
        {
            return _currentElement.Value;
        }
    }
}