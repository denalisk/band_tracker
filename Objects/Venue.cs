using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BandTrackerApp
{
    public class Venue
    {
        // Create Venue Here
        private string _name;
        private int _id;

        public Venue(string newName, int newId)
        {
            _name = newName;
            _id = newId;
        }

        public static List<Venue> GetAll()
        {
            List<Venue> allVenues = new List<Venue>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM venues;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                allVenues.Add(new Venue(rdr.GetString(1), rdr.GetInt32(0)));
            }

            DB.CloseSqlConnection(conn, rdr);
            return allVenues;
        }

        public static void DeleteAll()
        {
            DB.DeleteAll("venues");
        }
    }
}
