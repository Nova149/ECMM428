using System;
using System.Collections.Generic;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace ECMM428
{
    public class MainClass
    {
        static void Main(string[] args)
        {
            //Timetable constants
            int NO_MODULES_PER_STUDENT = 3;
            int NO_COURSES = 100;
            int NO_MODULES_PER_COURSE = 5;
            int NO_LECTURERS_PER_COURSE = 5;
            int NO_ROOMS = 60;
            int NO_TIMESLOTS_PER_DAY = 9;
            int NO_STUDENTS = 500;
            int HOURS_OF_LECTURES_PER_MODULE = 3;
            int MIN_VENUE_CAPACITY = 3;
            int MAX_VENUE_CAPACITY = 5;
            int SEED = 0;

            //Socha: Small
            /*int NO_MODULES_PER_STUDENT = 1;
            int NO_COURSES = 100;
            int NO_MODULES_PER_COURSE = 1;
            int NO_LECTURERS_PER_COURSE = 5;
            int NO_ROOMS = 5;
            int NO_TIMESLOTS_PER_DAY = 9;
            int NO_STUDENTS = 80;
            int HOURS_OF_LECTURES_PER_STUDENT = 3;
            int MIN_VENUE_CAPACITY = 10;
            int MAX_VENUE_CAPACITY = 10;
            int SEED = 0;*/

            //Socha: Medium
            /*int NO_MODULES_PER_STUDENT = 1;
            int NO_COURSES = 400;
            int NO_MODULES_PER_COURSE = 1;
            int NO_LECTURERS_PER_COURSE = 5;
            int NO_ROOMS = 10;
            int NO_TIMESLOTS_PER_DAY = 9;
            int NO_STUDENTS = 200;
            int HOURS_OF_LECTURES_PER_STUDENT = 3;
            int MIN_VENUE_CAPACITY = 10;
            int MAX_VENUE_CAPACITY = 10;
            int SEED = 0;*/

            //Socha: Large
            /*int NO_MODULES_PER_STUDENT = 1;
            int NO_COURSES = 400;
            int NO_MODULES_PER_COURSE = 1;
            int NO_LECTURERS_PER_COURSE = 5;
            int NO_ROOMS = 10;
            int NO_TIMESLOTS_PER_DAY = 9;
            int NO_STUDENTS = 400;
            int HOURS_OF_LECTURES_PER_STUDENT = 3;
            int MIN_VENUE_CAPACITY = 10;
            int MAX_VENUE_CAPACITY = 10;
            int SEED = 0;*/

            //Initialize two identical timetables
            Timetable timetable = MainClass.Initialize(NO_MODULES_PER_STUDENT, NO_COURSES, NO_MODULES_PER_COURSE, NO_LECTURERS_PER_COURSE, NO_ROOMS, NO_TIMESLOTS_PER_DAY, NO_STUDENTS, HOURS_OF_LECTURES_PER_MODULE, MIN_VENUE_CAPACITY, MAX_VENUE_CAPACITY, SEED);
            //Timetable timetable2 = new Timetable(timetable1);

            //Perform genetic algorithm on each
            //timetable1 = GeneticAlgorithm.Execute(timetable1, 0.55, 1, 1000, 0, 0, 2);
            //timetable2 = GeneticAlgorithm.Execute(timetable2, 0.55, 1, 1000, 0, 2, 2);

            //No of asynchronous lectures and total lectures
            //int noLecturesTotal = timetable2.GetLectures().Count;
            //int noLecturesAsynchronous = timetable2.GetAsynchronousLectures().Count;

            //Fitness of each timetable
            //double timetable1Fitness = timetable1.GetFitness();
            //double timetable2Fitness = timetable2.GetFitness();

            //timetable = HeuristicAlgorithm.Execute(timetable);
            //timetable = AsynchronousInitializationAlgorithm.Execute(timetable, 0.2);
            //TestGeneticAlgorithmExpand(timetable, 0.55, 50, 1000, 500, 1000, 20, 10);
            //TestGeneticAlgorithmCrossoverProbability(timetable, 1, 0, 1000, 0, 1, 0.05, 100);
            TestAlgorithmOnline("Genetic", timetable, 0, 0.55, 1, 1000, 0, 2, 1, 1);
            //TestAlgorithmAsynchronous("Differential Evolution", timetable, 0, 0.55, 1, 1000, 0, 2, 2, 10);
            //TestAsynchronousInitializationAlgorithm("Genetic", timetable, 0, 0.55, 1, 1000, 0, 0.3, 0.06, 100);
        }

        //Initialize a blank timetable with course, room and timeslot information
        public static Timetable Initialize(int noModulesPerStudent, int noCourses, int noModulesPerCourse, int noLecturersPerCourse, int noRooms, int noTimeslotsPerDay, int noStudents, int hoursOfLecturesPerModule, int minVenueCapacity, int maxVenueCapacity)
        {
            return Initialize(noModulesPerStudent, noCourses, noModulesPerCourse, noLecturersPerCourse, noRooms, noTimeslotsPerDay, noStudents, hoursOfLecturesPerModule, minVenueCapacity, maxVenueCapacity, new Random().Next(), false);
        }
        public static Timetable Initialize(int noModulesPerStudent, int noCourses, int noModulesPerCourse, int noLecturersPerCourse, int noRooms, int noTimeslotsPerDay, int noStudents, int hoursOfLecturesPerModule, int minVenueCapacity, int maxVenueCapacity, int seed)
        {
            return Initialize(noModulesPerStudent, noCourses, noModulesPerCourse, noLecturersPerCourse, noRooms, noTimeslotsPerDay, noStudents, hoursOfLecturesPerModule, minVenueCapacity, maxVenueCapacity, seed, false);
        }
        public static Timetable Initialize(int noModulesPerStudent, int noCourses, int noModulesPerCourse, int noLecturersPerCourse, int noRooms, int noTimeslotsPerDay, int noStudents, int hoursOfLecturesPerModule, int minVenueCapacity, int maxVenueCapacity, int seed, bool allLecturesLastOneHour)
        {
            int MIN_YEAR_OF_STUDY = 1;
            int MAX_YEAR_OF_STUDY = 4;
            List<double> LECTURE_HOUR_DISTRIBUTIONS_PROBABILITY;
            if (allLecturesLastOneHour)
            {
                LECTURE_HOUR_DISTRIBUTIONS_PROBABILITY = new List<double>() { 1 };
            }
            else
            {
                LECTURE_HOUR_DISTRIBUTIONS_PROBABILITY = new List<double>() { 0.6, 0.4 };
            }


            //Initialize variables
            List<Course> courses = new List<Course>();
            List<Module> modules = new List<Module>();
            List<Student> students = new List<Student>();
            List<Venue> venues = new List<Venue>();
            Random rnd = new Random(seed);

            //Initialize courses and modules
            int moduleId = 0;
            int lectureId = 0;
            int lecturerId = 0;
            for (int courseId = 0; courseId < noCourses; courseId++)
            {
                Course newCourse = new Course(courseId, new List<Module>(), null);

                //Initialize lecturers for course
                List<Lecturer> lecturersForCourse = new List<Lecturer>();
                for (int lecNum = 0; lecNum < noLecturersPerCourse; lecNum++)
                {
                    Lecturer newLecturer = new Lecturer(lecturerId, new List<Module>(), newCourse);
                    lecturersForCourse.Add(newLecturer);
                    lecturerId++;
                }
                newCourse.SetLecturers(lecturersForCourse);

                int lecturerIndex = 0;

                for (int modNum = 0; modNum < noModulesPerCourse; modNum++)
                {
                    //Assign lectures and modules to course
                    List<Lecture> lecturesPerWeek = new List<Lecture>();
                    int lectureHoursRemaining = hoursOfLecturesPerModule;

                    while (lectureHoursRemaining > 0)
                    {
                        if (lectureHoursRemaining > 1 && !allLecturesLastOneHour)
                        {
                            double lectureDurationDecider = rnd.Next(0, 1);
                            int lectureDuration;
                            if (lectureDurationDecider <= LECTURE_HOUR_DISTRIBUTIONS_PROBABILITY[0]) lectureDuration = 1;
                            else lectureDuration = 2;

                            lecturesPerWeek.Add(new Lecture(lectureId, lectureDuration, null, null, -1));
                            lectureHoursRemaining -= lectureDuration;
                        } else
                        {
                            lecturesPerWeek.Add(new Lecture(lectureId, 1, null, null, -1));
                            lectureHoursRemaining -= 1;
                        }
                        lectureId++;
                    }

                    //Assign the same amount of lecturers to each module
                    Lecturer lecturerForModule = lecturersForCourse[lecturerIndex];

                    Module newModule = new Module(moduleId, new List<Student>(), lecturerForModule, lecturesPerWeek, newCourse);

                    lecturerForModule.AssignModule(newModule);
                    if (lecturerIndex == noLecturersPerCourse - 1) lecturerIndex = 0;
                    else lecturerIndex++;

                    foreach (Lecture l in newModule.GetLectures())
                    {
                        l.SetModule(newModule);
                    }

                    modules.Add(newModule);
                    newCourse.AddModule(newModule);
                    moduleId++;
                }
                courses.Add(newCourse);
            }


            //Initialize students
            for (int i = 0; i < noStudents; i++)
            {
                //Get a random course, from which the student will be assigned modules
                int courseIndex = rnd.Next(0, courses.Count);
                Course course = courses[courseIndex];

                //Randomly assign modules to each student
                List<Module> modulesToTake = new List<Module>();
                List<Module> availableModules = new List<Module>(course.GetModules());
                for (int j = 0; j < noModulesPerStudent; j++)
                {
                    int index = rnd.Next(0, availableModules.Count);
                    modulesToTake.Add(availableModules[index]);
                    availableModules.RemoveAt(index);
                }
                int yearOfStudy = rnd.Next(MIN_YEAR_OF_STUDY, MAX_YEAR_OF_STUDY);
                Student newStudent = new Student(i, yearOfStudy, modulesToTake);
                students.Add(newStudent);

                //Enroll student in modules
                for (int k = 0; k < modulesToTake.Count; k++)
                {
                    modulesToTake[k].AddStudent(newStudent);
                }
            }

            //Initialize venues
            for (int i = 0; i < noRooms; i++)
            {
                Venue newVenue = new Venue(i, rnd.Next(minVenueCapacity, maxVenueCapacity), noTimeslotsPerDay * 5);
                venues.Add(newVenue);
            }

            //Initialize timetable
            Timetable timetable = new Timetable(courses, venues, noTimeslotsPerDay);

            return timetable;
        }
        public static void ExportToExcel(double[,] results)
        {
            Excel.Application xlApp = new Excel.Application();
            xlApp.Visible = true;
            Excel.Workbook wb = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets.get_Item(1);

            Excel.Range rng = ws.Cells.get_Resize(results.GetLength(0), results.GetLength(1));
            rng.Value2 = results;
        }
        public static void TestGeneticAlgorithmCrossoverProbability(Timetable initialTimetable, double fitnessMargin, int seed, int noInMarginToStopAfter, double initialValue, double finalValue, double step, int noRunsPerStep)
        {
            int noSteps = (int)Math.Floor((double)(finalValue - initialValue) / step) + 1;
            double[,] results = new double[noSteps, 5];
            var stopwatch = new System.Diagnostics.Stopwatch();

            Timetable[] timetableArray = new Timetable[noRunsPerStep];
            for (int j = 0; j < noRunsPerStep; j++)
            {
                Timetable timetable = new Timetable(initialTimetable);
                timetable.GenericInitialization(seed + j);
                timetableArray[j] = timetable;
            }

            for (int i = 0; i < noSteps; i++)
            {
                System.Diagnostics.Debug.WriteLine("i = " + i + " / " + (noSteps - 1));
                double currentValue = initialValue + i * step;
                double[] resultsFitnessInStep = new double[noRunsPerStep];
                double[] resultsStopwatchInStep = new double[noRunsPerStep];
                for (int j = 0; j < noRunsPerStep; j++)
                {
                    System.Diagnostics.Debug.WriteLine("j = " + j + " / " + (noRunsPerStep - 1));
                    Timetable timetable = new Timetable(timetableArray[j]);
                    stopwatch.Restart();
                    timetable = GeneticAlgorithm.Execute(timetable, currentValue, fitnessMargin, noInMarginToStopAfter);
                    stopwatch.Stop();
                    resultsFitnessInStep[j] = timetable.GetFitness();
                    resultsStopwatchInStep[j] = stopwatch.ElapsedMilliseconds;
                }
                results[i, 0] = currentValue;
                results[i, 1] = Util.GetAverage(resultsFitnessInStep);
                results[i, 2] = Util.GetStandardDeviation(resultsFitnessInStep);
                results[i, 3] = Util.GetAverage(resultsStopwatchInStep);
                results[i, 4] = Util.GetStandardDeviation(resultsStopwatchInStep);
            }
            ExportToExcel(results);
        }
        public static void TestAlgorithmOnline(string algorithm, Timetable initialTimetable, int seed, double fitnessMargin, int noInMarginToStopAfter, int initialValue, int finalValue, int step, int noRunsPerStep)
        {
            TestAlgorithmOnline(algorithm, initialTimetable, seed, 0, fitnessMargin, noInMarginToStopAfter, initialValue, finalValue, step, noRunsPerStep);
        }
        public static void TestAlgorithmOnline(string algorithm, Timetable initialTimetable, int seed, double crossoverProbability, double fitnessMargin, int noInMarginToStopAfter, int initialValue, int finalValue, int step, int noRunsPerStep)
        {
            int noSteps = (int)Math.Floor((double)(finalValue - initialValue) / step) + 1;
            double[,] results = new double[noSteps, 7];
            var stopwatch = new System.Diagnostics.Stopwatch();

            Timetable[] timetableArray = new Timetable[noRunsPerStep];
            for (int j = 0; j < noRunsPerStep; j++)
            {
                Timetable timetable = new Timetable(initialTimetable);
                timetable.GenericInitialization(seed + j);
                timetableArray[j] = timetable;

                int totalOnlineCount = 0;
                foreach (Module m in timetable.GetModules())
                {
                    totalOnlineCount += m.GetNoOnlineLectures();
                }
            }

            for (int i = 0; i < noSteps; i++)
            {
                System.Diagnostics.Debug.WriteLine("i = " + i + " / " + (noSteps - 1));

                int currentValue = initialValue + i * step;
                double[] resultsFitnessInStep = new double[noRunsPerStep];
                double[] resultsStopwatchInStep = new double[noRunsPerStep];
                double[] fractionTotalLecturesOnlineInStep = new double[noRunsPerStep];
                for (int j = 0; j < noRunsPerStep; j++)
                {
                    System.Diagnostics.Debug.WriteLine("j = " + j + " / " + (noRunsPerStep - 1));

                    Timetable timetable = new Timetable(timetableArray[j]);
                    stopwatch.Restart();
                    if (algorithm.Equals("Genetic"))
                    {
                        timetable = GeneticAlgorithm.Execute(timetable, crossoverProbability, fitnessMargin, noInMarginToStopAfter, currentValue, 0, 2);
                    } else if (algorithm.Equals("Differential Evolution"))
                    {
                        timetable = DifferentialEvolutionAlgorithm.Execute(timetable, fitnessMargin, noInMarginToStopAfter, noInMarginToStopAfter, currentValue, 0, 2);
                    }
                    else
                    {
                        throw new ArgumentException("Argument 'algorithm' has an incorrect name.");
                    }
                    stopwatch.Stop();
                    resultsFitnessInStep[j] = timetable.GetFitness();
                    resultsStopwatchInStep[j] = stopwatch.ElapsedMilliseconds;

                    List<Module> modules = timetable.GetModules();
                    double totalLectureCount = timetable.GetLectures().Count;
                    double totalOnlineCount = 0;

                    foreach (Module m in modules)
                    {
                        totalOnlineCount += m.GetNoOnlineLectures();
                    }

                    fractionTotalLecturesOnlineInStep[j] = totalOnlineCount / totalLectureCount;
                 }
                results[i, 0] = currentValue;
                results[i, 1] = Util.GetAverage(resultsFitnessInStep);
                results[i, 2] = Util.GetStandardDeviation(resultsFitnessInStep);
                results[i, 3] = Util.GetAverage(resultsStopwatchInStep);
                results[i, 4] = Util.GetStandardDeviation(resultsStopwatchInStep);
                results[i, 5] = Util.GetAverage(fractionTotalLecturesOnlineInStep);
                results[i, 6] = Util.GetStandardDeviation(fractionTotalLecturesOnlineInStep);
            }

            ExportToExcel(results);
        }
        public static void TestAlgorithmAsynchronous(string algorithm, Timetable initialTimetable, int seed, double crossoverProbability, double fitnessMargin, int noInMarginToStopAfter, int initialValue, int finalValue, int step, int noRunsPerStep)
        {
            int noSteps = (int)Math.Floor((double)(finalValue - initialValue) / step) + 1;
            double[,] results = new double[noSteps, 7];
            var stopwatch = new System.Diagnostics.Stopwatch();

            Timetable[] timetableArray = new Timetable[noRunsPerStep];
            for (int j = 0; j < noRunsPerStep; j++)
            {
                Timetable timetable = new Timetable(initialTimetable);
                timetable.GenericInitialization(seed + j);
                timetableArray[j] = timetable;
            }

            for (int i = 0; i < noSteps; i++)
            {
                System.Diagnostics.Debug.WriteLine("i = " + i + " / " + (noSteps - 1));

                int currentValue = initialValue + i * step;
                double[] resultsFitnessInStep = new double[noRunsPerStep];
                double[] resultsStopwatchInStep = new double[noRunsPerStep];
                double[] fractionTotalLecturesAsynchronousInStep = new double[noRunsPerStep];
                for (int j = 0; j < noRunsPerStep; j++)
                {
                    System.Diagnostics.Debug.WriteLine("j = " + j + " / " + (noRunsPerStep - 1));

                    Timetable timetable = new Timetable(timetableArray[j]);
                    stopwatch.Restart();
                    if (algorithm.Equals("Genetic"))
                    {
                        timetable = GeneticAlgorithm.Execute(timetable, crossoverProbability, fitnessMargin, noInMarginToStopAfter, 0, currentValue, 2);
                    }
                    else if (algorithm.Equals("Differential Evolution"))
                    {
                        timetable = DifferentialEvolutionAlgorithm.Execute(timetable, fitnessMargin, noInMarginToStopAfter, noInMarginToStopAfter, 0, currentValue, 2);
                    }
                    else
                    {
                        throw new ArgumentException("Argument 'algorithm' has an incorrect name.");
                    }
                    stopwatch.Stop();
                    resultsFitnessInStep[j] = timetable.GetFitness();
                    resultsStopwatchInStep[j] = stopwatch.ElapsedMilliseconds;

                    List<Module> modules = timetable.GetModules();
                    double totalLectureCount = timetable.GetLectures().Count;
                    double totalAsynchronousCount = 0;

                    foreach (Module m in modules)
                    {
                        totalAsynchronousCount += m.GetNoAsynchronousLectures();
                    }

                    fractionTotalLecturesAsynchronousInStep[j] = totalAsynchronousCount / totalLectureCount;
                }
                results[i, 0] = currentValue;
                results[i, 1] = Util.GetAverage(resultsFitnessInStep);
                results[i, 2] = Util.GetStandardDeviation(resultsFitnessInStep);
                results[i, 3] = Util.GetAverage(resultsStopwatchInStep);
                results[i, 4] = Util.GetStandardDeviation(resultsStopwatchInStep);
                results[i, 5] = Util.GetAverage(fractionTotalLecturesAsynchronousInStep);
                results[i, 6] = Util.GetStandardDeviation(fractionTotalLecturesAsynchronousInStep);
            }

            ExportToExcel(results);
        }
        public static void TestAsynchronousInitializationAlgorithm(string algorithm, Timetable initialTimetable, int seed, double crossoverProbability, double fitnessMargin, int noInMarginToStopAfter, double initialValue, double finalValue, double step, int noRunsPerStep)
        {
            int noSteps = (int)Math.Floor((double)(finalValue - initialValue) / step) + 1;
            double[,] results = new double[noSteps, 5];
            var stopwatch = new System.Diagnostics.Stopwatch();

            for (int i = 0; i < noSteps; i++)
            {
                System.Diagnostics.Debug.WriteLine("i = " + i + " / " + (noSteps - 1));

                double currentValue = initialValue + i * step;
                double[] resultsFitnessInStep = new double[noRunsPerStep];
                double[] resultsStopwatchInStep = new double[noRunsPerStep];

                Timetable currentStepTimetable = new Timetable(initialTimetable);
                currentStepTimetable = AsynchronousInitializationAlgorithm.Execute(currentStepTimetable, currentValue);
                Timetable[] timetableArray = new Timetable[noRunsPerStep];

                for (int j = 0; j < noRunsPerStep; j++)
                {
                    Timetable timetable = new Timetable(currentStepTimetable);
                    timetable.GenericInitialization(seed + j);
                    timetableArray[j] = timetable;
                }

                for (int j = 0; j < noRunsPerStep; j++)
                {
                    System.Diagnostics.Debug.WriteLine("j = " + j + " / " + (noRunsPerStep - 1));

                    Timetable timetable = new Timetable(timetableArray[j]);
                    stopwatch.Restart();
                    if (algorithm.Equals("Genetic"))
                    {
                        timetable = GeneticAlgorithm.Execute(timetable, crossoverProbability, fitnessMargin, noInMarginToStopAfter, 0, 0, 2);
                    }
                    else if (algorithm.Equals("Differential Evolution"))
                    {
                        timetable = DifferentialEvolutionAlgorithm.Execute(timetable, fitnessMargin, noInMarginToStopAfter, noInMarginToStopAfter, 0, 0, 2);
                    }
                    else
                    {
                        throw new ArgumentException("Argument 'algorithm' has an incorrect name.");
                    }
                    stopwatch.Stop();
                    resultsFitnessInStep[j] = timetable.GetFitness();
                    resultsStopwatchInStep[j] = stopwatch.ElapsedMilliseconds;

                }
                results[i, 0] = currentValue;
                results[i, 1] = Util.GetAverage(resultsFitnessInStep);
                results[i, 2] = Util.GetStandardDeviation(resultsFitnessInStep);
                results[i, 3] = Util.GetAverage(resultsStopwatchInStep);
                results[i, 4] = Util.GetStandardDeviation(resultsStopwatchInStep);
            }

            ExportToExcel(results);
        }
    }
}
