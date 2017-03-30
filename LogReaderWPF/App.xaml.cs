using System;
using System.Collections.Generic;
using System.Windows;
using Trustsoft.SingleInstanceApp;

namespace LogReaderWPF
{
    public partial class App : Application, ISingleInstanceApp
    {
        public static string OpenFileName;

        private const string UniqueKey = "{7E50467D-D491-4C83-9A48-91438D6D56ED}";

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(UniqueKey))
            {
                var application = new App();

                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 1)
                OpenFileName = e.Args[0];
        }

        internal void OpenFile(string fileName)
        {
            MainWindow window = (MainWindow)Current.MainWindow;
            window.OpenFile(fileName);
        }

        public bool OnActivate(IList<string> args)
        {
            if (args.Count == 1)
                OpenFile(args[0]);

            return true;

        }
    }
}
