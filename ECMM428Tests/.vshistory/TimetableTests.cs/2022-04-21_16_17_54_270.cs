using ECMM428;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace ECMM428Tests
{
    [TestClass]
    public class TimetableTests
    {
        [TestMethod]
        public void GenericInitializationTest()
        {
            //Test that two different timetables modified with GenericInitialization with the same seed give the same result
            Timetable timetable1 = MainClass.Initialize(5, 5, 5, 1, 20, 6, 500, 100, 150);
            Timetable timetable2 = new Timetable(timetable1);
            timetable1.GenericInitialization(1);
            Thread.Sleep(1);
            timetable2.GenericInitialization(1);

            bool isEqual = CheckTimetablesAreEqual(timetable1, timetable2);
            Assert.AreEqual(true, isEqual);
        }
        [TestMethod]
        public void NewTimetableTest()
        {
            Timetable timetable1 = MainClass.Initialize(5, 5, 5, 1, 20, 6, 500, 100, 150);
            timetable1.GenericInitialization();
            Timetable timetable2 = new Timetable(timetable1);

            bool isEqual = CheckTimetablesAreEqual(timetable1, timetable2);
            Assert.AreEqual(true, isEqual);
        }
        [TestMethod]
        public void GetAvailableVenuesAtTimeTest1()
        {
            Timetable timetable = MainClass.Initialize(1, 1, 2, 1, 5, 6, 1, 100, 100);
            Module module1 = timetable.GetModules()[0];
            Module module2 = timetable.GetModules()[1];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Lecture> module2Lectures = module2.GetLectures();
            List<Venue> timetableVenues = timetable.GetVenues();

            timetable.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetableVenues[0], 0);
            timetable.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetableVenues[1], 0);
            timetable.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetableVenues[2], 0);
            timetable.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetableVenues[0], 1);
            timetable.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetableVenues[3], 2);
            timetable.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetableVenues[4], 3);

            List<Venue> actualResult = timetable.GetAvailableVenuesAtTime(0);
            List<Venue> expectedResult = new List<Venue> { timetableVenues[3], timetableVenues[4] };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        public void GetAvailableVenuesAtTimeTest2()
        {
            Timetable timetable = MainClass.Initialize(1, 1, 3, 1, 5, 6, 1, 100, 100);
            Module module1 = timetable.GetModules()[0];
            Module module2 = timetable.GetModules()[1];
            Module module3 = timetable.GetModules()[2];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Lecture> module2Lectures = module2.GetLectures();
            List<Lecture> module3Lectures = module3.GetLectures();
            List<Venue> timetableVenues = timetable.GetVenues();

            timetable.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetableVenues[0], 0);
            timetable.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetableVenues[1], 0);
            timetable.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetableVenues[2], 0);
            timetable.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetableVenues[0], 1);
            timetable.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetableVenues[3], 2);
            timetable.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetableVenues[4], 3);

            List<Venue> actualResult = timetable.GetAvailableVenuesAtTime(0);
            List<Venue> expectedResult = new List<Venue> { timetableVenues[3], timetableVenues[4] };
            CollectionAssert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void LectureCanBeAllocatedAtTest1()
        {
            //Check that a true value is recognized
            Timetable timetable1 = MainClass.Initialize(1, 1, 1, 1, 1, 6, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            module1Lectures[0].SetDuration(1);
            module1Lectures[1].SetDuration(3);
            module1Lectures[2].SetDuration(3);

            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[0], 4);

            bool actualValue = timetable1.LectureCanBeAllocatedAt(module1Lectures[2], timetable1Venues[0], 1);
            bool expectedValue = true;
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void LectureCanBeAllocatedAtTest2()
        {
            //Check that another lecture being in the middle of the lecture in the same room results in a false return
            Timetable timetable1 = MainClass.Initialize(1, 1, 1, 1, 1, 6, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            module1Lectures[0].SetDuration(1);
            module1Lectures[1].SetDuration(3);
            module1Lectures[2].SetDuration(3);

            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[0], 3);

            bool actualValue = timetable1.LectureCanBeAllocatedAt(module1Lectures[2], timetable1Venues[0], 1);
            bool expectedValue = false;
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void LectureCanBeAllocatedAtTest3()
        {
            //Check that another lecture being at the same time and same room results in a false return
            Timetable timetable1 = MainClass.Initialize(1, 1, 1, 1, 1, 6, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            module1Lectures[0].SetDuration(1);
            module1Lectures[1].SetDuration(3);
            module1Lectures[2].SetDuration(3);

            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);

            bool actualValue = timetable1.LectureCanBeAllocatedAt(module1Lectures[1], timetable1Venues[0], 0);
            bool expectedValue = false;
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void LectureCanBeAllocatedAtTest4()
        {
            //Check that another lecture ending at the same time in the same room results in a true return
            Timetable timetable1 = MainClass.Initialize(1, 1, 1, 1, 1, 6, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            module1Lectures[0].SetDuration(1);
            module1Lectures[1].SetDuration(1);

            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 1);

            bool actualValue = timetable1.LectureCanBeAllocatedAt(module1Lectures[1], timetable1Venues[0], 0);
            bool expectedValue = true;
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void LectureCanBeAllocatedAtTest5()
        {
            //Test whether a lecture that finishes right at the end of the day in the same room results in a true return
            Timetable timetable1 = MainClass.Initialize(1, 1, 1, 1, 1, 6, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            module1Lectures[0].SetDuration(3);

            bool actualValue = timetable1.LectureCanBeAllocatedAt(module1Lectures[0], timetable1Venues[0], 3);
            bool expectedValue = true;
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void LectureCanBeAllocatedAtTest6()
        {
            //Test whether a lecture that finishes on a different day to when it starts results in a false return
            Timetable timetable1 = MainClass.Initialize(1, 1, 1, 1, 1, 6, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            module1Lectures[0].SetDuration(4);

            bool actualValue = timetable1.LectureCanBeAllocatedAt(module1Lectures[0], timetable1Venues[0], 4);
            bool expectedValue = false;
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void MakeLectureOnlineTest1()
        {
            Timetable timetable1 = MainClass.Initialize(1, 1, 1, 1, 1, 6, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            List<Lecture> module1Lectures = module1.GetLectures();
            module1Lectures[1].SetTime(1);
            module1Lectures[2].SetTime(2);
            timetable1.MakeLectureOnline(module1Lectures[1]);
            timetable1.MakeLectureOnline(module1Lectures[2]);
            int actualValue1 = module1.GetNoOnlineLectures();
            int actualValue2 = timetable1.GetOnlineLecturesFromModule(module1).Count;
            int expectedValue = 2;
            Assert.AreEqual(expectedValue, actualValue1);
            Assert.AreEqual(expectedValue, actualValue2);
        }

        public bool CheckTimetablesAreEqual(Timetable timetable1, Timetable timetable2)
        {
            bool isEqual = true;

            List<Student> students1 = timetable1.GetStudents();
            List<Student> students2 = timetable2.GetStudents();

            List<Lecturer> lecturers1 = timetable1.GetLecturers();
            List<Lecturer> lecturers2 = timetable2.GetLecturers();

            List<Venue> venues1 = timetable1.GetVenues();
            List<Venue> venues2 = timetable2.GetVenues();

            List<Module> modules1 = timetable1.GetModules();
            List<Module> modules2 = timetable2.GetModules();

            List<Course> courses1 = timetable1.GetCourses();
            List<Course> courses2 = timetable2.GetCourses();

            Module[,,] slots1 = timetable1.GetTimetable();
            Module[,,] slots2 = timetable2.GetTimetable();

            //Check students
            for (int i = 0; i < students1.Count; i++)
            {
                //Check number
                if (students1[i].GetNumber() != students2[i].GetNumber()) isEqual = false;

                //Check year of study
                if (students1[i].GetYearOfStudy() != students2[i].GetYearOfStudy()) isEqual = false;

                //Check modules
                foreach (Module m1 in students1[i].GetModules())
                {
                    int moduleNo = m1.GetNumber();
                    bool exists = false;
                    foreach (Module m2 in students2[i].GetModules())
                    {
                        if (m2.GetNumber() == moduleNo)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) isEqual = false;
                }

                //Check lectures
                foreach (Lecture l1 in students1[i].GetLectures())
                {
                    int lectureNo = l1.GetNumber();
                    bool exists = false;
                    foreach (Lecture l2 in students2[i].GetLectures())
                    {
                        if (l2.GetNumber() == lectureNo)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) isEqual = false;
                }
            }

            //Check lecturers
            for (int i = 0; i < lecturers1.Count; i++)
            {
                //Check number
                if (lecturers1[i].GetNumber() != lecturers2[i].GetNumber()) isEqual = false;

                //Check modules
                foreach (Module m1 in lecturers1[i].GetModules())
                {
                    int moduleNo = m1.GetNumber();
                    bool exists = false;
                    foreach (Module m2 in lecturers2[i].GetModules())
                    {
                        if (m2.GetNumber() == moduleNo)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) isEqual = false;
                }

                //Check course
                if (lecturers1[i].GetCourse().GetNumber() != lecturers2[i].GetCourse().GetNumber()) isEqual = false;

                //Check lectures
                foreach (Lecture l1 in lecturers1[i].GetLectures())
                {
                    int lectureNo = l1.GetNumber();
                    bool exists = false;
                    foreach (Lecture l2 in lecturers2[i].GetLectures())
                    {
                        if (l2.GetNumber() == lectureNo)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) isEqual = false;
                }
            }

            //Check venues
            for (int i = 0; i < venues1.Count; i++)
            {
                //Check number
                if (venues1[i].GetNumber() != venues2[i].GetNumber()) isEqual = false;

                //Check capacity
                if (venues1[i].GetCapacity() != venues2[i].GetCapacity()) isEqual = false;

                //Check lectures
                Dictionary<int, Lecture> lectures1 = venues1[i].GetLectures();
                Dictionary<int, Lecture> lectures2 = venues2[i].GetLectures();
                for (int j = 0; j < lectures1.Count; j++)
                {
                    if (lectures1[i] == null || lectures2[i] == null)
                    {
                        if (lectures1[i] != lectures2[i]) isEqual = false;
                    }
                    else if (lectures1[i].GetNumber() != lectures2[i].GetNumber()) isEqual = false;
                }
            }

            //Check modules
            for (int i = 0; i < modules1.Count; i++)
            {
                //Check number
                if (modules1[i].GetNumber() != modules2[i].GetNumber()) isEqual = false;

                //Check students
                foreach (Student s1 in modules1[i].GetStudents())
                {
                    int studentNo = s1.GetNumber();
                    bool exists = false;
                    foreach (Student s2 in modules2[i].GetStudents())
                    {
                        if (s2.GetNumber() == studentNo)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) isEqual = false;
                }

                //Check lectures
                foreach (Lecture l1 in modules1[i].GetLectures())
                {
                    int lectureNo = l1.GetNumber();
                    bool exists = false;
                    foreach (Lecture l2 in modules2[i].GetLectures())
                    {
                        if (l2.GetNumber() == lectureNo)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) isEqual = false;
                }

                //Check course number
                if (modules1[i].GetCourse().GetNumber() != modules2[i].GetCourse().GetNumber()) isEqual = false;

                //Check unassigned lectures
                foreach (Lecture l1 in modules1[i].GetUnassignedLectures())
                {
                    int lectureNo = l1.GetNumber();
                    bool exists = false;
                    foreach (Lecture l2 in modules2[i].GetUnassignedLectures())
                    {
                        if (l2.GetNumber() == lectureNo)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) isEqual = false;
                }
            }

            //Check courses
            for (int i = 0; i < courses1.Count; i++)
            {
                //Check number
                if (courses1[i].GetNumber() != courses2[i].GetNumber()) isEqual = false;

                //Check modules
                foreach (Module m1 in courses1[i].GetModules())
                {
                    int moduleNo = m1.GetNumber();
                    bool exists = false;
                    foreach (Module m2 in courses2[i].GetModules())
                    {
                        if (m2.GetNumber() == moduleNo)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists) isEqual = false;
                }
            }

            //Check slots
            for (int i = 0; i < slots1.GetLength(0); i++)
            {
                for (int j = 0; j < slots1.GetLength(0); j++)
                {
                    for (int k = 0; k < slots1.GetLength(0); k++)
                    {
                        if (!(slots1[i, j, k] == null && slots2[i, j, k] == null)
                            && ((slots1[i, j, k] != null && slots2[i, j, k] == null)
                            || (slots1[i, j, k] == null && slots2[i, j, k] != null)
                            || slots1[i, j, k].GetNumber() != slots2[i, j, k].GetNumber())) isEqual = false;
                    }
                }
            }
            return isEqual;
        }
    }
}
