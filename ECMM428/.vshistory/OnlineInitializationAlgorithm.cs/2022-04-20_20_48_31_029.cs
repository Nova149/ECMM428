using System;
using System.Collections.Generic;

namespace ECMM428
{
    //Code to implement a heuristic algorithm, based on https://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=7349832

    public class OnlineInitializationAlgorithm
    {
        public static Timetable Execute(Timetable timetable)
        {
            //Initialize variables
            List<Module> modules = timetable.GetModules();
            List<Venue> venues = timetable.GetVenues();
            int n = modules.Count;

            //Algorithm 1 - Calculate pairing ability
            List<List<int>> x = new List<List<int>>();
            List<int> y = new List<int>();
            for (int i = 0; i < n; i++)
            {
                List<int> xi = new List<int>();
                y.Add(0);
                for (int j = 0; j < n; j++)
                {
                    int newXi;
                    if (Intersection(modules[i].GetStudents(), modules[j].GetStudents()).Count == 0 && modules[i].GetLecturer() != modules[j].GetLecturer())
                    {
                        newXi = 1;
                    }
                    else
                    {
                        newXi = 0;
                    }
                    xi.Add(newXi);
                    y[i] += newXi;
                }
                x.Add(xi);
            }

            //Algorithm 2 - Assign courses into groups
            List<bool> assign = new List<bool>();
            for (int j = 0; j < modules.Count; j++)
            {
                assign.Add(false);
            }
            bool allAssign = false;
            int total_venue_capacity = 0;
            List<List<Module>> g = new List<List<Module>>();
            foreach (Venue venue in venues)
            {
                total_venue_capacity += venue.GetCapacity();
            }
            while (!allAssign)
            {
                //Set k to index of minimum Y
                int leastY = -1;
                int k = -1;

                for (int i = 0; i < y.Count; i++)
                {
                    if (!assign[i] && (y[i] < leastY || leastY == -1))
                    {
                        leastY = y[i];
                        k = i;
                    }
                }
                List<Module> gm = new List<Module>();
                gm.Add(modules[k]);
                bool maxVenueNoReached = false;
                for (int j = 0; j < n && !maxVenueNoReached; j++)
                {
                    if (!assign[k] && !assign[j] && x[k][j] == 1)
                    {
                        bool allXEqualsOne = true;
                        Module module = modules[j];
                        int gmCount = gm.Count;
                        List<int> moduleIndexes = new List<int>();
                        foreach (Module m in gm)
                        {
                            moduleIndexes.Add(modules.IndexOf(m));
                        }
                        for (int gmVar = 0; gmVar < gmCount; gmVar++)
                        {
                            foreach (int i in moduleIndexes)
                            {
                                if (x[i][j] != 1)
                                {
                                    allXEqualsOne = false;
                                    break;
                                }
                            }
                            if (!assign[j] && allXEqualsOne && module.GetStudents().Count <= total_venue_capacity)
                            {
                                gm.Add(module);
                                gmCount++;
                                total_venue_capacity -= module.GetStudents().Count;
                                assign[j] = true;
                                //If the size of gm equals the max number of venues, break
                                if (gm.Count == timetable.GetVenues().Count) maxVenueNoReached = true;
                            }
                        }
                    }
                }
                assign[k] = true;
                foreach (bool b in assign)
                {
                    allAssign = true;
                    if (!b)
                    {
                        allAssign = false;
                        break;
                    }
                }
                g.Add(gm);
            }

            //Algorithm 3 - Timeslot Allocation
            int noTimeslotsInDay = timetable.GetNoTimeslotsInDay();
            int noOfDays = timetable.GetNoOfDays();
            int[] noOfLectureHours = new int[noOfDays];
            bool progressMade = true;
            while (!timetable.AllLecturesAssigned() && progressMade)
            {
                progressMade = false;
                for (int k = 0; k < g.Count; k++)
                {
                    List<Module> gk = g[k];
                    //Find day with least lecture hours
                    int i = 0;
                    for (int day = 1; day < noOfDays; day++)
                    {
                        if (noOfLectureHours[day] < noOfLectureHours[i])
                        {
                            i = day;
                        }
                    }

                    for (int j = 0; j < noTimeslotsInDay; j++)
                    {
                        //Find venues that are closest to the size required for each member of gm, prioritising venues that are larger than required
                        int gkCount = gk.Count;
                        for (int m = 0; m < gkCount; m++)
                        {
                            List<Venue> availableVenuesAtIJ = timetable.GetAvailableVenuesAtTime(i, j);
                            //If there are no available venues and there are still modules to assign at the current time slot, move to the next time slot
                            if (availableVenuesAtIJ.Count == 0) continue;

                            //Find venue that is closest to the capacity required, and the venue that is closest and is above the capacity
                            int capacityRequired = gk[m].GetStudents().Count;
                            Venue closestVenueAboveCap = null;
                            Venue closestVenue = availableVenuesAtIJ[0];
                            for (int r = 0; r < availableVenuesAtIJ.Count; r++)
                            {
                                if (availableVenuesAtIJ[r].GetCapacity() > capacityRequired
                                    && (closestVenueAboveCap == null
                                    || availableVenuesAtIJ[r].GetCapacity() < closestVenueAboveCap.GetCapacity())) closestVenueAboveCap = availableVenuesAtIJ[r];
                                if (Math.Abs(availableVenuesAtIJ[r].GetCapacity() - capacityRequired) < Math.Abs(closestVenue.GetCapacity() - capacityRequired))
                                    closestVenue = availableVenuesAtIJ[r];
                            }

                            //Assign one lecture from the module to the current timeslot, prioritizing the venue being closestVenueAboveCap if it is not null
                            if (closestVenueAboveCap != null) closestVenue = closestVenueAboveCap;
                            int maxLectureLengthPossible = timetable.MaxFreeSlotForRoom(closestVenue, i, j);
                            List<Lecture> lecturesInModule = new List<Lecture>(gk[m].GetUnassignedLectures());
                            foreach (Lecture l in lecturesInModule)
                            {
                                int lectureLength = l.GetDuration();
                                if (lectureLength <= maxLectureLengthPossible)
                                {
                                    timetable.SetSlot(gk[m], lectureLength, gk[m].GetCourse(), closestVenue, i, j);
                                    progressMade = true;
                                    noOfLectureHours[i]++;
                                    break;
                                }
                            }

                            //If all lectures from the module have been assigned, remove it from gk
                            if (gk[m].GetUnassignedLectures().Count == 0)
                            {
                                gk.RemoveAt(m);
                                gkCount--;
                            }
                        }
                    }
                }
            }
            //Calculate fitness values
            timetable.CalculateAllFitnessValues();

            return timetable;
        }

        public static List<T> Intersection<T>(List<T> a, List<T> b)
        {
            List<T> output = new List<T>();
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < b.Count; j++)
                {
                    if (EqualityComparer<T>.Default.Equals(a[i], b[j]))
                    {
                        output.Add(a[i]);
                    }
                }
            }
            return output;
        }
    }
}
