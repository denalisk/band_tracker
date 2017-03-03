using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;

namespace BandTrackerApp
{
    public class BandTest : IDisposable
    {
        public BandTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=band_tracker_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Band_DatabaseStartsEmpty_NothingInDatabase()
        {
            // This test will check to see if the database starts empty and the Dipsose method is working correctly
            // arrange act assert
            Assert.Equal(0, Band.GetAll().Count);
        }


        [Fact]
        public void Band_IdentityTest_CheckIfObjectsAreIdentitcal()
        {
            // This test will check to see if the overwritten .Equals function for the object can detect identical objects
            // arrange
            Band newBand = new Band("Fire");

            // act
            Band duplicateBand = new Band("Fire");

            // assert
            Assert.Equal(newBand, duplicateBand);
        }

        [Fact]
        public void Save_SavesToDBandAltersLocalObject_ReturnsNewIdentity()
        {
            // This test will check to see the save functionality for band works, to test that the program is writing to the database and assigning a new unique Id
            // arrange
            Band newBand = new Band("Fire");

            // act
            newBand.Save();

            // assert
            Assert.Equal(newBand, Band.GetAll()[0]);
        }

        [Fact]
        public void Save_DoesNotSaveDuplicate_NoAdditionalBands()
        {
            // This test will check to see the save functionality for band works, to test that the program is writing to the database and assigning a new unique Id
            // arrange
            Band newBand = new Band("Fire");
            Band duplicateBand = new Band("Fire");

            // act
            newBand.Save();
            duplicateBand.Save();

            // assert
            Assert.Equal(1, Band.GetAll().Count);
        }

        [Fact]
        public void Find_GetObjectFromDatabase_ReturnBand()
        {
            // This test will check to see if the programs Band.Find() method can get a band object from the database and return it
            // arrange
            Band newBand = new Band("Fire");
            newBand.Save();

            // act
            Band foundBand = Band.Find(newBand.GetId());

            // assert
            Assert.Equal(newBand, foundBand);
        }

        [Fact]
        public void Update_AlterDatabaseEntry_ReturnNewBand()
        {
            // This test will check to see if the Band.Update() method writes new values to the database and the returns the altered object to the local instance
            // arrange
            Band newBand = new Band("Fire");
            newBand.Save();

            Band otherBand = new Band("Water");

            // act
            newBand.Update("Water");
            otherBand.SetId(newBand.GetId());

            // assert
            Assert.Equal(otherBand, Band.Find(newBand.GetId()));
        }

        [Fact]
        public void AddVenue_AlterJoinTable_IncrementBandVenues()
        {
            // This test will check to see if the program can successfully write to the join table and add a connection between VENUES and BANDS
            // arrange
            Band newBand = new Band("Fire");
            newBand.Save();
            Venue newVenue = new Venue("Boston");
            newVenue.Save();

            // act
            newBand.AddVenue(newVenue);

            // assert
            Assert.Equal(newVenue, newBand.GetVenues()[0]);
        }


        [Fact]
        public void Delete_RemoveFromDatabase_DecrementDatabase()
        {
            // This test will check to see if the delete functionality of the program can successfully remove items from the database
            // arrange
            Band newBand = new Band("Fire");
            newBand.Save();
            Band otherBand = new Band("Water");
            otherBand.Save();

            // act
            newBand.Delete();

            // assert
            Assert.Equal(1, Band.GetAll().Count);
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
            newBand.Delete();

            // assert
            Assert.Equal(0, newVenue.GetBands().Count);
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
            Band.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
