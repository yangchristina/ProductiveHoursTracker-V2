using NUnit.Framework.Legacy;
using ProductiveHoursTracker.model;

namespace ProductiveHoursTracker.test.model;

using System;
using System.Collections.Generic;
using NUnit.Framework;

public class ProductivityLogTest
{
    private static readonly User User = new User("test");

    private ProductivityLog Log;
    private ProductivityLog Log2;

    private ProductivityEntry EnergyEntry;
    private ProductivityEntry EnergyEntry2;
    private ProductivityEntry FocusEntry;
    private ProductivityEntry MotivationEntry;

    [SetUp]
    public void RunBefore()
    {
        Log = new ProductivityLog(User);

        EnergyEntry =
            new ProductivityEntry(ProductivityEntry.Label.Energy, DateTime.Now.Date, TimeSpan.FromHours(5), 8);
        EnergyEntry2 =
            new ProductivityEntry(ProductivityEntry.Label.Energy, DateTime.Now.Date, TimeSpan.FromHours(5), 4);
        FocusEntry = new ProductivityEntry(ProductivityEntry.Label.Focus, DateTime.Now.Date, TimeSpan.FromHours(7), 8);
        MotivationEntry = new ProductivityEntry(ProductivityEntry.Label.Motivation, DateTime.Now.Date,
            TimeSpan.FromHours(8), 10);

        Log2 = new ProductivityLog(User, CreateEntriesList());
    }

    [Test]
    public void TestConstructorNoParams()
    {
        ClassicAssert.True(Log.Entries.Count == 0);
        ClassicAssert.NotNull(Log.DailyAverageLog.Log);
        ClassicAssert.AreEqual(User, Log.User);
    }

    [Test]
    public void TestConstructorTwoParams()
    {
        ClassicAssert.False(Log2.Entries.Count == 0);
        CollectionAssert.AreEquivalent(CreateEntriesList(), Log2.Entries);
        ClassicAssert.NotNull(Log.DailyAverageLog);
        ClassicAssert.AreEqual(User, Log.User);
    }

    [Test]
    public void TestAdd()
    {
        Log.Add(EnergyEntry);
        Log.Add(EnergyEntry2);
        ClassicAssert.AreEqual(2, Log.Entries.Count);
        ClassicAssert.AreEqual(EnergyEntry, Log.Entries[0]);
        ClassicAssert.AreEqual(EnergyEntry2, Log.Entries[1]);

        Log.Add(FocusEntry);
        ClassicAssert.AreEqual(3, Log.Entries.Count);
        ClassicAssert.AreEqual(FocusEntry, Log.Entries[2]);

        Log.Add(MotivationEntry);
        ClassicAssert.AreEqual(4, Log.Entries.Count);
        ClassicAssert.AreEqual(MotivationEntry, Log.Entries[3]);
    }

    [Test]
    public void TestRemove()
    {
        // initial size of log 2 is 4
        ClassicAssert.NotNull(Log2.Remove(EnergyEntry));
        ClassicAssert.AreEqual(3, Log2.Entries.Count);

        ClassicAssert.Null(Log2.Remove(EnergyEntry2));
        ClassicAssert.AreEqual(2, Log2.Entries.Count);

        ClassicAssert.Null(Log2.Remove(FocusEntry));
        ClassicAssert.AreEqual(1, Log2.Entries.Count);

        ClassicAssert.Null(Log2.Remove(MotivationEntry));
        ClassicAssert.True(Log2.Entries.Count == 0);
    }

    [Test]
    public void TestIsEmpty()
    {
        ClassicAssert.True(Log.Entries.Count == 0);
        Log.Add(FocusEntry);
        ClassicAssert.False(Log.Entries.Count == 0);
    }

    [Test]
    public void TestUpdate()
    {
        Log.Add(EnergyEntry);
        Log.Update(FocusEntry, EnergyEntry);
        
        ClassicAssert.IsFalse(Log.DailyAverageLog.Log[ProductivityEntry.Label.Focus].ContainsKey(EnergyEntry2.Time));
        double? newLevel = Log.DailyAverageLog?.Log?[ProductivityEntry.Label.Focus][FocusEntry.Time];

        ClassicAssert.AreEqual(FocusEntry.Level, newLevel);

        bool isContained = false;
        foreach (Event e in EventLog.Instance)
        {
            if (e.Description.Equals(User.Name + " edited entry: \n" + EnergyEntry + " –––>\n" + FocusEntry))
            {
                isContained = true;
                break;
            }
        }

        ClassicAssert.True(isContained);
    }

    // EFFECTS: creates and returns a sample energy list
    private List<ProductivityEntry> CreateEntriesList()
    {
        var list = new List<ProductivityEntry>();
        list.Add(EnergyEntry);
        list.Add(EnergyEntry2);
        
        list.Add(MotivationEntry);
        list.Add(FocusEntry);
        return list;
    }
}