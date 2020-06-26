using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.Commands;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.IO;
using Slb.Ocean.Petrel.DomainObject.Simulation;

namespace OceanTestingZone
{
    class Add_SCAL_DATA : SimpleCommandHandler
    {
        public static string ID = "OceanTestingZone.AddSCAL";

        #region SimpleCommandHandler Members

        public override bool CanExecute(Slb.Ocean.Petrel.Contexts.Context context)
        { 
            return true;
        }

        public override void Execute(Slb.Ocean.Petrel.Contexts.Context context)
        {
            //TODO: Add command execution logic here
            //测试通过按钮加载SCAL及PVT数据
            string[] files;
            object[] objs;
            FileFormat format = PetrelSystem.FileFormats.WellKnownFileFormats.EclipseSCAL;
            //从文件加载到Petrel
            PetrelSystem.FileFormats.ShowImportDialog(format, "d:\\", SimulationRoot.Get(PetrelProject.PrimaryProject).RockPhysicsCollection.RockPhysicsCollections.First(), out files, out objs);

            //新建一个SCAL并赋值

            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(SimulationRoot.Get(PetrelProject.PrimaryProject).RockPhysicsCollection
                    .RockPhysicsCollections.First());
                SaturationFunction sf = SimulationRoot.Get(PetrelProject.PrimaryProject).RockPhysicsCollection
                    .RockPhysicsCollections.First().CreateSaturationFunction("test");
                List<SaturationFunction.CapillaryPressure.Curve.Record> cpRecords = new List<SaturationFunction.CapillaryPressure.Curve.Record>();
                for (int i = 0; i < 5; i++)
                {
                    double m = 0.5 + ((double)i / 10);
                    cpRecords.Add(new SaturationFunction.CapillaryPressure.Curve.Record(m, 10-i));
                }

                sf.CapillaryPressures.GasWater.Enable();
                sf.CapillaryPressures.GasWater.Records = cpRecords;
                trans.Commit();
            }
            

        }
        #endregion
    }
}
