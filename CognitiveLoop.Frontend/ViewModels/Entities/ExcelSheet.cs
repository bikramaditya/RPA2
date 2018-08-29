namespace FluentTest.ViewModels.Entities
{
    using System.Collections.ObjectModel;

    public class ExcelSheet
    {
        public string ExcelSheetPath { get; set; }

        public ObservableCollection<string> SorkSheets { get; set; }

        public bool FirstRowContainsHeader { get; set; }
    }
}
