using System;
using System.Collections.Generic;

namespace ECMM428
{
    //Code to implement an algorithm for finding the lectures that would benefit most from being made asynchronous, based on https://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=7349832

    public class OnlineInitializationAlgorithm
    {
        public static Timetable Execute(Timetable timetable, double fracToMakeOnline)
        {
            //Initialize variables
            List<Module> modules = timetable.GetModules();
            List<Module> modulesInOrder = new List<Module>() { modules[0] };

            //Create list of modules in order of most to least students attending
            for (int i = 1; i < modules.Count; i++)
            {
                Module m = modules[i];
                int moduleAttendeeCount = m.GetStudents().Count;

                if(moduleAttendeeCount < modulesInOrder[0].GetStudents().Count)
                {
                    modulesInOrder.Insert(0, m);
                } 
                else
                {
                    for (int j = 0; j < modulesInOrder.Count; j++)
                    {
                        if (moduleAttendeeCount >= modulesInOrder[j].GetStudents().Count)
                        {
                            modulesInOrder.Insert(j, m);
                            break;
                        }
                    }
                }
            }



            return timetable;
        }
    }
}
