namespace FluentTest.ViewModels.Entities
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using Helpers;

    public class TestSuitViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _Name;
        private bool isExpanded;
        private bool isSelected;
        private ICommand _ShowInfoCommand;
        private ObservableCollection<object> _AllCollection = new ObservableCollection<object>();

        [DisplayField(true)]
        public string Description { get; set; }

        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }

            set
            {
                isExpanded = value;
                this.RaisePropertyChanged(() => this.IsExpanded);
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                this.RaisePropertyChanged(() => this.IsSelected);
            }
        }

        [DisplayField(true)]
        public string Name
        {
            get
            {
                return this._Name;
            }

            set
            {
                if (this._Name != value)
                {
                    this._Name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        public ObservableCollection<object> AllCollection
        {
            get
            {
                return this._AllCollection;
            }

            set
            {
                if (this._AllCollection != value)
                {
                    this._AllCollection = value;
                    this.RaisePropertyChanged(() => this.AllCollection);
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
