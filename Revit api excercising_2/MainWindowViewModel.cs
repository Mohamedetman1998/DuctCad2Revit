using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD2REVIT
{
    public class MainWindowViewModel
    {
        public static ExternalEvent m_ExEvent { get; set; }
        public static RequestHandler m_Handler { get; set; }






        public MvCommand RunCommand { get; set; }


        public MainWindowViewModel()
        {
            RunCommand = new MvCommand(ExecuteRun);
        }

        private void MakeRequest(RequestId request)
        {
            m_Handler.Request.Make(request);
            m_ExEvent.Raise();
        }
        public void ExecuteRun(object parameter)
        {
            MakeRequest(RequestId.Class1execute);
        }
    }
}
