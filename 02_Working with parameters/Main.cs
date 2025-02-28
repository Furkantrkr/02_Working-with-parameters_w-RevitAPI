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

            // Current selection of user in revit
            Selection sel = uiDoc.Selection;
            ICollection<Element> selectedElements = sel.GetElementIds().Select(id => doc.GetElement(id)).ToList(); // Get all selected elements

            // Transcation
            using (Transaction trans = new Transaction(doc,"Set Values"))
            {
                trans.Start();

                foreach (Element element in selectedElements)
                {
                    Parameter commentParam = element.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                    commentParam.Set("This is a generic comment.");
                }
                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
}
