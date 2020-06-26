using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Slb.Ocean.Petrel.Seismic;

namespace OceanTestingZone
{
    partial class Parameter : UserControl
    {
        private AmpAverageTest.Arguments arguments = null;
        private IGeneratorContext context = null;

        public Parameter(AmpAverageTest.Arguments arguments, IGeneratorContext context)
        {
            this.arguments = arguments;
            this.context = context;

            InitializeComponent();
        }
    }
}
