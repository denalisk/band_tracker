using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
// using Microsoft.Scripting;

namespace BandTrackerApp
{
    public class DB
    {
        public static SqlConnection Connection()
        {
            SqlConnection conn = new SqlConnection(DBConfiguration.ConnectionString);
            return conn;
        }

        public static void CloseSqlConnection(SqlConnection conn, SqlDataReader rdr = null)
        // This function will close a SqlConnection and reader opened with SqlConnection and SqlDataReader
        {
            if(rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static Dictionary<string, object> Search(string targetString)
        {
            // This function will search through both tables and return a list of all the relevant items
            Dictionary<string, object> searchResults = new Dictionary<string, object>{};
            List<Band> resultBands = new List<Band>{};
            List<Venue> resultVenues = new List<Venue>{};

            // Search Bands
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE name LIKE @TargetString;", conn);
            cmd.Parameters.Add(new SqlParameter("@TargetString", "%" + targetString + "%"));
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                resultBands.Add(new Band(rdr.GetString(1), rdr.GetInt32(0)));
            }

            DB.CloseSqlConnection(conn, rdr);

            // Search Venues
            SqlConnection conn2 = DB.Connection();
            conn2.Open();

            SqlCommand cmd2 = new SqlCommand("SELECT * FROM venues WHERE name LIKE @TargetString;", conn2);
            cmd2.Parameters.Add(new SqlParameter("@TargetString", "%" + targetString + "%"));
            SqlDataReader rdr2 = cmd2.ExecuteReader();

            while(rdr2.Read())
            {
                resultVenues.Add(new Venue(rdr2.GetString(1), rdr2.GetInt32(0)));
            }

            DB.CloseSqlConnection(conn2, rdr2);
            // Compose dict results
            searchResults.Add("bands", resultBands);
            searchResults.Add("venues", resultVenues);

            return searchResults;
        }

        // public static int CheckIsNew(string tableName, string targetColumnName, string targetName, [ParamDictionary] IAttributesCollection kwargs)
        // // This function will check see if a table contains an identical item to targetName with kwargs other key:value associations
        // {
        //     string SqlCommandString = string.Format("SELECT id FROM {0} WHERE {1} = {2}", tableName, targetColumnName, targetName);
        //     foreach(SymbolId key in kwargs.SymbolAttributes.Keys)
        //     {
        //         SqlCommandString += string.Format(" AND {0} = {1}", key, kwargs[key]);
        //     }
        //     SqlCommandString += ";";
        //
        //     SqlConnection conn = Connection();
        //     conn.Open();
        //     SqlCommand cmd = new SqlCommand(SqlCommandString, conn);
        //     SqlDataReader rdr = cmd.ExecuteReader();
        //
        //     int foundId = -1;
        //     while (rdr.Read())
        //     {
        //         foundId = rdr.GetInt32(0);
        //     }
        //
        //     CloseSqlConnection(conn, rdr);
        //     return foundId;
        // }

        public static void Delete(int passedId, string baseTableName, string baseTableNameSingular, params string[] tableList)
        // This function will delete an item with id of passedId from a table baseTableName, and additionally delete it from tableList other tables where the foreign key follows the format baseTableNameSingular_id
        {
            string targetId = passedId.ToString();
            string sqlCommandString = string.Format("DELETE FROM {0} WHERE id = {1}; ", baseTableName, targetId);
            foreach(string table in tableList)
            {
                sqlCommandString += string.Format("DELETE FROM {0} WHERE {1}_id = {2}", table, baseTableNameSingular, targetId);
            }
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand(sqlCommandString, conn);

            cmd.ExecuteNonQuery();
            DB.CloseSqlConnection(conn);
        }

        public static void DeleteAll(string tableName)
        // This function will delete all values in a table tableName
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            string command = string.Format("DELETE FROM {0};", tableName);
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
