using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using DynamicXml;

namespace DynXml.Tests
{
    class when_building_xml
    {
        private Because of_conversion_model_into_xml = () => _xml = _builder.Compile();

        public class with_root_node_only
        {
            private Establish building = () =>
            {
                _builder = DynXML.Builder();
                _builder.root_node = "root node content";
            };

            private It should_convert_builder_to_xml_properly = 
                () => _xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>\r\n<root_node>root node content</root_node>");
        }

        public class with_nested_nodes
        {
            private Establish building = () =>
            {
                _builder = DynXML.Builder();

                _builder.persons.person(
                    new
                    {
                        name = "name1",
                        surname = "surname1"
                    },
                    new
                    {
                        name = "name2",
                        surname = "surname2"
                    });
            };

            private It should_convert_builder_to_xml_properly =
                () => _xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>\r\n"
                                     + "<persons>\r\n"
                                     + "  <person>\r\n"
                                     + "    <name>name1</name>\r\n"
                                     + "    <surname>surname1</surname>\r\n"
                                     + "  </person>\r\n"
                                     + "  <person>\r\n"
                                     + "    <name>name2</name>\r\n"
                                     + "    <surname>surname2</surname>\r\n"
                                     + "  </person>\r\n"
                                     + "</persons>");

            class adding_new_nested_node
            {
                private Establish new_node_added = () => _builder.persons.person(
                                                                new
                                                                {
                                                                    name = "name3",
                                                                    surname = "surname3"
                                                                });

                It should_add_new_node_to_Existing =
                    () => _xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>\r\n"
                                     + "<persons>\r\n"
                                     + "  <person>\r\n"
                                     + "    <name>name1</name>\r\n"
                                     + "    <surname>surname1</surname>\r\n"
                                     + "  </person>\r\n"
                                     + "  <person>\r\n"
                                     + "    <name>name2</name>\r\n"
                                     + "    <surname>surname2</surname>\r\n"
                                     + "  </person>\r\n"
                                     + "  <person>\r\n"
                                     + "    <name>name3</name>\r\n"
                                     + "    <surname>surname3</surname>\r\n"
                                     + "  </person>\r\n"
                                     + "</persons>");
            }

            class adding_new_nested_element_to_another_nested_node
            {
                private Establish new_element_added = () => _builder.persons.person[1].birth_year = "1980";

                It should_add_new_node_to_Existing =
                    () => _xml.ShouldEqual("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>\r\n"
                                         + "<persons>\r\n"
                                         + "  <person>\r\n"
                                         + "    <name>name1</name>\r\n"
                                         + "    <surname>surname1</surname>\r\n"
                                         + "  </person>\r\n"
                                         + "  <person>\r\n"
                                         + "    <name>name2</name>\r\n"
                                         + "    <surname>surname2</surname>\r\n"
                                         + "    <birth_year>1980</birth_year>\r\n"
                                         + "  </person>\r\n"
                                         + "</persons>");
            }
        }

        private static dynamic _builder;
        private static string _xml;
    }
}
