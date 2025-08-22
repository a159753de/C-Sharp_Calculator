using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Claculator
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private string input = "";

        public MainWindow()
        {
            InitializeComponent();
        }


        public void Insert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = show.Text;
                string preorder = Preorder.Text;
                string postorder = Postorder.Text;
                string binaryResult = Binary.Text;
                double decimalResult = double.Parse(Decimal.Text);
                
                DbConn dbConn = new DbConn();

                // 先確認是否有相同的expression存在?
                ExpressionRecord existed = dbConn.QueryByExpression(expression);
                if (existed != null)
                {
                    MessageBox.Show("該算式已存在!");
                    return;
                }

                if (show.Text != "0" && (Preorder.Text == "0" || Postorder.Text == "0") || Binary.Text == "0" || Decimal.Text == "0")
                {
                    MessageBox.Show("請先按下\"=\"再進行Insert!");
                    return;
                }

                bool insertResult = dbConn.Insert(expression, preorder, postorder, decimalResult, binaryResult);
                if (insertResult)
                {
                    MessageBox.Show("Insert成功!");
                }
                else
                {
                    MessageBox.Show("Insert失敗!");
                }
            }
            catch(FormatException fe)
            {
                MessageBox.Show("Decimal欄位不能為空!");
            }
        }

        public void Query_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
        }

        public void Button_Click_For_Number(object sender, RoutedEventArgs e)
        {
            /*1.顯示在TextBlock上 */
            // 獲取btn
            var button = sender as Button;
            // 取得btn值(轉string)
            string value = button.Content.ToString();

            if (value == "0" && input == "") {
                input = "";
                return;
            }
            if ((value == "+" || value == "-" || value == "*" || value == "/") && input == "") {
                input = "";
                return;
            }

            // 將值show在畫面上
            input += value;
            show.Text = input;

        }

        public void Equal(object sender, RoutedEventArgs e)
        {
            // 用DataTable的函數直接運算
            try
            {
                int result = Convert.ToInt32(new DataTable().Compute(input, null));

                // show在Decimal
                Decimal.Text = result.ToString();
                // 轉成Binary
                Binary.Text = Convert.ToString(result, 2);
                // 轉成Preorder
                Preorder.Text = ToPreorder(input);
                // 轉成Postorder
                Postorder.Text = ToPostorder(input);

                // 顯示在畫面上
                //input = result.ToString();
                //show.Text = input;
            }
            catch
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show("請先輸入數字");
            }
        }


        public void Clear(object sender, RoutedEventArgs e)
        {
            input = "";
            show.Text ="0";
            Preorder.Text = "0";
            Postorder.Text = "0";
            Decimal.Text = "0";
            Binary.Text = "0";
        }

        public int Precedence(char input)
        {
            int val;
            switch (input)
            {
                case '*': 
                case '/':
                    val = 1;
                    break;
                case '+':
                case '-':
                    val= 0;
                    break;

                default:
                    val = -1;
                    break;
            }
            return val;
        }

        public string ToPostorder(string input)
        {
            string postorderText = "";
            Stack<char> stack = new Stack<char>();

            for(int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                // 若字元是數字，直接加入postorderText
                if(char.IsDigit(c))
                {
                    postorderText += c;
                }
                else
                {
                    /* 若為operater，判斷他與stack.top()的優先權大小*/
                    // 若stack為空，則直接推入stack
                    if(stack.Count() == 0)
                    {
                        stack.Push(c);
                    }
                    else
                    {
                        //比較優先權，比較大就push入stack
                        if(Precedence(c) > Precedence(stack.Peek())) 
                        {
                            stack.Push(c);
                        }
                        else
                        {
                            // 優先權比較小，一路pop直到優先權比較大，再push進去
                            while (stack.Count > 0 && (Precedence(c) <= Precedence(stack.Peek())))
                            {
                                postorderText += stack.Pop();
                            }
                            stack.Push(c);
                        }
                    }
                }
            }
            while(stack.Count() > 0) 
            {
                postorderText += stack.Pop();
            }
            ;
            return postorderText;
        }

        public string ToPreorder(string input)
        {

            string preorderText = "";
            string reversedStr = "";
            Stack<char> stack = new Stack<char>();

            // 1.反轉infix string
            char[] reversedInput = input.Reverse().ToArray();
            foreach (var c in reversedInput)
            {
                reversedStr += c;
            }

            // 2. 轉成postfix即得到答案
            string postfixStr = ToPostorder(reversedStr);
            char[] tokens = postfixStr.Reverse().ToArray();
            foreach (char token in tokens)
            {
                preorderText += token;
            }

            return preorderText;
        }
    }
}
