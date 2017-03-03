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
            int potentialId = -1;

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT id FROM bands WHERE name = @TargetName", conn);
            cmd.Parameters.Add(new SqlParameter("@TargetName", this.GetName()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                potentialId = rdr.GetInt32(0);
            }
            DB.CloseSqlConnection(conn, rdr);

            return potentialId;

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
                DB.CloseSqlConnection(conn, rdr);
            }
            this.SetId(potentialId);
        }

        public void Update(string newName)
        {
            // This function updates the saved values associated with a band entry in the database and rewrites the local values
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE bands SET name = @NewName OUTPUT INSERTED.name WHERE id = @TargetId;", conn);
            cmd.Parameters.Add(new SqlParameter("@NewName", newName));
            cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this.SetName(rdr.GetString(0));
            }
            DB.CloseSqlConnection(conn, rdr);
        }

        public static Band Find(int targetId)
        {
            // This function will search the database for an existing entry with the passedId and return it
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM bands WHERE id = @TargetId", conn);
            cmd.Parameters.Add(new SqlParameter("@TargetId", targetId));

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundName = null;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
            }

            DB.CloseSqlConnection(conn, rdr);

            return new Band(foundName, foundId);
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

        public void AddVenue(Venue newVenue)
        {
            // This function will add a connection between the BAND and the VENUE in the join table
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM bands_venues WHERE band_id = @NewBandId AND venue_id = @NewVenueId; INSERT INTO bands_venues (band_id, venue_id) VALUES (@NewBandId, @NewVenueId);", conn);
            cmd.Parameters.Add(new SqlParameter("@NewBandId", this.GetId()));
            cmd.Parameters.Add(new SqlParameter("@NewVenueId", newVenue.GetId()));

            cmd.ExecuteNonQuery();

            DB.CloseSqlConnection(conn);
        }

        public List<Venue> GetVenues()
        {
            // This function will return a list of all venues associated with the band
            List<Venue> bandVenues = new List<Venue>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT venues.* FROM bands JOIN bands_venues ON (bands.id = bands_venues.band_id) JOIN venues ON (venues.id = bands_venues.venue_id) WHERE band_id = @TargetId;", conn);
            cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                bandVenues.Add(new Venue(rdr.GetString(1), rdr.GetInt32(0)));
            }

            DB.CloseSqlConnection(conn, rdr);
            return bandVenues;
        }

        public static void DeleteAll()
        {
            // This function deletes all bands from the DB
            DB.DeleteAll("bands");
        }
    }
}
