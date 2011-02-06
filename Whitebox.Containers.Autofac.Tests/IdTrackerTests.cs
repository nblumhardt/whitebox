using System;
using NUnit.Framework;

namespace Whitebox.Containers.Autofac.Tests
{
    public class IdTrackerTests
    {
        [TestFixture]
        public class WhenAnIdIsAssigned
        {
            IdTracker _idTracker;
            object _instance;
            string _id;

            [SetUp]
            public void SetUp()
            {
                _idTracker = new IdTracker();
                _instance = new object();
                _id = _idTracker.GetOrAssignId(_instance);
            }

            [Test]
            public void GettingTheIdForTheInstanceReturnsTheId()
            {
                Assert.AreEqual(_id, _idTracker.GetIdOrUnknown(_instance));
            }
        }

        [TestFixture]
        public class WhenAnIdIsForgotten
        {
            IdTracker _idTracker;
            object _instance;

            [SetUp]
            public void SetUp()
            {
                _idTracker = new IdTracker();
                _instance = new object();
                _idTracker.GetOrAssignId(_instance);
                _idTracker.ForgetId(_instance);
            }

            [Test]
            public void GettingTheIdOrUnknownReturnsUnknown()
            {
                Assert.AreEqual(IdTracker.UnknownId, _idTracker.GetIdOrUnknown(_instance));
            }

            [Test]
            public void GettingTheIdOrFailingThrows()
            {
                Assert.Throws<ArgumentException>(() => _idTracker.GetIdOrFail(_instance));
            }
        }
    }
}
