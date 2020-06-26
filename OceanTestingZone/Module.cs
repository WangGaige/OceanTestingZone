using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace OceanTestingZone
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    public class Module : IModule
    {
        private Process m_multithread_seismiccubeInstance;
        public Module()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IModule Members

        /// <summary>
        /// This method runs once in the Module life; when it loaded into the petrel.
        /// This method called first.
        /// </summary>
        public void Initialize()
        {
            // Register OceanTestingZone.AmpAverageTest
            PetrelSystem.AddDataSourceFactory(new OceanTestingZone.AmpAverageTest.ArgumentPackageDataSourceFactory());
            // TODO:  Add Module.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
            // Register GetTemplateIcon
            PetrelSystem.CommandManager.CreateCommand(OceanTestingZone.GetTemplateIcon.ID, new OceanTestingZone.GetTemplateIcon());
            // Register OceanTestingZone.MultiThread_SeismicCube
            OceanTestingZone.MultiThread_SeismicCube multithread_seismiccubeInstance = new OceanTestingZone.MultiThread_SeismicCube();
            PetrelSystem.WorkflowEditor.Add(multithread_seismiccubeInstance);
            m_multithread_seismiccubeInstance = new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper(multithread_seismiccubeInstance);
            PetrelSystem.ProcessDiagram.Add(m_multithread_seismiccubeInstance, "Plug-ins");
            // Register MultiThreadSeismicData
            PetrelSystem.CommandManager.CreateCommand(OceanTestingZone.MultiThreadSeismicData.ID, new OceanTestingZone.MultiThreadSeismicData());
            // Register welltops_probing
            PetrelSystem.CommandManager.CreateCommand(OceanTestingZone.welltops_probing.ID, new OceanTestingZone.welltops_probing());
            // Register Probing_SeisCube_Z_intervel_unit
            PetrelSystem.CommandManager.CreateCommand(OceanTestingZone.Probing_SeisCube_Z_intervel_unit.ID, new OceanTestingZone.Probing_SeisCube_Z_intervel_unit());
            // Register OceanTestingZone.AmpAverageTest
            if (Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService == null)
                throw new LifecycleException("Required AttributeService is not available.");
            Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService.AddSeismicAttribute(new OceanTestingZone.AmpAverageTest());
            Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService.AddSeismicAttributeUIFactory(new OceanTestingZone.AmpAverageTest.UIFactory());
            // Register Add_SCAL_DATA
            PetrelSystem.CommandManager.CreateCommand(OceanTestingZone.Add_SCAL_DATA.ID, new OceanTestingZone.Add_SCAL_DATA());
            // Register SeismicDataTypeProbing
            PetrelSystem.CommandManager.CreateCommand(OceanTestingZone.SeismicDataTypeProbing.ID, new OceanTestingZone.SeismicDataTypeProbing());

            // TODO:  Add Module.Integrate implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {
            // Add Ribbon Configuration file
            PetrelSystem.ConfigurationService.AddConfiguration(OceanTestingZone.Properties.Resources.OceanRibbonConfiguration);

            // TODO:  Add Module.IntegratePresentation implementation
        }

        /// <summary>
        /// This method runs once in the Module life.
        /// right before the module is unloaded. 
        /// It usually happens when the application is closing.
        /// </summary>
        public void Disintegrate()
        {
            PetrelSystem.ProcessDiagram.Remove(m_multithread_seismiccubeInstance);
            // Unregister OceanTestingZone.AmpAverageTest
            if (Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService.GetSeismicAttributeUIFactory<OceanTestingZone.AmpAverageTest>() != null)
                Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService.RemoveSeismicAttributeUIFactory<OceanTestingZone.AmpAverageTest.Arguments>();
            // TODO:  Add Module.Disintegrate implementation
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add Module.Dispose implementation
        }

        #endregion

    }


}