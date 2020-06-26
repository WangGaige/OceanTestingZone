using System;
using System.Collections.Generic;

using Slb.Ocean.Core;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.Data;
using Slb.Ocean.Petrel.Data.Persistence;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.Seismic;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.UI;

namespace OceanTestingZone
{
    class AmpAverageTest : SeismicAttribute<AmpAverageTest.Arguments>, IDescriptionSource
    {


        #region Overrides from SeismicAttribute

        protected override Arguments CreateArgumentPackageCore(IDataSourceManager manager)
        {
            Arguments argPack = new Arguments();

            StructuredArchiveDataSource dataSource = manager.GetSource(ArgumentPackageDataSourceFactory.DataSourceId) as StructuredArchiveDataSource;

            if (dataSource != null)
            {
                argPack.Droid = dataSource.GenerateDroid();
                dataSource.AddItem(argPack.Droid, argPack);
            }

            return argPack;
        }


        public override void CopyArgumentPackage(AmpAverageTest.Arguments fromArgumentPackage, AmpAverageTest.Arguments toArgumentPackage)
        {
            if (fromArgumentPackage != null && toArgumentPackage != null)
            {
                toArgumentPackage.CopyFrom(fromArgumentPackage);
            }
        }

        public override bool CompareArgumentPackage(AmpAverageTest.Arguments firstArgumentPackage, AmpAverageTest.Arguments secondArgumentPackage)
        {
            if (firstArgumentPackage != null && secondArgumentPackage != null)
            {
                return firstArgumentPackage.EqualsTo(secondArgumentPackage);
            }

            return false;
        }

        public override SeismicAttributeGenerator CreateAttributeGenerator(AmpAverageTest.Arguments argumentPackage, IGeneratorContext context)
        {
            return new AmpAverageTest.Generator(argumentPackage, context);
        }

        public override bool Validate(AmpAverageTest.Arguments argumentPackage, IGeneratorContext context, out string errorMessage)
        {
            errorMessage = "N/A";

            // TODO: Please implement the validation logic for the argumentPackage.
            // return true, when the given argumentPackage is valid.
            // return false, and fill the errorMessage when the given argumentPackage is not valid.

            return true;
        }

        public override SeismicAttributeInfo CreateSeismicAttributeInfo(AmpAverageTest.Arguments argumentPackage, IGeneratorContext context)
        {

            IList<Slb.Ocean.Petrel.DomainObject.Template> templates = new List<Slb.Ocean.Petrel.DomainObject.Template>();
            IList<Range1<float>> ranges = new List<Range1<float>>();

            templates.Add(Slb.Ocean.Petrel.DomainObject.Template.NullObject);

            ranges.Add(new Range1<float>(float.NaN, float.NaN));

            return new SeismicAttributeInfo(
                templates,
                ranges,
                new Index3(1, 1, 1),
                BorderProcessingMethod.Repeat);

        }

        /// <summary>
        /// Gets the category of the attribute
        /// </summary>
        public override string CategoryName
        {
            get { return WellKnownAttributeCategory.Basic; }
        }

        /// <summary>
        /// Gets the number of the expected input cubes
        /// </summary>
        public override int InputCount
        {
            get { return 1; }
        }

        public override int OutputCount
        {
            get { return 1; }
        }

        protected override IEnumerable<string> GetInputLabels(AmpAverageTest.Arguments argumentPackage, IGeneratorContext context)
        {
            yield return "Input";
        }

        protected override IEnumerable<string> GetOutputLabels(AmpAverageTest.Arguments argumentPackage, IGeneratorContext context)
        {
            yield return "Output";
        }


        #endregion

        #region Attribute Description related members

        public IDescription Description
        {
            get { return new AttributeDescription(); }
        }

        private class AttributeDescription : IDescription
        {
            #region IDescription Members

            /// <summary>
            /// Gets the name of the attribute
            /// </summary>
            public string Name
            {
                get { return "AmpAverageTest"; }
            }

            /// <summary>
            /// Gets the description of the attribute
            /// </summary>
            public string Description
            {
                get { return ""; }
            }

            /// <summary>
            /// Gets the short description of the attribute
            /// Currently it is not in use.
            /// </summary>
            public string ShortDescription
            {
                get { return string.Empty; }
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// This class contains the arguments of the attribute, if it has any.
        /// </summary>
        [Archivable(FromRelease = "2018.1")]
        public class Arguments : IIdentifiable
        {
            public Arguments() { }

            [Archived(Name = "Droid")]
            private Droid droid;
            public Droid Droid
            {
                get { return droid; }
                set { droid = value; }
            }

            [Archived(Name = "Cube1")]
            public Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube Cube1 { get; set; }

            [Archived(Name = "Cube2")]
            public Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube Cube2 { get; set; }


            public void CopyFrom(Arguments another)
            {
                // TODO: implement the argument copying
                throw new NotImplementedException();
            }

            public bool EqualsTo(Arguments another)
            {
                // TODO: implement the argument comparing.
                // return true if the arguments are considered equal,
                // return false if they are considered not equal.

                throw new NotImplementedException();
            }

        }

        public class ArgumentPackageDataSourceFactory : DataSourceFactory
        {
            public static string DataSourceId = @"57293ac7-75a0-4423-87d0-6e9fb14d7a4f";
            public override IDataSource GetDataSource()
            {
                return new StructuredArchiveDataSource(DataSourceId, new[] { typeof(Arguments) });
            }
        }


        public class Generator : SeismicAttributeGenerator
        {
            /// <summary>
            /// Argument package
            /// </summary>
            private AmpAverageTest.Arguments arguments;
            /// <summary>
            /// Generator context for the attribute
            /// </summary>
            private IGeneratorContext generatorContext;

            /// <summary>
            /// Parameterized constructor to set argument package and generator context
            /// </summary>
            /// <param name="arguments">Argument package</param>
            /// <param name="context">Generator context</param>
            public Generator(AmpAverageTest.Arguments arguments, IGeneratorContext generatorContext)
            {
                this.arguments = arguments;
                this.generatorContext = generatorContext;
            }

            #region Overrides from SeismicAttributeGenerator

            public override void Initialize()
            {
                // TODO: add any initialization logic here
            }

            /// <summary>
            /// This method does the actual work of the attribute.
            /// </summary>
            /// <param name="input">array of the input subcubes</param>
            /// <param name="output">the result cube</param>
            public override void Calculate(ISubCube[] input, ISubCube[] output)
            {
                // TODO: Implement the attribute behaviour here
                return;
            }

            #endregion
        }

        public class UIFactory : SeismicAttributeUIFactory<AmpAverageTest.Arguments>
        {
            public class Page1 : SeismicAttributePageUI<AmpAverageTest.Arguments>, IAppearance
            {
                string IAppearance.Text { get{ return "Parameter"; } }
                System.Drawing.Bitmap IAppearance.Image { get{ return null; } }
                event EventHandler<TextChangedEventArgs> IAppearance.TextChanged { add { } remove { } }
                event EventHandler<ImageChangedEventArgs> IAppearance.ImageChanged { add { } remove { } }

                protected override System.Windows.Forms.Control CreateUI(AmpAverageTest.Arguments argumentPackage, IGeneratorContext context)
                {
                    System.Windows.Forms.Control control = new Parameter(argumentPackage, context);
                    return control;
                }
            }

            public override IEnumerable<SeismicAttributePageUI> Pages()
            {
                // TODO: Add each page class constructor listed above to create page list
                return new SeismicAttributePageUI[] {
                    new Page1()};
            }
        }

    }
}
