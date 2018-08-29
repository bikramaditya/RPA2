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
    using System.Windows.Controls;

    public class DataSourceViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<DataSource> _SourceTypes = new ObservableCollection<DataSource>();

        [DisplayField(true)]
        public string Name { get; set; }

        [DisplayField(true)]
        public string Description { get; set; }

        public int SelectedIndex { get; set; }
        
        [DisplayField(true)]
        public ObservableCollection<DataSource> SourceTypes
        {
            get
            {
                return _SourceTypes;
            }

            set
            {
                if (this._SourceTypes != value)
                {
                    _SourceTypes = value;
                    this.RaisePropertyChanged(() => this.SourceTypes);
                }
            }
        }

        public object SelectedDataSource { get; set; }
        
        public string IconPath
        {
            get
            {
                return "/Icons/data_source.png";
            }

            set { }
        }
        
        private bool isSelected;
        
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