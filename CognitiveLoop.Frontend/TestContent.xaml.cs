#pragma warning disable SA1402 // File may only contain a single class
namespace FluentTest
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Xml.Serialization;
    using Fluent;
    using FluentTest.Helpers;
    using FluentTest.ViewModels;
    using FluentTest.ViewModels.Entities;
    using Button = Fluent.Button;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.ComponentModel;
    using FluentTest.Pages;

    public partial class TestContent
    {
        private readonly MainViewModel viewModel;

        public TestContent()
        {
            this.InitializeComponent();

            this.HookEvents();

            this.viewModel = new MainViewModel();
            this.DataContext = this.viewModel;

            ColorGallery.RecentColors.Add(((SolidColorBrush)Application.Current.Resources["Fluent.Ribbon.Brushes.AccentBaseColorBrush"]).Color);
        }

        private void HookEvents()
        {
            this.Loaded += this.HandleTestContentLoaded;

            this.buttonBold.Checked += (s, e) => Debug.WriteLine("Checked");
            this.buttonBold.Unchecked += (s, e) => Debug.WriteLine("Unchecked");

            this.PreviewMouseWheel += this.OnPreviewMouseWheel;
        }

        private static void OnScreenTipHelpPressed(object sender, ScreenTipHelpEventArgs e)
        {
            Process.Start((string)e.HelpTopic);
        }

        private void HandleTestContentLoaded(object sender, RoutedEventArgs e)
        {
            ScreenTip.HelpPressed += OnScreenTipHelpPressed;
        }

        private void OnLauncherButtonClick(object sender, RoutedEventArgs e)
        {
            var groupBox = (RibbonGroupBox)sender;

            var wnd = new Window
            {
                Content = $"Launcher-Window for: {groupBox.Header}",
                Width = 300,
                Height = 100,
                Owner = Window.GetWindow(this),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            wnd.ShowDialog();
        }

        private void OnSplitClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Split Clicked!!!");
        }

        private void OnEnlargeClick(object sender, RoutedEventArgs e)
        {
            if (this.InRibbonGallery.IsLoaded)
            {
                this.InRibbonGallery.Enlarge();
            }
        }

        private void OnReduceClick(object sender, RoutedEventArgs e)
        {
            if (this.InRibbonGallery.IsLoaded)
            {
                this.InRibbonGallery.Reduce();
            }
        }

        public Button CreateRibbonButton()
        {
            var fooCommand1 = new TestRoutedCommand();

            var button = new Button
            {
                Command = fooCommand1.ItemCommand,
                Header = "Foo",
                Icon = new BitmapImage(new Uri("pack://application:,,,/CognitiveLoop.Frontend;component/Images/Green.png", UriKind.Absolute)),
                LargeIcon = new BitmapImage(new Uri("pack://application:,,,/CognitiveLoop.Frontend;component/Images/GreenLarge.png", UriKind.Absolute)),
            };

            this.CommandBindings.Add(fooCommand1.ItemCommandBinding);
            return button;
        }

        #region Logical tree

        private void OnShowLogicalTreeClick(object sender, RoutedEventArgs e)
        {
           
        }

        private static string GetDebugInfo(DependencyObject element)
        {
            if (element == null)
            {
                return "NULL";
            }

            var ribbonControl = element as IHeaderedControl;

            var header = ribbonControl != null
                           ? ribbonControl.Header
                           : string.Empty;

            var frameworkElement = element as FrameworkElement;
            var name = frameworkElement != null
                           ? frameworkElement.Name
                           : string.Empty;

            return $"[{element}] (Header: {header} || Name: {name})";
        }

        private void CheckLogicalTree(DependencyObject root)
        {
            var children = LogicalTreeHelper.GetChildren(root);
            foreach (var child in children.OfType<DependencyObject>())
            {
                if (ReferenceEquals(LogicalTreeHelper.GetParent(child), root) == false)
                {
                    Debug.WriteLine($"Incorrect logical parent for {GetDebugInfo(child)}");
                    Debug.WriteLine($"\tExpected: {GetDebugInfo(root)}");
                    Debug.WriteLine($"\tFound: {GetDebugInfo(LogicalTreeHelper.GetParent(child))}");
                }

                this.CheckLogicalTree(child);
            }
        }

        private void BuildLogicalTree(DependencyObject current, ItemsControl parentControl)
        {
            var newItem = new TreeViewItem
            {
                Header = GetDebugInfo(current),
                Tag = current
            };

            parentControl.Items.Add(newItem);

            var children = LogicalTreeHelper.GetChildren(current);
            foreach (var child in children.OfType<DependencyObject>())
            {
                this.BuildLogicalTree(child, newItem);
            }
        }

        private void OnTreeDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void BuildBackLogicalTree(DependencyObject current, StringBuilder stringBuilder)
        {
            if (current == null
                || ReferenceEquals(current, this.ribbon))
            {
                return;
            }

            stringBuilder.AppendFormat(" -> {0}\n", GetDebugInfo(current));

            var parent = LogicalTreeHelper.GetParent(current);

            this.BuildBackLogicalTree(parent, stringBuilder);
        }

        #endregion Logical tree

        private void OnFormatPainterClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("FP");
        }

        private void OnHelpClick(object sender, RoutedEventArgs e)
        {
            if (this.tabGroup1.Visibility == Visibility.Visible)
            {
                this.tabGroup1.Visibility = Visibility.Collapsed;
                this.tabGroup2.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.tabGroup1.Visibility = Visibility.Visible;
                this.tabGroup2.Visibility = Visibility.Visible;
            }
        }

        private void OnSpinnerValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // MessageBox.Show(String.Format("Changed from {0} to {1}", e.OldValue, e.NewValue));
        }

        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            var wnd = new TestWindow
            {
                Owner = Window.GetWindow(this)
            };
            wnd.Show();
        }

        private void AddRibbonTab_OnClick(object sender, RoutedEventArgs e)
        {
            var tab = new RibbonTabItem
            {
                Header = "Test"
            };

            var group = new RibbonGroupBox();
            for (var i = 0; i < 20; i++)
            {
                group.Items.Add(this.CreateRibbonButton());
            }

            tab.Groups.Add(group);

            this.ribbon.Tabs.Add(tab);
        }

        private void HandleSaveAsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You clicked \"Save as\".");
        }

        private BackgroundWorker bw;

        SaveProgressWindow progressWindow;

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            bool isSuccess = false;
            progressWindow = new SaveProgressWindow();
            bw = new BackgroundWorker();
            //var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CognitiveLoop", "roboData.xml");

            bw.WorkerReportsProgress = true;
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();

            //Progress Bar Window
            progressWindow.Show();


            RoboViewModel robo = ((MainViewModel)this.DataContext).RoboViewModel;

            XmlSerializer serializer = new XmlSerializer(robo.GetType(), App.types);
            using (XmlTextWriter writer = new XmlTextWriter(App.xmlSource, System.Text.Encoding.UTF8))
            {
                serializer.Serialize(writer, robo);
            }
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            for (double i = 0; i < 250; i++)
            {
                bw.ReportProgress((int)(100 * (i / 250)));
                Thread.Sleep(2);
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressWindow.Hide();
            progressWindow.Close();
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressWindow.prgTest.Value = e.ProgressPercentage;
        }

        private void ZoomSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var textFormattingMode = e.NewValue > 1.0 || Math.Abs(e.NewValue - 1.0) < double.Epsilon ? TextFormattingMode.Ideal : TextFormattingMode.Display;
            TextOptions.SetTextFormattingMode(this, textFormattingMode);
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) == false
                && Keyboard.IsKeyDown(Key.RightCtrl) == false)
            {
                return;
            }

            var newZoomValue = this.zoomSlider.Value + (e.Delta > 0 ? 0.1 : -0.1);

            this.zoomSlider.Value = Math.Max(Math.Min(newZoomValue, this.zoomSlider.Maximum), this.zoomSlider.Minimum);

            e.Handled = true;
        }

        private void SleepButton_OnClick(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        private void OpenModalRibbonWindow_OnClick(object sender, RoutedEventArgs e)
        {
            new TestWindow().ShowDialog();
        }

        private void OpenRibbonWindowOnNewThread_OnClick(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() =>
                                    {
                                        new TestWindow().Show();
                                        System.Windows.Threading.Dispatcher.Run();
                                    })
                         {
                             IsBackground = true
                         };
            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();
        }

        private void HandleAddItemToFontsClick(object sender, RoutedEventArgs e)
        {
            this.viewModel.FontsViewModel.FontsData.Add($"Added item {this.viewModel.FontsViewModel.FontsData.Count}");
        }

        private void New_Suit_Button_Click(object sender, RoutedEventArgs e)
        {
            TestSuitViewModel suitModel = new TestSuitViewModel();
            suitModel.Name = "New Suit";
            suitModel.Description = "New suit desc";
            suitModel.IsSelected = true;
            this.viewModel.RoboViewModel.TestSuitCollection.Add(suitModel);
        }

        private void New_Sequence_Button_Click(object sender, RoutedEventArgs e)
        {
            object element = this.viewModel.RoboViewModel.SelectedNode;
            if (element != null && element.GetType().Name.Equals("TestSuitViewModel"))
            {
                TestSuitViewModel elementModel = (TestSuitViewModel)element;
                TestSequenceViewModel newSequence = new TestSequenceViewModel();
                newSequence.Name = "3rd Sequence";
                newSequence.Description = "3rd Sequence desc";

                elementModel.AllCollection.Add(newSequence);

                elementModel.IsExpanded = true;
                //newSequence.IsSelected = true;
            }
            else
            {
                MessageBox.Show("Please select the suit under which new sequence to be added.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void New_Step_Button_Click(object sender, RoutedEventArgs e)
        {
            object element = this.viewModel.RoboViewModel.SelectedNode;

            if (element != null && element.GetType().Name.Equals("TestSequenceViewModel"))
            {
                TestSequenceViewModel elementModel = (TestSequenceViewModel)element;
                TestStepViewModel newStep = new TestStepViewModel();
                newStep.Name = "Nth Step";
                newStep.Description = "3rd Step desc";

                elementModel.TestStepCollection.Add(newStep);

                elementModel.IsExpanded = true;
                newStep.IsSelected = true;
                newStep.IsExpanded = true;

            }
            else
            {
                MessageBox.Show("Please select the sequence under which new step to be added.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void New_App_Button_Click(object sender, RoutedEventArgs e)
        {
            object element = this.viewModel.RoboViewModel.SelectedNode;
            if (element != null && element.GetType().Name.Equals("TestSuitViewModel"))
            {
                TestSuitViewModel elementModel = (TestSuitViewModel)element;
                AppsViewModel app = new AppsViewModel();
                app.Name = "3rd App";
                app.Description = "3rd App desc";

                elementModel.AllCollection.Add(app);
                elementModel.IsExpanded = true;
                app.IsSelected = true;
            }
            else
            {
                MessageBox.Show("Please select the suit under which new Web/App to be added.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void New_DataSource_Button_Click(object sender, RoutedEventArgs e)
        {
            object element = this.viewModel.RoboViewModel.SelectedNode;
            if (element != null && element.GetType().Name.Equals("TestSuitViewModel"))
            {
                TestSuitViewModel elementModel = (TestSuitViewModel)element;
                DataSourceViewModel ds = new DataSourceViewModel();
                ds.Name = "3rd Data Source";
                ds.Description = "3rd DS desc";

                elementModel.AllCollection.Add(ds);
                elementModel.IsExpanded = true;
                ds.IsSelected = true;
            }
            else
            {
                MessageBox.Show("Please select the suit under which new Data Source to be added.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void New_Variable_Button_Click(object sender, RoutedEventArgs e)
        {
            object element = this.viewModel.RoboViewModel.SelectedNode;
            if (element != null && element.GetType().Name.Equals("TestSuitViewModel"))
            {
                TestSuitViewModel elementModel = (TestSuitViewModel)element;
                VariableViewModel variable = new VariableViewModel();
                variable.Name = "3rd Variable";
                variable.Description = "3rd Variable desc";

                elementModel.AllCollection.Add(variable);
                elementModel.IsExpanded = true;
                variable.IsExpanded = true;
                variable.IsSelected = true;
                variable.IsExpanded = true;
            }
            else
            {
                MessageBox.Show("Please select the suit under which new Variable to be added.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Function_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class TestRoutedCommand
    {
        public static RoutedCommand TestPresenterCommand { get; } = new RoutedCommand("TestPresenterCommand", typeof(TestRoutedCommand));

        public ICommand ItemCommand => TestPresenterCommand;

        public CommandBinding ItemCommandBinding => new CommandBinding(TestPresenterCommand, OnTestCommandExecuted, CanExecuteTestCommand);

        private static void CanExecuteTestCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private static void OnTestCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("TestPresenterCommand");
        }
    }
}