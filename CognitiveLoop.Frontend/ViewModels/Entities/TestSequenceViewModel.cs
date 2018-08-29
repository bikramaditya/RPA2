﻿namespace FluentTest.ViewModels.Entities
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq.Expressions;

    public class TestSequenceViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<TestStepViewModel> _testStepCollection = new ObservableCollection<TestStepViewModel>();
        private string _Name;

        [DisplayField(true)]
        public string Description { get; set; }

        public string IconPath
        {
            get
            {
                return "/Icons/sequence.png";
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

        public ObservableCollection<TestStepViewModel> TestStepCollection
        {
            get
            {
                return this._testStepCollection;
            }

            set
            {
                if (this._testStepCollection != value)
                {
                    this._testStepCollection = value;
                    this.RaisePropertyChanged(() => this.TestStepCollection);
                }
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
