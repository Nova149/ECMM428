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
            List<Module> modules = timetable.GetModule();
            List<Module> modulesInOrder = new List<Module>() { modules[0] };

            //Create list of modules in order of most to least students attending
            for (int i = 1; i < modules.Count; i++)
            {
                Module m = modules[i];
                int lectureAttendeeCount = m.;

                //If the lecture is between two lectures
                for (int i = 0; i < assignedLectures.Count - 1; i++)
                {
                    if (lectureAttendeeCount > assignedLectures[i].GetTime() && lectureAttendeeCount <= assignedLectures[i + 1].GetTime())
                    {
                        assignedLectures.Insert(i + 1, lecture);
                        return;
                    }
                }

                //If the lecture is the last one
                assignedLectures.Add(lecture);
            }

            return timetable;
        }
    }
}
