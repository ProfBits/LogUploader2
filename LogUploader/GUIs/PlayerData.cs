using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.GUIs
{
    public partial class PlayerData : UserControl
    {
        public PlayerData()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [Description("Image at start of row 20x20")]
        public Image ClassImage { get => pbClass.Image; set => pbClass.Image = value; }
        
        [Browsable(true)]
        [Description("The text in the Name col")]
        public string DisplayName { get => lblName.Text; set => lblName.Text = value; }
        
        [Browsable(true)]
        [Description("The test in the sg should be singel digit")]
        public string SubGroup { get => lblSG.Text; set => lblSG.Text = value; }
        
        [Browsable(true)]
        [Description("the dps field, the last one")]
        public string DPS { get => lblDPS.Text; set => lblDPS.Text = value; }
    }
}
