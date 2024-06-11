using NUnit.Framework.Legacy;
using ProductiveHoursTracker.model;

namespace ProductiveHoursTracker.test.model;

using System.Collections.Generic;
using NUnit.Framework;

public class EventLogTest
{
    private Event e1;
    private Event e2;
    private Event e3;

    [SetUp]
    public void LoadEvents()
    {
        e1 = new Event("A1");
        e2 = new Event("A2");
        e3 = new Event("A3");
        EventLog.Instance.LogEvent(e1);
        EventLog.Instance.LogEvent(e2);
        EventLog.Instance.LogEvent(e3);
    }

    [Test]
    public void TestLogEvent()
    {
        List<Event> events = new List<Event>();

        foreach (Event e in EventLog.Instance)
        {
            events.Add(e);
        }

        ClassicAssert.IsTrue(events.Contains(e1));
        ClassicAssert.IsTrue(events.Contains(e2));
        ClassicAssert.IsTrue(events.Contains(e3));
    }

    [Test]
    public void TestClear()
    {
        EventLog.Instance.Clear();
        IEnumerator<Event> iterator = EventLog.Instance.GetEnumerator();

        ClassicAssert.IsTrue(iterator.MoveNext()); // After log is cleared, the clear log event is added
        ClassicAssert.AreEqual("Event log cleared.", iterator.Current.Description);
        ClassicAssert.IsFalse(iterator.MoveNext());
        
        iterator.Dispose();
    }
}