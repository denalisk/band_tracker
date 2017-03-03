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
            // This function constructs a new band object with the given name and id
            _name = newString;
            _id = newId;
        }

        public string GetName()
        {
            return _name;
        }
        public void SetName(string newName)
        {
            _name = newName;
        }

        public int GetId()
        {
            return _id;
        }
        public void SetId(int newId)
        {
            _id = newId;
        }

        public override bool Equals(System.Object otherBand)
        {
            // This function overrides the built-in Equals Method to ensure it returns true if two band objects are identical
            if (!(otherBand is Band))
            {
                return false;
            }
            else
            {
                Band newBand = (Band) otherBand;
                bool nameEquality = this.GetName() == newBand.GetName();
                bool idEquality = this.GetId() == newBand.GetId();
                return (nameEquality && idEquality);
            }
        }

        public override int GetHashCode()
        {
            // This override alters the GetHashCode method to use the objects properties to navigate the hash tables
            return this.GetName().GetHashCode();
        }

        public int IsNewEntry()
        {
            // This function checks to see if the object instance already exists in the database, returning the DB id if it already exists and -1 if it does not
            return -1;

        }

        public void Save()
        {
            // This function saves a band object to a database if it is a new band objects
            int potentialId = this.IsNewEntry();
            if (potentialId == -1)
            {
                SqlConnection conn = DB.Connection();
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO bands (name) OUTPUT INSERTED.id VALUES (@NewName);", conn);
                cmd.Parameters.Add(new SqlParameter("@NewName", this.GetName()));

                SqlDataReader rdr = cmd.ExecuteReader();

                while(rdr.Read())
                {
                    potentialId = rdr.GetInt32(0);
                }
                DB.CloseSqlConnection(conn);
            }
            this.SetId(potentialId);
        }

        public static List<Band> GetAll()
        {
            // This function returns a list of all the bands in the DB
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
            // This function deletes all bands from the DB
            DB.DeleteAll("bands");
        }
    }
}
