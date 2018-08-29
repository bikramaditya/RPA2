namespace FluentTest.Helpers
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using FluentTest.ViewModels.Entities;
    using Microsoft.Win32;

    public class AppsSourceDealer : EntityDealer
    {
        private static TreeView testCaseTreeView;
        private static Grid main_grid;
        private static Grid child_grid;

        internal static void DealWithAppsSourceViewModel(AppsViewModel TSModel, Grid designer_main_panel_grid, Grid designer_child_panel_grid, TreeView TestCaseTreeView)
        {
            main_grid = designer_main_panel_grid;
            child_grid = designer_child_panel_grid;
            testCaseTreeView = TestCaseTreeView;

            string name = TSModel.Name;
            string desc = TSModel.Description;

            Label labelName = new Label();
            labelName.Content = "Name";
            addToGrid(main_grid, labelName, 0, true);

            TextBox txtName = new TextBox();
            txtName.Text = name;
            txtName.Name = "Name";
            addToGrid(main_grid, txtName, 1, false);


            Label labelDesc = new Label();
            labelDesc.Content = "Description";
            addToGrid(main_grid, labelDesc, 0, true);

            TextBox txtDesc = new TextBox();
            txtDesc.TextWrapping = TextWrapping.Wrap;
            txtDesc.Name = "Description";
            txtDesc.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            txtDesc.MinHeight = 70;
            txtDesc.Text = desc;
            addToGrid(main_grid, txtDesc, 1, false);


            Label labelType = new Label();
            labelType.Content = "Select App";
            addToGrid(main_grid, labelType, 0, true);

            if (TSModel.AppTypes.Count == 0)
            {
                TSModel.AppTypes.Add("Chrome");
                TSModel.AppTypes.Add("Firefox");
                TSModel.AppTypes.Add("IE");
                TSModel.AppTypes.Add("EDGE");
                TSModel.AppTypes.Add("Safari");
                TSModel.AppTypes.Add("Outlook");
                TSModel.AppTypes.Add("Other");
            }


            ComboBox comboType = new ComboBox();
            comboType.ItemsSource = TSModel.AppTypes;
            comboType.SelectionChanged += new SelectionChangedEventHandler(ComboboxSelectionChnaged);
            addToGrid(main_grid, comboType, 1, false);
            comboType.SelectedIndex = TSModel.SelectedIndex;

            Label labelHost = new Label();
            labelHost.Content = "App Path";
            addToGrid(main_grid, labelHost, 0, true);

            TextBox txtPath = new TextBox();
            txtPath.Name = "AppPath";
            txtPath.IsEnabled = false;
            txtPath.Text = TSModel.AppPath;
            addToGrid(main_grid, txtPath, 1, false);

            Button browse = new Button();
            browse.Content = "Browse";
            browse.Click += Browse_Click;
            addToGrid(main_grid, browse, 2, false);

            main_grid.LostFocus += new RoutedEventHandler(AppDetailsEdited);
        }


        private static void ComboboxSelectionChnaged(object sender, SelectionChangedEventArgs e)
        {
            AppsViewModel DSViewModel = (AppsViewModel)testCaseTreeView.SelectedItem;

            ComboBox combo = (ComboBox)sender;
            string app = (string)combo.SelectedValue;
            DSViewModel.SelectedIndex = combo.SelectedIndex;

            if (app.Equals("Chrome"))
            {
                object o = GetRegKey("chrome.exe");

                if (o != null)
                {
                    setAppPath(o.ToString());
                }

            }
            if (app.Equals("Firefox"))
            {
                object o = GetRegKey("Firefox.exe");

                if (o != null)
                {
                    setAppPath(o.ToString());
                }

            }
            if (app.Equals("IE"))
            {
                object o = GetRegKey("IEXPLORE.EXE");

                if (o != null)
                {
                    setAppPath(o.ToString());
                }

            }
            if (app.Equals("EDGE"))
            {
                setAppPath("MicrosoftEdge");
            }
            if (app.Equals("Safari"))
            {
                object o = GetRegKey("Safari.exe");

                if (o != null)
                {
                    setAppPath(o.ToString());
                }
            }

            if (app.Equals("Outlook"))
            {
                object o = GetRegKey("OUTLOOK.EXE");

                if (o != null)
                {
                    setAppPath(o.ToString());
                }

            }
            else if (app.Equals("Other"))
            {
                setAppPath("");
            }
        }

        private static void setAppPath(string v)
        {
            var children = main_grid.Children;

            foreach (var child in children)
            {
                object element = ((Border)child).Child;
                Type type = element.GetType();

                if (type.Name.Equals("TextBox"))
                {
                    if (((TextBox)element).Name.Equals("AppPath"))
                    {
                        ((TextBox)element).Text = v;
                    }
                }
            }
        }

        private static object GetRegKey(string exe)
        {
            string regKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\";

            string pathExe = regKey + exe;
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(pathExe))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue(null);
                        return o;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
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

                var children = main_grid.Children;

                foreach (var child in children)
                {
                    object element = ((Border)child).Child;
                    Type type = element.GetType();

                    if (type.Name.Equals("TextBox"))
                    {
                        if (((TextBox)element).Name.Equals("AppPath"))
                        {
                            ((TextBox)element).Text = filename;
                        }
                    }
                }
            }
        }

        private static void AppDetailsEdited(object sender, RoutedEventArgs e)
        {
            object obj = testCaseTreeView.SelectedItem;
            if (!obj.GetType().Name.Equals("AppsViewModel"))
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
                        if (!((AppsViewModel)obj).Name.Equals(((TextBox)element).Text))
                        {
                            ((AppsViewModel)obj).Name = ((TextBox)element).Text;
                            anyChange = true;
                        }
                    }
                    else if (((TextBox)element).Name.Equals("Description"))
                    {
                        if (!((AppsViewModel)obj).Description.Equals(((TextBox)element).Text))
                        {
                            ((AppsViewModel)obj).Description = ((TextBox)element).Text;
                        }
                    }
                    else if (((TextBox)element).Name.Equals("AppPath"))
                    {
                        if (!((AppsViewModel)obj).AppPath.Equals(((TextBox)element).Text))
                        {
                            ((AppsViewModel)obj).AppPath = ((TextBox)element).Text;
                        }
                    }
                }
            }

            if (anyChange)
            {
                testCaseTreeView.Items.Refresh();
            }
        }
    }
}
