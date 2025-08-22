using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Claculator
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void showQueryBtn_Click(object sender, RoutedEventArgs e)
        {
            DbConn dbConn = new DbConn();
            List<ExpressionRecord> ExpressionRecord = dbConn.QueryAll();

            if (ExpressionRecord.Count() == 0)
            {
                MessageBox.Show("資料庫無內容，請進行新增!");
                return;
            }

            foreach (var record in ExpressionRecord)
            {
                string s = $"ID: {record.ID}, Expression: {record.Expression}, CreatedAt: {record.CreatedAt}, Preorder: {record.Preorder}, Postorder: {record.Postorder}, DecimalResult: {record.DecimalResult}, BinaryResult: {record.BinaryResult}\n";
                queryResult.Text += s;
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            DbConn dbConn = new DbConn();
            bool result = dbConn.DeleteAll();
            if (result)
            {
                MessageBox.Show("資料刪除成功!");
                queryResult.Text = "";
            }
            else
            {
                MessageBox.Show("資料刪除失敗!");
            }
        }
    }
}
