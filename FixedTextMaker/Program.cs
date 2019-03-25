using System;
using System.Windows.Forms;

namespace FixedTextMaker
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //ThreadExceptionイベントハンドラを追加
            Application.ThreadException +=
                new System.Threading.ThreadExceptionEventHandler(
                    Application_ThreadException);
            //常にThreadExceptionが発生されるようにする
            Application.SetUnhandledExceptionMode(
                UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TextMaker());
        }

        //ThreadExceptionイベントハンドラ
        private static void Application_ThreadException(object sender,
            System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                //エラーメッセージを表示する
                MessageBox.Show(e.Exception.Message, "エラー");
            }
            finally
            {
                //アプリケーションを終了する
                //Application.Exit();
            }
        }
    }
}
