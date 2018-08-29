namespace FluentTest
{
    using System;
    using System.Windows;
    using Fluent;
    using FluentTest.ViewModels.Entities;

    public partial class App
    {
        public static string xmlSource = @"C:\Users\bipadh\Documents\RPA1.1\roboData.xml";

        private static Type suitCollectionType = new TestSuitViewModel().GetType();
        private static Type sequenceCollectionType = new TestSequenceViewModel().GetType();
        private static Type stepCollectionType = new TestStepViewModel().GetType();
        private static Type appsCollectionType = new AppsViewModel().GetType();
        private static Type dataSourceCollectionType = new DataSourceViewModel().GetType();
        private static Type variableCollectionType = new VariableViewModel().GetType();
        private static Type dataBaseType = new DataBase().GetType();
        private static Type dataSource = new DataSource().GetType();
        private static Type dataBase = new DataBase().GetType();
        private static Type excel = new ExcelSheet().GetType();

        public static Type[] types = { suitCollectionType, sequenceCollectionType, stepCollectionType, appsCollectionType,
                dataSourceCollectionType, variableCollectionType, dataBaseType, dataSource, dataBase, excel };

    public App()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
        }

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            ThemeManager.IsAutomaticWindowsAppModeSettingSyncEnabled = true;
            ThemeManager.SyncAppThemeWithWindowsAppModeSetting();

            base.OnStartup(e);
        }
    }
}