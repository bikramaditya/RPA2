namespace FluentTest.Helpers
{
    using System;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Xps.Packaging;
    using ExcelDataReader;
    using FluentTest.ViewModels.Entities;
    using Microsoft.Win32;

    public class DataSourceDealer : EntityDealer
    {
        private static TreeView testCaseTreeView;
        private static Grid main_grid;
        private static Grid child_grid;
        private static DataSet result = null;
        private static Grid help_grid;

        internal static void DealWithDataSourceViewModel(DataSourceViewModel TSModel, Grid maingrid, Grid childGrid, Grid support_grid, TreeView TestCaseTreeView)
        {
            main_grid = maingrid;
            child_grid = childGrid;
            help_grid = support_grid;
            testCaseTreeView = TestCaseTreeView;

            string name = TSModel.Name;
            string desc = TSModel.Description;

            int SelectedIndex = TSModel.SelectedIndex;

            if (TSModel.SourceTypes.Count == 0)
            {
                TSModel.SourceTypes.Add(new DataSource("Database"));
                TSModel.SourceTypes.Add(new DataSource("Excel Sheet"));
                TSModel.SourceTypes.Add(new DataSource("CSV File"));
            }

            ObservableCollection<DataSource> SourceTypes = TSModel.SourceTypes;

            Label labelName = new Label();
            labelName.Content = "Name";
            addToGrid(main_grid, labelName, 0, true);

            TextBox txtName = new TextBox();
            txtName.Text = name;

            addToGrid(main_grid, txtName, 1, false);


            Label labelDesc = new Label();
            labelDesc.Content = "Description";
            addToGrid(main_grid, labelDesc, 0, true);

            TextBox txtDesc = new TextBox();
            txtDesc.TextWrapping = TextWrapping.Wrap;
            txtDesc.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            txtDesc.MinHeight = 70;
            txtDesc.Text = desc;
            addToGrid(main_grid, txtDesc, 1, false);

            Label labelDS = new Label();
            labelDS.Content = "Data Source Type";
            addToGrid(main_grid, labelDS, 0, true);

            ComboBox comboDS = new ComboBox();
            comboDS.SelectionChanged += new SelectionChangedEventHandler(ComboboxSelectionChnaged);

            Binding bind = new Binding();

            bind.Source = SourceTypes;
            comboDS.DisplayMemberPath = "Name";
            comboDS.SetBinding(ComboBox.ItemsSourceProperty, bind);

            addToGrid(main_grid, comboDS, 1, false);

            comboDS.SelectedIndex = SelectedIndex;

            main_grid.LostFocus += new RoutedEventHandler(DSMainGridDetailsEdited);
        }

        private static void DSMainGridDetailsEdited(object sender, RoutedEventArgs e)
        {
            object obj = testCaseTreeView.SelectedItem;
            if (!obj.GetType().Name.Equals("DataSourceViewModel"))
            {
                return;
            }

            bool anyChange = false;

            var children = main_grid.Children;

            foreach (var child in children)
            {
                object element = ((Border)child).Child;
                Type type = element.GetType();

                if (type.Name.Equals("TextBox"))
                {
                    if (((TextBox)element).Name.Equals("Name"))
                    {
                        if (!((TestStepViewModel)obj).Name.Equals(((TextBox)element).Text))
                        {
                            ((TestStepViewModel)obj).Name = ((TextBox)element).Text;
                            anyChange = true;
                        }
                    }
                    else if (((TextBox)element).Name.Equals("Description"))
                    {
                        if (((TestStepViewModel)obj).Description.Equals(((TextBox)element).Text))
                        {
                            ((TestStepViewModel)obj).Description = ((TextBox)element).Text;
                            anyChange = true;
                        }
                    }
                }
            }

            if (anyChange)
            {
                testCaseTreeView.Items.Refresh();
            }
        }

        private static void ComboboxSelectionChnaged(object sender, SelectionChangedEventArgs e)
        {
            DataSourceViewModel DSViewModel = (DataSourceViewModel)testCaseTreeView.SelectedItem;

            ComboBox combo = (ComboBox)sender;
            DataSource ds = (DataSource)combo.SelectedValue;
            DSViewModel.SelectedIndex = combo.SelectedIndex;

            if (ds.Name.Equals("Database"))
            {
                DealWithDataBase(DSViewModel.SelectedDataSource);
            }
            if (ds.Name.Equals("Excel Sheet"))
            {
                DealWithExcelSheet(DSViewModel.SelectedDataSource);
            }
        }

        private static void DealWithExcelSheet(object CurrentDB)
        {
            child_grid.Children.Clear();
            child_grid.RowDefinitions.Clear();

            ExcelSheet currentExcelSheet = null;

            if (CurrentDB == null)
            {
                currentExcelSheet = new ExcelSheet();
            }
            else
            {
                try
                {
                    currentExcelSheet = (ExcelSheet)CurrentDB;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    currentExcelSheet = new ExcelSheet();
                }
            }

            Label labelHost = new Label();
            labelHost.Content = "Excel Sheet Path";
            addToGrid(child_grid, labelHost, 0, true);

            TextBox txtPath = new TextBox();
            txtPath.Name = "ExcelSheetPath";
            txtPath.IsEnabled = false;
            txtPath.Text = currentExcelSheet.ExcelSheetPath;
            addToGrid(child_grid, txtPath, 1, false);

            Button browse = new Button();
            browse.Content = "Browse";
            browse.Click += Browse_Click;
            addToGrid(child_grid, browse, 2, false);


            Label labelIsHeader = new Label();
            labelIsHeader.Content = "Does 1st row contain header?";
            addToGrid(child_grid, labelIsHeader, 0, true);

            CheckBox chkHead = new CheckBox();
            chkHead.Name = "IsFirstRowHeader";
            chkHead.IsChecked = currentExcelSheet.FirstRowContainsHeader;
            addToGrid(child_grid, chkHead, 1, false);

            populateExcel(txtPath.Text);

            child_grid.LostFocus += new RoutedEventHandler(ExcelSheetDetailsEdited);
        }

        private static void populateExcel(string path)
        {
            help_grid.RowDefinitions.Clear();
            help_grid.Children.Clear();

            if (path != null && File.Exists(path))
            {
                Label worksheets = new Label();
                worksheets.Content = "WorkSheets";

                ComboBox excelTabs = new ComboBox();
                excelTabs.Items.Clear();
                excelTabs.SelectionChanged += new SelectionChangedEventHandler(ExcelTabSelected);

                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        result = reader.AsDataSet();
                        foreach (DataTable dt in result.Tables)
                        {
                            excelTabs.Items.Add(dt.TableName);
                        }
                    }
                }
                addToGrid(help_grid, worksheets, 0, true, 40);
                addToGrid(help_grid, excelTabs, 0, true, 40);
                excelTabs.SelectedIndex = 0;
            }
        }

        private static void ExcelTabSelected(object sender, SelectionChangedEventArgs e)
        {
            Label preview = new Label();
            preview.Content = "Excel Preview";
            addToGrid(help_grid, preview, 0, true, 40);

            DataGrid excelDataGrid = new DataGrid();
            excelDataGrid.ItemsSource = result.Tables[((ComboBox)sender).SelectedIndex].DefaultView;

            double h = help_grid.ActualHeight;

            DockPanel dockPanel = new DockPanel();
            dockPanel.Children.Add(excelDataGrid);

            addToGrid(help_grid, dockPanel, 0, true, (int)(h - 115));
        }

        private static void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".xlsx";
            openFileDialog.Filter = "Excel files (.xlsx)|*.xlsx";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filename = openFileDialog.FileName;

                var children = child_grid.Children;

                foreach (var child in children)
                {
                    object element = ((Border)child).Child;
                    Type type = element.GetType();

                    if (type.Name.Equals("TextBox"))
                    {
                        if (((TextBox)element).Name.Equals("ExcelSheetPath"))
                        {
                            ((TextBox)element).Text = filename;
                            populateExcel(filename);

                        }
                    }
                }
            }
        }

        private static void ExcelSheetDetailsEdited(object sender, RoutedEventArgs e)
        {
            if (!testCaseTreeView.SelectedItem.GetType().Name.Equals("DataSourceViewModel"))
            {
                return;
            }
            DataSourceViewModel datasourceVM = (DataSourceViewModel)testCaseTreeView.SelectedItem;

            ExcelSheet excel = null;

            if (datasourceVM.SelectedDataSource == null || datasourceVM.SelectedDataSource.GetType().Name != "ExcelSheet")
            {
                excel = new ExcelSheet();
            }
            else
            {
                excel = (ExcelSheet)datasourceVM.SelectedDataSource;
            }

            var children = child_grid.Children;

            foreach (var child in children)
            {
                object element = ((Border)child).Child;
                Type type = element.GetType();

                if (type.Name.Equals("TextBox"))
                {
                    if (((TextBox)element).Name.Equals("ExcelSheetPath"))
                    {
                        excel.ExcelSheetPath = ((TextBox)element).Text;
                    }
                }
                else if (type.Name.Equals("CheckBox"))
                {
                    if (((CheckBox)element).Name.Equals("IsFirstRowHeader"))
                    {
                        excel.FirstRowContainsHeader = ((CheckBox)element).IsChecked.Value;
                    }
                }
            }

            ((DataSourceViewModel)testCaseTreeView.SelectedItem).SelectedDataSource = excel;
        }

        private static void DealWithDataBase(object CurrentDB)
        {
            child_grid.Children.Clear();
            child_grid.RowDefinitions.Clear();
            DataBase CurrentDataBase = null;

            if (CurrentDB == null)
            {
                CurrentDataBase = new DataBase();
            }
            else
            {
                try
                {
                    CurrentDataBase = (DataBase)CurrentDB;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    CurrentDataBase = new DataBase();
                }
            }

            if (CurrentDataBase.DatabaseTypes.Count == 0)
            {
                CurrentDataBase.DatabaseTypes.Add("MS SQL Server");
                CurrentDataBase.DatabaseTypes.Add("Oracle");
                CurrentDataBase.DatabaseTypes.Add("MySQL");
            }


            Label labelType = new Label();
            labelType.Content = "DataBase Type";
            addToGrid(child_grid, labelType, 0, true);

            ComboBox comboDBType = new ComboBox();
            comboDBType.Name = "DataBaseType";
            comboDBType.ItemsSource = CurrentDataBase.DatabaseTypes;

            foreach (string dbType in CurrentDataBase.DatabaseTypes)
            {
                if (dbType.Equals(CurrentDataBase.SelectedDataBaseType))
                {
                    comboDBType.SelectedValue = dbType;
                }
            }

            addToGrid(child_grid, comboDBType, 1, false);

            Label labelHost = new Label();
            labelHost.Content = "DataBase Host";
            addToGrid(child_grid, labelHost, 0, true);

            TextBox txtHost = new TextBox();
            txtHost.Name = "DataBaseHost";
            txtHost.Text = CurrentDataBase.DatabaseHost;
            addToGrid(child_grid, txtHost, 1, false);


            Label labelPort = new Label();
            labelPort.Content = "DataBase Port";
            addToGrid(child_grid, labelPort, 0, true);

            TextBox txtPort = new TextBox();
            txtPort.Text = CurrentDataBase.DatabasePort;
            txtPort.Name = "DataBasePort";
            addToGrid(child_grid, txtPort, 1, false);

            Label labelUName = new Label();
            labelUName.Content = "User Name";
            addToGrid(child_grid, labelUName, 0, true);

            TextBox txtUname = new TextBox();
            txtUname.Text = CurrentDataBase.UserName;
            txtUname.Name = "UserName";
            addToGrid(child_grid, txtUname, 1, false);

            Label labePW = new Label();
            labePW.Content = "Password";
            addToGrid(child_grid, labePW, 0, true);

            PasswordBox txtPW = new PasswordBox();
            txtPW.Password = CurrentDataBase.Password;
            txtPW.Name = "Password";
            addToGrid(child_grid, txtPW, 1, false);


            Label labelQuery = new Label();
            labelQuery.Content = "Query";
            addToGrid(child_grid, labelQuery, 0, true);

            TextBox txtQuery = new TextBox();
            txtQuery.TextWrapping = TextWrapping.Wrap;
            txtQuery.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            txtQuery.Name = "Query";
            txtQuery.MinHeight = 70;
            txtQuery.Text = CurrentDataBase.Query;
            addToGrid(child_grid, txtQuery, 1, false);


            child_grid.LostFocus += new RoutedEventHandler(DataBaseDetailsEdited);
        }

        private static void DataBaseDetailsEdited(object sender, RoutedEventArgs e)
        {
            DataSourceViewModel datasourceVM = (DataSourceViewModel)testCaseTreeView.SelectedItem;

            DataBase tempDB = null;

            if (datasourceVM.SelectedDataSource == null || datasourceVM.SelectedDataSource.GetType().Name != "DataBase")
            {
                tempDB = new DataBase();
            }
            else
            {
                tempDB = (DataBase)datasourceVM.SelectedDataSource;
            }

            var children = child_grid.Children;

            foreach (var child in children)
            {
                object element = ((Border)child).Child;

                Type type = element.GetType();

                if (type.Name.Equals("ComboBox"))
                {
                    if (((ComboBox)element).Name.Equals("DataBaseType"))
                    {
                        if (((ComboBox)element).SelectedValue != null)
                        {
                            tempDB.SelectedDataBaseType = ((ComboBox)element).SelectedValue.ToString();
                        }
                    }
                }

                if (type.Name.Equals("TextBox"))
                {
                    if (((TextBox)element).Name.Equals("DataBaseHost"))
                    {
                        tempDB.DatabaseHost = ((TextBox)element).Text;
                    }
                    else if (((TextBox)element).Name.Equals("DataBasePort"))
                    {
                        tempDB.DatabasePort = ((TextBox)element).Text;
                    }
                    else if (((TextBox)element).Name.Equals("UserName"))
                    {
                        tempDB.UserName = ((TextBox)element).Text;
                    }
                    else if (((TextBox)element).Name.Equals("Query"))
                    {
                        tempDB.Query = ((TextBox)element).Text;
                    }
                }

                if (type.Name.Equals("PasswordBox"))
                {
                    if (((PasswordBox)element).Name.Equals("Password"))
                    {
                        tempDB.Password = ((PasswordBox)element).Password;
                    }
                }
            }

            ((DataSourceViewModel)testCaseTreeView.SelectedItem).SelectedDataSource = tempDB;
        }
    }
}
