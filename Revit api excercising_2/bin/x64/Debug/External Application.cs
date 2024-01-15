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
    internal class External_Application : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {

            return Result.Succeeded;

        }

        public Result OnStartup(UIControlledApplication application)
        {

            application.CreateRibbonTab("Cad To Revit");

            RibbonPanel ribbonpanel = application.CreateRibbonPanel("CAD2REVIT");

            string path = Assembly.GetExecutingAssembly().Location;

            //button

            PushButtonData button = new PushButtonData("Button", "Draw Ducts", path, "Revit_api_excercising_2.Class1");


            BitmapImage bti = new BitmapImage(new Uri("pack://application:,,,/CAD2REVIT;component/duct.png"));

            button.Image = bti;

            PushButton btn = ribbonpanel.AddItem(button) as PushButton;
            btn.LargeImage = bti;

            return Result.Succeeded;
        }
    }
}
