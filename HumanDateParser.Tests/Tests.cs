using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanDateParser.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TodayTest()
        {
            var actual = DateTime.Now;
            var parsed = DateParser.Parse("today");

            Assert.IsNotNull(parsed);
            Assert.AreEqual(actual.Year, parsed.Year);
            Assert.AreEqual(actual.Month, parsed.Month);
            Assert.AreEqual(actual.Date, parsed.Date);
        }

        [TestMethod]
        public void TomorrowTest()
        {
            var actual = DateTime.Now.AddDays(1);
            var parsed = DateParser.Parse("tomorrow");

            Assert.IsNotNull(parsed);
            Assert.AreEqual(actual.Year, parsed.Year);
            Assert.AreEqual(actual.Month, parsed.Month);
            Assert.AreEqual(actual.Date, parsed.Date);
        }

        [TestMethod]
        public void MonthTest()
        {
            var actual = DateTime.Now.AddMonths(-1);
            var parsed = DateParser.Parse("1 month ago");
            
            Assert.IsNotNull(parsed);
            Assert.AreEqual(actual.Year, parsed.Year);
            Assert.AreEqual(actual.Month, parsed.Month);
            Assert.AreEqual(actual.Date, parsed.Date);
        }
        
        [TestMethod]
        public void DaysTest()
        {
            var actual = DateTime.Now.AddDays(15);
            var parsed = DateParser.Parse("after 15 days");
            
            Assert.IsNotNull(parsed);
            Assert.AreEqual(actual.Year, parsed.Year);
            Assert.AreEqual(actual.Month, parsed.Month);
            Assert.AreEqual(actual.Date, parsed.Date);
        }
        
        [TestMethod]
        public void SpecificTimeTest()
        {
            var actual = DateTime.Parse("2010-2-15 05:30 PM");
            var parsed = DateParser.Parse("15th feb 2010 at 5:30pm");
            
            Assert.IsNotNull(parsed);
            Assert.AreEqual(actual.Year, parsed.Year);
            Assert.AreEqual(actual.Month, parsed.Month);
            Assert.AreEqual(actual.Date, parsed.Date);
            Assert.AreEqual(actual.Hour, parsed.Hour);
            Assert.AreEqual(actual.Minute, parsed.Minute);
        }
        
        
    }
}