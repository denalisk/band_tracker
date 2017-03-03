using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;

namespace BandTrackerApp
{
    public class VenueTest : IDisposable
    {
        public VenueTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Venue_DatabaseStartsEmpty_NothingInDatabase()
        {
            // This test will check to see if the database starts empty and the Dipsose method is working correctly
            // arrange act assert
            Assert.Equal(0, Venue.GetAll().Count);
        }

        [Fact]
        public void Venue_IdentityTest_CheckIfObjectsAreIdentitcal()
        {
            // This test will check to see if the overwritten .Equals function for the object can detect identical objects
            // arrange
            Venue newVenue = new Venue("Fire");

            // act
            Venue duplicateVenue = new Venue("Fire");

            // assert
            Assert.Equal(newVenue, duplicateVenue);
        }

        [Fact]
        public void Save_SavesToDVenueAltersLocalObject_ReturnsNewIdentity()
        {
            // This test will check to see the save functionality for venue works, to test that the program is writing to the database and assigning a new unique Id
            // arrange
            Venue newVenue = new Venue("Fire");

            // act
            newVenue.Save();

            // assert
            Assert.Equal(newVenue, Venue.GetAll()[0]);
        }

        [Fact]
        public void Save_DoesNotSaveDuplicate_NoAdditionalVenues()
        {
            // This test will check to see the save functionality for venue works, to test that the program is writing to the database and assigning a new unique Id
            // arrange
            Venue newVenue = new Venue("Fire");
            Venue duplicateVenue = new Venue("Fire");

            // act
            newVenue.Save();
            duplicateVenue.Save();

            // assert
            Assert.Equal(1, Venue.GetAll().Count);
        }

        [Fact]
        public void Find_GetObjectFromDatabase_ReturnVenue()
        {
            // This test will check to see if the programs Venue.Find() method can get a venue object from the database and return it
            // arrange
            Venue newVenue = new Venue("Fire");
            newVenue.Save();

            // act
            Venue foundVenue = Venue.Find(newVenue.GetId());

            // assert
            Assert.Equal(newVenue, foundVenue);
        }

        [Fact]
        public void Update_AlterDatabaseEntry_ReturnNewVenue()
        {
            // This test will check to see if the Venue.Update() method writes new values to the database and the returns the altered object to the local instance
            // arrange
            Venue newVenue = new Venue("Fire");
            newVenue.Save();

            Venue otherVenue = new Venue("Water");

            // act
            newVenue.Update("Water");
            otherVenue.SetId(newVenue.GetId());

            // assert
            Assert.Equal(otherVenue, Venue.Find(newVenue.GetId()));
        }

        [Fact]
        public void AddBand_AlterJoinTable_IncrementVenueBands()
        {
            // This test will check to see if the program can successfully write to the join table and add a connection between VENUES and BANDS
            // arrange
            Band newBand = new Band("Fire");
            newBand.Save();
            Venue newVenue = new Venue("Boston");
            newVenue.Save();

            // act
            newVenue.AddBand(newBand);

            // assert
            Assert.Equal(newBand, newVenue.GetBands()[0]);
        }

        [Fact]
        public void Delete_RemoveFromDatabase_DecrementDatabase()
        {
            // This test will check to see if the delete functionality of the program can successfully remove items from the database
            // arrange
            Venue newVenue = new Venue("Fire");
            newVenue.Save();
            Venue otherVenue = new Venue("Water");
            otherVenue.Save();

            // act
            newVenue.Delete();

            // assert
            Assert.Equal(1, Venue.GetAll().Count);
        }

        [Fact]
        public void Delete_RemoveFromJoinTable_DecrementDatabase()
        {
            // This test will check to see if the delete functionality of the program can successfully remove items from the join table
            // arrange
            Band newBand = new Band("Fire");
            newBand.Save();
            Venue newVenue = new Venue("Boston");
            newVenue.Save();

            // act
            newBand.AddVenue(newVenue);
            newVenue.Delete();

            // assert
            Assert.Equal(0, newBand.GetVenues().Count);
        }

        [Fact]
        public void Search_LocateObjectsInDatabase_ReturnList()
        {
            // This test will check to see if the database search functionality returns items correctly and accurately
            // arrange
            Band newBand = new Band("Fire");
            newBand.Save();
            Venue newVenue = new Venue("Boston");
            newVenue.Save();
            Venue otherVenue = new Venue("Bosnia");
            otherVenue.Save();
            Venue otherVenue2 = new Venue("Yugoslav");
            otherVenue2.Save();

            // act
            Dictionary<string, object> searchResults = DB.Search("Bos");
            List<Venue> expectedList = new List<Venue> {newVenue, otherVenue};

            // assert
            Assert.Equal(expectedList, searchResults["venues"]);
        }

        [Fact]
        public void TESTMETHOD_TESTFUNCTIONALITY_TESTRESULT()
        {
            // This test will ...................................................... by .........................
            // arrange

            // act

            // assert
        }

        public void Dispose()
        {
            Venue.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
