using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.Commands;
using Slb.Ocean.Petrel.Contexts;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Seismic;

namespace OceanTestingZone
{
    class SeismicDataTypeProbing : SimpleCommandHandler
    {
        public static string ID = "OceanTestingZone.SeismicDataProbing";

        #region SimpleCommandHandler Members

        public override bool CanExecute(Slb.Ocean.Petrel.Contexts.Context context)
        { 
            return true;
        }

        public override void Execute(Slb.Ocean.Petrel.Contexts.Context context)
        {
            foreach (object obj in context.GetSelectedObjects())
            {
                PetrelLogger.InfoOutputWindow("Object type: " + obj.GetType().ToString());
                PetrelLogger.InfoOutputWindow("Ocean Object type: " + GetPublicDomainObjectTypeName(obj));
                //测试3d cube下的虚拟地震属性和3d cube的差别,用反射扫描所有属性
                foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
                {
                    PetrelLogger.InfoOutputWindow( p.Name + " Value//"+ p.GetValue(obj));
                }
                //return;
                //测试通过线道号读取地震解释
                //HorizonInterpretation hi=new HorizonInterpretation();

                //测试三维地震体线道号 20200306
                
                SeismicRoot sr = SeismicRoot.Get(PetrelProject.PrimaryProject);
                /*
                SeismicCollection sc = new SeismicCollection();
                SeismicCube scube = new SeismicCube();
                scube.AnnotationAtIndex(0,0,0).I- scube.AnnotationAtIndex(1, 0, 0).I
                scube.SampleSpacingIJK
                scube.Origin
                scube.NumSamplesIJK;
                scube.AnnotationAtIndex()
                */


                //测试2d plane的删除功能



                foreach (SurveyCollection surveyCollection in sr.SeismicProject.SurveyCollections)
                {
                    if (surveyCollection.Name== "All 2D seismic folder")
                    {
                        foreach (SeismicCollection seismicCollection in surveyCollection.SeismicCollections)
                        {
                            if (seismicCollection.Name=="2D Seismic - seismic survey")
                            {
                                foreach (SeismicLine2DCollection seismicLine2DCollection in seismicCollection.SeismicLine2DCollections)
                                {
                                    if (seismicLine2DCollection.Name== "2DInline 520 Time")
                                    {
                                        seismicLine2DCollection.Delete();
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
        public static string GetPublicDomainObjectTypeName(object domainObject)
        {
            if (domainObject == null)
                return "<null>";

            //Walk the type heirarchy looking for a public Ocean.Petrel domain object type
            Type type = domainObject.GetType();
            while (type != null && !IsPublicDomainObjectType(type))
                type = type.BaseType;

            //Special case for non-specific domain model types like UnknownEntity, right
            // now we only have one such thing - UnknownEntity.
            if (type == typeof(Slb.Ocean.Petrel.DomainObject.UnknownEntity))
                return "<UnknownEntity - not modelled in Ocean>";

            //Found nothing, Ocean does not allow definition of domain objects using interfaces
            // so if we found nothing then it is not a domain object.
            if (type == null)
            {
                // handle special cases where we know the internal instance to Ocean public facade mapping
                string rtnString = "<not a public domain object>";
                if (domainObject.GetType().ToString() == "Slb.Petrel.Sim.RockPhysics.AppModel.RockPhysicsFolder")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Simulation.RockPhysicsCollection";
                else if (domainObject.GetType().ToString() == "Slb.Petrel.Sim.RockPhysics.Compaction.AppModel.RockCompactionSubject")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Simulation.RockCompactionFunction";
                else if (domainObject.GetType().ToString() == "Slb.Petrel.Sim.RockPhysics.Saturation.AppModel.SaturationFunctionFolder")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Simulation.SaturationFunction";
                else if (domainObject.GetType().ToString() == "Slb.Petrel.Sim.RockPhysics.Saturation.AppModel.SaturationFunctionSubject")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Simulation.SaturationFunction.CapillaryPressure or .RelativePermeability";
                else if (domainObject.GetType().ToString() == "Slb.Petrel.Sim.Plotting.AppModel.PlottingSubjectFolder")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Simulation.Study";
                else if (domainObject.GetType().ToString() == "Slb.Petrel.Sim.Plotting.AppModel.PlottingSubject")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Simulation.Chart";
                else if (domainObject.GetType().ToString() == "Slb.Petrel.Wells.LogAttributes.AppModel.LogAttributeFolder")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Well.LogPropertyCollection";
                else if (domainObject.GetType().ToString() == "Slb.Petrel.Wells.LogAttributes.AppModel.LogAttributeSubject")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Well.LogProperty";
                else if (domainObject.GetType().ToString() == "Slb.Petrel.Geology.Stratigraphy.ApplicationModel.Objects.Subjects.ChartSubject")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Geology.Stratigraphy.StratigraphicChart";
                else if (domainObject.GetType().ToString() == "Slb.VolumeInterpretation.PetrelIntegration.ProbeFolder")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Seismic.Probe.ProbeCollection";
                else if (domainObject.GetType().ToString() == "Slb.VolumeInterpretation.DomainObject.WellProbe")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.Seismic.Probe.WellProbe";
                else if (domainObject.GetType().ToString() == "Slb.VolumeInterpretation.PetrelIntegration.GeobodyFolder")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.SeismicGeobody.GeobodyCollection";
                else if (domainObject.GetType().ToString() == "Slb.VolumeInterpretation.DomainObject.Geobody")
                    rtnString += " Ocean public facade is Slb.Ocean.Petrel.DomainObject.SeismicGeobody.Geobody";
                else
                    rtnString += " Ocean public facade is not known";
                return rtnString;
            }
            return type.Namespace + "." + type.Name;
        }

        /// <summary>
        /// Test if the given type is a public Ocean type, no check for null argument
        /// </summary>
        private static bool IsPublicDomainObjectType(Type t)
        {
            const string namespaceOceanPetrelDomainObject = "Slb.Ocean.Petrel.DomainObject";
            return t.IsPublic && (t.Namespace != null && t.Namespace.StartsWith(namespaceOceanPetrelDomainObject));
        }

        #endregion
    }
}
