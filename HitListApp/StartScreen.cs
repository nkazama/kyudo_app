using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HitListApp
{
    public partial class StartScreen : Form
    {
        int mDisplayPlayNum;
        public StartScreen()
        {
            InitializeComponent();
        }
        public int GetOption_DisplayNum()
        {
            return mDisplayPlayNum;
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            mDisplayPlayNum = (int)MaxPlayerNum.Value;
            this.Close();
        }
    }
}
