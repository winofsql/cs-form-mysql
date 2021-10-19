using System;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace cs_form_mysql_datagridview
{
    public partial class Form1 : Form
    {
        // *****************************
        // SQL文字列格納用
        // *****************************
        private string query = "select * from 社員マスタ";

        // *****************************
        // 接続文字列作成用
        // *****************************
        private OdbcConnectionStringBuilder builder = new OdbcConnectionStringBuilder();

        public Form1()
        {
            InitializeComponent();

            SetBuilderData();
        }

        // *****************************
        // 接続文字列の準備
        // *****************************
        private void SetBuilderData()
        {
            // ドライバ文字列をセット ( 波型括弧{} は必要ありません ) 
            builder.Driver = "MySQL ODBC 8.0 Unicode Driver";

            // 接続用のパラメータを追加
            builder.Add("server", "localhost");
            builder.Add("database", "lightbox");
            builder.Add("uid", "root");
            builder.Add("pwd", "");
        }

        // *****************************
        // SELECT 文よりデータ表示
        // *****************************
        private void LoadMySQL()
        {

            // 接続と実行用のクラス
            using (OdbcConnection connection = new OdbcConnection())
            using (OdbcCommand command = new OdbcCommand())
            {
                // 接続文字列
                connection.ConnectionString = builder.ConnectionString;

                try
                {
                    // 接続文字列を使用して接続
                    connection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                // コマンドオブジェクトに接続をセット
                command.Connection = connection;
                // コマンドを通常 SQL用に変更
                command.CommandType = CommandType.Text;

                // *****************************
                // 実行 SQL
                // *****************************
                command.CommandText = query;

                try
                {
                    // レコードセット取得
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        // データを格納するテーブルクラス
                        DataTable dataTable = new DataTable();

                        // DataReader よりデータを格納
                        dataTable.Load(reader);

                        // 画面の一覧表示用コントロールにセット
                        dataGridView1.DataSource = dataTable;

                        // リーダを使い終わったので閉じる
                        reader.Close();
                    }

                }
                catch (Exception ex)
                {
                    // 接続解除
                    connection.Close();
                    MessageBox.Show(ex.Message);
                    return;
                }

                // 接続解除
                connection.Close();
            }

            // カラム幅の自動調整
            dataGridView1.AutoResizeColumns();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoadMySQL();
        }
    }
}
