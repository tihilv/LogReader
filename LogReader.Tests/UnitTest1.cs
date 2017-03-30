using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogReader.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            RingCache<long> rc = new RingCache<long>(5, Calc);

            for (int i = 0; i <= 4; i++)
            {
                var v = rc[i];
                Assert.AreEqual(v, i*2+1);
                Assert.AreEqual(_callCount, i+1);
            }

            for (int i = 4; i >=0; i--)
            {
                var v = rc[i];
                Assert.AreEqual(v, i * 2 + 1);
                Assert.AreEqual(_callCount, 5);
            }

            _callCount = 0;

            Assert.AreEqual(rc[5], 11);
            Assert.AreEqual(_callCount, 1);

            for (int i = 1; i < 6; i++)
            {
                var v = rc[i];
                Assert.AreEqual(v, i * 2 + 1);
                Assert.AreEqual(_callCount, 1);
            }

            _callCount = 0;

            Assert.AreEqual(rc[0], 1);
            Assert.AreEqual(_callCount, 1);

            for (int i = 0; i < 5; i++)
            {
                var v = rc[i];
                Assert.AreEqual(v, i * 2 + 1);
                Assert.AreEqual(_callCount, 1);
            }

            _callCount = 0;
            for (int i = 100; i < 105; i++)
            {
                var v = rc[i];
                Assert.AreEqual(v, i * 2 + 1);
                Assert.AreEqual(_callCount, i-99);
            }

            for (int i = 100; i < 105; i++)
            {
                var v = rc[i];
                Assert.AreEqual(v, i * 2 + 1);
                Assert.AreEqual(_callCount, 5);
            }

            _callCount = 0;

            Assert.AreEqual(rc[99], 199);
            Assert.AreEqual(_callCount, 1);

            Assert.AreEqual(rc[100], 201);
            Assert.AreEqual(_callCount, 1);

        }

        private int _callCount = 0;
        public long Calc(long index)
        {
            _callCount++;
            return index*2+1;
        }
    }
}
