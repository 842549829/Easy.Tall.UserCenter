using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Easy.Tall.UserCenter.Test
{
    /// <summary>
    /// UnitTest
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        /// <summary>
        /// TestMethod
        /// </summary>
        [TestMethod]
        public void TestMethod()
        {
            var data = DateTimeOffset.Now;
            var data1 = data.DateTime;
        }
    }
}