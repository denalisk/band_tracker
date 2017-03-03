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
