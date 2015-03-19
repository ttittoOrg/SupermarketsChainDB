using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SQLManager
{
    using System.Data;
    using System.Globalization;

    public class SqlManagerConnection
    {
        private string connectionString;

        public SqlManagerConnection(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }

            set
            {
                this.connectionString = value;
            }
        }

        public List<Product> GetProducts(DateTime startDate, DateTime endDate)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);
            SqlCommand command = new SqlCommand();
            
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("p_date_from", startDate);
            command.Parameters.AddWithValue("p_date_to", endDate);
            DataSet result = new DataSet();
            List<Product> productResult = new List<Product>();

            command.Connection = connection;

            command.CommandText = "usp_get_procducts_total_period";
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            try
            {
                connection.Open();
                //command.ExecuteNonQuery();
                adapter.Fill(result);

                if (result.Tables[0].Rows.Count > 0)
                {
                    Product product;
                    for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                    {

                        product = new Product();
                        product.Name = result.Tables[0].Rows[i]["Product"].ToString();
                        product.Quantity = Convert.ToInt32(result.Tables[0].Rows[i]["Quantity"].ToString());
                        product.Price = Convert.ToDecimal(result.Tables[0].Rows[i]["Unit_price"].ToString());
                        product.Location = result.Tables[0].Rows[i]["Location"].ToString();
                        product.Sum = Convert.ToDecimal(result.Tables[0].Rows[i]["Summ"].ToString());
                        product.LoadingDate = DateTime.ParseExact(result.Tables[0].Rows[i]["LoadingDate"].ToString(), "yyyymmdd", CultureInfo.InvariantCulture);
                        product.Total = Convert.ToDecimal(result.Tables[0].Rows[i]["Total"].ToString());
                        productResult.Add(product);
                    }
                }
                return productResult;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}