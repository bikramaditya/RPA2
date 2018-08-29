namespace FluentTest.Helpers
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using FluentTest.ViewModels.Entities;

    public class TestSequenceDealer : EntityDealer
    {
        private static TreeView testCaseTreeView;
        private static Grid main_grid;

        internal static void DealWithTestSeqViewModel(TestSequenceViewModel TSModel, Grid grid, TreeView TestCaseTreeView)
        {
            main_grid = grid;
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

            main_grid.LostFocus += new RoutedEventHandler(TestSeqDetailsEdited);
        }

        private static void TestSeqDetailsEdited(object sender, RoutedEventArgs e)
        {
            object obj = testCaseTreeView.SelectedItem;
            if (!obj.GetType().Name.Equals("TestSequenceViewModel"))
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
                        if (!((TestSequenceViewModel)obj).Name.Equals(((TextBox)element).Text))
                        {
                            ((TestSequenceViewModel)obj).Name = ((TextBox)element).Text;
                            anyChange = true;
                        }
                    }
                    else if (((TextBox)element).Name.Equals("Description"))
                    {
                        if (!((TestSequenceViewModel)obj).Description.Equals(((TextBox)element).Text))
                        {
                            ((TestSequenceViewModel)obj).Description = ((TextBox)element).Text;
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
