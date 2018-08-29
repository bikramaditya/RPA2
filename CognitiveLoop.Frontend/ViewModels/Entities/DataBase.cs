using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentTest.ViewModels.Entities
{
    public class DataBase
    {
        public int SelectedDataBaseTypeIndex;

        private ObservableCollection<string> _DatabaseTypes = new ObservableCollection<string>();

        public string SelectedDataBaseType { get; set; }

        public ObservableCollection<string> DatabaseTypes
        {
            get
            {
                return _DatabaseTypes;
            }

            set
            {
                if (this._DatabaseTypes != value)
                {
                    _DatabaseTypes = value;
                }
            }
        }
        public string DatabaseHost { get; set; }
        public string DatabasePort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Query { get; set; }
    }
}
