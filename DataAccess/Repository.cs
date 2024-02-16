using Microsoft.Data.Sqlite;
using Shared.Models;
using System.Reflection;

namespace DataAccess
{
    public class Repository : IRepository
    {
        //convert the bellow path to a path relative to the root of the project
        private const string ConnectionString = "Data Source=C:\\Users\\Guzz\\source\\repos\\QBuildAssignment\\DataAccess\\QBuild.sqlite";


        public List<PART> GetParts()
        {
            return ExecuteReaderMultiple<PART>("SELECT * FROM PART");
        }
        
        public List<BOM> GetBOMs()
        {
            return ExecuteReaderMultiple<BOM>("SELECT * FROM BOM");
        }

        public BOM GetBOM(int id)
        {
            return ExecuteReaderSingle<BOM>($"SELECT * FROM BOM where Id = {id}");
        }

        public PART GetPART(int id)
        {
            return ExecuteReaderSingle<PART>($"SELECT * FROM PART where Id = {id}");
        }

        public PART GetParentPart(int id)
        {
            return ExecuteReaderSingle<PART>($"SELECT * FROM PART WHERE Name = (SELECT ParentName FROM BOM where PartFkId = {id});");
        }

        public List<PART> GetChildParts(int id)
        {
            return ExecuteReaderMultiple<PART>($"SELECT * FROM PART where Id = (SELECT PartFkId FROM BOM WHERE ParentName = (SELECT * FROM PART WHERE Id = {id}) ); ");
        }

        public List<DetailedPART> GetPartsWithDetails()
        {
            return ExecuteReaderMultiple<DetailedPART>(
                $"SELECT PART.Id, " +
                $"BOM.ParentName, " +
                $"BOM.ComponentName, " +
                $"PART.PartNumber, " +
                $"PART.Title, " +
                $"BOM.Quantity, " +
                $"PART.Type, " +
                $"PART.Item, " +
                $"PART.Material " +
                $"FROM PART JOIN BOM " +
                $"ON PART.Name = BOM.ComponentName");
        }
        public DetailedPART GetPartWithDetails(int id)
        {
            return ExecuteReaderSingle<DetailedPART>(
                $"SELECT PART.Id, " +
                $"BOM.ParentName, " +
                $"BOM.ComponentName, " +
                $"PART.PartNumber, " +
                $"PART.Title, " +
                $"BOM.Quantity, " +
                $"PART.Type, " +
                $"PART.Item, " +
                $"PART.Material " +
                $"FROM PART JOIN BOM " +
                $"ON PART.Name = BOM.ComponentName " +
                $"WHERE PART.Id = {id}");
        }

        public List<DetailedPART> GetComponentChildPartsWithDetails(int id)
        {
            return ExecuteReaderMultiple<DetailedPART>(
                $"SELECT PART.Id, " +
                $"BOM.ParentName, " +
                $"BOM.ComponentName, " +
                $"PART.PartNumber, " +
                $"PART.Title, " +
                $"BOM.Quantity, " +
                $"PART.Type, " +
                $"PART.Item, " +
                $"PART.Material " +
                $"FROM PART JOIN BOM " +
                $"ON PART.Name = BOM.ComponentName " +
                $"WHERE BOM.ParentName = (SELECT Name FROM PART WHERE Id = {id});");
        }

        //implement a method: using the ComponentName (BOM table) select the PARTs from the PART table that do NOT have a ParentName (ParentName is null) in the BOM table.
        public List<DetailedPART> GetOrphanParts()
        {
            return ExecuteReaderMultiple<DetailedPART>(
                $"SELECT PART.Id, " +
                $"BOM.ParentName, " +
                $"BOM.ComponentName, " +
                $"PART.PartNumber, " +
                $"PART.Title, " +
                $"BOM.Quantity, " +
                $"PART.Type, " +
                $"PART.Item, " +
                $"PART.Material " +
                $"FROM PART LEFT JOIN BOM " +
                $"ON PART.Name = BOM.ComponentName " +
                $"WHERE BOM.ParentName IS NULL;");
        }
        
        

        public void ExecuteNonQuery(string sql)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        //public void ExecuteReader(string sql)
        //{
        //    using (var connection = new SqliteConnection(ConnectionString))
        //    {
        //        connection.Open();

        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandText = sql;

        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Handle the data from the reader
        //                    Console.WriteLine($"Id: {reader["Id"]}, Name: {reader["Name"]}");
        //                }
        //            }
        //        }
        //    }
        //}

        public List<T> ExecuteReaderMultiple<T>(string sql) where T : new()
        {
            List<T> resultList = new List<T>();

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T obj = new T();
                            MapData(reader, obj);
                            resultList.Add(obj);
                        }
                    }
                }
            }

            return resultList;
        }

        private void MapData<T>(SqliteDataReader reader, T obj)
        {
            // Using reflection to dynamically set property values based on column names
            Type type = typeof(T);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                PropertyInfo property = type.GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    object value = reader.GetValue(i);
                    if (value != DBNull.Value)
                    {
                        //convert value to int32 if property is int
                        if (property.PropertyType == typeof(int))
                        {
                            value = Convert.ToInt32(value);
                        }
                        property.SetValue(obj, value);
                    }
                }
            }
        }

        public T ExecuteReaderSingle<T>(string sql) where T : new()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            T obj = new T();
                            MapData(reader, obj);
                            return obj;
                        }
                    }
                }
            }

            return default(T);
        }

       

    }
}
