using System;
using System.Collections.Generic;

namespace ECMM428
{
    //Code to implement a genetic algorithm, based on https://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=5393667

    public class GeneticAlgorithm : NatureInspiredAlgorithm
    {
        public static Timetable Execute(Timetable timetable, double crossoverProbability, double fitnessMargin, int noInMarginToStopAfter, int maxOnlineLectures, int maxAsynchronousLectures, int resets)
        {
            //Initialize timetable by placing lectures in random timeslotes, if needed
            timetable.GenericInitialization();

            //Initialize fitnessHistory arrays
            double[] fitnessHistoryTimetable1 = new double[noInMarginToStopAfter];
            double[] fitnessHistoryTimetable2 = new double[noInMarginToStopAfter];

            //No of current max online and asynchronous lectures per module
            int currentMaxOnlineLectures = 0;
            int currentMaxAsynchronousLectures = 0;

            //Current reset number
            int currentReset = 0;

            //Whether a lecture can be moved to be online or asynchronous
            bool canMoveLectureOnline = false;
            bool canMoveLectureAsynchronous = false;

            for (int i = 0; i < fitnessHistoryTimetable1.Length; ++i)
            {
                fitnessHistoryTimetable1[i] = -1;
            }
            for (int i = 0; i < fitnessHistoryTimetable2.Length; ++i)
            {
                fitnessHistoryTimetable2[i] = -1;
            }

            Timetable timetable1 = new Timetable(timetable);
            Timetable timetable2 = new Timetable(timetable);

            double timetable1Fitness = timetable1.GetFitness();
            double timetable2Fitness = timetable2.GetFitness();
            int currentIndex = 0;

            //Begin the genetic algorithm
            while (true)
            {
                //Either move a lecture to be online or asynchronous (if possible) or perform crossover/mutation
                Random rnd = new Random();
                if ((canMoveLectureOnline || canMoveLectureAsynchronous) && rnd.Next(0, 2) == 0)
                {
                    //Move a lecture to be online or asynchronous, if required
                    if (canMoveLectureOnline)
                    {
                        bool lectureMoved1 = LectureToOnlineMutation(timetable1, currentMaxOnlineLectures);
                        bool lectureMoved2 = LectureToOnlineMutation(timetable2, currentMaxOnlineLectures);
                        if (!lectureMoved1 && !lectureMoved2) canMoveLectureOnline = false;
                    }
                    else if (canMoveLectureAsynchronous)
                    {
                        bool lectureMoved1 = LectureToAsynchronousMutation(timetable1, currentMaxAsynchronousLectures);
                        bool lectureMoved2 = LectureToAsynchronousMutation(timetable2, currentMaxAsynchronousLectures);
                        if (!lectureMoved1 && !lectureMoved2) canMoveLectureAsynchronous = false;
                    }
                }
                else
                {
                    //Determine which operator to use
                    double p = rnd.NextDouble();
                    if (p < crossoverProbability)
                    {
                        //Perform crossover
                        Crossover(timetable1, timetable2, true);
                    }
                    else
                    {
                        //Perform mutation
                        Mutation(timetable1, true);
                        Mutation(timetable2, true);
                    }
                }

                //Compare old and new timetables. Keep the timetable with the better (smaller) fitness value
                double newTimetable1Fitness = timetable1.GetFitness();
                double newTimetable2Fitness = timetable2.GetFitness();

                if (newTimetable1Fitness < timetable1Fitness)
                {
                    timetable1Fitness = newTimetable1Fitness;
                }
                if (newTimetable2Fitness < timetable2Fitness)
                {
                    timetable2Fitness = newTimetable2Fitness;
                }

                //Update fitness histories
                fitnessHistoryTimetable1[currentIndex] = timetable1Fitness;
                fitnessHistoryTimetable2[currentIndex] = timetable2Fitness;

                //If we are at the end of the array, go back to the beginning. Otherwise advance the index
                if (currentIndex == noInMarginToStopAfter - 1) currentIndex = 0;
                else currentIndex++;

                //If either timetable's fitness has not changed by fitnessRequirment within marginLengthRequirement, return that timetable.
                //If both have satisfied it, return the timetable that currently has the lower fitness function
                bool timetable1Satisfied = CheckFitnessSatisfied(fitnessHistoryTimetable1, fitnessMargin);
                bool timetable2Satisfied = CheckFitnessSatisfied(fitnessHistoryTimetable2, fitnessMargin);

                if (timetable1Satisfied || timetable2Satisfied)
                {
                    if (currentMaxOnlineLectures < maxOnlineLectures || currentMaxAsynchronousLectures < maxAsynchronousLectures)
                    {
                        if (fitnessHistoryTimetable1[currentIndex] != 0 && fitnessHistoryTimetable2[currentIndex] != 0)
                        {
                            //Increase number of current max online/asynchronous lectures, prioritizing online lectures, if required
                            if (currentMaxOnlineLectures < maxOnlineLectures)
                            {
                                currentMaxOnlineLectures++;
                                canMoveLectureOnline = true;
                            }
                            else if (currentMaxAsynchronousLectures < maxAsynchronousLectures)
                            {
                                currentMaxAsynchronousLectures++;
                                canMoveLectureAsynchronous = true;
                            }
                        }
                    }
                    if (currentReset < resets)
                    {
                        //Reset fitness histories to allow for time to experiment with new timetables
                        currentReset++;
                        currentIndex = 0;
                        for (int i = 0; i < fitnessHistoryTimetable1.Length; ++i)
                        {
                            fitnessHistoryTimetable1[i] = -1;
                        }
                        for (int i = 0; i < fitnessHistoryTimetable2.Length; ++i)
                        {
                            fitnessHistoryTimetable2[i] = -1;
                        }
                    }
                    else if (timetable1Satisfied && timetable2Satisfied)
                    {
                        if (fitnessHistoryTimetable1[currentIndex] < fitnessHistoryTimetable2[currentIndex]) return timetable1;
                        else return timetable2;
                    }
                    else if (timetable1Satisfied) return timetable1;
                    else return timetable2;
                }
            }
        }
        public static Timetable Execute(Timetable timetable, double crossoverProbability, double fitnessMargin, int noInMarginToStopAfter)
        {
            return Execute(timetable, crossoverProbability, fitnessMargin, noInMarginToStopAfter, noInMarginToStopAfter, 0, 0, 0);
        }
        public static Timetable Execute(Timetable timetable, double crossoverProbability, double fitnessMargin, int noToExpandSearchAfter, int noInMarginToStopAfter)
        {
            return Execute(timetable, crossoverProbability, fitnessMargin, noToExpandSearchAfter, noInMarginToStopAfter, 0, 0, 0);
        }
    }
}
