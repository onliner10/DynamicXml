using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;

namespace DynamicXml.Tests
{
    class when_parsing_xml_file_with_only_one_node
    {
        private Because of_parsing_xml = () => _document = DynXML.FromString("<body>working</body>");

        private It should_have_correct_node_value = () =>
        {
            string rootValue = _document.root;
            rootValue.ShouldEqual("working");
        };

        private static dynamic _document;
    }

    class when_parsing_xml_file_with_nested_nodes
    {
        private class single_nested_node
        {
            private Because of_parsing_xml = () => _document = DynXML.FromString("<body><p><b>text</b></p></body>");

            private It should_have_correct_node_value = () =>
            {
                string bold = _document.root.p.b;
                bold.ShouldEqual("text");
            };
        }

        private class many_nested_nodes
        {
            private Because of_parsing_xml = () => _document = DynXML.FromString("<body><p><b>text1</b><b>text2</b></p></body>");

            private It should_have_correct_nodes_values = () =>
            {
                string firstBold = _document.root.p.b[0];
                string secondBold = _document.root.p.b[1];

                firstBold.ShouldEqual("text1");
                secondBold.ShouldEqual("text2");
            };


        }

        private class when_trying_to_get_not_existing_node
        {
            private Because of_parsing_xml = () => _document = DynXML.FromString("<body><p><b>text1</b><b>text2</b></p></body>");

            private It should_throw_exception = () =>
            {
                var exception = Catch.Exception(() => _document.root.b);

                exception.ShouldBeOfExactType(typeof(ArgumentException));
            };
        }

        private static DynamicXml.DynXML _document;
    }
}
