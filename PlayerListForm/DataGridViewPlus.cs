using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PlayerListForm
{
    public class DataGridViewPlus : System.Windows.Forms.DataGridView
    {
        // ----------------------------------------------------------------------------------
        // DataGridViewのカラム幅をファイルへ保存／読込するための実装↓↓↓↓↓↓↓↓↓↓↓↓
        // ----------------------------------------------------------------------------------
        /// <summary>
        /// DataGridViewのカラム幅をXML形式でシリアライズするためのクラス
        /// </summary>
        public class ColWidths
        {
            /// <summary>
            /// カラム幅s
            /// </summary>
            public int[] Widths;
        }

        /// <summary>
        /// DataGridViewのカラム幅をファイルへ保存
        /// </summary>
        /// <remarks>DataGridViewのカラム幅をXML形式でシリアライズ</remarks>
        public void SaveColWidths()
        {
            try
            {
                // EXEファイルのPATH
                String ExePath = System.AppDomain.CurrentDomain.BaseDirectory;
                // XMLファイルのPATH
                String XmlPath = ExePath + "\\" + this.Parent.Name + "_" + this.Name + ".xml";
                // XMLファイルオープン
                StreamWriter sw = new StreamWriter(XmlPath, false, Encoding.Default);
                try
                {
                    // シリアライザー
                    XmlSerializer serializer = new XmlSerializer(typeof(ColWidths));
                    // DataGridViewのカラム幅取得
                    ColWidths colw = new ColWidths();
                    Array.Resize<int>(ref colw.Widths, this.Columns.Count);
                    for (int i = 0; i <= this.Columns.Count - 1; i++)
                    {
                        colw.Widths[i] = this.Columns[i].Width;
                    }
                    // XMLファイル保存
                    serializer.Serialize(sw, colw);
                }
                catch { }
                finally
                {
                    // XMLファイルクローズ
                    if (sw != null) sw.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// DataGridViewのカラム幅を前回保存したファイルから読込
        /// </summary>
        /// <remarks>DataGridViewのカラム幅をXML形式でデシリアライズ</remarks>
        public void ReadColWidths()
        {
            try
            {
                // EXEファイルのPATH
                String ExePath = System.AppDomain.CurrentDomain.BaseDirectory;
                // XMLファイルのPATH
                String XmlPath = ExePath + "\\" + this.Parent.Name + "_" + this.Name + ".xml";
                // XMLファイルオープン
                StreamReader sr = new StreamReader(XmlPath, Encoding.Default);
                try
                {
                    // シリアライザー
                    XmlSerializer serializer = new XmlSerializer(typeof(ColWidths));
                    // XMLファイル読込
                    ColWidths colw = new ColWidths();
                    colw = (ColWidths)(serializer.Deserialize(sr));
                    // DataGridViewにカラム幅設定
                    for (int i = 0; i <= this.Columns.Count - 1; i++)
                    {
                        this.Columns[i].Width = colw.Widths[i];
                    }
                }
                catch { }
                finally
                {
                    if (sr != null) sr.Close();
                }
            }
            catch { }
        }
        // ----------------------------------------------------------------------------------
        // DataGridViewのカラム幅をファイルへ保存／読込するための実装↑↑↑↑↑↑↑↑↑↑↑↑
        // ----------------------------------------------------------------------------------

        // ----------------------------------------------------------------------------------
        // DataGridViewの各カラムへの入力可能文字を制限するための実装↓↓↓↓↓↓↓↓↓↓↓↓
        // ----------------------------------------------------------------------------------
        /// <summary>
        /// カラムへの入力可能文字を指定するための配列
        /// </summary>
        /// <remarks>ColumnChars[0]="1234567890"</remarks>
        public String[] ColumnChars = new String[] { };
        /// <summary>
        /// 編集中のカラム番号
        /// </summary>
        /// <remarks></remarks>
        private int _editingColumn;
        /// <summary>
        /// 編集中のTextBoxEditingControl
        /// </summary>
        /// <remarks></remarks>
        DataGridViewTextBoxEditingControl _editingCtrl;

        public DataGridViewPlus()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridViewPlus
            // 
            this.RowTemplate.Height = 21;
            this.CellEndEdit +=
                new System.Windows.Forms.DataGridViewCellEventHandler(
                this.DataGridViewPlus_CellEndEdit);
            this.EditingControlShowing +=
                new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(
                this.DataGridViewPlus_EditingControlShowing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(
                this.DataGridViewPlus_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// セルが編集中になった時の処理
        /// </summary>
        /// <param name="sender">イベントの発生元</param>
        /// <param name="e">イベントの情報</param>
        /// <remarks>編集中のTextBoxEditingControlにKeyPressイベント設定</remarks>
        private void DataGridViewPlus_EditingControlShowing(
            object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // 編集中のカラム番号を保存
            _editingColumn = ((DataGridView)(sender)).CurrentCellAddress.X;
            try
            {
                // 編集中のTextBoxEditingControlにKeyPressイベント設定
                _editingCtrl = ((DataGridViewTextBoxEditingControl)(e.Control));
                _editingCtrl.KeyPress +=
                    new KeyPressEventHandler(DataGridViewPlus_CellKeyPress);
            }
            catch { }
        }

        /// <summary>
        /// セルの編集が終わった時の処理
        /// </summary>
        /// <param name="sender">イベントの発生元</param>
        /// <param name="e">イベントの情報</param>
        /// <remarks>編集中のTextBoxEditingControlからKeyPressイベント削除</remarks>
        private void DataGridViewPlus_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_editingCtrl != null)
            {
                // 編集中のTextBoxEditingControlからKeyPressイベント削除
                _editingCtrl.KeyPress -=
                    new KeyPressEventHandler(DataGridViewPlus_CellKeyPress);
                _editingCtrl = null;

            }
        }

        /// <summary>
        /// 編集中のTextBoxEditingControlのKeyPressの処理
        /// </summary>
        /// <param name="sender">イベントの発生元</param>
        /// <param name="e">イベントの情報</param>
        /// <remarks>力可能文字の判定</remarks>
        private void DataGridViewPlus_CellKeyPress(object sender, KeyPressEventArgs e)
        {
            // カラムへの入力可能文字を指定するための配列が指定されているかチェック
            if (ColumnChars.GetType().IsArray)
            {
                // カラムへの入力可能文字を指定するための配列数チェック
                if (ColumnChars.GetLength(0) - 1 >= _editingColumn)
                {
                    // カラムへの入力可能文字が指定されているかチェック
                    if (ColumnChars[_editingColumn] != "")
                    {
                        //カラムへの入力可能文字かチェック
                        if (ColumnChars[_editingColumn].IndexOf(e.KeyChar) < 0 &&
                            e.KeyChar != (char)Keys.Back)
                        {
                            // カラムへの入力可能文字では無いので無効
                            e.Handled = true;
                        }
                    }
                }
            }
        }
        // ----------------------------------------------------------------------------------
        // DataGridViewの各カラムへの入力可能文字を制限するための実装↑↑↑↑↑↑↑↑↑↑↑↑
        // ----------------------------------------------------------------------------------

        // ----------------------------------------------------------------------------------
        // DataGridViewでCtrl-Vキー押下時にクリップボードから貼り付けるための実装↓↓↓↓↓↓
        // DataGridViewでDelやBackspaceキー押下時にセルの内容を消去するための実装↓↓↓↓↓↓
        // ----------------------------------------------------------------------------------
        private void DataGridViewPlus_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dgv = (DataGridView)(sender);
            int x = dgv.CurrentCellAddress.X;
            int y = dgv.CurrentCellAddress.Y;
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                // セルの内容を消去
                dgv[x, y].Value = "";
            }
            else if ((e.Modifiers & Keys.Control) == Keys.Control && e.KeyCode == Keys.V)
            {
                // クリップボードの内容を取得
                String clipText = Clipboard.GetText();
                // 改行を変換
                clipText = clipText.Replace("\r\n", "\n");
                clipText = clipText.Replace("\r", "\n");
                // 改行で分割
                String[] lines = clipText.Split('\n');

                int r;
                Boolean nflag = true;
                for (r = 0; r <= lines.GetLength(0) - 1; r++)
                {
                    // 最後のNULL行をコピーするかどうか
                    if (r >= lines.GetLength(0) - 1 &&
                        "".Equals(lines[r]) && nflag == false) break;
                    if ("".Equals(lines[r]) == false) nflag = false;

                    // タブで分割
                    String[] vals = lines[r].Split('\t');

                    // 各セルの値を設定
                    int c = 0;
                    int c2 = 0;
                    for (c = 0; c <= vals.GetLength(0) - 1; c++)
                    {
                        // セルが存在しなければ貼り付けない
                        if (!(x + c2 >= 0 && x + c2 < dgv.ColumnCount &&
                            y + r >= 0 && y + r < dgv.RowCount))
                        {
                            continue;
                        }
                        // 非表示セルには貼り付けない
                        if (dgv[x + c2, y + r].Visible == false)
                        {
                            c = c - 1;
                            continue;
                        }
                        //// 貼り付け処理(入力可能文字チェック無しの時)------------
                        //// 行追加モード&(最終行の時は行追加)
                        //if (y + r == dgv.RowCount - 1 &&
                        //    dgv.AllowUserToAddRows == true)
                        //{
                        //    dgv.RowCount = dgv.RowCount + 1;
                        //}
                        //// 貼り付け
                        //dgv[x + c2, y + r].Value = vals[c];
                        //// ------------------------------------------------------
                        // 貼り付け処理(入力可能文字チェック有りの時)------------
                        String pststr = "";
                        for (int i = 0; i <= vals[c].Length - 1; i++)
                        {
                            _editingColumn = x + c2;
                            byte[] cc = Encoding.GetEncoding(
                                "SHIFT-JIS").GetBytes(vals[c].Substring(i, 1));
                            KeyPressEventArgs tmpe = new KeyPressEventArgs((char)cc[0]);
                            tmpe.Handled = false;
                            DataGridViewPlus_CellKeyPress(sender, tmpe);
                            if (tmpe.Handled == false)
                            {
                                pststr = pststr + vals[c].Substring(i, 1);
                            }
                        }
                        // 行追加モード＆最終行の時は行追加
                        if (y + r == dgv.RowCount - 1 &&
                            dgv.AllowUserToAddRows == true)
                        {
                            dgv.RowCount = dgv.RowCount + 1;
                        }
                        // 貼り付け
                        dgv[x + c2, y + r].Value = pststr;
                        // ------------------------------------------------------
                        // 次のセルへ
                        c2 = c2 + 1;
                    }
                }
            }
        }
        // ----------------------------------------------------------------------------------
        // DataGridViewでCtrl-Vキー押下時にクリップボードから貼り付けるための実装↑↑↑↑↑↑
        // DataGridViewでDelやBackspaceキー押下時にセルの内容を消去するための実装↑↑↑↑↑↑
        // ----------------------------------------------------------------------------------
    }
}