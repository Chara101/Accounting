using System.Data.Common;
using System.Data;
using AccountAPI.EnumList;
using AccountAPI.Models;
using Microsoft.Data.SqlClient;
using TestAcounting.DataStorage;

namespace AccountAPI.DataStorage
{
    public class MssqlCtrl : IDataStorage
    {
        private Dictionary<string, string> _condition = new Dictionary<string, string>()
        {
            { "id", "record_id = @id" },
            { "time", "record_date = @date" },
            { "category_id", "category_id = @category" },
            { "subcategory_id", "subcategory_id = @sid"},
            { "money", "record_amount = @amount" }
        };
        private void Update_totals(RecordForm r, bool pos)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "update Totals set subcount = subcount + 1 , subamount = subamount + @amount where category_id = @tcid and subcategory_id = @tsid";
                int affect_row = 0;
                while (affect_row == 0)
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters.Add("@tcid", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@tsid", SqlDbType.NVarChar, 50);
                        command.Parameters["@amount"].Value = pos ? r.Amount : r.Amount * -1;
                        command.Parameters["@tcid"].Value = r.Category_id;
                        command.Parameters["@tsid"].Value = r.SubCategory_id;
                        affect_row = command.ExecuteNonQuery();
                    }
                    if (affect_row == 0)
                    {
                        string sql2 = "insert into Totals (category_id, subcategory_id) values ( @category, @subcategory)";
                        using (SqlCommand command2 = new SqlCommand(sql2, connection))
                        {
                            command2.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                            command2.Parameters.Add("@subcategory", SqlDbType.NVarChar, 50);
                            command2.Parameters["@category"].Value = r.Category_id;
                            command2.Parameters["@subcategory"].Value = r.SubCategory_id;
                            command2.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update_totals failed." + e.Message);
            }
        }
        public void Add(RecordForm r)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "insert into Record (category_id, subcategory_id, user_id, record_amount, description) values ( @cid, @scid, @uid, @amount, @comment);";
                using(SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@cid", SqlDbType.Int);
                    command.Parameters.Add("@scid", SqlDbType.Int);
                    command.Parameters.Add("@uid", SqlDbType.Int);
                    command.Parameters.Add("@amount", SqlDbType.Int);
                    command.Parameters.Add("@comment", SqlDbType.NVarChar, 50);
                    command.Parameters["@cid"].Value = r.Category_id;
                    command.Parameters["@scid"].Value = r.SubCategory_id;
                    command.Parameters["@uid"].Value = r.User_id;
                    command.Parameters["@amount"].Value = r.Amount;
                    command.Parameters["@comment"].Value = string.IsNullOrEmpty(r.Comment) ? "nothing" : r.Comment;
                    command.ExecuteNonQuery();
                    Update_totals(r, true); //之後加上交易紀錄成功才更新統計資料
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Add failed." + e.Message);
            }
        }
        public void AddCategory(string name)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "insert into CategoryList (category_name) values ( @name);";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@name", SqlDbType.NVarChar, 50);
                    command.Parameters["@name"].Value = name;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AddCategory failed." + e.Message);
            }
        }
        public void AddSubCategory(int category_id, string name)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;
                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "INSERT INTO SubCategoryList\r\n (subcategory_name)\r\n VALUES ( @name ) ;\r\n\r\nINSERT INTO CategoryWithSubcategory \r\n\t(category_id, subcategory_id) \r\nVALUES ( @cid, (SELECT TOP 1 \r\n\tsubcategory_id\r\n\tFROM SubCategoryList\r\n\tORDER BY subcategory_id DESC\r\n))";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@cid", SqlDbType.Int);
                    command.Parameters.Add("@name", SqlDbType.NVarChar, 50);
                    command.Parameters["@cid"].Value = category_id;
                    command.Parameters["@name"].Value = name;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AddSubCategory failed." + e.Message);
            }
        }
        public void Remove(RecordForm r)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "DELETE FROM Record WHERE ";
                int count = 0;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (r.Id > 0)
                    {
                        if(count > 0) command.CommandText += " AND ";
                        command.CommandText += "record_id = @id";
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r.Id;
                        count++;
                    }
                    if (r.Date != DateTime.MinValue)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "record_date = @date";
                        command.Parameters.Add("@date", SqlDbType.Date);
                        command.Parameters["@date"].Value = r.Date.Date;
                        count++;
                    }
                    if (r.Category_id != 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "category_id = @cid";
                        command.Parameters.Add("@cid", SqlDbType.Int);
                        command.Parameters["@cid"].Value = r.Category_id;
                        count++;
                        if(r.SubCategory_id != 0)
                        {
                            if (count > 0) command.CommandText += " AND ";
                            command.CommandText += "subcategory_id = @sid";
                            command.Parameters.Add("@sid", SqlDbType.Int);
                            command.Parameters["@sid"].Value = r.SubCategory_id;
                            count++;
                        }
                    }
                    if(count == 0) throw new ArgumentException("Invalid argument for removal.");
                    command.ExecuteNonQuery();
                    Update_totals(r, false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Remove failed." + e.Message);
            }
        }
        public void RmCategory(int id)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;
                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                List<int> sub_ids = new List<int>();
                string sql = "SELECT subcategory_id\r\nFROM CategoryWithSubcategory\r\nWHERE category_id = @category_id ; ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@category_id", SqlDbType.Int);
                    command.Parameters["@category_id"].Value = id;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sub_ids.Add(Convert.ToInt32(reader["subcategory_id"]));
                        }
                    }
                }
                foreach(int sid in sub_ids)
                {
                    RmSubCategory(sid);
                }
                string sql2 = "DELETE FROM CategoryList \r\nWHERE category_id = @cid";
                using(SqlCommand command = new SqlCommand(sql2, connection))
                {
                    command.Parameters.Add("@cid", SqlDbType.Int);
                    command.Parameters["@cid"].Value = id;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("RmCategory failed." + e.Message);
            }
        }
        public void RmSubCategory(int id)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;
                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "DELETE FROM CategoryWithSubcategory WHERE subcategory_id = @sid ; " +
                    "DELETE FROM SubCategoryList WHERE subcategory_id = @sid2 ; ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@sid", SqlDbType.Int);
                    command.Parameters.Add("@sid2", SqlDbType.Int);
                    command.Parameters["@sid"].Value = id;
                    command.Parameters["@sid2"].Value = id;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("RmSubCategory failed." + e.Message);
            }
        }
        public List<RecordForm> GetAllRecords()
        {
            List<RecordForm> records = new List<RecordForm>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT record_id, record_date, R.category_id, C.category_name, R.subcategory_id, S.subcategory_name, record_amount, description FROM Record R\r\n" +
                    "INNER JOIN CategoryList C ON C.category_id = R.category_id\r\nINNER JOIN SubCategoryList S on S.subcategory_id = R.subcategory_id";
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    records.Add(new RecordForm()
                    {
                        Id = 1,
                        Date = DateTime.Now,
                        Category = "TestCategory",
                        SubCategory = "TestSubCategory",
                        Amount = 1000,
                        SubCount = 1,
                        SubAmount = 1000,
                        Comment = "TestComment"
                    });
                }
                else
                {
                    while (reader.Read())
                    {
                        RecordForm record = new RecordForm
                        {
                            Id = Convert.ToInt32(reader["record_id"]),
                            Date = Convert.ToDateTime(reader["record_date"]),
                            Category_id = Convert.ToInt32(reader["category_id"]),
                            Category = reader["category_name"].ToString() ?? string.Empty,
                            SubCategory_id = Convert.ToInt32(reader["subcategory_id"]),
                            SubCategory = reader["subcategory_name"].ToString() ?? string.Empty,
                            Amount = Convert.ToInt32(reader["record_amount"]),
                            Comment = reader["description"].ToString() ?? string.Empty
                        };
                        records.Add(record);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("GetAllRecords failed." + e.Message);
            }
            return records;
        }
        public List<RecordForm> GetRecordsBy(RecordForm r)
        {
            List<RecordForm> records = new List<RecordForm>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "SELECT \r\n    R.record_id,\r\n\tR.record_date,\r\n   R.category_id, \r\n    C.category_name, \r\n    R.subcategory_id, \r\n    S.subcategory_name, \r\n    R.user_id, \r\n    U.user_name,\r\n\tR.record_amount,\r\n\tR.description\r\nFROM Record R\r\nINNER JOIN CategoryList C \r\n    ON R.category_id = C.category_id\r\nINNER JOIN SubCategoryList S \r\n    ON R.subcategory_id = S.subcategory_id\r\nINNER JOIN UserList U \r\n    ON R.user_id = U.user_id\r\n " +
                    "where ";
                //string sql = "SELECT * FROM Record as R LEFT JOIN CategoryList C ON R.category_id = C.category_id join SubCategoryList as S on S.category_id = R.category_id join UserLIst as U on U.user_id = R.user_id where ";
                //string sql2 = " join CategoryList as C on R.categoryid = C.category_id join SubCategoryList as S on S.category_id = R.category_id join UserLIst as U on U.user_id = R.user_id;";
                int count = 0;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (r.Id > 0)
                    {
                        if(count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.record_id = @id";
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r.Id;
                        count++ ;
                    }
                    if (r.Date > DateTime.MinValue)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.record_date = @date";
                        command.Parameters.Add("@date", SqlDbType.Date);
                        command.Parameters["@date"].Value = r.Date.Date;
                        count++;
                    }
                    if (r.Category_id > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.category_id = @cid";
                        command.Parameters.Add("@cid", SqlDbType.Int);
                        command.Parameters["@cid"].Value = r.Category_id;
                        count++;
                        if(r.SubCategory_id > 0)
                        {
                            if (count > 0) command.CommandText += " AND ";
                            command.CommandText += "R.subcategory_id = @sid";
                            command.Parameters.Add("@sid", SqlDbType.Int, 50);
                            command.Parameters["@sid"].Value = r.SubCategory_id;
                            count++;
                        }
                    }
                    if (r.User_id > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.user_id = @uid";
                        command.Parameters.Add("@uid", SqlDbType.Int);
                        command.Parameters["@uid"].Value = r.User_id;
                        count++;
                    }
                    if (r.Amount > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.record_amount = @amount";
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters["@amount"].Value = r.Amount;
                        count++;
                    }
                    if (count == 0) throw new ArgumentException("Invalid argument for search.");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Id = Convert.ToInt32(reader["record_id"]),
                                Date = Convert.ToDateTime(reader["record_date"]),
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                Category = reader["category_name"].ToString() ?? "",
                                SubCategory_id = Convert.ToInt32(reader["subcategory_id"]),
                                SubCategory = reader["subcategory_name"].ToString() ?? "",
                                Amount = Convert.ToInt32(reader["record_amount"]),
                                Comment = reader["description"].ToString() ?? ""
                            };
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllRecords failed." + e.Message);
            }
            return records;
        }
        public List<RecordForm> GetRecordsBy(RecordForm r1, RecordForm r2)
        {
            List<RecordForm> records = new List<RecordForm>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "SELECT \r\n    R.record_id,\r\n\tR.record_date,\r\n   R.category_id, \r\n    C.category_name, \r\n    R.subcategory_id, \r\n    S.subcategory_name, \r\n    R.user_id, \r\n    U.user_name,\r\n\tR.record_amount,\r\n\tR.description\r\nFROM Record R\r\nINNER JOIN CategoryList C \r\n    ON R.category_id = C.category_id\r\nINNER JOIN SubCategoryList S \r\n    ON R.subcategory_id = S.subcategory_id\r\nINNER JOIN UserList U \r\n    ON R.user_id = U.user_id\r\n " +
                    "where "; 
                int count = 0;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (r1.Id > 0 && r2.Id > 0)
                    {
                        if(count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.record_id BETWEEN @id AND @id2";
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r1.Id;
                        command.Parameters.Add("@id2", SqlDbType.Int);
                        command.Parameters["@id2"].Value = r2.Id;
                        count++;
                    }
                    if (r1.Date > DateTime.MinValue.Date && r2.Date >= r1.Date)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.record_date BETWEEN @date AND @date2";
                        command.Parameters.Add("@date", SqlDbType.Date);
                        command.Parameters["@date"].Value = r1.Date.Date;
                        command.Parameters.Add("@date2", SqlDbType.Date);
                        command.Parameters["@date2"].Value = r2.Date.Date;
                        count++;
                    }
                    if (r1.Category_id > 0 && r2.Category_id > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.category_id BETWEEN @cid AND @cid2";
                        command.Parameters.Add("@cid", SqlDbType.Int);
                        command.Parameters["@cid"].Value = r1.Category_id;
                        command.Parameters.Add("@cid2", SqlDbType.Int);
                        command.Parameters["@cid2"].Value = r2.Category_id;
                        count++;
                        if (r1.SubCategory_id > 0 && r2.SubCategory_id > 0)
                        {
                            if (count > 0) command.CommandText += " AND ";
                            command.CommandText += "R.subcategory_id BETWEEN @sid AND @sid2";
                            command.Parameters.Add("@sid", SqlDbType.Int);
                            command.Parameters["@sid"].Value = r1.SubCategory_id;
                            command.Parameters.Add("@sid2", SqlDbType.Int);
                            command.Parameters["@sid2"].Value = r2.SubCategory_id;
                            count++;
                        }
                    }
                    if (r1.User_id > 0 && r2.User_id > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.user_id BETWEEN @uid AND @uid2";
                        command.Parameters.Add("@uid", SqlDbType.Int);
                        command.Parameters["@uid"].Value = r1.User_id;
                        command.Parameters.Add("@uid2", SqlDbType.Int);
                        command.Parameters["@uid2"].Value = r2.User_id;
                        count++;
                    }
                    if (r1.Amount > 0 && r2.Amount >= r1.Amount)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.record_amount BETWEEN @amount AND @amount2";
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters["@amount"].Value = r1.Amount;
                        command.Parameters.Add("@amount2", SqlDbType.Int);
                        command.Parameters["@amount2"].Value = r2.Amount;
                        count++;
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Id = Convert.ToInt32(reader["record_id"]),
                                Date = Convert.ToDateTime(reader["record_date"]),
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                Category = reader["category_name"].ToString() ?? "",
                                SubCategory_id = Convert.ToInt32(reader["subcategory_id"]),
                                SubCategory = reader["subcategory_name"].ToString() ?? "",
                                Amount = Convert.ToInt32(reader["record_amount"]),
                                Comment = reader["description"].ToString() ?? ""
                            };
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllRecords failed." + e.Message);
            }
            return records;
        }
        public List<RecordForm> GetAllTotals()
        {
            List<RecordForm> totals = new List<RecordForm>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "MSI";
            builder.InitialCatalog = "Cash";
            builder.UserID = "Apple";
            builder.Password = "ApplePen";
            builder.Encrypt = true;
            builder.TrustServerCertificate = true;
            string connectionString = builder.ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT Top 100\r\n\trecord_id,\r\n\trecord_date, \r\n\tcategory_id, \r\n\tsubcategory_id, \r\n\tsubcount,\r\n\tsubamount\r\nFROM Totals";
            /*
             * SELECT Top 100
	                record_id,
	                record_date, 
	                category_id, 
	                subcategory_id, 
	                subcount,
	                subamount
                FROM Totals
             */
            try
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Id = Convert.ToInt32(reader["record_id"]),
                                Date = Convert.ToDateTime(reader["record_date"]),
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                SubCategory_id = Convert.ToInt32(reader["subcategory_id"]),
                                SubCount = Convert.ToInt32(reader["subcount"]),
                                SubAmount = Convert.ToInt32(reader["subamount"])
                            };
                            totals.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetTotals failed." + e.Message);
            }
            return totals;
        }
        public List<RecordForm> GetTotals(RecordForm r)
        {
            List<RecordForm> totals = new List<RecordForm>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "MSI";
            builder.InitialCatalog = "Cash";
            builder.UserID = "Apple";
            builder.Password = "ApplePen";
            builder.Encrypt = true;
            builder.TrustServerCertificate = true;

            string connectionString = builder.ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT\r\n\trecord_id,\r\n\trecord_date, \r\n\tcategory_id, \r\n\tsubcategory_id, \r\n\tsubcount,\r\n\tsubamount\r\nFROM Totals\r\nWHERE ";
            int count = 0;
            try
            {
                using(var command = new SqlCommand(sql, connection))
                {
                    if(r.Id > 0)
                    {
                        if(count > 0) command.CommandText += " AND ";
                        command.CommandText += "record_id = @id";
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r.Id;
                        count++;
                    }
                    if (r.Date != DateTime.MinValue)
                    {
                        if(count > 0) command.CommandText += " AND ";
                        command.CommandText += "record_date = @date";
                        command.Parameters.Add("@date", SqlDbType.Date);
                        command.Parameters["@date"].Value = r.Date.Date;
                        count++;
                    }
                    if (r.Category_id > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "category_id = @cid";
                        command.Parameters.Add("@cid", SqlDbType.Int);
                        command.Parameters["@cid"].Value = r.Category;
                        count++;
                        if (r.SubCategory_id > 0)
                        {
                            if (count > 0) command.CommandText += " AND ";
                            command.CommandText += "subcategory_id = @sid";
                            command.Parameters.Add("@sid", SqlDbType.Int);
                            command.Parameters["@subcategory_id"].Value = r.SubCategory;
                            count++;
                        }
                    }
                    if(count == 0) throw new ArgumentException("Invalid argument for search.");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Id = Convert.ToInt32(reader["record_id"]),
                                Date = Convert.ToDateTime(reader["record_date"]),
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                SubCategory_id = Convert.ToInt32(reader["subcategory_id"]),
                                SubCount = Convert.ToInt32(reader["subcount"]),
                                SubAmount = Convert.ToInt32(reader["subamount"])
                            };
                            totals.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetTotals failed." + e.Message);
            }
            return totals;
        }

        public List<RecordForm> GetTotals(RecordForm r1, RecordForm r2)
        {
            List<RecordForm> totals = new List<RecordForm>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "MSI";
            builder.InitialCatalog = "Cash";
            builder.UserID = "Apple";
            builder.Password = "ApplePen";
            builder.Encrypt = true;
            builder.TrustServerCertificate = true;

            string connectionString = builder.ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();
            string sql = "SELECT\r\n\trecord_id,\r\n\trecord_date, \r\n\tcategory_id, \r\n\tsubcategory_id, \r\n\tsubcount,\r\n\tsubamount\r\nFROM Totals\r\nWHERE ";
            int count = 0;
            try
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    if(r1.Id > 0 && r2.Id > 0)
                    {
                        if(count > 0) command.CommandText += " AND ";
                        command.CommandText += "record_id BETWEEN @id AND @id2";
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r1.Id;
                        command.Parameters.Add("@id2", SqlDbType.Int);
                        command.Parameters["@id2"].Value = r2.Id;
                        count++;
                    }
                    if (r1.Date > DateTime.MinValue.Date && r2.Date >= r1.Date)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.record_date BETWEEN @date AND @date2";
                        command.Parameters.Add("@date", SqlDbType.Date);
                        command.Parameters["@date"].Value = r1.Date.Date;
                        command.Parameters.Add("@date2", SqlDbType.Date);
                        command.Parameters["@date2"].Value = r2.Date.Date;
                        count++;
                    }
                    if (r1.Category_id > 0 && r2.Category_id > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "R.category_id BETWEEN @cid AND @cid2";
                        command.Parameters.Add("@cid", SqlDbType.Int);
                        command.Parameters["@cid"].Value = r1.Category_id;
                        command.Parameters.Add("@cid2", SqlDbType.Int);
                        command.Parameters["@cid2"].Value = r2.Category_id;
                        count++;
                        if (r1.SubCategory_id > 0 && r2.SubCategory_id > 0)
                        {
                            if (count > 0) command.CommandText += " AND ";
                            command.CommandText += "R.subcategory_id BETWEEN @sid AND @sid2";
                            command.Parameters.Add("@sid", SqlDbType.Int);
                            command.Parameters["@sid"].Value = r1.SubCategory_id;
                            command.Parameters.Add("@sid2", SqlDbType.Int);
                            command.Parameters["@sid2"].Value = r2.SubCategory_id;
                            count++;
                        }
                    }
                    if(r1.SubCount > 0 && r2.SubCount > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "subcount BETWEEN @subcount AND @subcount2";
                        command.Parameters.Add("@subcount", SqlDbType.Int);
                        command.Parameters["@subcount"].Value = r1.SubCount;
                        command.Parameters.Add("@subcount2", SqlDbType.Int);
                        command.Parameters["@subcount2"].Value = r2.SubCount;
                        count++;
                    }
                    if (r1.Amount > 0 && r2.Amount > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += "subamount BETWEEN @amount AND @amount2";
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters["@amount"].Value = r1.Amount;
                        command.Parameters.Add("@amount2", SqlDbType.Int);
                        command.Parameters["@amount2"].Value = r2.Amount;
                        count++;
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Id = Convert.ToInt32(reader["record_id"]),
                                Date = Convert.ToDateTime(reader["record_date"]),
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                SubCategory_id = Convert.ToInt32(reader["subcategory_id"]),
                                SubCount = Convert.ToInt32(reader["subcount"]),
                                SubAmount = Convert.ToInt32(reader["subamount"])
                            };
                            totals.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetTotals failed." + e.Message);
            }
            return totals;
        }
        public List<RecordForm> GetAllCategories()
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT category_id, category_name FROM CategoryList";
                using(SqlCommand command = new SqlCommand(sql, connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                Category = reader["category_name"].ToString() ?? ""
                            };
                            result.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllCategories failed." + e.Message);
            }
            return result;
        }
        public List<RecordForm> GetAllSubCategories()
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT subcategory_id, subcategory_name FROM SubCategoryList";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Category_id = Convert.ToInt32(reader["subcategory_id"]),
                                Category = reader["subcategory_name"].ToString() ?? ""
                            };
                            result.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllCategories failed." + e.Message);
            }
            return result;
        }
        public List<RecordForm> GetAllCategoriesAndSub()
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                string sql = "SELECT TOP (1000) [category_id]\r\n      ,[subcategory_id]\r\n  FROM [Cash].[dbo].[CategoryWithSubcategory]";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                SubCategory_id = Convert.ToInt32(reader["subcategory_id"])
                            };
                            result.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllCategories failed." + e.Message);
            }
            return result;
        }
        public void Update(RecordForm r, RecordForm content)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";
                builder.Encrypt = true;
                builder.TrustServerCertificate = true;

                string connectionString = builder.ConnectionString;
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = "UPDATE Record SET record_date = @ndate, category_id = @ncategory_id, subcategory_id = @nsubcategory_id, record_amount = @namount, description = @ndescription where ";
                int count = 0;
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (r.Id > 0)
                    {
                        if(count > 0) command.CommandText += " AND ";
                        command.CommandText += _condition["id"] + "2";
                        command.Parameters.Add("@id" + "2", SqlDbType.Int);
                        command.Parameters["@id" + "2"].Value = r.Id;
                        count++;
                    }
                    if (r.Date > DateTime.MinValue)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += _condition["time"] + "2";
                        command.Parameters.Add("@date" + "2", SqlDbType.Date);
                        command.Parameters["@date" + "2"].Value = r.Date.Date;
                        count++;
                    }
                    if (r.Category_id > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += _condition["category_id"] + "2";
                        command.Parameters.Add("@category_id" + "2", SqlDbType.NVarChar, 50);
                        command.Parameters["@category_id" + "2"].Value = r.Category_id;
                        count++;
                        if (r.SubCategory_id > 0)
                        {
                            if (count > 0) command.CommandText += " AND ";
                            command.CommandText += _condition["subcategory_id"] + "2";
                            command.Parameters.Add("@subcategory_id" + "2", SqlDbType.NVarChar, 50);
                            command.Parameters["@subcategory_id" + "2"].Value = r.SubCategory_id;
                            count++;
                        }
                    }
                    if (r.Amount > 0)
                    {
                        if (count > 0) command.CommandText += " AND ";
                        command.CommandText += _condition["money"] + "2";
                        command.Parameters.Add("@amount" + "2", SqlDbType.Int);
                        command.Parameters["@amount" + "2"].Value = r.Amount;
                        count++;
                    }
                    if(count == 0) throw new ArgumentException("Invalid argument for update.");
                    command.Parameters.Add("@ndate", SqlDbType.Date);
                    command.Parameters.Add("@ncategory_id", SqlDbType.Int);
                    command.Parameters.Add("@nsubcategory_id", SqlDbType.Int);
                    command.Parameters.Add("@namount", SqlDbType.Int);
                    command.Parameters.Add("@ndescription", SqlDbType.NVarChar, 50);
                    command.Parameters["@ndate"].Value = content.Date.Date;
                    command.Parameters["@ncategory_id"].Value = content.Category_id;
                    command.Parameters["@nsubcategory_id"].Value = content.SubCategory_id;
                    command.Parameters["@namount"].Value = content.Amount;
                    command.Parameters["@ndescription"].Value = content.Comment ?? "";
                    command.ExecuteNonQuery();
                    Update_totals(r, false);
                    Update_totals(content, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update failed." + e.Message);
            }
        }
    }
}
