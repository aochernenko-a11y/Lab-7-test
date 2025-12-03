using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab_7;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab_7_test
{
    [TestClass]
    public class MusicTests
    {
        [TestInitialize]
        public void Init()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
        }


        [TestMethod]
        public void Test_SaveLoad_CSV()
        {
            string path = "test_tracks.csv";
            if (File.Exists(path)) File.Delete(path);

            MusicManager m = new MusicManager();
            m.Tracks.Add(new MusicTrack("SongA", "AA", 180, "Rock"));
            m.Tracks.Add(new MusicTrack("SongB", "BB", 200, "Pop"));

            m.SaveCsv(path);
            Assert.IsTrue(File.Exists(path));

            MusicManager m2 = new MusicManager();
            int loaded = m2.LoadCsv(path);

            Assert.AreEqual(2, loaded);
            Assert.AreEqual(2, m2.Tracks.Count);
            Assert.AreEqual("SongA", m2.Tracks[0].Title);
        }


        [TestMethod]
        public void Test_CSV_InvalidLines()
        {
            string path = "broken.csv";

            File.WriteAllLines(path, new string[]
            {
                "Title;Artist;Duration;Genre",
                "Good;AA;180;Rock",
                "InvalidLine",
                "NoDuration;AA;XYZ;Pop",
                "Ok;BB;200;Jazz"
            });

            MusicManager m = new MusicManager();
            int loaded = m.LoadCsv(path);

            Assert.AreEqual(2, loaded);
            Assert.AreEqual("Good", m.Tracks[0].Title);
            Assert.AreEqual("Ok", m.Tracks[1].Title);
        }

        [TestMethod]
        public void Test_SaveLoad_JSON()
        {
            string path = "test.json";
            if (File.Exists(path)) File.Delete(path);

            MusicManager m = new MusicManager();
            m.Tracks.Add(new MusicTrack("One", "A", 100, "Pop"));
            m.Tracks.Add(new MusicTrack("Two", "B", 200, "Rock"));

            m.SaveJson(path);
            Assert.IsTrue(File.Exists(path));

            MusicManager m2 = new MusicManager();
            int loaded = m2.LoadJson(path);

            Assert.AreEqual(2, loaded);
            Assert.AreEqual("One", m2.Tracks[0].Title);
            Assert.AreEqual("Two", m2.Tracks[1].Title);
        }


        [TestMethod]
        public void Test_Manager_AddSortRemoveClear()
        {
            MusicManager m = new MusicManager();

            m.Tracks.Add(new MusicTrack("C", "A", 100, "Pop"));
            m.Tracks.Add(new MusicTrack("A", "B", 150, "Rock"));
            m.Tracks.Add(new MusicTrack("B", "C", 180, "Jazz"));

            // Test Sort
            m.Sort();
            Assert.AreEqual("A", m.Tracks[0].Title);
            Assert.AreEqual("B", m.Tracks[1].Title);
            Assert.AreEqual("C", m.Tracks[2].Title);

            // Test Remove
            m.Tracks.RemoveAll(t => t.Title == "B");
            Assert.AreEqual(2, m.Tracks.Count);

            // Test Clear
            m.Clear();
            Assert.AreEqual(0, m.Tracks.Count);
        }


        [TestMethod]
        public void Test_Static_NormalizeTitle()
        {
            Assert.AreEqual("Hello", MusicTrack.NormalizeTitle("   hELLo   "));
            Assert.AreEqual("A", MusicTrack.NormalizeTitle("a"));
            Assert.AreEqual("", MusicTrack.NormalizeTitle("   "));
        }


        [TestMethod]
        public void Test_Static_NormalizeAllTitles()
        {
            List<MusicTrack> list = new List<MusicTrack>()
            {
                new MusicTrack("  sOnG oNe ", "A", 100, "Pop"),
                new MusicTrack("SONG TWO", "B", 200, "Rock")
            };

            MusicTrack.NormalizeAllTitles(list);

            Assert.AreEqual("Song one", list[0].Title);
            Assert.AreEqual("Song two", list[1].Title);
        }


        [TestMethod]
        public void Test_Static_FindByGenre()
        {
            List<MusicTrack> list = new List<MusicTrack>()
            {
                new MusicTrack("A", "AA", 100, "Pop"),
                new MusicTrack("B", "BB", 150, "Rock"),
                new MusicTrack("C", "CC", 200, "Pop")
            };

            var found = MusicTrack.FindByGenre(list, "Pop");

            Assert.AreEqual(2, found.Count);
            Assert.AreEqual("A", found[0].Title);
            Assert.AreEqual("C", found[1].Title);
        }


        [TestMethod]
        public void Test_Static_AverageDuration()
        {
            List<MusicTrack> list = new List<MusicTrack>()
            {
                new MusicTrack("A", "AA", 100, "Pop"),
                new MusicTrack("B", "BB", 200, "Rock")
            };

            double avg = MusicTrack.CalculateAverageDuration(list);

            Assert.AreEqual(150, avg);
        }


        [TestMethod]
        public void Test_Static_IsValidGenre()
        {
            Assert.IsTrue(MusicTrack.IsValidGenre("Rock"));
            Assert.IsTrue(MusicTrack.IsValidGenre("rock"));
            Assert.IsFalse(MusicTrack.IsValidGenre("TrashHop"));
            Assert.IsFalse(MusicTrack.IsValidGenre(""));
        }


        [TestMethod]
        public void Test_Static_FormatDuration()
        {
            Assert.AreEqual("03:00", MusicTrack.FormatDuration(180));
            Assert.AreEqual("04:05", MusicTrack.FormatDuration(245));
            Assert.AreEqual("00:00", MusicTrack.FormatDuration(-10));
        }
    }
}
