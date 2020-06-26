using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Slb.Ocean.Petrel.Commands;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.Contexts;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Basics;
using Slb.Ocean.Coordinates;
using Slb.Ocean.Geometry;

namespace OceanTestingZone
{
    class Probing_SeisCube_Z_intervel_unit : SimpleCommandHandler
    {
        public static string ID = "SeisCube_Z_intvel";

        #region SimpleCommandHandler Members

        public override bool CanExecute(Slb.Ocean.Petrel.Contexts.Context context)
        { 
            return true;
        }

        public override void Execute(Slb.Ocean.Petrel.Contexts.Context context)
        {
            //TODO: Add command execution logic here
            foreach (object obj in context.GetSelectedObjects()) {
                SeismicCube scube = obj as SeismicCube;
                Template t_cube = scube.Template;
                PetrelLogger.InfoOutputWindow(scube.SampleSpacingIJK.Z.ToString());
                ITemplateService service = PetrelSystem.TemplateService;
                Template t1 = service.FindTemplateByName("Elevation time");
                double val_UI = PetrelUnitSystem.ConvertToUI(t1, scube.SampleSpacingIJK.Z);
                
                //HorizonInterpretation3D h3d0 = Htop.GetHorizonInterpretation3D(args.srcSeisCube.SeismicCollection);
                //foreach (HorizonInterpretation3DSample horSample in h3d0.Samples) {
                //    scube.IndexAtPosition(new Point3(scube.Origin.X,scube.Origin.Y, horSample.Value));
                //}

            }
        }
    
        #endregion
    }
}
