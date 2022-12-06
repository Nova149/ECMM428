using ECMM428;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ECMM428Tests
{
    [TestClass]
    public class LecturerTests
    {
        [TestMethod]
        public void GetTimetabledLectures()
        {
            Timetable timetable1 = MainClass.Initialize(1, 1, 1, 1, 1, 6, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            List<Course> module1Lectures = module1.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            timetable1.SetSlot(module1Lectures[0], timetable1Venues[0], 0);
            timetable1.SetSlot(module1Lectures[1], timetable1Venues[0], 1);
            timetable1.MakeLectureAsynchronous(module1Lectures[2]);

            List<Course> expctedResult = new List<Course>() { module1Lectures[0], module1Lectures[1] };
            List<Course> actualResult = module1.GetLecturer().GetTimetabledLectures();
            CollectionAssert.AreEqual(expctedResult, actualResult);
        }
    }
}
