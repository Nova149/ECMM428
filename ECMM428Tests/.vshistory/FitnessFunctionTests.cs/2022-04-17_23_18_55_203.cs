using ECMM428;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ECMM428Tests
{
    [TestClass]
    public class FitnessFunctionTests
    {
        [TestMethod]
        public void ModuleHardConstraint1Test()
        {
            //Hard constraint 1: All lectures of the same module must occur at different times
            Timetable timetable1 = MainClass.Initialize(1, 1, 3, 1, 7, 5, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            Module module2 = timetable1.GetModules()[1];
            Module module3 = timetable1.GetModules()[2];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Lecture> module2Lectures = module2.GetLectures();
            List<Lecture> module3Lectures = module3.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();
            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[1], 0);
            timetable1.SetSlot(module1, module1Lectures[2].GetDuration(), module1.GetCourse(), timetable1Venues[2], 0);
            timetable1.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetable1Venues[3], 1);
            timetable1.SetSlot(module2, module2Lectures[1].GetDuration(), module2.GetCourse(), timetable1Venues[4], 1);
            timetable1.SetSlot(module2, module2Lectures[2].GetDuration(), module2.GetCourse(), timetable1Venues[5], 2);
            timetable1.SetSlot(module3, module3Lectures[0].GetDuration(), module3.GetCourse(), timetable1Venues[6], 2);

            double penalty = timetable1.ModuleHardConstraint1(module1) + timetable1.ModuleHardConstraint1(module2) + timetable1.ModuleHardConstraint1(module3);
            double expectedValue = 3 * timetable1.HARD_CONSTRAINT_PENALTY;
            Assert.AreEqual(expectedValue, penalty, 0, "Hard constraint 1 test generated incorrect value");
        }
        [TestMethod]
        public void ModuleHardConstraint2Test()
        {
            //Hard constraint 2: All lectures should be assigned
            Timetable timetable1 = MainClass.Initialize(3, 1, 3, 1, 1, 5, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            Module module2 = timetable1.GetModules()[1];
            Module module3 = timetable1.GetModules()[2];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Lecture> module2Lectures = module2.GetLectures();
            List<Lecture> module3Lectures = module3.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();
            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[0], 1);
            timetable1.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetable1Venues[0], 2);
            timetable1.SetSlot(module2, module2Lectures[1].GetDuration(), module2.GetCourse(), timetable1Venues[0], 3);
            timetable1.SetSlot(module2, module2Lectures[2].GetDuration(), module2.GetCourse(), timetable1Venues[0], 4);
            timetable1.SetSlot(module3, module3Lectures[0].GetDuration(), module3.GetCourse(), timetable1Venues[0], 5);

            double penalty = 0;
            foreach (Module m in timetable1.GetModules())
            {
                penalty += timetable1.ModuleHardConstraint2(m);
            }
            double expectedValue = 3 * timetable1.HARD_CONSTRAINT_PENALTY;
            Assert.AreEqual(expectedValue, penalty, 0, "Hard constraint 2 test generated incorrect value");
        }
        [TestMethod]
        public void LecturerHardConstraint1Test()
        {
            //Hard constraint 3: No lecturer should have more than one simultaneous lecture
            Timetable timetable1 = MainClass.Initialize(3, 1, 3, 1, 1, 5, 1, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            Module module2 = timetable1.GetModules()[1];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Lecture> module2Lectures = module2.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();
            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[2].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetable1Venues[0], 2);
            timetable1.SetSlot(module2, module2Lectures[1].GetDuration(), module2.GetCourse(), timetable1Venues[0], 2);
            timetable1.SetSlot(module2, module2Lectures[2].GetDuration(), module2.GetCourse(), timetable1Venues[0], 3);

            double penalty = 0;
            foreach (Lecturer lec in timetable1.GetLecturers())
            {
                penalty += timetable1.LecturerHardConstraint1(lec);
            }
            double expectedValue = 3 * timetable1.HARD_CONSTRAINT_PENALTY;
            Assert.AreEqual(expectedValue, penalty, 0, "Hard constraint 3 test generated incorrect value");
        }
        [TestMethod]
        public void ModuleSoftConstraint1Test()
        {
            //Soft constraint 1: Lectures should be assigned to rooms with sufficient capacity for all the students to attend the lecture
            Timetable timetable1 = MainClass.Initialize(1, 1, 1, 1, 3, 5, 100, 100, 100);
            timetable1.ClearVenues();
            timetable1.AddVenue(50);
            timetable1.AddVenue(50);
            timetable1.AddVenue(150);
            Module module1 = timetable1.GetModules()[0];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();
            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[1], 1);
            timetable1.SetSlot(module1, module1Lectures[2].GetDuration(), module1.GetCourse(), timetable1Venues[2], 2);

            double penalty = 0;
            foreach (Module m in timetable1.GetModules())
            {
                penalty += timetable1.ModuleSoftConstraint1(m);
            }
            double expectedValue = 100 * timetable1.MODULE_SOFT_CONSTRAINT_1_PENALTY;
            Assert.AreEqual(expectedValue, penalty, 0, "Soft constraint 1 test generated incorrect value");
        }
        [TestMethod]
        public void CourseSoftConstraint1Test()
        {
            //Soft constraint 2: Lectures of a course should take place over a minimum number of days in the week
            Timetable timetable1 = MainClass.Initialize(1, 1, 3, 1, 3, 5, 0, 100, 100);
            Module module1 = timetable1.GetModules()[0];
            Module module2 = timetable1.GetModules()[1];
            Module module3 = timetable1.GetModules()[2];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Lecture> module2Lectures = module2.GetLectures();
            List<Lecture> module3Lectures = module3.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            //Day 1
            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[0], 1);
            timetable1.SetSlot(module1, module1Lectures[2].GetDuration(), module1.GetCourse(), timetable1Venues[0], 2);
            timetable1.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetable1Venues[0], 3);
            timetable1.SetSlot(module2, module2Lectures[1].GetDuration(), module2.GetCourse(), timetable1Venues[0], 4);

            //Day 2
            timetable1.SetSlot(module2, module2Lectures[2].GetDuration(), module2.GetCourse(), timetable1Venues[0], 5);

            //Day 3
            timetable1.SetSlot(module3, module3Lectures[0].GetDuration(), module3.GetCourse(), timetable1Venues[0], 10);
            timetable1.SetSlot(module3, module3Lectures[1].GetDuration(), module3.GetCourse(), timetable1Venues[0], 11);
            timetable1.SetSlot(module3, module3Lectures[2].GetDuration(), module3.GetCourse(), timetable1Venues[0], 12);

            double penalty = 0;
            foreach (Course c in timetable1.GetCourses())
            {
                penalty += timetable1.CourseSoftConstraint1(c);
            }
            double expectedValue = 1 * timetable1.COURSE_SOFT_CONSTRAINT_1_PENALTY;
            Assert.AreEqual(expectedValue, penalty, 0, "Soft constraint 2 test generated incorrect value");
        }
        [TestMethod]
        public void StudentSoftConstraint1Test()
        {
            //Soft constraint 3: Each student's lectures on the same day should be held as close together as possible
            Timetable timetable1 = MainClass.Initialize(4, 1, 4, 1, 3, 7, 1, 100, 100, 0, true);
            Module module1 = timetable1.GetModules()[0];
            Module module2 = timetable1.GetModules()[1];
            Module module3 = timetable1.GetModules()[2];
            Module module4 = timetable1.GetModules()[3];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Lecture> module2Lectures = module2.GetLectures();
            List<Lecture> module3Lectures = module3.GetLectures();
            List<Lecture> module4Lectures = module3.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            //Day 1
            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[0], 1);
            timetable1.SetSlot(module1, module1Lectures[2].GetDuration(), module1.GetCourse(), timetable1Venues[0], 2);
            timetable1.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetable1Venues[0], 3);
            timetable1.SetSlot(module2, module2Lectures[1].GetDuration(), module2.GetCourse(), timetable1Venues[0], 5);

            //Day 2
            timetable1.SetSlot(module2, module2Lectures[2].GetDuration(), module2.GetCourse(), timetable1Venues[0], 7);
            timetable1.SetSlot(module3, module3Lectures[0].GetDuration(), module3.GetCourse(), timetable1Venues[0], 8);
            timetable1.SetSlot(module3, module3Lectures[1].GetDuration(), module3.GetCourse(), timetable1Venues[0], 11);

            //Day 3
            timetable1.SetSlot(module4, module4Lectures[0].GetDuration(), module4.GetCourse(), timetable1Venues[0], 14);
            timetable1.SetSlot(module3, module3Lectures[2].GetDuration(), module3.GetCourse(), timetable1Venues[0], 16);


            //Day 4
            timetable1.SetSlot(module4, module4Lectures[1].GetDuration(), module4.GetCourse(), timetable1Venues[0], 22);

            //Day 5
            timetable1.SetSlot(module4, module4Lectures[2].GetDuration(), module4.GetCourse(), timetable1Venues[0], 28);

            double penalty = 0;
            foreach (Student s in timetable1.GetStudents())
            {
                penalty += timetable1.StudentSoftConstraint1(s);
            }
            double expectedValue = 4 * timetable1.STUDENT_SOFT_CONSTRAINT_1_PENALTY;
            Assert.AreEqual(expectedValue, penalty, 0, "Soft constraint 3 test generated incorrect value");
        }
        [TestMethod]
        public void StudentSoftConstraint4Test()
        {
            //Soft constraint 4: Each student should have no more than SOFT_CONSTRAINT_4_CONSECUTIVE_HOURS_ALLOWED hours of lectures in a row on the same day. Penalty scales up for each additional hour
            Timetable timetable1 = MainClass.Initialize(4, 1, 4, 1, 3, 6, 1, 100, 100, 0, true);
            Module module1 = timetable1.GetModules()[0];
            Module module2 = timetable1.GetModules()[1];
            Module module3 = timetable1.GetModules()[2];
            Module module4 = timetable1.GetModules()[3];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Lecture> module2Lectures = module2.GetLectures();
            List<Lecture> module3Lectures = module3.GetLectures();
            List<Lecture> module4Lectures = module3.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            //Day 1
            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[0], 1);
            timetable1.SetSlot(module1, module1Lectures[2].GetDuration(), module1.GetCourse(), timetable1Venues[0], 2);
            timetable1.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetable1Venues[0], 3);
            timetable1.SetSlot(module2, module2Lectures[1].GetDuration(), module2.GetCourse(), timetable1Venues[0], 5);

            //Day 2
            timetable1.SetSlot(module2, module2Lectures[2].GetDuration(), module2.GetCourse(), timetable1Venues[0], 6);
            timetable1.SetSlot(module3, module3Lectures[0].GetDuration(), module3.GetCourse(), timetable1Venues[1], 7);
            timetable1.SetSlot(module3, module3Lectures[1].GetDuration(), module3.GetCourse(), timetable1Venues[0], 9);
            timetable1.SetSlot(module3, module3Lectures[2].GetDuration(), module3.GetCourse(), timetable1Venues[0], 10);
            timetable1.SetSlot(module4, module4Lectures[0].GetDuration(), module4.GetCourse(), timetable1Venues[0], 11);

            //Day 4
            timetable1.SetSlot(module4, module4Lectures[1].GetDuration(), module4.GetCourse(), timetable1Venues[0], 18);

            //Day 5
            timetable1.SetSlot(module4, module4Lectures[2].GetDuration(), module4.GetCourse(), timetable1Venues[0], 24);

            double penalty = 0;
            foreach (Student s in timetable1.GetStudents())
            {
                penalty += timetable1.StudentSoftConstraint2(s);
            }
            double expectedValue = 4 * timetable1.STUDENT_SOFT_CONSTRAINT_2_PENALTY;
            Assert.AreEqual(expectedValue, penalty, 0, "Soft constraint 4 test generated incorrect value");
        }
        [TestMethod]
        public void ModuleSoftConstraint2Test()
        {
            //Soft constraint 5: A module should not have multiple lectures on the same day
            Timetable timetable1 = MainClass.Initialize(3, 1, 3, 1, 1, 5, 1, 100, 100, 0, true);
            Module module1 = timetable1.GetModules()[0];
            Module module2 = timetable1.GetModules()[1];
            Module module3 = timetable1.GetModules()[2];
            List<Lecture> module1Lectures = module1.GetLectures();
            List<Lecture> module2Lectures = module2.GetLectures();
            List<Lecture> module3Lectures = module3.GetLectures();
            List<Venue> timetable1Venues = timetable1.GetVenues();

            //Day 1
            timetable1.SetSlot(module1, module1Lectures[0].GetDuration(), module1.GetCourse(), timetable1Venues[0], 0);
            timetable1.SetSlot(module1, module1Lectures[1].GetDuration(), module1.GetCourse(), timetable1Venues[0], 1);
            timetable1.SetSlot(module1, module1Lectures[2].GetDuration(), module1.GetCourse(), timetable1Venues[0], 2);
            timetable1.SetSlot(module2, module2Lectures[0].GetDuration(), module2.GetCourse(), timetable1Venues[0], 3);
            timetable1.SetSlot(module2, module2Lectures[1].GetDuration(), module2.GetCourse(), timetable1Venues[0], 4);

            //Day 2
            timetable1.SetSlot(module2, module2Lectures[2].GetDuration(), module2.GetCourse(), timetable1Venues[0], 7);
            timetable1.SetSlot(module3, module3Lectures[0].GetDuration(), module3.GetCourse(), timetable1Venues[0], 8);
            timetable1.SetSlot(module3, module3Lectures[1].GetDuration(), module3.GetCourse(), timetable1Venues[0], 9);

            //Day 3
            timetable1.SetSlot(module3, module3Lectures[2].GetDuration(), module3.GetCourse(), timetable1Venues[0], 11);

            double penalty = 0;
            foreach (Module m in timetable1.GetModules())
            {
                penalty += timetable1.ModuleSoftConstraint2(m);
            }
            double expectedValue = 4 * timetable1.MODULE_SOFT_CONSTRAINT_2_PENALTY;
            Assert.AreEqual(expectedValue, penalty, 0, "Soft constraint 5 test generated incorrect value");
        }
    }
}
