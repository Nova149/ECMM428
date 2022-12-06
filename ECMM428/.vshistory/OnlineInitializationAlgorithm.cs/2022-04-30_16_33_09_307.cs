using System;
using System.Collections.Generic;

namespace ECMM428
{
    //Code to implement an algorithm for finding the lectures that would benefit most from being made asynchronous, based on https://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=7349832

    public class OnlineInitializationAlgorithm
    {
        public static Timetable Execute(Timetable timetable, double fracToMakeOnline)
        {
            //Create list of lectures in order of most to least students attending
            List<Lecture> lectures = timetable.GetLectures();
            List<Lecture> lecturesInOrder = new List<Lecture>() { lectures[0] };

            for (int i = 1; i < lectures.Count; i++)
            {

            }

            return timetable;
        }
    }
}
