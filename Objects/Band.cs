using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTrackerApp
{
    public class Band
    {
        private string _name;
        private int _id;

        public Band(string newString, int newId = 0)
        {
            _name = newString;
            _id = newId;
        }

        public static List<Band> GetAll()
        {
            List<Band> allBands = new List<Band>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                allBands.Add(new Band(rdr.GetString(1), rdr.GetInt32(0)));
            }

            DB.CloseSqlConnection(conn, rdr);
            return allBands;
        }


        public static void DeleteAll()
        {
            DB.DeleteAll("bands");
        }
    }
}
