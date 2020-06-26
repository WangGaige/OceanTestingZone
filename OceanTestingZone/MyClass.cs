using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Well;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceanTestingZone
{
    public class MyClass
    {
        public static void DoWork()
        {
            //Project project = PetrelProject.PrimaryProject;
            Project project = PetrelSystem.ProjectService.OpenPrimaryProject("D:\\OCEAN\\OceanforPetrel2017CourseMaterials\\Handout\\DemoProject\\Class_DemoProject2017.pet");
            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(project);
                project.CreateCollection("aaaaa");
                trans.Commit();
            }
            PetrelSystem.ProjectService.SavePrimaryProject();
        }

    }
}
