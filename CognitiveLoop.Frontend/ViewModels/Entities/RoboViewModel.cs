namespace FluentTest.ViewModels.Entities
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq.Expressions;
    using System.Security.Permissions;
    using System.Xml;
    using System.Xml.Serialization;

    public class RoboViewModel : INotifyPropertyChanged
    {
        private static object _SelectedNode;

        private static RoboViewModel _RoboViewModel = null;

        public event PropertyChangedEventHandler PropertyChanged;

        private static ObservableCollection<TestSuitViewModel> _TestSuitCollection;

        public RoboViewModel()
        {
            _TestSuitCollection = new ObservableCollection<TestSuitViewModel>();
        }

        internal static RoboViewModel GetInstance()
        {
            if (_RoboViewModel == null)
            {
                _RoboViewModel = ReadXMLFromFile(App.xmlSource);
            }
            return _RoboViewModel;
        }
        
        private static RoboViewModel ReadXMLFromFile(string xmlSource)
        {
            RoboViewModel robo = new RoboViewModel();
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(RoboViewModel), App.types);
                TextReader reader = new StreamReader(xmlSource);
                object obj = deserializer.Deserialize(reader);
                robo = (RoboViewModel)obj;
                reader.Close();
            }
            catch (Exception) { }

            return robo;
        }

        private static void PupulateTree(string xmlSource)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlSource);

            ObservableCollection<TestSuitViewModel> testSuits = new ObservableCollection<TestSuitViewModel>();

            XmlNode suitNodes = doc.SelectSingleNode("/Suits");

            foreach (XmlNode suit in suitNodes.ChildNodes)
            {
                TestSuitViewModel testSuit = new TestSuitViewModel();

                testSuit.Name = suit["Name"].InnerText;
                testSuit.Description = suit["Description"].InnerText;

                foreach (XmlNode caseNode in suit.SelectNodes("Sequence"))
                {
                    TestSequenceViewModel testCase = new TestSequenceViewModel();
                    testCase.Name = caseNode["Name"].InnerText;
                    testCase.Description = caseNode["Description"].InnerText;
                    foreach (XmlNode stepNode in caseNode.SelectNodes("Step"))
                    {
                        TestStepViewModel testStep = new TestStepViewModel();
                        testStep.Id = stepNode["Id"].InnerText;
                        testStep.Name = stepNode["Name"].InnerText;
                        testStep.Description = stepNode["Conference"].InnerText;
                        testStep.Price = stepNode["Price"].InnerText;

                        testCase.TestStepCollection.Add(testStep);
                    }

                    testSuit.AllCollection.Add(testCase);
                }

                foreach (XmlNode caseNode in suit.SelectNodes("App"))
                {
                    AppsViewModel apps = new AppsViewModel();
                    apps.Name = caseNode["Name"].InnerText;
                    apps.Description = caseNode["Description"].InnerText;

                    testSuit.AllCollection.Add(apps);
                }

                foreach (XmlNode caseNode in suit.SelectNodes("DataSource"))
                {
                    DataSourceViewModel apps = new DataSourceViewModel();
                    apps.Name = caseNode["Name"].InnerText;
                    apps.Description = caseNode["Description"].InnerText;

                    testSuit.AllCollection.Add(apps);
                }

                foreach (XmlNode caseNode in suit.SelectNodes("Variable"))
                {
                    VariableViewModel variables = new VariableViewModel();
                    variables.Name = caseNode["Name"].InnerText;
                    variables.Description = caseNode["Description"].InnerText;

                    testSuit.AllCollection.Add(variables);
                }

                testSuits.Add( testSuit);
            }

            _TestSuitCollection = testSuits;
        }

        public ObservableCollection<TestSuitViewModel> TestSuitCollection
        {
            get
            {
                return _TestSuitCollection;
            }

            set
            {
                if (_TestSuitCollection != value)
                {
                    _TestSuitCollection = value;
                    this.RaisePropertyChanged(() => this.TestSuitCollection);
                }
            }
        }

        public object SelectedNode
        {
            get { return _SelectedNode; }

            set
            {
                if (_SelectedNode != value)
                {
                    _SelectedNode = value;
                    this.RaisePropertyChanged(() => this.SelectedNode);
                }
            }
        }

        #region Event Handlers

        /// <summary>
        /// Get name of property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }
        /// <summary>
        /// Raise when property value propertychanged or override propertychage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        protected virtual void RaisePropertyChanged<T>
            (Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(GetPropertyName(propertyExpression));
        }
        /// <summary>
        /// Raise when property value propertychanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}