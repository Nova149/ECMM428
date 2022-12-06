using System;
using System.Collections.Generic;

namespace ECMM428
{
    public class Student
    {
        private int studentNumber;
        private int yearOfStudy;
        private List<Module> enrolledModules;
        private List<Lecture> assignedLectures;
        double fitness = 0;

        public Student(int studentNumber, int yearOfStudy, List<Module> enrolledModules)
        {
            this.studentNumber = studentNumber;
            this.yearOfStudy = yearOfStudy;
            this.enrolledModules = enrolledModules;
            this.assignedLectures = new List<Lecture>();
        }
        public Student(int studentNumber, int yearOfStudy, List<Module> enrolledModules, double fitness)
        {
            this.studentNumber = studentNumber;
            this.yearOfStudy = yearOfStudy;
            this.enrolledModules = enrolledModules;
            this.assignedLectures = new List<Lecture>();
            this.fitness = fitness;
        }

        public int GetNumber() { return studentNumber; }
        public int GetYearOfStudy() { return yearOfStudy; }
        public List<Module> GetModules() { return enrolledModules; }
        public void AddModule(Module module)
        {
            if (enrolledModules == null) enrolledModules = new List<Module>();
            enrolledModules.Add(module);
        }
        public List<Lecture> GetLectures() { return assignedLectures; }
        public List<Lecture> GetTimetabledLectures()
        {
            for (int i = 0; i < assignedLectures.Count; i++)
            {
                if (assignedLectures[i].GetTime() > -1)
                {
                    return assignedLectures.GetRange(i, assignedLectures.Count - i);
                }
            }
            return new List<Lecture>();
        }
        public double GetFitness() { return fitness; }
        public void SetFitness(double fitness) { this.fitness = fitness; }
        public void AssignLecture(Lecture lecture)
        {
            //If the lecture is the only one
            if (assignedLectures.Count == 0)
            {
                assignedLectures.Add(lecture);
                return;
            }

            //If the lecture is the earliest
            int lectureTime = lecture.GetTime();
            if (lectureTime <= assignedLectures[0].GetTime())
            {
                assignedLectures.Insert(0, lecture);
                return;
            }

            //If the lecture is between two lectures
            for (int i = 0; i < assignedLectures.Count - 1; i++)
            {
                if (lectureTime > assignedLectures[i].GetTime() && lectureTime <= assignedLectures[i + 1].GetTime())
                {
                    assignedLectures.Insert(i + 1, lecture);
                    return;
                }
            }

            //If the lecture is the last one
            assignedLectures.Add(lecture);
        }
        public void AssignLectures(List<Lecture> lectures)
        {
            foreach (Lecture lecture in lectures)
            {
                AssignLecture(lecture);
            }
        }
        public void UnassignLecture(Lecture lecture)
        {
            assignedLectures.Remove(lecture);
        }
    }
    public class Lecturer
    {
        private int lecturerNumber;
        private Course assignedCourse;
        private List<Module> assignedModules;
        private List<Lecture> assignedLectures;
        double fitness = 0;

        public Lecturer(int lecturerNumber, List<Module> assignedModules, Course assignedCourse)
        {
            this.lecturerNumber = lecturerNumber;
            this.assignedModules = assignedModules;
            this.assignedCourse = assignedCourse;
            this.assignedLectures = new List<Lecture>();
        }
        public Lecturer(int lecturerNumber, List<Module> assignedModules, Course assignedCourse, double fitness)
        {
            this.lecturerNumber = lecturerNumber;
            this.assignedModules = assignedModules;
            this.assignedCourse = assignedCourse;
            this.assignedLectures = new List<Lecture>();
            this.fitness = fitness;
        }

        public int GetNumber() { return lecturerNumber; }
        public List<Module> GetModules() { return assignedModules; }
        public void SetModules(List<Module> modules) { assignedModules = modules; }
        public void AssignModule(Module module) { assignedModules.Add(module); }
        public Course GetCourse() { return assignedCourse; }
        public void SetCourse(Course course)
        {
            assignedCourse = course;
        }
        public List<Lecture> GetLectures() { return assignedLectures; }
        public List<Lecture> GetTimetabledLectures()
        {
            for (int i = 0; i < assignedLectures.Count; i++)
            {
                if (assignedLectures[i].GetTime() > -1)
                {
                    return assignedLectures.GetRange(i, assignedLectures.Count - i);
                }
            }
            return new List<Lecture>();
        }
        public double GetFitness() { return fitness; }
        public void SetFitness(double fitness) { this.fitness = fitness; }
        public void AssignLecture(Lecture lecture)
        {
            //If the lecture is the only one
            if (assignedLectures.Count == 0)
            {
                assignedLectures.Add(lecture);
                return;
            }

            //If the lecture is the earliest
            int lectureTime = lecture.GetTime();
            if (lectureTime <= assignedLectures[0].GetTime())
            {
                assignedLectures.Insert(0, lecture);
                return;
            }

            //If the lecture is between two lectures
            for (int i = 0; i < assignedLectures.Count - 1; i++)
            {
                if (lectureTime > assignedLectures[i].GetTime() && lectureTime <= assignedLectures[i + 1].GetTime())
                {
                    assignedLectures.Insert(i + 1, lecture);
                    return;
                }
            }

            //If the lecture is the last one
            assignedLectures.Add(lecture);
        }
        public void AssignLectures(List<Lecture> lectures)
        {
            foreach (Lecture lecture in lectures)
            {
                AssignLecture(lecture);
            }
        }
        public void UnassignLecture(Lecture lecture)
        {
            assignedLectures.Remove(lecture);
        }
    }
    public class Lecture
    {
        int lectureNumber;
        int duration;
        Module module;
        Venue venue;
        int time;
        public Lecture(int lectureNumber, int duration, Module module, Venue venue, int time)
        {
            this.lectureNumber = lectureNumber;
            this.duration = duration;
            this.module = module;
            this.venue = venue;
            this.time = time;
        }

        public int GetNumber() { return lectureNumber; }
        public int GetDuration() { return duration; }
        public Module GetModule() { return module; }
        public Course GetCourse() { return module.GetCourse(); }
        public Venue GetVenue() { return venue; }
        public int GetTime() { return time; }
        public Lecturer GetLecturer() { return module.GetLecturer(); }
        public void SetDuration(int duration) { this.duration = duration; }
        public void SetModule(Module module) { this.module = module; }
        public void SetVenue(Venue venue) { this.venue = venue; }
        public void SetTime(int time) { this.time = time; }
        public void MakeOnline() { MakeOnline(time); }
        public void MakeOnline(int time)
        {
            module.UnassignLecture(this);
            module.AssignLecture(duration, null, time);
        }
        public void MakeAsynchronous() { MakeOnline(-1); }
        public bool IsOnline()
        {
            if (venue == null && time != -1) return true;
            else return false;
        }
        public bool IsAsynchronous()
        {
            if (venue == null && time == -1 && !module.GetUnassignedLectures().Contains(this)) return true;
            else return false;
        }
    }

    public class Module
    {
        private int moduleNumber;
        private List<Student> enrolledStudents = new List<Student>();
        private Lecturer lecturer;
        private List<Lecture> lectures = new List<Lecture>();
        private List<Lecture> unassignedLectures = new List<Lecture>();
        private int noOnlineLectures = 0;
        private int noAsynchronousLectures = 0;
        private Course course;
        double fitness = 0;

        public Module(int moduleNumber, List<Student> enrolledStudents, Lecturer lecturer, List<Lecture> lectures, Course course)
        {
            this.moduleNumber = moduleNumber;
            this.enrolledStudents = new List<Student>(enrolledStudents);
            this.lecturer = lecturer;
            this.lectures = new List<Lecture>(lectures);
            unassignedLectures = new List<Lecture>(lectures);
            this.course = course;
        }

        public Module(int moduleNumber, List<Student> enrolledStudents, Lecturer lecturer, List<Lecture> lectures, Course course, List<Lecture> unassignedLectures, double fitness)
        {
            this.moduleNumber = moduleNumber;
            this.enrolledStudents = enrolledStudents;
            this.lecturer = lecturer;
            this.lectures = lectures;
            this.unassignedLectures = unassignedLectures;
            this.course = course;
            this.fitness = fitness;
        }
        public Module(int moduleNumber, List<Student> enrolledStudents, Lecturer lecturer, List<Lecture> lectures, Course course, List<Lecture> unassignedLectures, int noOnlineLectures, int noAsynchronousLectures, double fitness)
        {
            this.moduleNumber = moduleNumber;
            this.enrolledStudents = enrolledStudents;
            this.lecturer = lecturer;
            this.lectures = lectures;
            this.unassignedLectures = unassignedLectures;
            this.course = course;
            this.noOnlineLectures = noOnlineLectures;
            this.noAsynchronousLectures = noAsynchronousLectures;
            this.fitness = fitness;
        }

        public void AddStudent(Student student)
        {
            enrolledStudents.Add(student);
        }
        public void AddLectures(int number, int durationOfLecture, int noOfLectures)
        {
            Lecture lecture = new Lecture(number, durationOfLecture, this, null, 0);
            for (int i = 0; i < noOfLectures; i++)
            {
                lectures.Add(lecture);
            }
        }
        public void SetCourse(Course course)
        {
            this.course = course;
        }
        public int GetNumber() { return moduleNumber; }
        public List<Student> GetStudents() { return enrolledStudents; }
        public void SetStudents(List<Student> enrolledStudents) { this.enrolledStudents = enrolledStudents; }
        public Lecturer GetLecturer() { return this.lecturer; }
        public void SetLecturer(Lecturer lecturer) { this.lecturer = lecturer; }
        public List<Lecture> GetLectures() { return lectures; }
        public List<Lecture> GetTimetabledLectures()
        {
            List<Lecture> output = new List<Lecture>();
            for (int i = 0; i < lectures.Count; i++)
            {
                if (lectures[i].GetTime() > -1)
                {
                    output.Add(lectures[i]);
                }
            }
            return output;
        }
        public List<Lecture> GetInPersonLectures() {
            List<Lecture> inPersonLectures = new List<Lecture>();
            foreach (Lecture l in lectures)
            {
                if (l.GetVenue() != null) inPersonLectures.Add(l);
            }
            return inPersonLectures; 
        }
        public void SetLectures(List<Lecture> lectures) { this.lectures = lectures; }
        public Lecture GetLecture(Venue venue, int time)
        {
            foreach (Lecture lecture in GetLectures())
            {
                if (lecture.GetVenue() == venue && lecture.GetTime() == time) return lecture;
            }
            return null;
        }
        public List<Lecture> GetUnassignedLectures() { return unassignedLectures; }
        public void SetUnassignedLectures(List<Lecture> unassignedLectures) { this.unassignedLectures = unassignedLectures; }
        public List<Lecture> GetAssignedLectures()
        {
            List<Lecture> assignedLectures = new List<Lecture>();
            foreach (Lecture lecture in lectures)
            {
                if (!unassignedLectures.Contains(lecture)) assignedLectures.Add(lecture);
            }
            return assignedLectures;
        }
        public Course GetCourse() { return course; }
        public List<Venue> GetVenuesAtTime(int time)
        {
            List<Lecture> lectures = GetLectures();
            List<Venue> venues = new List<Venue>();

            foreach (Lecture lecture in lectures)
            {
                if (lecture.GetTime() == time) venues.Add(lecture.GetVenue());
            }

            return venues;
        }
        public int GetNoOnlineLectures() { return noOnlineLectures; }
        public void IncrementNoOnlineLectures() { noOnlineLectures++; }
        public void ReduceNoOnlineLectures() { noOnlineLectures--; }
        public int GetNoAsynchronousLectures() { return noAsynchronousLectures; }
        public void IncrementNoAsynchronousLectures() { noAsynchronousLectures++; }
        public void ReduceNoAsynchronousLectures() { noAsynchronousLectures--; }
        public double GetFitness() { return fitness; }
        public void SetFitness(double fitness) { this.fitness = fitness; }
        public void AssignLecture(Lecture lecture, Venue venue, int time)
        {
            lecture.SetVenue(venue);
            lecture.SetTime(time);

            if (venue != null) venue.AddLecture(lecture);
            else if (time > -1) noOnlineLectures++;
            else if (time == -1) noAsynchronousLectures++;

            unassignedLectures.Remove(lecture);
            foreach (Student s in enrolledStudents)
            {
                if (!s.GetLectures().Contains(lecture))
                {
                    s.AssignLecture(lecture);
                }
            }
            if (!lecturer.GetLectures().Contains(lecture))
            {
                lecturer.AssignLecture(lecture);
            }
        }
        public void AssignLecture(int duration, Venue venue, int time)
        {
            Lecture lecture = null;
            foreach (Lecture l in unassignedLectures)
            {
                if (l.GetDuration() == duration)
                {
                    lecture = l;
                    break;
                }
            }

            AssignLecture(lecture, venue, time);
        }
        public void UnassignLecture(Lecture lecture)
        {
            unassignedLectures.Add(lecture);
            foreach (Student s in enrolledStudents) s.UnassignLecture(lecture);
            lecturer.UnassignLecture(lecture);
            Venue venue = lecture.GetVenue();
            Module module = lecture.GetModule();
            if (!module.GetUnassignedLectures().Contains(lecture))
            {
                if (venue != null) venue.RemoveLecture(lecture);
                else if (lecture.GetTime() != -1) ReduceNoOnlineLectures();
                else ReduceNoAsynchronousLectures();
            }
            lecture.SetTime(-1);
            lecture.SetVenue(null);
        }
        public void AddUnassignedLecture(Lecture lecture)
        {
            unassignedLectures.Add(lecture);
        }
        public bool AllLecturesAssigned()
        {
            return unassignedLectures.Count == 0;
        }
    }

    public class Course
    {
        private int courseNumber;
        private List<Module> modules;
        private List<Lecturer> lecturers = new List<Lecturer>();
        double fitness = 0;

        public Course(int courseNumber, List<Module> modules, List<Lecturer> lecturers)
        {
            this.courseNumber = courseNumber;
            this.modules = modules;
            this.lecturers = lecturers;
        }
        public Course(int courseNumber, List<Module> modules, List<Lecturer> lecturers, double fitness)
        {
            this.courseNumber = courseNumber;
            this.modules = modules;
            this.lecturers = lecturers;
            this.fitness = fitness;
        }

        public void AddModule(Module module)
        {
            modules.Add(module);
        }
        public int GetNumber() { return courseNumber; }
        public List<Student> GetStudents() {
            List<Student> students = new List<Student>();

            foreach (Module module in modules)
            {
                foreach (Student student in module.GetStudents())
                {
                    if (!students.Contains(student)) students.Add(student);
                }
            }
            return students;
        }

        public List<Module> GetModules()
        {
            return modules;
        }
        public void SetModules(List<Module> modules)
        {
            this.modules = modules;
        }
        public List<Lecturer> GetLecturers()
        {
            return lecturers;
        }
        public void SetLecturers(List<Lecturer> lecturers)
        {
            this.lecturers = lecturers;
        }
        public void AddLecturer(Lecturer lecturer)
        {
            lecturers.Add(lecturer);
        }
        public List<Lecture> GetLectures()
        {
            List<Lecture> lectures = new List<Lecture>();
            foreach (Module m in modules)
            {
                foreach (Lecture l in m.GetLectures())
                {
                    lectures.Add(l);
                }
            }
            return lectures;
        }
        public double GetFitness() { return fitness; }
        public void SetFitness(double fitness) { this.fitness = fitness; }
    }

    public class Venue
    {
        private int venueNumber;
        private int venueCapacity;
        private Dictionary<int, Lecture> lectures;

        public Venue(int venueNumber, int venueCapacity, int noTimeslots)
        {
            this.venueNumber = venueNumber;
            this.venueCapacity = venueCapacity;
            lectures = new Dictionary<int, Lecture>();
            for (int i = 0; i < noTimeslots; i++)
            {
                lectures.Add(i, null);
            }
        }
        public Venue(int venueNumber, int venueCapacity, Dictionary<int, Lecture> lectures)
        {
            this.venueNumber = venueNumber;
            this.venueCapacity = venueCapacity;
            this.lectures = lectures;
        }

        public int GetNumber() { return venueNumber; }
        public int GetCapacity() { return venueCapacity; }
        public Dictionary<int, Lecture> GetLectures() { return lectures; }
        public void AddLecture(Lecture lecture)
        {
            int startTime = lecture.GetTime();
            for (int i = 0; i < lecture.GetDuration(); i++)
            {
                lectures[startTime + i] = lecture;
            }
        }
        public void RemoveLecture(Lecture lecture)
        {
            int startTime = lecture.GetTime();
            for (int i = 0; i < lecture.GetDuration(); i++)
            {
                lectures[startTime + i] = null;
            }
        }
        public Lecture GetLectureAt(int time)
        {
            return lectures[time];
        }
    }

    public class Timetable
    {
        private Module[,,] timetable;
        private int days = 5;
        private int timeslotsPerDay;
        private List<Course> courses;
        private List<Venue> venues;
        private int numberOfCourses;
        private int numberOfVenues;
        private int numberOfTimes;
        private Dictionary<int, Course> courseIndexes = new Dictionary<int, Course>();
        private Dictionary<int, Venue> venueIndexes = new Dictionary<int, Venue>();
        private Dictionary<int, Student> studentIndexes = new Dictionary<int, Student>();
        private Dictionary<int, Lecturer> lecturerIndexes = new Dictionary<int, Lecturer>();

        //Hard constraints:
        //1: All lectures of the same module must occur at different times
        //2: All lectures should be assigned
        //3: No lecturer should have more than one simultaneous lecture

        //Soft constraints:
        //1: Lectures should be assigned to rooms with sufficient capacity for all the students to attend the lecture
        //2: Lectures of a course should take place over a minimum number of days in the week
        //3: Each student's lectures on the same day should be held as close together as possible
        //4: Each student should have no more than SOFT_CONSTRAINT_4_CONSECUTIVE_HOURS_ALLOWED hours of lectures in a row on the same day
        //5: A module should not have multiple lectures on the same day

        //Initialize constants
        int HARD_CONSTRAINT_PENALTY = 10000;
        int MODULE_SOFT_CONSTRAINT_1_PENALTY = 1;
        int COURSE_SOFT_CONSTRAINT_1_PENALTY = 1;
        double STUDENT_SOFT_CONSTRAINT_1_PENALTY = 0.1;
        double STUDENT_SOFT_CONSTRAINT_2_PENALTY = 0.1;
        int STUDENT_SOFT_CONSTRAINT_2_CONSECUTIVE_HOURS_ALLOWED = 2;
        double MODULE_SOFT_CONSTRAINT_2_PENALTY = 3;

        public Timetable(Timetable timetable)
        {
            Module[,,] originalTimetable = timetable.GetTimetable();
            this.timetable = new Module[originalTimetable.GetLength(0), originalTimetable.GetLength(1), originalTimetable.GetLength(2)];
            courses = new List<Course>();
            this.days = timetable.GetNoOfDays();
            this.numberOfTimes = timetable.GetNoTimeslots();

            //Copy students
            List<Student> newStudents = timetable.GetStudents().ConvertAll(student => new Student(student.GetNumber(), student.GetYearOfStudy(), new List<Module>(), student.GetFitness()));

            //Copy lecturers
            List<Lecturer> newLecturers = timetable.GetLecturers().ConvertAll(lecturer => new Lecturer(lecturer.GetNumber(), new List<Module>(), null, lecturer.GetFitness()));

            //Copy venues
            List<Venue> newVenues = timetable.GetVenues().ConvertAll(venue => new Venue(venue.GetNumber(), venue.GetCapacity(), numberOfTimes));

            //Index students
            foreach (Student s in newStudents)
            {
                studentIndexes.Add(s.GetNumber(), s);
            }

            //Index lecturers
            foreach (Lecturer lec in newLecturers)
            {
                lecturerIndexes.Add(lec.GetNumber(), lec);
            }

            //Index venues
            foreach (Venue v in newVenues)
            {
                venueIndexes.Add(v.GetNumber(), v);
            }

            foreach (Course c in timetable.GetCourses())
            {
                //Copy course
                Course newCourse = new Course(c.GetNumber(), null, new List<Lecturer>(), c.GetFitness());
                List<Module> newModulesForNewCourse = new List<Module>();
                foreach (Module m in c.GetModules())
                {
                    //Copy each module in original course to copied course
                    Lecturer moduleLecturer = lecturerIndexes[m.GetLecturer().GetNumber()];
                    Module newModule = new Module(m.GetNumber(), null, moduleLecturer, null, newCourse, null, m.GetNoOnlineLectures(), m.GetNoAsynchronousLectures(), m.GetFitness());
                    moduleLecturer.AssignModule(newModule);
                    moduleLecturer.SetCourse(newCourse);
                    if (!newCourse.GetLecturers().Contains(moduleLecturer)) newCourse.AddLecturer(moduleLecturer);

                    //Copy students
                    List<Student> enrolledStudents = new List<Student>();
                    foreach (Student s in m.GetStudents())
                    {
                        //Find corresponding student in studentIndex. Add module to the copied student, and add the copied student to the module's enrolled students
                        Student newStudent = studentIndexes[s.GetNumber()];
                        newStudent.AddModule(newModule);
                        enrolledStudents.Add(newStudent);
                    }

                    //Copy lectures
                    List<Lecture> newLectures = new List<Lecture>();
                    List<Lecture> newUnassignedLectures = new List<Lecture>();
                    foreach (Lecture l in m.GetLectures())
                    {
                        //Find venue for newLecture
                        Venue lectureVenue;
                        if (l.GetVenue() == null) lectureVenue = null;
                        else lectureVenue = venueIndexes[l.GetVenue().GetNumber()];

                        //Create newLecture
                        Lecture newLecture = new Lecture(l.GetNumber(), l.GetDuration(), newModule, lectureVenue, l.GetTime());
                        newLectures.Add(newLecture);

                        //Check if lecture has been assigned to a timeslot
                        if (m.GetUnassignedLectures().Contains(l)) newUnassignedLectures.Add(newLecture);
                        //If the lecture is assigned
                        else
                        {
                            //If the lecture is not online or asynchronous, add it to the timetable
                            if (l.GetVenue() != null)
                            {
                                //Assign lecture to venue
                                lectureVenue.AddLecture(newLecture);

                                for (int i = 0; i < l.GetDuration(); i++)
                                {
                                    this.timetable[m.GetCourse().GetNumber(), l.GetVenue().GetNumber(), l.GetTime() + i] = newModule;
                                }
                            }

                            //Assign lecture to students
                            foreach (Student s in m.GetStudents())
                            {
                                Student newS = studentIndexes[s.GetNumber()];
                                newS.AssignLecture(newLecture);
                            }

                            //Assign lecture to lecturer
                            lecturerIndexes[m.GetLecturer().GetNumber()].AssignLecture(newLecture);
                        }
                    }
                    newModule.SetStudents(enrolledStudents);
                    newModule.SetLectures(newLectures);
                    newModule.SetUnassignedLectures(newUnassignedLectures);
                    newModulesForNewCourse.Add(newModule);
                }
                newCourse.SetModules(newModulesForNewCourse);
                courses.Add(newCourse);
            }

            venues = newVenues;
            numberOfCourses = courses.Count;
            numberOfVenues = venues.Count;
            numberOfTimes = timetable.GetNoTimeslots();



            for (int i = 0; i < courses.Count; i++)
            {
                courseIndexes.Add(courses[i].GetNumber(), courses[i]);
            }

            timeslotsPerDay = numberOfTimes / days;
        }

        public Timetable(List<Course> courses, List<Venue> venues, int numberOfTimes)
        {
            this.courses = courses;
            this.venues = venues;
            numberOfCourses = courses.Count;
            numberOfVenues = venues.Count;
            this.numberOfTimes = numberOfTimes * days;
            timetable = new Module[numberOfCourses, numberOfVenues, this.numberOfTimes];

            for (int i = 0; i < courses.Count; i++)
            {
                courseIndexes.Add(courses[i].GetNumber(), courses[i]);
            }
            for (int i = 0; i < venues.Count; i++)
            {
                venueIndexes.Add(venues[i].GetNumber(), venues[i]);
            }

            foreach (Course course in courses)
            {
                foreach (Student s in course.GetStudents())
                {
                    studentIndexes.Add(s.GetNumber(), s);
                }
                foreach (Lecturer lec in course.GetLecturers())
                {
                    lecturerIndexes.Add(lec.GetNumber(), lec);
                }
            }
            timeslotsPerDay = this.numberOfTimes / days;
        }

        public Module[,,] GetTimetable()
        {
            return timetable;
        }

        public List<Lecture> GetLectures()
        {
            List<Lecture> lectures = new List<Lecture>();
            foreach (Course c in courses)
            {
                foreach (Module m in c.GetModules())
                {
                    foreach (Lecture l in m.GetLectures())
                    {
                        lectures.Add(l);
                    }
                }
            }
            return lectures;
        }
        public Lecture GetLecture(Course course, Venue venue, int time)
        {
            Module module = timetable[course.GetNumber(), venue.GetNumber(), time];
            if (module == null) return null;
            return module.GetLecture(venue, time);
        }
        public List<Lecture> GetUnassignedLectures()
        {
            List<Lecture> lectures = new List<Lecture>();
            foreach (Course c in courses)
            {
                foreach (Module m in c.GetModules())
                {
                    foreach (Lecture l in m.GetUnassignedLectures())
                    {
                        lectures.Add(l);
                    }
                }
            }
            return lectures;
        }
        public List<List<Lecture>> GetLecturesByCourse()
        {
            List<List<Lecture>> lectures = new List<List<Lecture>>();
            foreach (Course c in courses)
            {
                List<Lecture> lecturesInCourse = new List<Lecture>();
                foreach (Module m in c.GetModules())
                {
                    foreach (Lecture l in m.GetLectures())
                    {
                        lecturesInCourse.Add(l);
                    }
                }
                lectures.Add(lecturesInCourse);
            }
            return lectures;
        }
        public List<List<Lecture>> GetLecturesByModule()
        {
            List<List<Lecture>> lectures = new List<List<Lecture>>();
            foreach (Module m in GetModules())
            {
                List<Lecture> lecturesInModule = new List<Lecture>();
                foreach (Lecture l in m.GetLectures())
                {
                    lecturesInModule.Add(l);
                }
                lectures.Add(lecturesInModule);
            }
            return lectures;
        }
        public List<Lecture> GetLectures(Course course)
        {
            List<Lecture> lectures = new List<Lecture>();
            foreach (Module m in course.GetModules())
            {
                foreach (Lecture l in m.GetLectures())
                {
                    lectures.Add(l);
                }
            }
            return lectures;
        }
        public List<Lecture> GetLectures(Module module)
        {
            List<Lecture> lectures = new List<Lecture>();
            foreach (Lecture l in module.GetLectures())
            {
                lectures.Add(l);
            }
            return lectures;
        }
        public List<Lecture> GetLecturesStartAtTime(int time)
        {
            List<Lecture> lectures = new List<Lecture>();
            foreach (Module m in GetModules())
            {
                foreach (Lecture l in m.GetLectures())
                {
                    if (l.GetTime() == time) lectures.Add(l);
                }
            }
            return lectures;
        }
        public List<Lecture> GetOnlineLecturesStartAtTime(int time)
        {
            List<Lecture> lectures = new List<Lecture>();
            foreach (Module m in GetModules())
            {
                foreach (Lecture l in m.GetLectures())
                {
                    if (l.GetVenue() == null && l.GetTime() == time) lectures.Add(l);
                }
            }
            return lectures;
        }
        public List<Lecture> GetOnlineLecturesFromModule(Module module)
        {
            List<Lecture> lectures = new List<Lecture>();
            foreach (Lecture l in module.GetLectures())
            {
                if (l.GetVenue() == null && l.GetTime() != -1) lectures.Add(l);
            }
            return lectures;
        }
        public List<Lecture> GetLecturesStartAtTime(int day, int time)
        {
            int realTime = day * timeslotsPerDay + time;
            List<Lecture> lectures = new List<Lecture>();
            foreach (Module m in GetModules())
            {
                foreach (Lecture l in m.GetLectures())
                {
                    if (l.GetTime() == realTime) lectures.Add(l);
                }
            }
            return lectures;
        }
        public List<Course> GetCourses()
        {
            return courses;
        }
        public Module GetModule(int moduleID)
        {
            foreach (Module module in GetModules()) if (module.GetNumber() == moduleID) return module;
            return null;
        }
        public List<Module> GetModules()
        {
            List<Module> modules = new List<Module>();

            for (int i = 0; i < courses.Count; i++)
            {
                List<Module> modulesInCourse = courses[i].GetModules();
                for (int j = 0; j < modulesInCourse.Count; j++)
                {
                    if (!modules.Contains(modulesInCourse[j])) modules.Add(modulesInCourse[j]);
                }
            }
            return modules;
        }
        public List<Module> GetModulesWithLecturesAtTime(int time)
        {
            List<Module> modules = new List<Module>();
            Module[,] timeInfo = GetTimeInfo(time);

            for (int i = 0; i < timeInfo.GetLength(0); i++)
            {
                for (int j = 0; j < timeInfo.GetLength(1); j++)
                {
                    if (timeInfo[i, j] != null) modules.Add(timeInfo[i, j]);
                }
            }
            return modules;
        }
        public List<Module> GetModulesWithLecturesAtTime(int day, int time)
        {
            List<Module> modules = new List<Module>();
            Module[,] timeInfo = GetTimeInfo(day * timeslotsPerDay + time);

            for (int i = 0; i < timeInfo.GetLength(0); i++)
            {
                for (int j = 0; j < timeInfo.GetLength(1); j++)
                {
                    if (timeInfo[i, j] != null) modules.Add(timeInfo[i, j]);
                }
            }
            return modules;
        }

        public List<Venue> GetVenues()
        {
            return venues;
        }
        public int GetNoOfVenues()
        {
            return numberOfVenues;
        }
        public void AddVenue(int venueCapacity)
        {
            Venue venue = new Venue(venues.Count, venueCapacity, numberOfTimes);
            venueIndexes.Add(venues.Count, venue);
            venues.Add(venue);
        }
        public void ClearVenues()
        {
            venues.Clear();
            venueIndexes.Clear();
            venueIndexes.Clear();
        }

        public Module[,] GetCourseInfo(Course course)
        {
            int courseIndex = course.GetNumber();
            Module[,] courseInfo = new Module[numberOfVenues, numberOfTimes];
            for (int i = 0; i < numberOfVenues; i++)
            {
                for (int j = 0; j < numberOfTimes; j++)
                {
                    courseInfo[i, j] = timetable[courseIndex, i, j];
                }
            }
            return courseInfo;
        }

        public Module[,] GetVenueInfo(Venue venue)
        {
            int venueIndex = venue.GetNumber();
            Module[,] venueInfo = new Module[numberOfCourses, numberOfTimes];
            for (int i = 0; i < numberOfCourses; i++)
            {
                for (int j = 0; j < numberOfTimes; j++)
                {
                    venueInfo[i, j] = timetable[i, venueIndex, j];
                }
            }
            return venueInfo;
        }
        public List<Venue> GetAvailableVenuesAtTime(int timeNumber)
        {
            List<Venue> availableVenues = new List<Venue>();
            for (int j = 0; j < numberOfVenues; j++)
            {
                bool venueUsedByCourse = false;
                for (int i = 0; i < numberOfCourses; i++)
                {
                    if (timetable[i, j, timeNumber] != null)
                    {
                        venueUsedByCourse = true;
                        break;
                    }
                }
                if (!venueUsedByCourse) availableVenues.Add(venues[j]);
            }

            return availableVenues;
        }
        public List<Venue> GetAvailableVenuesAtTime(int dayNo, int timeNumber)
        {
            return GetAvailableVenuesAtTime(dayNo * timeslotsPerDay + timeNumber);
        }
        public List<Venue> GetAvailableVenuesAtTime(Lecture lecture, int time)
        {
            if (lecture == null) return null;
            List<Venue> availableVenues = new List<Venue>();

            foreach (Venue venue in venues)
            {
                if (LectureCanBeAllocatedAt(lecture, venue, time)) availableVenues.Add(venue);
            }

            return availableVenues;
        }
        public List<Venue> GetAvailableVenuesAtTime(Lecture lecture, int day, int time)
        {
            return GetAvailableVenuesAtTime(lecture, day * timeslotsPerDay + time);
        }
        public Module[,] GetTimeInfo(int timeNumber)
        {
            Module[,] timeInfo = new Module[numberOfCourses, numberOfVenues];
            for (int i = 0; i < numberOfCourses; i++)
            {
                for (int j = 0; j < numberOfVenues; j++)
                {
                    timeInfo[i, j] = timetable[i, j, timeNumber];
                }
            }
            return timeInfo;
        }
        public Module[,] GetTimeInfo(int dayNo, int timeNumber)
        {
            Module[,] timeInfo = new Module[numberOfCourses, numberOfVenues];
            for (int i = 0; i < numberOfCourses; i++)
            {
                for (int j = 0; j < numberOfVenues; j++)
                {
                    timeInfo[i, j] = timetable[i, j, timeslotsPerDay * dayNo + timeNumber];
                }
            }
            return timeInfo;
        }

        public List<Course> GetCoursesAtTime(int time)
        {
            List<Course> courses = new List<Course>();

            for (int i = 0; i < numberOfCourses; i++)
            {
                for (int j = 0; j < numberOfVenues; j++)
                {
                    if (timetable[i, j, time] != null) courses.Add(courses[i]);
                }
            }

            return courses;
        }

        public List<Course> GetCoursesAtTime(int day, int time)
        {
            return GetCoursesAtTime(day * timeslotsPerDay + time);
        }

        public int GetNoTimeslotsInDay()
        {
            return numberOfTimes / days;
        }
        public int GetNoTimeslots()
        {
            return numberOfTimes;
        }

        public Module[,,] GetDay(int dayNo)
        {
            Module[,,] day = new Module[numberOfCourses, numberOfVenues, timeslotsPerDay];

            for (int i = 0; i < timeslotsPerDay; i++)
            {
                for (int j = 0; j < numberOfCourses; j++)
                {
                    for (int k = 0; k < numberOfVenues; k++)
                    {
                        day[j, k, i] = timetable[j, k, i + dayNo * timeslotsPerDay];
                    }
                }
            }

            return day;
        }

        public int GetNoOfDays()
        {
            return days;
        }

        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student>();

            foreach (int key in studentIndexes.Keys)
            {
                students.Add(studentIndexes[key]);
            }
            return students;
        }
        public List<Lecturer> GetLecturers()
        {
            List<Lecturer> lecturer = new List<Lecturer>();

            foreach (int key in lecturerIndexes.Keys)
            {
                lecturer.Add(lecturerIndexes[key]);
            }
            return lecturer;
        }

        public void SetSlot(Module module, int lectureLength, Course course, Venue venue, int time)
        {
            int courseIndex = course.GetNumber();

            if (venue != null)
            {
                int venueIndex = venue.GetNumber();

                for (int i = 0; i < lectureLength; i++)
                {
                    timetable[courseIndex, venueIndex, time + i] = module;
                }
            }

            module.AssignLecture(lectureLength, venue, time);
        }

        public void SetSlot(Module module, int lectureLength, Course course, Venue venue, int day, int time)
        {
            SetSlot(module, lectureLength, course, venue, day * timeslotsPerDay + time);
        }
        public void SetSlot(Lecture lecture, Venue venue, int time)
        {
            int courseIndex = lecture.GetCourse().GetNumber();
            int venueIndex = venue.GetNumber();
            Module module = lecture.GetModule();

            module.AssignLecture(lecture, venue, time);

            for (int i = 0; i < lecture.GetDuration(); i++)
            {
                timetable[courseIndex, venueIndex, time + i] = module;
            }
        }
        public void SetSlot(Lecture lecture, Venue venue, int day, int time)
        {
            SetSlot(lecture, venue, day * timeslotsPerDay + time);
        }
        public void MakeLectureOnline(Lecture lecture, int time)
        {
            lecture.MakeOnline(time);
            if (lecture.GetVenue() != null)
            {
                for (int i = 0; i < lecture.GetDuration(); i++)
                {
                    timetable[lecture.GetCourse().GetNumber(), lecture.GetVenue().GetNumber(), time + i] = null;
                }
            }
        }
        public void MakeLectureOnline(Lecture lecture, int day, int time)
        {
            MakeLectureOnline(lecture, day * timeslotsPerDay + time);
        }
        public void MakeLectureOnline(Lecture lecture)
        {
            MakeLectureOnline(lecture, lecture.GetTime());
        }
        public void MakeLectureAsynchronous(Lecture lecture)
        {
            MakeLectureOnline(lecture, -1);
        }
        public bool LectureCanBeAllocatedAt(Lecture lecture, Venue venue, int time)
        {
            //If the lecture would start and end on different days, return false
            int lectureFinishTime = time + lecture.GetDuration();
            double lectureStartsOn = Math.Floor((double)time / timeslotsPerDay);
            double lectureFinishesOn = Math.Floor((double)lectureFinishTime / timeslotsPerDay);
            if (lectureStartsOn != lectureFinishesOn && !(lectureFinishesOn - lectureStartsOn == 1 && lectureFinishTime % timeslotsPerDay == 0)) return false;

            Dictionary<int, Lecture> lectures = venue.GetLectures();

            //Check if a lecture is already allocated to any time that the new lecture would cover
            for (int i = 0; i < lecture.GetDuration(); i++)
            {
                if (lectures[time + i] != null) return false;
            }

            return true;
        }
        public bool LectureCanBeAllocatedAt(Lecture lecture, Venue venue, int day, int time)
        {
            return LectureCanBeAllocatedAt(lecture, venue, day * timeslotsPerDay + time);
        }
        public int MaxFreeSlotForRoom(Venue venue, int time)
        {
            //Set initial value for the max time to be the time left in the day
            int maxTime = (timeslotsPerDay - time % timeslotsPerDay) - 1;

            Dictionary<int, Lecture> lectures = venue.GetLectures();
            for (int i = 0; i <= maxTime; i++)
            {
                if (lectures[time + i] != null) return i;
            }
            return maxTime;
        }
        public int MaxFreeSlotForRoom(Venue venue, int day, int time)
        {
            return MaxFreeSlotForRoom(venue, day * timeslotsPerDay + time);
        }

        public bool AllLecturesAssigned()
        {
            foreach (Module m in GetModules())
            {
                if (!m.AllLecturesAssigned()) return false;
            }
            return true;
        }
        public void UnassignLecture(Lecture lecture)
        {
            Module module = lecture.GetModule();
            module.UnassignLecture(lecture);

            Venue venue = lecture.GetVenue();
            if (venue != null)
            {
                int courseNo = lecture.GetCourse().GetNumber();
                int venueNo = venue.GetNumber();
                int time = lecture.GetTime();
                for (int i = 0; i < lecture.GetDuration(); i++)
                {
                    timetable[courseNo, venueNo, time + i] = null;
                }
            }
        }
        ///Initialize timetable by placing lectures in random timeslots
        public void GenericInitialization()
        {
            //Use a random seed
            Random rand = new Random();
            GenericInitialization(rand.Next());
        }
        public void GenericInitialization(int seed)
        {
            List<Lecture> unassignedLectures = GetUnassignedLectures();

            List<int> timeslots = new List<int>();
            for (int i = 0; i < GetNoTimeslots(); i++)
            {
                timeslots.Add(i);
            }

            for (int i = 0; i < unassignedLectures.Count; i++)
            {
                Lecture lecture = unassignedLectures[i];
                if (timeslots.Count == 0) lecture.MakeAsynchronous();

                //Get random timeslot
                timeslots.Shuffle(seed);

                for (int j = 0; j < timeslots.Count; j++)
                {
                    int time = timeslots[j];

                    //Get random venue
                    List<Venue> availableVenues = GetAvailableVenuesAtTime(time);
                    if (availableVenues.Count != 0)
                    {
                        Venue venue = availableVenues.GetRandom(seed);
                        SetSlot(lecture.GetModule(), lecture.GetDuration(), lecture.GetCourse(), venue, time);
                        break;
                    } else if (j == timeslots.Count - 1)
                    {
                        lecture.MakeOnline(time);
                    }
                }
            }
        }

        public double FitnessFunction()
        {
            if (this == null) return -1;

            double fitness = 0;
            foreach(Course course in courses)
            {
                fitness += course.GetFitness();
            }
            foreach(Student student in GetStudents())
            {
                fitness += student.GetFitness();
            }
            foreach (Lecturer lecturer in GetLecturers())
            {
                fitness += lecturer.GetFitness();
            }

            return fitness;
        }

        public double CourseConstraints(Course course)
        {
            double fitness = 0;
            fitness += CourseSoftConstraint1(course);
            return fitness;
        }
        public double ModuleConstraints(Module module)
        {
            double fitness = 0;
            fitness += ModuleHardConstraint1(module);
            fitness += ModuleHardConstraint2(module);
            fitness += ModuleSoftConstraint1(module);
            fitness += ModuleSoftConstraint2(module);
            return fitness;
        }
        public double StudentConstraints(Student student)
        {
            double fitness = 0;
            fitness += StudentSoftConstraint1(student);
            fitness += StudentSoftConstraint2(student);
            return fitness;
        }
        public double LecturerConstraints(Lecturer lecturer)
        {
            double fitness = 0;
            fitness += LecturerHardConstraint1(lecturer);
            return fitness;
        }

        ///Hard constraint 1: All lectures of the same module must occur at different times
        public double ModuleHardConstraint1(Module module)
        {
            int fitness = 0;
            List<int> lectureTimes = new List<int>();
            foreach (Lecture l in module.GetTimetabledLectures())
            {
                int lectureTime = l.GetTime();
                if (lectureTime != -1)
                {
                    if (!lectureTimes.Contains(lectureTime)) lectureTimes.Add(lectureTime);
                    else fitness += HARD_CONSTRAINT_PENALTY;
                }
            }
            return fitness;
        }

        ///Hard constraint 2: All lectures should be assigned
        public double ModuleHardConstraint2(Module module)
        {
            int fitness = 0;
            fitness += module.GetUnassignedLectures().Count * HARD_CONSTRAINT_PENALTY;
            return fitness;
        }

        ///Hard constraint 3: No lecturer should have more than one simultaneous lecture
        public double LecturerHardConstraint1(Lecturer lecturer)
        {
            double fitness = 0;

            List<Lecture> lectures = lecturer.GetTimetabledLectures();
            int lastLectureFinishTime = lectures[0].GetTime() + lectures[0].GetDuration();
            int nextLectureStartTime;
            for (int i = 1; i < lectures.Count; i++)
            {
                nextLectureStartTime = lectures[i].GetTime();
                if (nextLectureStartTime < lastLectureFinishTime) fitness += HARD_CONSTRAINT_PENALTY;
                lastLectureFinishTime = lectures[i].GetTime() + lectures[i].GetDuration();
            }
            return fitness;
        }

        ///Soft constraint 1: Lectures should be assigned to rooms with sufficient capacity for all the students to attend the lecture
        public double ModuleSoftConstraint1(Module module)
        {
            List<Lecture> lectures = module.GetLectures();
            double fitness = 0;
            foreach (Lecture lecture in lectures)
            {
                if (lecture.GetVenue() != null)
                {
                    int difference = lecture.GetModule().GetStudents().Count - lecture.GetVenue().GetCapacity();
                    if (difference > 0) fitness += difference * MODULE_SOFT_CONSTRAINT_1_PENALTY;
                }
            }

            return fitness;
        }

        ///Soft constraint 2: Lectures of a course should take place over a minimum number of days in the week
        public double CourseSoftConstraint1(Course course)
        {
            double fitness = 0;
            int noTimeslotsInDay = GetNoTimeslotsInDay();
            List<Lecture> lectures = course.GetLectures();
            int totalDuration = 0;
            List<int> daysWithLectures = new List<int>();
            foreach (Lecture lecture in lectures)
            {
                if (lecture.GetTime() != -1)
                {
                    totalDuration += lecture.GetDuration();
                    int day = (int)Math.Floor((double)lecture.GetTime() / noTimeslotsInDay);
                    if (!daysWithLectures.Contains(day)) daysWithLectures.Add(day);
                }
            }
            int difference = daysWithLectures.Count - (int)Math.Ceiling((double)totalDuration / noTimeslotsInDay);

            if (difference > 0) fitness += difference * COURSE_SOFT_CONSTRAINT_1_PENALTY;

            return fitness;
        }

        ///Soft constraint 3: Each student's lectures on the same day should be held as close together as possible
        public double StudentSoftConstraint1(Student student)
        {
            double fitness = 0;
            int noTimeslotsInDay = GetNoTimeslotsInDay();
            int totalGap = 0;
            int previousLectureTime = -1;
            foreach (Lecture l in student.GetTimetabledLectures())
            {
                if (l.GetTime() != -1)
                {
                    int currentLectureTime = l.GetTime();
                    //Check if times are in the same day
                    if (Math.Floor((double)(currentLectureTime) / noTimeslotsInDay) == Math.Floor((double)(previousLectureTime) / noTimeslotsInDay))
                    {
                        //Check time difference between lectures
                        int difference = currentLectureTime - previousLectureTime;
                        if (difference > 1) totalGap += difference - 1;
                    }
                    previousLectureTime = currentLectureTime;
                }
            }

            fitness += totalGap * STUDENT_SOFT_CONSTRAINT_1_PENALTY;

            return fitness;
        }

        ///Soft constraint 4: Each student should have no more than SOFT_CONSTRAINT_4_CONSECUTIVE_HOURS_ALLOWED hours of lectures in a row on the same day
        public double StudentSoftConstraint2(Student student)
        {
            int noTimeslotsInDay = GetNoTimeslotsInDay();
            int consecutiveHoursPenalty = 0;
            int previousLectureTime = -1;
            int consecutiveHoursInCurrentStreak = 1;
            foreach (Lecture l in student.GetTimetabledLectures())
            {
                if (l.GetTime() >= 0)
                {
                    int currentLectureTime = l.GetTime();
                    //Check if times are in the same day
                    if (Math.Floor((double)(currentLectureTime) / noTimeslotsInDay) == Math.Floor((double)(previousLectureTime) / noTimeslotsInDay))
                    {
                        //Check time difference between lectures
                        int difference = currentLectureTime - previousLectureTime;
                        if (difference == 1)
                        {
                            consecutiveHoursInCurrentStreak++;
                            //Express penalty as a multiple of a power of 2, since higher numbers of hours in a row are more serious
                            if (consecutiveHoursInCurrentStreak > STUDENT_SOFT_CONSTRAINT_2_CONSECUTIVE_HOURS_ALLOWED) consecutiveHoursPenalty += (int)Math.Pow(2, consecutiveHoursInCurrentStreak - STUDENT_SOFT_CONSTRAINT_2_CONSECUTIVE_HOURS_ALLOWED - 1);
                        }
                        else
                        {
                            consecutiveHoursInCurrentStreak = 1;
                        }
                    }
                    else
                    {
                        consecutiveHoursInCurrentStreak = 1;
                    }
                    previousLectureTime = currentLectureTime;
                }
            }
            double fitness = consecutiveHoursPenalty * STUDENT_SOFT_CONSTRAINT_1_PENALTY;

            return fitness;
        }

        ///Soft constraint 5: A module should not have multiple lectures on the same day
        public double ModuleSoftConstraint2(Module module)
        {
            int noTimeslotsInDay = GetNoTimeslotsInDay();
            int noDays = GetNoOfDays();
            int additionalLecturesInADay = 0;

            bool[] lectureOnDay = new bool[noDays];

            foreach (Lecture l in module.GetLectures())
            {
                if (l.GetTime() >= 0)
                {
                    int dayOfLecture = (int)Math.Floor((double)l.GetTime() / noTimeslotsInDay);
                    if (lectureOnDay[dayOfLecture]) additionalLecturesInADay++;
                    else lectureOnDay[dayOfLecture] = true;
                }
            }
            double fitness = additionalLecturesInADay * MODULE_SOFT_CONSTRAINT_2_PENALTY;

            return fitness;
        }
    }
}
