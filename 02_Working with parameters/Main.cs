using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace _02_Working_with_parameters
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements) {
            UIApplication uiApp = commandData.Application;
            Application app = uiApp.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            Selection sel = uiDoc.Selection;
            ICollection<Element> selectedElements = sel.GetElementIds().Select(id => doc.GetElement(id)).ToList();

            using(Transaction trans = new Transaction(doc,"Move Elements"))
            {
                trans.Start();

                string taskDialogText = "";

                foreach (Element element in selectedElements)
                {
                    Parameter commentParam = element.LookupParameter("Comments");
                    string paramStr = commentParam.AsString();

                    taskDialogText += $"Element ID: {element.Id} - Comments: {paramStr}\n";
                }

                TaskDialog.Show("Comments", taskDialogText);

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
}
