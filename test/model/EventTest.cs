using NUnit.Framework.Legacy;
using ProductiveHoursTracker.model;

namespace ProductiveHoursTracker.test.model;

using System;
using NUnit.Framework;

public class EventTest
{
    private Event e;
    private DateTime d;

    [SetUp]
    public void RunBefore()
    {
        e = new Event("Sensor open at door"); // (1)
        d = DateTime.Now; // (2)
    }

    [Test]
    public void TestEvent()
    {
        ClassicAssert.AreEqual("Sensor open at door", e.Description);
        // ClassicAssert.AreEqual(d, e.Date); // fails due to milliseconds
        ClassicAssert.AreEqual(e.Date.ToString("yyyy-MM-dd HH:mm:ss"), d.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [Test]
    public void TestToString()
    {
        string expected = d.ToString("g") + Environment.NewLine + "Sensor open at door";
        ClassicAssert.AreEqual(expected, e.ToString());
    }
}