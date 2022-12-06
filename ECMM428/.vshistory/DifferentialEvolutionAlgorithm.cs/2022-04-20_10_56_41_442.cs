using System;
using System.Collections.Generic;
using System.Linq;

namespace ECMM428
{
    //Code to implement a differential evolution algorithm, based on https://www.researchgate.net/publication/261450836_A_Differential_Evolution_Algorithm_for_the_University_course_timetabling_problem

    public class DifferentialEvolutionAlgorithm : NatureInspiredAlgorithm
    {
        public static Timetable Execute(Timetable timetable, double fitnessMargin, int noToExpandSearchAfter, int noInMarginToStopAfter, int maxOnlineLectures, int maxAsynchronousLectures, int resets)
        {
            //Initialize population
            Timetable bestSolution;
            Timetable timetable1_1 = new Timetable(timetable);
            Timetable timetable2_1 = new Timetable(timetable);
            timetable1_1.GenericInitialization();
            timetable2_1.GenericInitialization();
            Timetable timetable1_2 = new Timetable(timetable1_1);
            Timetable timetable2_2 = new Timetable(timetable2_1);

            Timetable bestTimetable1 = new Timetable(timetable1_1);
            Timetable bestTimetable2 = new Timetable(timetable1_2);

            //No of current max online and asynchronous lectures per module
            int currentMaxOnlineLectures = 0;
            int currentMaxAsynchronousLectures = 0;

            //Current reset number
            int currentReset = 0;

            //Whether a lecture can be moved to be online or asynchronous
            bool canMoveLectureOnline = false;
            bool canMoveLectureAsynchronous = false;

            //Find best solution out of initial population
            double bestSolutionFitness;
            double timetable1Fitness = timetable1_1.GetFitness();
            double timetable2Fitness = timetable2_1.GetFitness();
            int currentIndex = 0;

            if (timetable1Fitness <= timetable2Fitness)
            {
                bestSolution = timetable1_1;
                bestSolutionFitness = timetable1Fitness;
            } else
            {
                bestSolution = timetable2_1;
                bestSolutionFitness = timetable2Fitness;
            }

            //Initialize fitnessHistory array
            double[] fitnessHistoryBestSolution = new double[noInMarginToStopAfter];
            fitnessHistoryBestSolution[0] = bestSolutionFitness;
            for (int i = 1; i < fitnessHistoryBestSolution.Length; ++i)
            {
                fitnessHistoryBestSolution[i] = -1;
            }

            List<Timetable> population = new List<Timetable>{timetable1_1, timetable2_1};
            int noIdentical = 0;

            //Begin the differential evolution algorithm
            while (true)
            {
                //Get the two timetables from the population vector
                Timetable parent1 = population[0];
                Timetable parent2 = population[1];

                //Expand the search radius if the most recent 'noToExpandSearchAfter' entries are all within 'fitnessMargin'
                int noOperations = 1;
                if (noIdentical > noToExpandSearchAfter)
                {
                    noOperations = noIdentical - noToExpandSearchAfter;
                }

                //Generate children
                Timetable child1 = new Timetable(parent1);
                Timetable child2 = new Timetable(parent2);

                Random rnd = new Random();
                //Either move a lecture to be online or asynchronous (if possible) or perform crossover/mutation
                if ((canMoveLectureOnline || canMoveLectureAsynchronous) && rnd.Next(0, 2) == 0)
                {
                    //Move a lecture to be online or asynchronous, if required
                    if (canMoveLectureOnline)
                    {
                        bool lectureMoved1 = LectureToOnlineMutation(child1, currentMaxOnlineLectures);
                        bool lectureMoved2 = LectureToOnlineMutation(child2, currentMaxOnlineLectures);
                        if (!lectureMoved1 && !lectureMoved2) canMoveLectureOnline = false;
                    }
                    else if (canMoveLectureAsynchronous)
                    {
                        bool lectureMoved1 = LectureToAsynchronousMutation(child1, currentMaxAsynchronousLectures);
                        bool lectureMoved2 = LectureToAsynchronousMutation(child2, currentMaxAsynchronousLectures);
                        if (!lectureMoved1 && !lectureMoved2) canMoveLectureAsynchronous = false;
                    }
                }
                else
                {
                    Mutation(child1, noOperations);
                    Mutation(child2, noOperations);
                    Crossover(child1, child2, noOperations);
                }

                //Compare old and new timetables. Keep the timetable with the better (smaller) fitness value if there as an improvement over the current best solution,
                //and remove the worst solution
                double child1Fitness = child1.GetFitness();
                double child2Fitness = child2.GetFitness();

                if (child1Fitness < bestSolutionFitness && child1Fitness < child2Fitness)
                {
                    population.Add(child1);
                    bestSolution = child1;
                    bestSolutionFitness = child1Fitness;
                    population.RemoveAt(0);
                }
                else if (child2Fitness < bestSolutionFitness && child2Fitness < child1Fitness)
                {
                    population.Add(child2);
                    bestSolution = child2;
                    bestSolutionFitness = child2Fitness;
                    population.RemoveAt(0);
                }

                //Compare new fitness with most recent fitness. Increment noIdentical if necessary
                /*if (Math.Abs(timetable1Fitness - fitnessHistoryBestSolution[currentIndex]) < fitnessMargin) noIdentical++;
                else noIdentical = 0;*/

                //Update fitness histories
                fitnessHistoryBestSolution[currentIndex] = bestSolutionFitness;

                //If we are at the end of the array, go back to the beginning. Otherwise advance the index
                if (currentIndex == noInMarginToStopAfter - 1) currentIndex = 0;
                else currentIndex++;

                //If either timetable's fitness has not changed by fitnessRequirment within noInMarginToStopAfter, return that timetable.
                //If both have satisfied it, return the timetable that currently has the lower fitness function
                bool timetable1Satisfied = CheckFitnessSatisfied(fitnessHistoryBestSolution, fitnessMargin);

                if (timetable1Satisfied)
                {
                    if (currentMaxOnlineLectures < maxOnlineLectures || currentMaxAsynchronousLectures < maxAsynchronousLectures)
                    {
                        if (fitnessHistoryBestSolution[currentIndex] != 0)
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
                        //Reset fitness history to allow for time to experiment with new timetables
                        currentReset++;
                        currentIndex = 0;
                        noIdentical = 0;
                        for (int i = 0; i < fitnessHistoryBestSolution.Length; ++i)
                        {
                            fitnessHistoryBestSolution[i] = -1;
                        }
                    }
                    else return bestSolution;
                }
            }
        }
    }
}
