using System;
using System.Collections.Generic;

namespace ECMM428
{
    //Code to implement a heuristic algorithm, based on https://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=7349832

    public class AsynchronousInitializationAlgorithm
    {
        public static Timetable Execute(Timetable timetable, double fracToMakeAsynchronous)
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

            //Algorithm 2 - Assign modules into groups
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

            //Algorithm 3 - Set lectures in the smallest groups to be asynchronous, up to the input limit
            

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
