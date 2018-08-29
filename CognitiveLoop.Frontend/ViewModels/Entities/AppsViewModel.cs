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

    public class AppsViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name = string.Empty;

        [DisplayField(true)]
        public string Name {
            get
            {
                return _name;
            }

            set
            {
                if(value != null)
                { 
                    _name = value;
                }
            }
        }


        public ObservableCollection<string> _AppTypes = new ObservableCollection<string>();

        private string _Desc = string.Empty;

        [DisplayField(true)]
        public string Description
        {
            get
            {
                return _Desc;
            }

            set
            {
                if (value != null)
                {
                    _Desc = value;
                }
            }
        }

        public int SelectedIndex { get; set; }

        [DisplayField(true)]
        public ObservableCollection<string> AppTypes { get; set; }

        private string _appPath = string.Empty;

        [DisplayField(true)]
        public string AppPath
        {
            get
            {
                return _appPath;
            }

            set
            {
                if (value != null)
                {
                    _appPath = value;
                }
            }
        }

        public string IconPath
        {
            get
            {
                return "/Icons/app.png";
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