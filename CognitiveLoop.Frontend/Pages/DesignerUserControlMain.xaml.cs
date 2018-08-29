namespace FluentTest.Pages
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using FluentTest.Helpers;
    using FluentTest.ViewModels;
    using FluentTest.ViewModels.Entities;

    /// <summary>
    /// Interaction logic for DesignerUserControlMain.xaml
    /// </summary>
    public partial class DesignerUserControlMain : UserControl
    {
        private object BeingEdited;
        private Type TypeBeingEdited;
        private string xmlSource;

        public DesignerUserControlMain()
        {
            this.InitializeComponent();
        }

        private void TestCaseTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ((MainViewModel)this.DataContext).MainTree = this.TestCaseTreeView;

            var element = this.TestCaseTreeView.SelectedItem;

            if (element == null)
            {
                return;
            }

            ((RoboViewModel)((MainViewModel)this.DataContext).RoboViewModel).SelectedNode = element;

            string nodeType = element.GetType().Name;
            this.designer_main_panel_grid.RowDefinitions.Clear();
            this.designer_main_panel_grid.Children.Clear();
            this.designer_child_panel_grid.RowDefinitions.Clear();
            this.designer_child_panel_grid.Children.Clear();
            this.designer_help_panel_grid.RowDefinitions.Clear();
            this.designer_help_panel_grid.Children.Clear();

            if (nodeType.Equals("TestStepViewModel"))
            {
                TestStepDealer.DealWithTestStepViewModel((TestStepViewModel)element, designer_main_panel_grid, this.TestCaseTreeView);
            }
            else if (nodeType.Equals("TestSequenceViewModel"))
            {
                TestSequenceDealer.DealWithTestSeqViewModel((TestSequenceViewModel)element, designer_main_panel_grid, this.TestCaseTreeView);
            }
            else if (nodeType.Equals("TestSuitViewModel"))
            {
                TestSuitDealer.DealWithTestSuitViewModel((TestSuitViewModel)element, designer_main_panel_grid, this.TestCaseTreeView);
            }
            else if (nodeType.Equals("AppsViewModel"))
            {
                AppsSourceDealer.DealWithAppsSourceViewModel((AppsViewModel)element, designer_main_panel_grid, designer_child_panel_grid, this.TestCaseTreeView);
            }
            else if (nodeType.Equals("DataSourceViewModel"))
            {
                DataSourceDealer.DealWithDataSourceViewModel((DataSourceViewModel)element, designer_main_panel_grid, designer_child_panel_grid, designer_help_panel_grid, this.TestCaseTreeView);
            }
            else if (nodeType.Equals("VariableViewModel"))
            {
                VariableDealer.DealWithVariableViewModel((VariableViewModel)element, designer_main_panel_grid, designer_child_panel_grid, this.TestCaseTreeView);
            }

            detail_design_panel.Visibility = Visibility.Visible;
        }

        private void MenuItem_Suit_Delete_Click(object sender, RoutedEventArgs e)
        {
            TestSuitViewModel suit = (TestSuitViewModel)((MenuItem)sender).DataContext;

            ((ObservableCollection<TestSuitViewModel>)TestCaseTreeView.ItemsSource).Remove(suit);
        }

        private void MenuItem_Seq_Delete_Click(object sender, RoutedEventArgs e)
        {
            var SelectedSeq = ((MenuItem)sender).DataContext;

            ObservableCollection<TestSuitViewModel> suits = (ObservableCollection<TestSuitViewModel>)TestCaseTreeView.ItemsSource;

            foreach (TestSuitViewModel suit in suits)
            {
                suit.AllCollection.Remove(SelectedSeq);
            }
        }

        private void MenuItem_Step_Delete_Click(object sender, RoutedEventArgs e)
        {
            var SelectedStep = (TestStepViewModel)((MenuItem)sender).DataContext;

            ObservableCollection<TestSuitViewModel> suits = (ObservableCollection<TestSuitViewModel>)TestCaseTreeView.ItemsSource;

            foreach (TestSuitViewModel suit in suits)
            {
                foreach (var seq in suit.AllCollection)
                {
                    if (seq.GetType().Name.Equals("TestSequenceViewModel"))
                    {
                        ((TestSequenceViewModel)seq).TestStepCollection.Remove(SelectedStep);
                    }
                }
            }
        }
    }
}
