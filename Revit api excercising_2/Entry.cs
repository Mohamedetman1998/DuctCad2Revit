using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD2REVIT
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class Entry : IExternalCommand
    {
        public static UIDocument CommandUIdoc { get; set; }
        public static Document CommandDoc { get; set; }
        public static Autodesk.Revit.ApplicationServices.Application CommandApp { get; set; }
        public static ExternalCommandData Au {get;set;}
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            CommandApp = commandData.Application.Application;
            CommandUIdoc = commandData.Application.ActiveUIDocument;
            CommandDoc = CommandUIdoc.Document;
            Au = commandData;

            App application = new App();
            App.thisApp = application;

            try
            {
                application.ShowWindow1();
                return Result.Succeeded;
            }
            catch (Exception exp)
            {
                TaskDialog.Show("catch", exp.Message);
            }

            return Result.Succeeded;
        }


    }
}
