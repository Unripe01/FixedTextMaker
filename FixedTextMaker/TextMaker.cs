using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using FixedTextMaker.Model;
using System.IO;
using FixedTextMaker.Data;
using FixedTextMaker.CustomControls;

namespace FixedTextMaker
{
    public partial class TextMaker : Form
    {
        public TextMaker()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 定義された固定長テキストを設定または取得します。
        /// </summary>
        private FixedTextDefines DefinedItems { get; set; }

        /// <summary>
        /// テキストデータの解析を提供します。
        /// </summary>
        private TextDataAdapter MyTextDataAdapter { get; set; }

        /// <summary>
        /// 定義ファイルを読み込みます
        /// </summary>
        /// <param name="path"></param>
        private FixedTextDefines LoadDefinedItems(string path )
        {
            return Definitions.Load(path);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            tabControlMain.TabPages.Clear();
            MyTextBox.Clear();
            var dataFilePath = OpenFileDialogShow();
            if (string.IsNullOrWhiteSpace(dataFilePath)) return;
            DefinedItems = LoadDefinedItems(dataFilePath);
            MyTextDataAdapter = new TextDataAdapter(DefinedItems);
            MakeControls(DefinedItems);
        }

        /// <summary>
        /// オートスクロール可能なパネルに
        /// 定義されたラベル・テキストボックスを作成します
        /// </summary>
        private void MakeControls( FixedTextDefines source )
        {
            foreach (var record in source.Records)
            {
                //タブとパネル
                this.tabControlMain.TabPages.Add(record.Name, record.Name);
                var panel = new Panel()
                {
                    Dock = DockStyle.Fill
                    ,AutoScroll = true
                };

                //パネルの中身
                int y = 0;
                int startIndex = 0;
                foreach (var item in record.Items)
                {
                    var labelText = LabelTextFactory.Generate(item, startIndex);
                    labelText.Location = new Point(0, y);
                    panel.Controls.Add(labelText);
                    y += 25;
                    startIndex += item.Length;
                }
                this.tabControlMain.TabPages[record.Name].Controls.Add(panel);
            }
        }

        /// <summary>
        /// データファイルを開くボタンイベント
        /// </summary>
        private void btnOpenDataFile_Click(object sender, EventArgs e)
        {
            var dataFilePath = OpenFileDialogShow();
            if (string.IsNullOrWhiteSpace(dataFilePath)) return;
            var dataText = ReadFile(dataFilePath);

            //テキストボックスに描画
            MyTextDataAdapter = new TextDataAdapter(DefinedItems,dataText.ToString());
            MyTextBox.Text = MyTextDataAdapter.GetParsedText();
        }

        /// <summary>
        /// ファイル選択ダイアログを開き、選択されたファイルパスを取得します。
        /// </summary>
        /// <returns></returns>
        private string OpenFileDialogShow()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "DATAFILE";
            //ofd.InitialDirectory = @"C:\";
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = "txt csv(*.txt;*.csv)|*.txt;*.csv|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.Title = "開くファイルを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }
            return "";
        }

        /// <summary>
        /// データファイルを読み込み、文字オブジェクトを取得します。
        /// </summary>
        private StringBuilder ReadFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("Shift_JIS")))
            {
                var sb = new StringBuilder();
                sb.Append(sr.ReadToEnd());
                return sb;
            }
        }

        /// <summary>
        /// ステータスバー用
        /// </summary>
        private void richTextBox_SelectionChanged(object sender, EventArgs e)
        {
            Tuple<int, int> caretIndex = GetRowColIndex(MyTextBox);
            NotificationStatus(string.Format("{0}行　{1}列", caretIndex.Item1, caretIndex.Item2));
            BindTab();
            ((Control)sender).Focus();

            this.selectedItemHighLight();
        }

        private void NotificationStatus(string message)
        {
            toolStripStatusLabel1.Text = message;
        }

        /// <summary>
        /// 行列取得
        /// </summary>
        private Tuple<int, int> GetRowColIndex(RichTextBox tb)
        {
            //文字列
            string str = tb.Text;
            //カレットの位置を取得
            int selectPos = tb.SelectionStart;
            //カレットの位置までの行を数える
            int rowIndex = 0;
            int startPos = 0;
            for (int endPos = 0;
                (endPos = str.IndexOf('\n', startPos)) < selectPos && endPos > -1;
                rowIndex++)
            {
                startPos = endPos + 1;
            }
            //列の計算
            int colIndex = selectPos - startPos;

            return Tuple.Create<int, int>(rowIndex, colIndex);
        }

        /// <summary>
        /// 選択された行に合わせてタブを選択します。
        /// </summary>
        private void BindTab()
        {
            if (MyTextDataAdapter == null) return;
            var line = GetSelectedLine();
            if (line.Length == 0)return;
            
            try
            {
                var key = MyTextDataAdapter.GetKey(line);
                var record = MyTextDataAdapter.GetRecordKind(key);
                tabControlMain.SelectTab(record.Name);
                var labellTextList = tabControlMain.SelectedTab.Controls.OfType<Panel>().First()
                    .Controls.OfType<LabelText>();
                foreach (var tb in labellTextList)
                {
                    if (tb.TextBox.Text == "\\r\\n") break;
                    tb.TextBox.Text = line.Substring(tb.StartIndex, tb.ItemLength);
                }
                toolStripStatusLabel2.Text = "";
            }
            catch (ArgumentException ex)
            {
                toolStripStatusLabel2.Text = ex.Message;
            }
            catch (Exception exAll)
            {
                toolStripStatusLabel2.Text = exAll.Message;
            }

        }

        /// <summary>
        /// 選択されたタブの結合文字列を取得します。
        /// </summary>
        /// <returns></returns>
        private string GetSelectedTabsString()
        {
            var t = tabControlMain.SelectedTab.Controls.OfType<Panel>().First()
                .Controls.OfType<LabelText>().Select(v => v.TextBox.Text == "\\r\\n" ? "\r\n" : v.TextBox.Text);
            return string.Join("", t.ToList());
        }

        /// <summary>
        /// 選択された行のテキストを取得します。
        /// </summary>
        /// <returns></returns>
        private string GetSelectedLine()
        {
            Tuple<int, int> caretIndex = GetRowColIndex(MyTextBox);
            return MyTextBox.Text.Split('\n').ElementAt(caretIndex.Item1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(GetSelectionLine());
            MessageBox.Show(GetSelectedTabsString());
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            MyTextBox.SelectionChanged -= richTextBox_SelectionChanged;
            var tabTemp = tabControlMain.SelectedTab;
            var index = MyTextBox.GetFirstCharIndexOfCurrentLine();
            var insertText = GetSelectedTabsString();
            if ( ! MyTextDataAdapter.HasCrLf)
            {
                insertText = insertText + "\r\n";
            }
            var nextIndex = index + insertText.Length;
            MyTextBox.Text = MyTextBox.Text.Insert(index, insertText);
            MyTextBox.SelectionStart = nextIndex;

            tabControlMain.SelectedTab = tabTemp;
            MyTextBox.SelectionChanged += richTextBox_SelectionChanged;
            MyTextBox.Focus();
        }

        private void TextMaker_Load(object sender, EventArgs e)
        {
            tabControlMain.TabPages.Clear();
            MyTextBox.Clear();
            DefinedItems = LoadDefinedItems(@"FixedDefinitionXML.xml");
            MyTextDataAdapter = new TextDataAdapter(DefinedItems);
            MakeControls(DefinedItems);
        }

        /// <summary>
        /// 選択項目を強調表示します。
        /// </summary>
        private void selectedItemHighLight()
        {
            // 選択中のタブを取得
            TabPage page = this.tabControlMain.SelectedTab;

            // タブに配置されたテキストボックスの背景色をクリア
            foreach (Panel panel in page.Controls)
            {
                foreach (LabelText lt in panel.Controls)
                {
                    lt.BackColor = SystemColors.Control;
                }
            }

            // キャレットの位置を取得
            Tuple<int, int> caretIndex = this.GetRowColIndex(MyTextBox);

            // キャレット位置の項目のテキストボックスの背景色を変更
            foreach (Panel panel in page.Controls)
            {
                foreach (LabelText lt in panel.Controls)
                {
                    if (lt.StartIndex <= caretIndex.Item2 && (lt.StartIndex + lt.ItemLength) > caretIndex.Item2)
                    {
                        lt.BackColor = Color.Salmon;

                        break;
                    }
                }
            }
        }
    }
}
