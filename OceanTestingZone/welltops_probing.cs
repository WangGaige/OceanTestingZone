using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Slb.Ocean.Petrel.Commands;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Basics;
using Slb.Ocean.Petrel.Contexts;

namespace OceanTestingZone
{
    class welltops_probing : SimpleCommandHandler
    {
        public static string ID = "well_probing";

        #region SimpleCommandHandler Members

        public override bool CanExecute(Slb.Ocean.Petrel.Contexts.Context context)
        { 
            return true;
        }

        public override void Execute(Slb.Ocean.Petrel.Contexts.Context context)
        {
            //foreach (object obj in context.GetSelectedObjects())
            //{
            //    DictionaryWellPointPropertyFilter DPF = obj as DictionaryWellPointPropertyFilter;
            //    WellKnownMarkerPropertyTypes wf=new WellKnownMarkerPropertyTypes();
            //    
            //    PetrelLogger.InfoOutputWindow(DPF.Name+"--"+ DPF.Droid);
            //
            //}
            WellRoot wellroot = WellRoot.Get(PetrelProject.PrimaryProject);
            
            PetrelLogger.InfoOutputWindow("-------------------- Well Root Navigation ----------------------");
            
            if (wellroot.BoreholeCollection != null)
                PrintBoreholeCollection(wellroot.BoreholeCollection);
            else
            {
                PetrelLogger.InfoOutputWindow("No borehole information to print.");
            }
            if (wellroot.MarkerCollectionCount > 0)
                PrintMarkerCollectionDetails(wellroot.MarkerCollections.First());
            else
            {
                PetrelLogger.InfoOutputWindow("No Marker collections to print");
                return;
            }



        }
        private void PrintBoreholeCollection(BoreholeCollection boreholeCollection)
        {
            PetrelLogger.InfoOutputWindow("Borehole Collection: " + boreholeCollection.Name + " has " + boreholeCollection.Count.ToString() + " wells.");

            if (boreholeCollection.Count > 0)
            {

                foreach (Borehole well in boreholeCollection)
                {
                    double min = PetrelUnitSystem.ConvertToUI(Domain.MD, well.MDRange.Min);
                    double max = PetrelUnitSystem.ConvertToUI(Domain.MD, well.MDRange.Max);
                    //PetrelLogger.InfoOutputWindow("    Well: " + well.Name + "\t MD range: " + min.ToString() + ", " + max.ToString());
                    PetrelLogger.InfoOutputWindow("    Well: " + well.Name);
                }
            }

            if (boreholeCollection.BoreholeCollectionCount > 0)
            {
                foreach (BoreholeCollection col in boreholeCollection.BoreholeCollections)
                {
                    PrintBoreholeCollection(col);
                }
            }
        }

        private void PrintMarkerCollectionDetails(MarkerCollection markerColl)
        {
            /// Print the marker collection name.
            PetrelLogger.InfoOutputWindow("-------------------------------------------------------------------------------");
            PetrelLogger.InfoOutputWindow("Marker Collection Name        : " + markerColl.Name);

            if (markerColl.MarkerCount > 0)
            {
                /// Loop through all the markers and print each one's MD value, TVD value, and name of the surface it is attached to.
                PetrelLogger.InfoOutputWindow("Marker Collection Count       : " + markerColl.MarkerCount);
                PetrelLogger.InfoOutputWindow("             -------------------- Markers ---------------------------          ");
                foreach (Marker marker in markerColl.Markers)
                {
                    foreach (DictionaryWellPointProperty dwp in markerColl.MarkerPropertyCollection.DictionaryProperties)
                    {
                        if (dwp.Name == "Interpreter")
                        {
                            PetrelLogger.InfoOutputWindow(marker.PropertyAccess.GetPropertyValue<string>(dwp) + "--" + marker.Surface.Name + "--" + marker.Borehole.Name);
                        }
                    }
                }
                /*
                foreach (Marker marker in markerColl.Markers)
                {
                    double mdUI = PetrelUnitSystem.ConvertToUI(Domain.MD, marker.MD);
                    double tvdUI = PetrelUnitSystem.ConvertToUI(Domain.TVD_WRL, marker.TVD_WRL);
                    PetrelLogger.InfoOutputWindow("Marker MD, TVD : " + mdUI.ToString("N2") + "   " + tvdUI.ToString("N2") + "     " + marker.Surface.Name + marker.Droid);
                }*/
            }
        }

        #endregion
    }
}
