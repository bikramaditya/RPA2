namespace FluentTest.Helpers
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using FluentTest.ViewModels.Entities;

    public class TestStepDealer : EntityDealer
    {
        private static TreeView testCaseTreeView;
        private static Grid main_grid;

        internal static void DealWithTestStepViewModel(TestStepViewModel TSModel, Grid grid, TreeView TestCaseTreeView)
        {
            main_grid = grid;
            testCaseTreeView = TestCaseTreeView;

            string name = TSModel.Name;
            string desc = TSModel.Description;

            Label labelName = new Label();
            labelName.Content = "Name";
            addToGrid(grid, labelName, 0, true);

            TextBox txtName = new TextBox();
            txtName.Text = name;
            txtName.Name = "Name";
            addToGrid(grid, txtName, 1, false);


            Label labelDesc = new Label();
            labelDesc.Content = "Description";
            addToGrid(grid, labelDesc, 0, true);

            TextBox txtDesc = new TextBox();
            txtDesc.TextWrapping = TextWrapping.Wrap;
            txtDesc.Name = "Description";
            txtDesc.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            txtDesc.MinHeight = 70;
            txtDesc.Text = desc;
            addToGrid(grid, txtDesc, 1, false);

            grid.LostFocus += new RoutedEventHandler(TestStepDetailsEdited);
        }

        private static void TestStepDetailsEdited(object sender, RoutedEventArgs e)
        {
            object obj = testCaseTreeView.SelectedItem;
            if (!obj.GetType().Name.Equals("TestStepViewModel"))
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

    }
}
