using FluentTest.ViewModels.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FluentTest.Helpers
{
    public class ROBOXmlSerializer : XmlSerializer
    {
        private XmlTextWriter writer = null;

        public bool Serialize(XmlTextWriter w, RoboViewModel robo)
        {
            this.writer = w;

            Type suitCollectionType = new ObservableCollection<TestSuitViewModel>().GetType();
            Type sequenceCollectionType = new ObservableCollection<TestSequenceViewModel>().GetType();
            Type stepCollectionType = new ObservableCollection<TestStepViewModel>().GetType();
            Type appsCollectionType = new ObservableCollection<AppsViewModel>().GetType();
            Type dataSourceCollectionType = new ObservableCollection<DataSourceViewModel>().GetType();
            Type variableCollectionType = new ObservableCollection<VariableViewModel>().GetType();
            Type stringType = string.Empty.GetType();
            Type boolType = bool.TrueString.GetType();
            Type intType = int.MinValue.GetType();
            Type doubleType = double.MinValue.GetType();
            Type suitType = new TestSuitViewModel().GetType();
            Type seqType = new TestSequenceViewModel().GetType();
            Type stepType = new TestStepViewModel().GetType();
            Type appType = new AppsViewModel().GetType();
            Type dsType = new DataSourceViewModel().GetType();
            Type varType = new VariableViewModel().GetType();

            Type[] types = { suitCollectionType, sequenceCollectionType, stepCollectionType, appsCollectionType,
                dataSourceCollectionType, variableCollectionType, stringType, boolType, intType, doubleType,
                suitType, seqType, stepType, appType, dsType, varType};

            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("Suits");

            AppendNodeRobo(types, robo);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            return true;
        }

        private void AppendNodeRobo(Type[] types, RoboViewModel robo)
        {
            PropertyInfo[] listProperties = robo.GetType().GetProperties();
            foreach (var property in listProperties)
            {
                string nodeName = property.Name;
                writer.WriteStartElement(nodeName);
                var valueObj = property.GetValue(robo);

                if (valueObj != null)
                {
                    Type returnType = property.GetValue(robo).GetType();

                    foreach (Type type in types)
                    {
                        if (type == returnType)
                        {
                            var castedObject = Convert.ChangeType(valueObj, type);
                            Type list = this.isEnumerableType(type);
                            if (list != null)
                            {
                                IEnumerable enumerable = (IEnumerable)castedObject;
                                foreach (object item in enumerable)
                                {
                                    string className = item.GetType().Name;
                                    writer.WriteStartElement(className);

                                    AppendNodeSuits(types, item);

                                    writer.WriteEndElement();
                                }
                            }
                        }
                    }
                }
                else
                {
                    string value = "";
                    //XMLElement element = CreateXMLNode(nodeName, value, null);
                }

                writer.WriteEndElement();
            }
        }

        private void AppendNodeSuits(Type[] types, object item)
        {
            //TestSuitViewModel suit = (TestSuitViewModel)item;
            PropertyInfo[] listProperties = item.GetType().GetProperties();
            foreach (var property in listProperties)
            {
                string nodeName = property.Name;
                writer.WriteStartElement(nodeName);
                var valueObj = property.GetValue(item);

                if (valueObj != null)
                {
                    Type returnType = property.GetValue(item).GetType();

                    foreach (Type type in types)
                    {
                        if (type == returnType)
                        {
                            var castedObject = Convert.ChangeType(valueObj, type);
                            Type list = this.isEnumerableType(type);
                            if (list != null)
                            {
                                IEnumerable enumerable = (IEnumerable)castedObject;
                                foreach (object localItem in enumerable)
                                {
                                    string className = localItem.GetType().Name;
                                    writer.WriteStartElement(className);

                                    AppendNodeSuits(types, localItem);

                                    writer.WriteEndElement();
                                }
                            }
                        }
                    }
                }
                else
                {
                    string value = "";
                    //XMLElement element = CreateXMLNode(nodeName, value, null);
                }

                writer.WriteEndElement();
            }
        }

        Type isEnumerableType(Type type)
        {
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return intType.GetGenericArguments()[0];
                }
            }
            return null;
        }
    }
}
