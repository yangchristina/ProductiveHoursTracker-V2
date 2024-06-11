using NUnit.Framework.Legacy;

namespace ProductiveHoursTracker.test.model;

using NUnit.Framework;
using ProductiveHoursTracker.model;
using System;

public class ProductivityEntryTest
{
    private static readonly DateTime Date = DateTime.Now;
    private static readonly TimeSpan Time = DateTime.Now.TimeOfDay;
    private static readonly User User = new User("test");
    private ProductivityEntry _entry;

    [SetUp]
    public void RunBefore()
    {
        _entry = new ProductivityEntry(ProductivityEntry.Label.Energy, Date, Time, 5);
    }

    [Test]
    public void TestConstructor()
    {
        ClassicAssert.AreEqual(5, _entry.Level);
        ClassicAssert.AreEqual(Date, _entry.Date);
        ClassicAssert.AreEqual(Time, _entry.Time);
    }

    [Test]
    public void TestLabel()
    {
        ClassicAssert.AreEqual(ProductivityEntry.Label.Energy, _entry.GetLabel());
    }

    [Test]
    public void TestDescriptionWithoutKey()
    {
        ClassicAssert.AreEqual($"Energy level of 5 at {Time} on {Date.ToShortDateString()}.", _entry.ToString());
    }

    [Test]
    public void TestDescriptionWithKey()
    {
        ClassicAssert.AreEqual($"Energy level of 5 at {Time} on {Date.ToShortDateString()}.", _entry.ToString());
    }

    [Test]
    public void TestEdit()
    {
        var log = new ProductivityLog(User);
        log.Add(_entry);

        var time1 = TimeSpan.FromHours(4);
        var time2 = TimeSpan.Zero;

        _entry.Edit(ProductivityEntry.Label.Motivation, time1, 6);
        ClassicAssert.AreEqual(ProductivityEntry.Label.Motivation, _entry.GetLabel());
        ClassicAssert.AreEqual(4, _entry.Time.Hours);
        ClassicAssert.AreEqual(6, _entry.Level);

        _entry.Edit(ProductivityEntry.Label.Focus, time2, 1);
        ClassicAssert.AreEqual(ProductivityEntry.Label.Focus, _entry.GetLabel());
        ClassicAssert.AreEqual(0, _entry.Time.Hours);
        ClassicAssert.AreEqual(1, _entry.Level);

        _entry.Edit(ProductivityEntry.Label.Energy, TimeSpan.FromHours(23), 7);
        ClassicAssert.AreEqual(ProductivityEntry.Label.Energy, _entry.GetLabel());
        ClassicAssert.AreEqual(23, _entry.Time.Hours);
        ClassicAssert.AreEqual(7, _entry.Level);

        ClassicAssert.AreEqual(_entry.Level, log.DailyAverageLog.Log[_entry.GetLabel()][_entry.Time]);
        ClassicAssert.IsFalse(log.DailyAverageLog.Log[ProductivityEntry.Label.Motivation].ContainsKey(time1));
        ClassicAssert.IsFalse(log.DailyAverageLog.Log[ProductivityEntry.Label.Focus].ContainsKey(time2));
    }
}
