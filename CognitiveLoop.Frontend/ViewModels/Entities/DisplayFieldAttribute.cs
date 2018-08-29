using System;

namespace FluentTest.ViewModels.Entities
{
    public class DisplayFieldAttribute : Attribute
    {
        private bool isDisplayField;
        public DisplayFieldAttribute(bool isDisplay)
        {
            this.isDisplayField = isDisplay;
        }
    }
}