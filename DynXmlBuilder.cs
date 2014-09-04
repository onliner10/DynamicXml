using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DynamicXml
{
    public class DynXmlBuilder : DynamicObject
    {
        private readonly XElement _parentElement;

        public DynXmlBuilder(XElement parentElement = null)
        {
            _parentElement = parentElement;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_parentElement != null && _parentElement.Elements(binder.Name).Any())
            {
                result = _parentElement.Elements(binder.Name).Select(e => new DynXmlBuilder(e)).ToArray();
            }
            else
            {
                result = new DynXmlBuilder(GetElement(binder.Name));    
            }

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var currentElement = GetElement(binder.Name);
            currentElement.SetValue(value);

            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            foreach (var entity in args)
            {
                var childElements = entity.GetType().GetProperties();
                var entityElement = new XElement(binder.Name);

                foreach (var childElement in childElements)
                {
                    var newChild = new XElement(childElement.Name);
                    newChild.SetValue(childElement.GetValue(entity));

                    entityElement.Add(newChild);
                }

                _parentElement.Add(entityElement);
            }
            
            result = new DynXmlBuilder(_parentElement);

            return true;
        }

        private XElement _currentElement;
        private XElement GetElement(string name)
        {
            if (_currentElement == null)
            {
                _currentElement = new XElement(name);
                if (_parentElement != null)
                    _parentElement.Add(_currentElement);
            }

            return _currentElement;
        }

        public string Compile()
        {
            var resultDocument = new XDocument(new XDeclaration("1.0", "utf-8", "no"));
            resultDocument.Add(_currentElement);

            return new StringBuilder()
                .AppendLine(resultDocument.Declaration.ToString())
                .Append(resultDocument)
                .ToString();
        }
    }
}