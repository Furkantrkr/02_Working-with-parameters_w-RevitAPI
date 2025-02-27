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
            ICollection<ElementId> elementIds = sel.GetElementIds();

            IList<Element> elementsFromIds = new List<Element>();

            foreach (ElementId elementId in elementIds)
            {
                Element el = doc.GetElement(elementId);

                elementsFromIds.Add(el);
            }

            using(Transaction trans = new Transaction(doc,"Move Elements"))
            {
                trans.Start();

                foreach(Element element in elementsFromIds)
                {
                    Location loc = element.Location;

                    if (loc is LocationPoint || loc is LocationCurve)
                    {
                        loc.Move(new XYZ(2, 2, 2));
                    }
                }

                trans.Commit();
            }

            return Result.Succeeded;
        }
    }
}
