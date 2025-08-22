using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Claculator
{
    internal class DbConn
    {
        private string connString = "server=127.0.0.1;uid=root;pwd=g5982734de;database=c#calculator";

        public List<ExpressionRecord> QueryAll()
        {
            List<ExpressionRecord> results = new List<ExpressionRecord>();

            using (MySqlConnection conn = new MySqlConnection(connString)) 
            {
                try
                {
                    //open a connection
                    conn.Open();
                    // set SQL
                    string sql = "SELECT * FROM expressions;";

                    // create a MySQL command
                    using (MySqlCommand myCommand = new MySqlCommand(sql, conn))
                    {
                        using(MySqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            while (myReader.Read())
                            {
                                ExpressionRecord record = new ExpressionRecord
                                {
                                    ID = myReader.GetInt32("id"),
                                    Expression = myReader.GetString("expression"),
                                    CreatedAt = myReader.GetDateTime("created_at"),
                                    Preorder = myReader.GetString("preorder"),
                                    Postorder = myReader.GetString("postorder"),
                                    DecimalResult = myReader.GetDouble("decimal_result"),
                                    BinaryResult = myReader.GetString("binary_result")
                                };
                                results.Add(record);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                    return null;
                }
            }
            return results;
        }

        
        public ExpressionRecord QueryByExpression(string expression) 
        {
            
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    //open a connection
                    conn.Open();
                    Console.WriteLine("連線建立成功!");
                    // set SQL
                    string sql = "SELECT * FROM expressions WHERE expression = @expression;";

                    // create a MySQL command
                    using (MySqlCommand myCommand = new MySqlCommand(sql, conn))
                    {
                    myCommand.Parameters.AddWithValue("@expression", expression);
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            if (myReader.Read())
                            {
                                ExpressionRecord record = new ExpressionRecord();
                                record.ID = myReader.GetInt32("id");
                                record.Expression = myReader.GetString("expression");
                                record.CreatedAt = myReader.GetDateTime("created_at");
                                record.Preorder = myReader.GetString("preorder");
                                record.Postorder = myReader.GetString("postorder");
                                record.DecimalResult = myReader.GetDouble("decimal_result");
                                record.BinaryResult = myReader.GetString("binary_result");

                                return record;
                            }
                            else 
                            {
                                return null;
                            }

                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                    return null;
                }
            }
        }
        public bool Insert(string expression, string preorder, string postorder, double decimalResult, string binaryResult)
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    //open a connection
                    conn.Open();
                    Console.WriteLine("連線建立成功!");
                    // set SQL
                    string sql = "INSERT INTO expressions (expression, created_at, preorder, postorder, decimal_result, binary_result) VALUES(@expression, @createdAt, @preorder, @postorder, @decimalResult, @binaryResult);";

                    // create a MySQL command
                    using (MySqlCommand myCommand = new MySqlCommand(sql, conn))
                    {
                        // 加入參數
                        myCommand.Parameters.AddWithValue("@expression", expression);
                        myCommand.Parameters.AddWithValue("@createdAt", DateTime.Now);
                        myCommand.Parameters.AddWithValue("@preorder", preorder);
                        myCommand.Parameters.AddWithValue("@postorder", postorder);
                        myCommand.Parameters.AddWithValue("@decimalResult", decimalResult);
                        myCommand.Parameters.AddWithValue("@binaryResult", binaryResult);

                        myCommand.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return true;
        }

        public bool DeleteAll() {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                try
                {
                    //open a connection
                    conn.Open();
                    Console.WriteLine("連線建立成功!");
                    // set SQL
                    string sql = "DELETE FROM expressions;";

                    // create a MySQL command
                    using (MySqlCommand myCommand = new MySqlCommand(sql, conn))
                    {
                            int affectedRows = myCommand.ExecuteNonQuery();
                            return affectedRows >= 0;
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
        }
    }
}
