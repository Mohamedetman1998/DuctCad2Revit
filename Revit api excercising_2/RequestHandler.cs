using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD2REVIT
{
    public class RequestHandler : IExternalEventHandler
    {
        public UIDocument Uidoc { get; set; } = Entry.CommandUIdoc;
        public Document doc { get; set; } = Entry.CommandDoc;
        public Autodesk.Revit.ApplicationServices.Application CurrentApp { get; set; } = Entry.CommandApp;

        private Request m_request = new Request();
        public Request Request
        {
            get { return m_request; }
        }
        public void Execute(UIApplication app)
        {

            try
            {
                switch (Request.Take())
                {
                    case RequestId.None:
                        {
                            return;
                        }


                    case RequestId.Class1execute:
                        {
                            class1(Uidoc, CurrentApp);
                        }
                        break;




                    default:
                        {
                            break;
                        }
                }
            }
            finally
            {
                //ExternalApplication.thisApp.WakeFormUp();
            }

            return;
        }
        public void class1(UIDocument _udoc, Autodesk.Revit.ApplicationServices.Application _CurrentApp)
        {

                UIDocument uidoc = _udoc;
                Autodesk.Revit.ApplicationServices.Application app = _CurrentApp;
                Document doc = uidoc.Document;
                double tolerance = app.ShortCurveTolerance;

                // Pick Import Instance

                ImportInstance import = null;
                
                    Reference r = uidoc.Selection.PickObject(ObjectType.Element, new Util.ElementsOfClassSelectionFilter<ImportInstance>());
                    import = doc.GetElement(r) as ImportInstance;


                // Fetch baselines
                List<Curve> wallCrvs = new List<Curve>();
                var wallLayers = Util.Misc.GetLayerNames(Properties.Settings.Default.layerWall);
                try
                {
                    foreach (string wallLayer in wallLayers)
                    {
                        wallCrvs.AddRange(Util.TeighaGeometry.ShatterCADGeometry(uidoc, import, wallLayer, tolerance));
                    }
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message, "Tips");
                }
                if (wallCrvs == null || wallCrvs.Count == 0)
                {
                    System.Windows.MessageBox.Show("Baselines not found", "Tips");
                }

            #region LevelId

            var docLevels = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .OfClass(typeof(Level))
                    .Cast<Level>()
                    .First();

            #endregion

            #region SystemFamily

            MEPSystemType mepSystemType = new FilteredElementCollector(doc)
                .OfClass(typeof(MEPSystemType))
                .Cast<MEPSystemType>()
                .FirstOrDefault(sysType => sysType.SystemClassification == MEPSystemClassification.SupplyAir);

            #endregion

            #region DuctType

                   FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof       (DuctType)).WhereElementIsElementType();
                    DuctType DT = collector.First() as DuctType;

            #endregion

                    Transaction trans2 = new Transaction(doc, "Draw MEP");

                    trans2.Start();

                    foreach (Curve e in wallCrvs)
                    {

                        XYZ LineStartPoint = e.GetEndPoint(0);
                        XYZ LineEndPoint = e.GetEndPoint(1);

                        Duct duct = Duct.Create(doc, mepSystemType.Id, DT.Id, docLevels.Id, LineStartPoint, LineEndPoint);
                    }

                    trans2.Commit();
        }
        public string GetName()
        {
            return "None";
        }
    }
}
