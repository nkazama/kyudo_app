using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayerListForm
{
    public partial class Form1 : Form
    {
        private List<PlayerPanel> PlayerList;

        int PlayerNum;

        public Form1()
        {
            InitializeComponent();
            PlayerNum = 0;

            //PlayerList = new List<PlayerPanel>();
            //AddedPlayerList();
        }

        private void AddedPlayerList()
        {
            PlayerPanel player = new PlayerPanel();
            player.InitializeComponent(PlayerNum);
            PlayerList.Add(player);
            this.Controls.Add(PlayerList[PlayerList.Count - 1]);
            PlayerNum++;
        }
    }
}
