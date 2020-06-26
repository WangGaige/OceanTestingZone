using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Slb.Ocean.Petrel.Commands;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Contexts;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Core;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace OceanTestingZone
{
    class GetTemplateIcon : SimpleCommandHandler
    {
        public static string ID = "GetTemplateIcon";

        #region SimpleCommandHandler Members

        public override bool CanExecute(Slb.Ocean.Petrel.Contexts.Context context)
        { 
            return true;
        }

        public override void Execute(Slb.Ocean.Petrel.Contexts.Context context)
        {
            //TODO: Add command execution logic here
            /*
            Template twtTemplate = PetrelProject.WellKnownTemplates.GeometricalGroup.TimeTwoWay;

            ITemplateSettingsInfoFactory factory = CoreSystem.GetService<ITemplateSettingsInfoFactory>(twtTemplate);
            TemplateSettingsInfo info = factory.GetTemplateSettingsInfo(twtTemplate);
            //arguments.Label = info.LegendLabel;
            //arguments.DisplayUnit = info.Unit.Symbol;
            //arguments.Precision = info.NumericPrecision.PrecisionValue;

            // create custom template with more detailed info
            TemplateCollection parent = twtTemplate.TemplateCollection;
            ITemplateService ts = PetrelSystem.TemplateService;
            string newName = ts.GetUniqueName("My New Detailed Time");
            Template newTWT = Template.NullObject;

            using (ITransaction txn = DataManager.NewTransaction())
            {
                txn.Lock(parent);
                newTWT = parent.CreateTemplate(newName, info.DefaultColorTable, twtTemplate.UnitMeasurement);
                newTWT.Comments = "New two way time with more digits";
                newTWT.TemplateType = twtTemplate.TemplateType.;
                txn.Commit();
            }
            factory = CoreSystem.GetService<ITemplateSettingsInfoFactory>(newTWT);
            TemplateSettingsInfo info2 = factory.GetTemplateSettingsInfo(newTWT);
            info2.NumericPrecision = new NumericPrecision(6, WellKnownPrecisionTypes.DecimalPlaces);
            */
            
            PetrelLogger.InfoOutputWindow(string.Format("{0} clicked", @"GetTemplateIcon" ));
            foreach (object obj in context.GetSelectedObjects()) {
                Template g = obj as Template;
                string gname = "D:\\test\\"+obj.GetHashCode()+".bmp";
                ITemplateService imageFact;
                imageFact = CoreSystem.GetService<ITemplateService>();
                Bitmap imageInfo = imageFact.GetTemplateTypeImage(g.TemplateType);
                using (MemoryStream mem = new MemoryStream())
                {
                    //这句很重要，不然不能正确保存图片或出错（关键就这一句）
                    Bitmap bmp = new Bitmap(imageInfo);
                    //保存到磁盘文件
                    bmp.Save(gname, imageInfo.RawFormat);
                    bmp.Dispose();
                    MessageBox.Show("附件另存成功！", "注意", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show("附件另存成功a");

                }


            }
           
        }

        #endregion
    }
}
