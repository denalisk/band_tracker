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
