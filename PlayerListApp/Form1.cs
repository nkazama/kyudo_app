using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PlayerListApp
{
    public partial class Form1 : Form
    {
        const string FILENAME_PLAYERLIST = "result_data\\PlayerList.csv";

        enum TABLE_COL
        {
            COL_BIB_NO,
            COL_NAME,
            COL_GROUP,
            COL_ATTENDANCE,
            COL_MEMO,
        };

        private List<PlayerPanel> PlayerList;

        int PlayerNum;

        AutoCompleteStringCollection autoCompList;

        public Form1()
        {
            InitializeComponent();
            PlayerNum = 1;

            if (File.Exists(FILENAME_PLAYERLIST))
            {
                Load_FromCSVPlayerList();
            }

            //PlayerList = new List<PlayerPanel>();
            //AddedPlayerList();
            autoCompList = new AutoCompleteStringCollection();
            //textBox1.AutoCompleteCustomSource = autoCompList;

            // 候補リストに項目を追加（初期設定）
            autoCompList.AddRange(
              new string[] {
              "大和市弓道協会", "joey", "monica", "phoebe", "ross","rachel",
            }
            );
        }

        private void AddedPlayerList()
        {
            PlayerPanel player = new PlayerPanel();
            player.InitializeComponent(PlayerNum);
            //player.SetPlayerID(PlayerNum);

            PlayerList.Add(player);

            this.Controls.Add(PlayerList[PlayerList.Count - 1]);
            PlayerNum++;
        }

        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            PlayerNum++;
            int row = e.RowIndex;
            DataGridView obj = (DataGridView)sender;
            obj[(int)TABLE_COL.COL_BIB_NO, row].Value = PlayerNum;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.S))
            {
                // 保存処理
                // Ctrl + S
                //toolStripStatusLabel1.Text = "Ctrl + S が押されました。";
                SaveData_FromCSVPlayerList();
            }
        }

#region Save Datas
        private void SaveData_FromCSVPlayerList()
        {
            StreamWriter sw = new StreamWriter(FILENAME_PLAYERLIST, false, System.Text.Encoding.GetEncoding("shift_jis"));

            int row_max = this.dataGridView1.RowCount;
            for (int i = 0; i < row_max; i++)
            {
                //object o = this.;
                //Boolean b = o as Boolean;
                //string attendance = (o.Checked) ? "欠" : "";
                sw.WriteLine(
                    this.dataGridView1[0, i].Value 
                    + ","
                    + this.dataGridView1[1, i].Value
                    + ","
                    + this.dataGridView1[2, i].Value
                    + ","
                    + this.dataGridView1[3, i].Value
                    + ","
                    + this.dataGridView1[4, i].Value
                    );
            }
            sw.Close();
        }
        private void Load_FromCSVPlayerList()
        {
            string text = "";
            using (StreamReader sr = new StreamReader(FILENAME_PLAYERLIST, Encoding.GetEncoding("Shift_JIS")))
            {
                int row = 0;
                while ((text = sr.ReadLine()) != null)
                {
                    //if (row >= this.dataGridView1.RowCount)
                    //{
                        this.dataGridView1.Rows.Add();
                    //}

                    string[] stArrayData = text.Split(',');
                    for(int i = 0; i < stArrayData.Length; i++)
                    {
                        if (i == (int)TABLE_COL.COL_ATTENDANCE)
                        {
                            this.dataGridView1[i, row].Value = (stArrayData[i] == "") ? "False" : stArrayData[i];
                        }
                        else
                            this.dataGridView1[i, row].Value = stArrayData[i];
                    }
                    row++;
                }
            }
        }

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            if (tb == null)
            {
                // テキストボックスでなければ何もしない
                return;
            }

            if (dataGridView1.CurrentCell.ColumnIndex == (int)TABLE_COL.COL_GROUP)
            {
                // COL_GROUP列目の場合にはオートコンプリート
                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                tb.AutoCompleteCustomSource = autoCompList;
                //tb.AutoCompleteMode
                
            }
            else
            {
                // COL_GROUP列目以外ではオートコンプリートをオフ
                tb.AutoCompleteMode = AutoCompleteMode.None;
            }
        }
    }
#endregion

}
