namespace FluentTest.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using FluentTest.ViewModels.Entities;

    public class VariableDealer : EntityDealer
    {
        private static TreeView testCaseTreeView;
        private static Grid main_grid;
        private static Grid child_grid;
        
        internal static void DealWithVariableViewModel(VariableViewModel TSModel, Grid designer_main_panel_grid, Grid designer_child_panel_grid, TreeView TestCaseTreeView)
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


            Label labelKey = new Label();
            labelKey.Content = "Key";
            addToGrid(main_grid, labelKey, 0, true);

            Label labelKeyHint = new Label();
            labelKeyHint.FontSize = 9;
            labelKeyHint.Content = "Only alphanumeric.\nNo space/special chars.)";
            addToGrid(main_grid, labelKeyHint, 2, false);

            TextBox txtKey = new TextBox();
            txtKey.Text = TSModel.Key;
            txtKey.Name = "Key";
            txtKey.TextChanged += new TextChangedEventHandler(TextChanged);
            addToGrid(main_grid, txtKey, 1, false);

            Label varTypeLabel = new Label();
            varTypeLabel.Content = "Variable Type";
            addToGrid(main_grid, varTypeLabel, 0, true);

            ComboBox varTypes = new ComboBox();
            varTypes.Items.Add("Simple Constant");
            varTypes.Items.Add("Random Number");
            varTypes.Items.Add("Random String");
            varTypes.Items.Add("REST API");
            varTypes.Items.Add("Python Script");


            varTypes.SelectionChanged += new SelectionChangedEventHandler(VariableTypeChanged);
            varTypes.SelectedIndex = TSModel.SelectedVarTypeIndex;

            TSModel.VarType = varTypes.SelectedValue.ToString();

            addToGrid(main_grid, varTypes, 1, false);

            main_grid.LostFocus += new RoutedEventHandler(VarDetailsEdited);
            child_grid.LostFocus += new RoutedEventHandler(ChildVarDetailsEdited);
        }

        private static void TextChanged(object sender, TextChangedEventArgs e)
        {
            List<char> validChars = new List<char>( "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray());
            var textBox = sender as TextBox;
            string text = textBox.Text;
            char[] chars = text.ToCharArray();

            if (text.Contains(" "))
            {
                text = text.Replace(" ", "");
                ((TextBox)sender).Text = text;
                ((TextBox)sender).CaretIndex = text.Length;
                e.Handled = true;
                return;
            }

            foreach (char c in chars)
            {
                if (!validChars.Contains(c))
                {
                    text = text.Replace(c.ToString(), "".ToString());
                    ((TextBox)sender).Text = text;
                    ((TextBox)sender).CaretIndex = text.Length;
                    e.Handled = true;
                    return;
                }
            }
        }

        private static void VariableTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            VariableViewModel model = (VariableViewModel)testCaseTreeView.SelectedItem;


            child_grid.RowDefinitions.Clear();
            child_grid.Children.Clear();

            ComboBox varTypes = (ComboBox)sender;
            string selection = varTypes.SelectedValue.ToString();

            model.VarType = selection;
            model.SelectedVarTypeIndex = varTypes.SelectedIndex;

            if (selection.Equals("Simple Constant"))
            {
                Label labelVal = new Label();
                labelVal.Content = "Value";
                addToGrid(child_grid, labelVal, 0, true);

                TextBox txtVal = new TextBox();
                txtVal.TextWrapping = TextWrapping.Wrap;
                txtVal.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                txtVal.MinHeight = 70;
                txtVal.Text = model.SimpleValue;
                txtVal.Name = "SimpleValue";
                addToGrid(child_grid, txtVal, 1, false);
            }
            else if (selection.Equals("Random Number"))
            {
                Label labelMinVal = new Label();
                labelMinVal.Content = "Min Number";
                addToGrid(child_grid, labelMinVal, 0, true);

                TextBox txtMinVal = new TextBox();
                txtMinVal.Text = model.MinValue.ToString();
                txtMinVal.Name = "MinValue";
                addToGrid(child_grid, txtMinVal, 1, false);

                Label labelMaxVal = new Label();
                labelMaxVal.Content = "Max Number";
                addToGrid(child_grid, labelMaxVal, 0, true);

                TextBox txtMaxVal = new TextBox();
                txtMaxVal.Text = model.MaxValue.ToString();
                txtMaxVal.Name = "MaxValue";
                addToGrid(child_grid, txtMaxVal, 1, false);
            }
            else if (selection.Equals("Random String"))
            {
                Label labelVal = new Label();
                labelVal.Content = "Enter possible chars \nconsecutively";
                addToGrid(child_grid, labelVal, 0, true);

                TextBox txtVal = new TextBox();
                txtVal.TextWrapping = TextWrapping.Wrap;
                txtVal.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                txtVal.MinHeight = 70;
                txtVal.Text = model.AllowedChars;
                txtVal.Name = "AllowedChars";
                addToGrid(child_grid, txtVal, 1, false);
            }
            else if (selection.Equals("REST API"))
            {
                Label labelEndPointURL = new Label();
                labelEndPointURL.Content = "End Point URL";
                addToGrid(child_grid, labelEndPointURL, 0, true);

                TextBox txtEndPointURL = new TextBox();
                txtEndPointURL.Text = model.EndPointURL;
                txtEndPointURL.Name = "EndPointURL";
                addToGrid(child_grid, txtEndPointURL, 1, false);

                Label labelHTTPMethod = new Label();
                labelHTTPMethod.Content = "HTTPMethod";
                addToGrid(child_grid, labelHTTPMethod, 0, true);

                ComboBox txtHTTPMethod = new ComboBox();
                List<string> iList = new List<string>();
                iList.Add("GET");
                iList.Add("POST");
                txtHTTPMethod.ItemsSource = iList;
                txtHTTPMethod.Name = "HTTPMethod";
                txtHTTPMethod.SelectedIndex = 0;
                addToGrid(child_grid, txtHTTPMethod, 1, false);

                Label labelHTTPHeaders = new Label();
                labelHTTPHeaders.Content = "HTTP Headers";
                addToGrid(child_grid, labelHTTPHeaders, 0, true);

                TextBox txtHTTPHeaders = new TextBox();
                txtHTTPHeaders.TextWrapping = TextWrapping.Wrap;
                txtHTTPHeaders.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                txtHTTPHeaders.MinHeight = 70;
                txtHTTPHeaders.Text = model.SimpleValue;
                txtHTTPHeaders.Name = "HTTPHeaders";
                addToGrid(child_grid, txtHTTPHeaders, 1, false, 120);

                Label labelHTTPBody = new Label();
                labelHTTPBody.Content = "HTTP Body";
                addToGrid(child_grid, labelHTTPBody, 0, true);

                TextBox txtHTTPBody = new TextBox();
                txtHTTPBody.TextWrapping = TextWrapping.Wrap;
                txtHTTPBody.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                txtHTTPBody.MinHeight = 100;
                txtHTTPBody.Text = model.SimpleValue;
                txtHTTPBody.Name = "HTTPBody";
                addToGrid(child_grid, txtHTTPBody, 1, false, 170);

                Button testButton = new Button();
                testButton.Content = "Test";
                addToGrid(child_grid, testButton, 2, true, 40);
            }
            else if (selection.Equals("Python Script"))
            {
                Label labelPythonScript = new Label();
                labelPythonScript.Content = "Python Script Body";
                addToGrid(child_grid, labelPythonScript, 0, true);

                TextBox txtPythonScript = new TextBox();
                txtPythonScript.TextWrapping = TextWrapping.Wrap;
                txtPythonScript.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                txtPythonScript.MinHeight = 300;
                txtPythonScript.Text = model.PythonScript;
                txtPythonScript.Name = "PythonScript";
                addToGrid(child_grid, txtPythonScript, 1, false, 300);

                Button testButton = new Button();
                testButton.Content = "Test";
                addToGrid(child_grid, testButton, 2, true, 40);
            }
        }

        private static void VarDetailsEdited(object sender, RoutedEventArgs e)
        {
            object obj = testCaseTreeView.SelectedItem;
            if (!obj.GetType().Name.Equals("VariableViewModel"))
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
                        if (((TextBox)element).Text != null)
                        {
                            if (!((TextBox)element).Text.Equals(((VariableViewModel)obj).Name))
                            {
                                ((VariableViewModel)obj).Name = ((TextBox)element).Text;
                                anyChange = true;
                            }
                        }
                    }
                    else if (((TextBox)element).Name.Equals("Description"))
                    {
                        if (((TextBox)element).Text != null)
                        {
                            if (!((TextBox)element).Text.Equals(((VariableViewModel)obj).Description))
                            {
                                ((VariableViewModel)obj).Description = ((TextBox)element).Text;
                            }
                        }
                    }
                    else if (((TextBox)element).Name.Equals("Key"))
                    {
                        if (((TextBox)element).Text != null)
                        {
                            if (!((TextBox)element).Text.Equals(((VariableViewModel)obj).Key))
                            {
                                ((VariableViewModel)obj).Key = ((TextBox)element).Text;
                            }
                        }
                    }
                    else if (((TextBox)element).Name.Equals("Value"))
                    {
                        if(((TextBox)element).Text !=null)
                        {
                            if (!((TextBox)element).Text.Equals(((VariableViewModel)obj).SimpleValue))
                            {
                                ((VariableViewModel)obj).SimpleValue = ((TextBox)element).Text;
                            }
                        }
                    }
                }
            }
        }

        private static void ChildVarDetailsEdited(object sender, RoutedEventArgs e)
        {
            object obj = testCaseTreeView.SelectedItem;
            if (!obj.GetType().Name.Equals("VariableViewModel"))
            {
                return;
            }

            var children = child_grid.Children;

            foreach (var child in children)
            {
                object element = ((Border)child).Child;
                Type type = element.GetType();

                if (type.Name.Equals("TextBox"))
                {
                    if (((TextBox)element).Name.Equals("PythonScript"))
                    {
                        if (((TextBox)element).Text != null)
                        {
                            if (!((TextBox)element).Text.Equals(((VariableViewModel)obj).PythonScript))
                            {
                                ((VariableViewModel)obj).PythonScript = ((TextBox)element).Text;
                            }
                        }
                    }
                    else if (((TextBox)element).Name.Equals("AllowedChars"))
                    {
                        if (((TextBox)element).Text != null)
                        {
                            if (!((TextBox)element).Text.Equals(((VariableViewModel)obj).AllowedChars))
                            {
                                ((VariableViewModel)obj).AllowedChars = ((TextBox)element).Text;
                            }
                        }
                    }
                }
            }
        }
    }
}
