using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace CAD2REVIT
{
    public class App : IExternalApplication
    {
        public static App thisApp;
        private MainWindow Win;
        public Result OnShutdown(UIControlledApplication application)
        {

            return Result.Succeeded;

        }

        public Result OnStartup(UIControlledApplication application)
        {

            string RibbonTabName = "Cad To Revit";
            application.CreateRibbonTab(RibbonTabName);
            string path = Assembly.GetExecutingAssembly().Location;

            RibbonPanel panel = application.CreateRibbonPanel(RibbonTabName, "CadToRevit");
            //button

            PushButtonData button = new PushButtonData("Button", "Draw Ducts", path, "CAD2REVIT.Entry");


            BitmapImage bti = new BitmapImage(new Uri("pack://application:,,,/CAD2REVIT;component/duct.png"));

            button.Image = bti;

            PushButton btn = panel.AddItem(button) as PushButton;
            btn.LargeImage = bti;

            return Result.Succeeded;
        }

        public void ShowWindow1() 
        {
            if (Win == null)
            {
                RequestHandler _Handler = new RequestHandler();
                MainWindowViewModel.m_Handler = _Handler;

                ExternalEvent exe = ExternalEvent.Create(_Handler);
                MainWindowViewModel.m_ExEvent = exe;
                Win = new MainWindow();
                Win.Show();

            }
        }
    }
}
