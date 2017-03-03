using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;

namespace BandTrackerApp
{
    public class BandTrackerTest : IDisposable
    {
        public BandTrackerTest()
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
        public void TESTMETHOD_TESTFUNCTIONALITY_TESTRESULT()
        {
            // This test will ...................................................... by .........................
            // arrange

            // act

            // assert
        }

        [Fact]
        public void Venue_DatabaseStartsEmpty_NothingInDatabase()
        {
            // This test will check to see if the database starts empty and the Dipsose method is working correctly
            // arrange act assert
            Assert.Equal(0, Venue.GetAll().Count);
        }
        
        public void Dispose()
        {
            Band.DeleteAll();
            Venue.DeleteAll();
        }
    }
}
