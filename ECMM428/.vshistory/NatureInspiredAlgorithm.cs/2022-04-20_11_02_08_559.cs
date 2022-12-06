using System;
using System.Collections.Generic;

namespace ECMM428
{
    //Class containing functions for nature-inspired algorithms

    public class NatureInspiredAlgorithm
    {
        ///Swap the timeslots of 2 random lectures from the same random module between the timetables
        public static void Crossover(Timetable timetable1, Timetable timetable2)
        {
            double timetable1InitialFitness = timetable1.GetFitness();
            double timetable2InitialFitness = timetable2.GetFitness();
            List<Module> modules1 = new List<Module>(timetable1.GetModules());
            List<Module> modules2 = new List<Module>(timetable2.GetModules());
            modules1.Shuffle();
            modules2.Shuffle();

            for (int moduleIndex = 0; moduleIndex < modules1.Count; moduleIndex++)
            {
                //Get module and lectures
                Module module1 = modules1[moduleIndex];
                Module module2 = modules2[moduleIndex];
                List<Lecture> lectures1 = new List<Lecture>(module1.GetLectures());
                List<Lecture> lectures2 = new List<Lecture>(module2.GetLectures());
                lectures1.Shuffle();
                lectures2.Shuffle();
                bool lecture1Found = false;
                bool lecture2Found = false;

                Lecture lecture1 = null;
                Venue lecture1Venue = null;
                int lecture1Time = -1;

                Lecture lecture2 = null;
                Venue lecture2Venue = null;
                int lecture2Time = -1;

                for (int lectureIndex = 0; lectureIndex < lectures1.Count; lectureIndex++)
                {
                    //Get lectures to be swapped
                    if (!lecture1Found)
                    {
                        lecture1 = lectures1[lectureIndex];
                        lecture1Venue = lecture1.GetVenue();
                        lecture1Time = lecture1.GetTime();
                    }
                    if (!lecture2Found)
                    {
                        lecture2 = lectures2[lectureIndex];
                        lecture2Venue = lecture2.GetVenue();
                        lecture2Time = lecture2.GetTime();
                    }

                    //If lecture1 is online
                    if (lecture1Venue == null || lecture2Venue == null)
                    {
                        //Get random online lecture at the same time from the same module. If this is not possible, continue
                        Lecture onlineLecture1;
                        Lecture onlineLecture2;
                        if (lecture1Venue == null)
                        {
                            onlineLecture1 = lecture1;
                            List<Lecture> time2Lectures = new List<Lecture>(timetable2.GetOnlineLecturesFromModule(timetable2.GetModule(module1.GetNumber())));
                            if (time2Lectures.Count > 1) onlineLecture2 = time2Lectures.GetRandom();
                            else continue;
                        } else
                        {
                            onlineLecture2 = lecture2;
                            List<Lecture> time1Lectures = new List<Lecture>(timetable1.GetOnlineLecturesFromModule(timetable1.GetModule(module1.GetNumber())));
                            if (time1Lectures.Count > 1) onlineLecture1 = time1Lectures.GetRandom();
                            else continue;
                        }
                        int onlineLecture1Time = onlineLecture1.GetTime();
                        int onlineLecture2Time = onlineLecture2.GetTime();

                        //Move lecture1 to lecture2's time
                        timetable1.MakeLectureOnline(onlineLecture1, onlineLecture2Time);

                        //Move lecture2 to lecture1's time
                        timetable2.MakeLectureOnline(onlineLecture2, onlineLecture1Time);

                        //Recalculate fitness values
                        timetable1.RecalculateFitnessValues(onlineLecture1);
                        timetable2.RecalculateFitnessValues(onlineLecture2);

                        //If either of the timetables have a better fitness value, keep it. Otherwise, reverse the operation
                        if (timetable1InitialFitness < timetable1.GetFitness())
                        {
                            timetable1.MakeLectureOnline(onlineLecture1, onlineLecture1Time);
                            timetable1.RecalculateFitnessValues(onlineLecture1);
                        }
                        if (timetable2InitialFitness < timetable2.GetFitness())
                        {
                            timetable2.MakeLectureOnline(onlineLecture2, onlineLecture2Time);
                            timetable2.RecalculateFitnessValues(onlineLecture2);
                        }
                        return;
                    }
                    else
                    {
                        //Unassign lectures
                        timetable1.UnassignLecture(lecture1);
                        timetable2.UnassignLecture(lecture2);

                        List<Venue> availableVenues1 = null;
                        List<Venue> availableVenues2 = null;
                        //Get venues for new timeslots
                        if (!lecture2Found)
                        {
                            availableVenues1 = timetable1.GetAvailableVenuesAtTime(lecture1, lecture2Time);
                        }
                        if (!lecture1Found)
                        {
                            availableVenues2 = timetable2.GetAvailableVenuesAtTime(lecture2, lecture1Time);
                        }

                        //Get random venue for each lecture
                        Venue newVenue1 = null;
                        Venue newVenue2 = null;
                        if (availableVenues1 != null && availableVenues1.Count > 0)
                        {
                            newVenue1 = availableVenues1.GetRandom();
                            lecture2Found = true;
                        }
                        if (availableVenues2 != null && availableVenues2.Count > 0)
                        {
                            newVenue2 = availableVenues2.GetRandom();
                            lecture1Found = true;
                        }

                        //Reset original slots if a venue cannot be found
                        if (newVenue1 == null || newVenue2 == null)
                        {
                            timetable1.SetSlot(lecture1, lecture1Venue, lecture1Time);
                            timetable2.SetSlot(lecture2, lecture2Venue, lecture2Time);
                            continue;
                        }

                        //Set new timeslots
                        timetable1.SetSlot(lecture1, newVenue1, lecture2Time);
                        timetable2.SetSlot(lecture2, newVenue2, lecture1Time);

                        //Recalculate fitness values
                        timetable1.RecalculateFitnessValues(lecture1);
                        timetable2.RecalculateFitnessValues(lecture2);

                        //If either of the timetables have a better fitness value, keep it. Otherwise, reverse the operation
                        if (timetable1InitialFitness < timetable1.GetFitness())
                        {
                            timetable1.UnassignLecture(lecture1);
                            timetable1.SetSlot(lecture1, lecture1Venue, lecture1Time);
                            timetable1.RecalculateFitnessValues(lecture1);
                        }
                        if (timetable2InitialFitness < timetable2.GetFitness())
                        {
                            timetable2.UnassignLecture(lecture2);
                            timetable2.SetSlot(lecture2, lecture2Venue, lecture2Time);
                            timetable2.RecalculateFitnessValues(lecture2);
                        }
                        return;
                    }
                }
            }
        }
        ///Perform crossover a certain number of times
        public static void Crossover(Timetable timetable1, Timetable timetable2, int times)
        {
            for (int i = 0; i < times; i++) Crossover(timetable1, timetable2);
        }

        ///Swap 2 random lectures in different timeslots from the same timetable
        public static void Mutation(Timetable timetable, bool revertIfWorse)
        {
            double timetableInitialFitness = timetable.GetFitness();
            //Initialize timeslots
            int noTimeslots = timetable.GetNoTimeslots();
            if (noTimeslots <= 1) return;
            List<int> timeslots1 = new List<int>();
            for (int i = 0; i < noTimeslots; i++)
            {
                timeslots1.Add(i);
            }
            timeslots1.Shuffle();
            List<int> timeslots2 = new List<int>(timeslots1);
            timeslots2.Shuffle();

            //Initialize other lists
            List<Course> courses = new List<Course>(timetable.GetCourses());
            List<Venue> venues = new List<Venue>(timetable.GetVenues());

            for (int time1Index = 0; time1Index < timeslots1.Count; time1Index++)
            {
                for (int time2Index = 0; time2Index < timeslots2.Count; time2Index++)
                {
                    //Get times
                    int lecture1Time = timeslots1[time1Index];
                    int lecture2Time = timeslots2[time2Index];
                    //Initialize list of modules that have a lecture at time1
                    List<Lecture> time1Lectures = new List<Lecture>(timetable.GetLecturesStartAtTime(lecture1Time));
                    time1Lectures.Shuffle();
                    for (int lecture1Index = 0; lecture1Index < time1Lectures.Count; lecture1Index++)
                    {
                        //Get lecture at time1 that will be swapped
                        Lecture lecture1 = time1Lectures[lecture1Index];
                        Module lecture1Module = lecture1.GetModule();
                        int lecture1Duration = lecture1.GetDuration();
                        Venue lecture1Venue = lecture1.GetVenue();
                        Course lecture1Course = lecture1.GetCourse();

                        courses.Shuffle();
                        venues.Shuffle();
                        for (int courseIndex = 0; courseIndex < courses.Count; courseIndex++)
                        {
                            //If lecture1 is online
                            if (lecture1Venue == null)
                            {
                                List<Lecture> time2Lectures = new List<Lecture>(timetable.GetOnlineLecturesStartAtTime(lecture2Time));

                                //Move lecture1 to lecture2Time
                                timetable.UnassignLecture(lecture1);
                                timetable.SetSlot(lecture1Module, lecture1Duration, lecture1Course, null, lecture2Time);
                                timetable.RecalculateFitnessValues(lecture1);

                                bool lecture2Swapped = false;
                                Lecture lecture2 = null;
                                Module lecture2Module = null;
                                int lecture2Duration = -1;
                                Course lecture2Course = null;

                                if (time2Lectures.Count != 0)
                                {
                                    //Flip a coin as to whether a lecture from time2Lectures should be swapped with lecture1
                                    Random rnd = new Random();
                                    if (rnd.Next(0, 2) == 0)
                                    {
                                        lecture2Swapped = true;
                                        lecture2 = time2Lectures.GetRandom();
                                        lecture2Module = lecture2.GetModule();
                                        lecture2Duration = lecture2.GetDuration();
                                        lecture2Course = lecture2.GetCourse();

                                        timetable.UnassignLecture(lecture2);
                                        timetable.SetSlot(lecture2Module, lecture2Duration, lecture2Course, null, lecture1Time);
                                        timetable.RecalculateFitnessValues(lecture2);
                                    }
                                }

                                //If the timetable has a better fitness value, keep it. Otherwise, reverse the operation
                                if (timetableInitialFitness < timetable.GetFitness())
                                {
                                    timetable.UnassignLecture(lecture1);
                                    timetable.SetSlot(lecture1Module, lecture1Duration, lecture1Course, null, lecture1Time);
                                    timetable.RecalculateFitnessValues(lecture1);

                                    if (lecture2Swapped)
                                    {
                                        timetable.UnassignLecture(lecture2);
                                        timetable.SetSlot(lecture2Module, lecture2Duration, lecture2Course, null, lecture2Time);
                                        timetable.RecalculateFitnessValues(lecture2);
                                    }
                                }
                                if (timetable.GetFitness() > 10000)
                                {
                                    int hi = 0;
                                }

                                return;
                            }
                            //Else if lecture1 is in-person
                            else
                            {
                                Course lecture2Course = courses[courseIndex];
                                for (int venueIndex = 0; venueIndex < venues.Count; venueIndex++)
                                {
                                    //Get slot that will be swapped with module1
                                    Venue lecture2Venue = venues[venueIndex];

                                    //If the two lectures are the same, continue
                                    if (lecture1Course == lecture2Course && lecture1Venue == lecture2Venue && lecture1Time == lecture2Time) continue;

                                    Lecture lecture2 = timetable.GetLecture(lecture2Course, lecture2Venue, lecture2Time);

                                    //Unassign lectures to free the venues
                                    timetable.UnassignLecture(lecture1);
                                    if (lecture2 != null)
                                    {
                                        timetable.UnassignLecture(lecture2);
                                    }

                                    //Get venues that are free
                                    List<Venue> venuesForLecture1 = timetable.GetAvailableVenuesAtTime(lecture1, lecture2Time);
                                    List<Venue> venuesForLecture2 = timetable.GetAvailableVenuesAtTime(lecture2, lecture1Time);

                                    //If no venues are available, reassign lecture1 and lecture2 and continue
                                    if (venuesForLecture1.Count == 0 || (venuesForLecture2 != null && venuesForLecture2.Count == 0))
                                    {
                                        timetable.SetSlot(lecture1, lecture1Venue, lecture1Time);
                                        if (lecture2 != null)
                                        {
                                            timetable.SetSlot(lecture2, lecture2Venue, lecture2Time);
                                        }
                                        break;
                                    }

                                    //Perform swap
                                    timetable.SetSlot(lecture1, venuesForLecture1.GetRandom(), lecture2Time);
                                    timetable.RecalculateFitnessValues(lecture1);

                                    if (lecture2 != null)
                                    {
                                        timetable.SetSlot(lecture2, venuesForLecture2.GetRandom(), lecture1Time);
                                        timetable.RecalculateFitnessValues(lecture2);
                                    }

                                    if (revertIfWorse)
                                    {
                                        //If the timetable has a better fitness value, keep it. Otherwise, reverse the operation
                                        if (timetableInitialFitness < timetable.GetFitness())
                                        {
                                            timetable.UnassignLecture(lecture1);
                                            timetable.SetSlot(lecture1, lecture1Venue, lecture1Time);
                                            timetable.RecalculateFitnessValues(lecture1);

                                            if (lecture2 != null)
                                            {
                                                timetable.UnassignLecture(lecture2);
                                                timetable.SetSlot(lecture2, lecture2Venue, lecture2Time);
                                                timetable.RecalculateFitnessValues(lecture2);
                                            }
                                        }
                                    }

                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        ///Perform mutation a certain number of times
        public static void Mutation(Timetable timetable, int times)
        {
            for (int i = 0; i < times; i++) Mutation(timetable, false);
        }
        public static void Mutation(Timetable timetable)
        {
            Mutation(timetable, false);
        }

        ///Identify lecture contributing the most fitness due to its room allocation from module has less than max number of online lectures, and make it online
        public static bool LectureToOnlineMutation(Timetable timetable, int maxNoOnlineLectures)
        {
            int maxDifference = 0;
            List<Lecture> lecturesWithHighestDifference = new List<Lecture>();
            List<Module> modules = timetable.GetModules();

            foreach (Module m in modules)
            {
                if (m.GetNoOnlineLectures() < maxNoOnlineLectures)
                {
                    List<Lecture> inPersonLectures = m.GetInPersonLectures();
                    if (inPersonLectures.Count != 0)
                    {
                        Lecture l = inPersonLectures.GetRandom();
                        int difference = l.GetModule().GetStudents().Count - l.GetVenue().GetCapacity();
                        if (difference > maxDifference)
                        {
                            maxDifference = difference;
                            lecturesWithHighestDifference.Clear();
                            lecturesWithHighestDifference.Add(l);
                        }
                        else if (difference == maxDifference)
                        {
                            lecturesWithHighestDifference.Add(l);
                        }
                    }
                }
            }

            if (maxDifference > 0)
            {
                Lecture lecture = lecturesWithHighestDifference.GetRandom();
                timetable.MakeLectureOnline(lecture);
                timetable.RecalculateFitnessValues(lecture);
                return true;
            }
            else
            {
                return false;
            }
        }

        ///Identify lecture contributing the most fitness from module that has less than max number of asynchronous lectures, and make it asynchronous
        public static bool LectureToAsynchronousMutation(Timetable timetable, int maxNoAsynchronousLectures)
        {
            double maxDifference = 0;
            double initialFitness = timetable.GetFitness();
            List<Lecture> lecturesWithHighestDifference = new List<Lecture>();
            List<Module> modules = timetable.GetModules();

            foreach (Module m in modules)
            {
                if (m.GetNoAsynchronousLectures() < maxNoAsynchronousLectures)
                {
                    Lecture l;
                    List<Lecture> inPersonLectures = m.GetInPersonLectures();
                    List<Lecture> onlineLectures = m.GetOnlineLectures();
                    if (inPersonLectures.Count > 0)
                    {
                        l = inPersonLectures.GetRandom();
                    } else if (onlineLectures.Count > 0)
                    {
                        l = m.GetOnlineLectures().GetRandom();
                    } else
                    {
                        continue;
                    }
                    Venue lectureVenue = l.GetVenue();
                    int lectureTime = l.GetTime();
                    timetable.MakeLectureAsynchronous(l);
                    timetable.RecalculateFitnessValues(l);
                    double newFitness = timetable.GetFitness();
                    double difference = initialFitness - newFitness;
                    if (difference > maxDifference)
                    {
                        maxDifference = difference;
                        lecturesWithHighestDifference.Clear();
                        lecturesWithHighestDifference.Add(l);
                    }
                    else if (difference == maxDifference)
                    {
                        lecturesWithHighestDifference.Add(l);
                    }
                    m.UnassignLecture(l);
                    timetable.SetSlot(l, lectureVenue, lectureTime);
                    timetable.RecalculateFitnessValues(l);
                }
            }
            if (maxDifference > 0)
            {
                Lecture lecture = lecturesWithHighestDifference.GetRandom();
                timetable.MakeLectureAsynchronous(lecture);
                timetable.RecalculateFitnessValues(lecture);
                return true;
            }
            else
            {
                return false;
            }
        }

        ///Check if requirement to end the algorithm has been satisfied
        public static bool CheckFitnessSatisfied(double[] fitnessHistory, double fitnessMargin)
        {
            if (fitnessHistory[1] == -1 || fitnessHistory[2] == -1) return false;

            double largestValue = Math.Max(fitnessHistory[1], fitnessHistory[2]);
            double smallestValue = Math.Min(fitnessHistory[1], fitnessHistory[2]);
            double maxDeviation = largestValue - smallestValue;

            foreach (double i in fitnessHistory)
            {
                if (i == -1) return false;
                if (i > largestValue)
                {
                    largestValue = i;
                    maxDeviation = largestValue - smallestValue;
                }
                else if (i < smallestValue)
                {
                    smallestValue = i;
                    maxDeviation = largestValue - smallestValue;
                }
            }
            if (maxDeviation <= fitnessMargin) return true;
            return false;
        }
    }
}
