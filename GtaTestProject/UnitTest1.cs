using System;
using Xunit;
using System.IO;

namespace GtaTestProject
{
    public class UnitTest1
    {

        


        [Fact]
        public void Test1()
        {
            string textFile = @"C:\Users\Bruger\source\repos\gta scripting\gta scripting\test.gta";

            string file = File.ReadAllText(textFile);



            Assert.Equal("callout", file);
        }
    }
}
