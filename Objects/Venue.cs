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

        public Venue(string newName, int newId = 0)
        {
            _name = newName;
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

        public override bool Equals(System.Object otherVenue)
        {
            // This function overrides the built-in Equals Method to ensure it returns true if two venue objects are identical
            if (!(otherVenue is Venue))
            {
                return false;
            }
            else
            {
                Venue newVenue = (Venue) otherVenue;
                bool nameEquality = this.GetName() == newVenue.GetName();
                bool idEquality = this.GetId() == newVenue.GetId();
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

            SqlCommand cmd = new SqlCommand("SELECT id FROM venues WHERE name = @TargetName", conn);
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
            // This function saves a venue object to a database if it is a new venue objects
            int potentialId = this.IsNewEntry();
            if (potentialId == -1)
            {
                SqlConnection conn = DB.Connection();
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO venues (name) OUTPUT INSERTED.id VALUES (@NewName);", conn);
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
            // This function updates the saved values associated with a venue entry in the database and rewrites the local values
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE venues SET name = @NewName OUTPUT INSERTED.name WHERE id = @TargetId;", conn);
            cmd.Parameters.Add(new SqlParameter("@NewName", newName));
            cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this.SetName(rdr.GetString(0));
            }
            DB.CloseSqlConnection(conn);
        }

        public static Venue Find(int targetId)
        {
            // This function will search the database for an existing entry with the passedId and return it
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM venues WHERE id = @TargetId", conn);
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

            return new Venue(foundName, foundId);
        }

        public void Delete()
        {
            // This function deletes instances of the local object from it's database and all associated databases
            DB.Delete(this.GetId(), "venues", "venue", "bands_venues");
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

        public void AddBand(Band newBand)
        {
            // This function will add a connection between the BAND and the VENUE in the join table
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM bands_venues WHERE band_id = @NewBandId AND venue_id = @NewVenueId; INSERT INTO bands_venues (band_id, venue_id) VALUES (@NewBandId, @NewVenueId);", conn);
            cmd.Parameters.Add(new SqlParameter("@NewVenueId", this.GetId()));
            cmd.Parameters.Add(new SqlParameter("@NewBandId", newBand.GetId()));

            cmd.ExecuteNonQuery();

            DB.CloseSqlConnection(conn);
        }

        public List<Band> GetBands()
        {
            // This function will return a list of all venues associated with the band
            List<Band> venueBands = new List<Band>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT bands.* FROM bands JOIN bands_venues ON (bands.id = bands_venues.band_id) JOIN venues ON (venues.id = bands_venues.venue_id) WHERE venue_id = @TargetId;", conn);
            cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                venueBands.Add(new Band(rdr.GetString(1), rdr.GetInt32(0)));
            }

            DB.CloseSqlConnection(conn, rdr);
            return venueBands;
        }


        public static void DeleteAll()
        {
            DB.DeleteAll("venues");
        }

















    }
}
