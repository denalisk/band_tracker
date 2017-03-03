using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;

namespace MyApp
{
    public class NEWCLASSTEST : IDisposable
    {
        public NEWCLASSTEST()
        {
            // DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=DATABASENAMEHERE_TEST;Integrated Security=SSPI;";
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
        // DELETE ALLS GO HERE
    }
}
