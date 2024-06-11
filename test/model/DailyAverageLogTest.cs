namespace ProductiveHoursTracker.test.model;

using NUnit.Framework.Legacy;
using ProductiveHoursTracker.model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class DailyAverageLogTest
{
    private DailyAverageLog log;
    private DailyAverageLog log2;

    private ProductivityEntry energyEntry;
    private ProductivityEntry focusEntry;
    private ProductivityEntry motivationEntry;

    [SetUp]
    public void RunBefore()
    {
        energyEntry =
            new ProductivityEntry(ProductivityEntry.Label.Energy, DateTime.Now.Date, new TimeSpan(21, 0, 0), 1);
        focusEntry = new ProductivityEntry(ProductivityEntry.Label.Focus, DateTime.Now.Date, new TimeSpan(22, 0, 0), 5);
        motivationEntry = new ProductivityEntry(ProductivityEntry.Label.Motivation, DateTime.Now.Date,
            new TimeSpan(23, 0, 0), 6);

        log = new DailyAverageLog();
        log2 = new DailyAverageLog(CreateEntriesList());
    }

    [Test]
    public void TestConstructorNoParams()
    {
        foreach (var label in Enum.GetValues(typeof(ProductivityEntry.Label)).Cast<ProductivityEntry.Label>())
        {
            ClassicAssert.IsEmpty(log.Log[label]);
        }
    }

    [Test]
    public void TestConstructorWithParams()
    {
        foreach (var label in Enum.GetValues(typeof(ProductivityEntry.Label)).Cast<ProductivityEntry.Label>())
        {
            ClassicAssert.AreEqual(1, log2.Log[label].Count);
        }

        ClassicAssert.AreEqual(energyEntry.Level, log2.Log[ProductivityEntry.Label.Energy][energyEntry.Time]);
        ClassicAssert.AreEqual(focusEntry.Level, log2.Log[ProductivityEntry.Label.Focus][focusEntry.Time]);
        ClassicAssert.AreEqual(motivationEntry.Level, log2.Log[ProductivityEntry.Label.Motivation][motivationEntry.Time]);
    }

    [Test]
    public void TestAdd()
    {
        log.Add(energyEntry);
        log.Add(focusEntry);
        log.Add(motivationEntry);

        foreach (var addLabel in Enum.GetValues(typeof(ProductivityEntry.Label)).Cast<ProductivityEntry.Label>())
        {
            ClassicAssert.AreEqual(1, log.Log[addLabel].Count);
        }

        ClassicAssert.AreEqual(energyEntry.Level, log.Log[ProductivityEntry.Label.Energy][energyEntry.Time]);
        ClassicAssert.AreEqual(focusEntry.Level, log.Log[ProductivityEntry.Label.Focus][focusEntry.Time]);
        ClassicAssert.AreEqual(motivationEntry.Level, log.Log[ProductivityEntry.Label.Motivation][motivationEntry.Time]);

        TimeSpan time = new TimeSpan(0, 0, 0);
        ProductivityEntry.Label label = ProductivityEntry.Label.Energy;
        log.Add(new ProductivityEntry(label, DateTime.Now.Date, time, 0));
        ClassicAssert.AreEqual(0, log.Log[label][time]);
        log.Add(new ProductivityEntry(label, DateTime.Now.Date, time, 2));
        ClassicAssert.AreEqual(1, log.Log[label][time]);

        time = new TimeSpan(1, 0, 0);
        label = ProductivityEntry.Label.Focus;
        log.Add(new ProductivityEntry(label, DateTime.Now.Date, time, 1));
        ClassicAssert.AreEqual(1, log.Log[label][time]);
        log.Add(new ProductivityEntry(label, DateTime.Now.Date, time, 3));
        ClassicAssert.AreEqual(2, log.Log[label][time]);
        log.Add(new ProductivityEntry(label, DateTime.Now.Date, time, 5));
        ClassicAssert.AreEqual(3, log.Log[label][time]);

        time = new TimeSpan(2, 0, 0);
        label = ProductivityEntry.Label.Motivation;
        log.Add(new ProductivityEntry(label, DateTime.Now.Date, time, 0));
        ClassicAssert.AreEqual(0, log.Log[label][time]);
        log.Add(new ProductivityEntry(label, DateTime.Now.Date, time, 1));
        ClassicAssert.AreEqual(0.5, log.Log[label][time]);
    }

    [Test]
    public void TestRemove()
    {
        TimeSpan time = new TimeSpan(0, 0, 0);
        ProductivityEntry.Label label = ProductivityEntry.Label.Energy;

        var entry1 = new ProductivityEntry(label, DateTime.Now.Date, time, 0);
        var entry2 = new ProductivityEntry(label, DateTime.Now.Date, time, 2);

        log.Add(entry1);
        log.Add(entry2);
        ClassicAssert.AreEqual(2, log.Remove(entry1));
        ClassicAssert.IsNull(log.Remove(entry2));

        ClassicAssert.IsNull(log2.Remove(energyEntry));
        ClassicAssert.IsNull(log2.Remove(focusEntry));
        ClassicAssert.IsNull(log2.Remove(motivationEntry));
    }

    // EFFECTS: constructs and returns a sample energy list
    private List<ProductivityEntry> CreateEntriesList()
    {
        var list = new List<ProductivityEntry>
        {
            energyEntry,
            focusEntry,
            motivationEntry
        };

        return list;
    }
}


