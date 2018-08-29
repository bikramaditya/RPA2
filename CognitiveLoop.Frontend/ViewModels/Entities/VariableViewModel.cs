namespace FluentTest.ViewModels.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public class VariableViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [DisplayField(true)]
        public string Name { get; set; }

        [DisplayField(true)]
        public string Key { get; set; }

        public int SelectedVarTypeIndex { get; set; }

        public string VarType { get; set; }

        [DisplayField(true)]
        public string SimpleValue { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public string EndPointURL { get; set; }

        public string HTTPMethod { get; set; }

        public string HTTPHeaders { get; set; }

        public string HTTPBody { get; set; }

        public string PythonScript { get; set; }
        
        public string AllowedChars { get; set; }

        [DisplayField(true)]
        public string Description { get; set; }

        public string IconPath
        {
            get
            {
                return "/Icons/variable.png";
            }

            set { }
        }

        private bool isExpanded;

        private bool isSelected;

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