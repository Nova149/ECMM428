using System;
using System.Collections.Generic;
using System.Text;

namespace ECMM428
{
    public class LectureTransform
    {
        int lectureNo;
        int venueNo1;
        int venueNo2;
        int time1;
        int time2;
        public LectureTransform(int lectureNo, int venueNo1, int venueNo2, int time1, int time2)
        {
            this.lectureNo = lectureNo;
            this.venueNo1 = venueNo1;
            this.venueNo2 = venueNo2;
            this.time1 = time1;
            this.time2 = time2;
        }
        public int[] GetTransform()
        {
            return new int[] { lectureNo, venueNo1, venueNo2, time1, time2 };
        }
        public void ChangeVariable(int no, Venue venue, int time)
        {
            if (no == 1)
            {
                venueNo1 = venue.GetNumber();
                time1 = time;
            } else if (no == 2)
            {
                venueNo2 = venue.GetNumber();
                time2 = time;
            }
        }

        public class LectureTransformArray
        {
            Dictionary<int, LectureTransform> array;

            public LectureTransformArray()
            {
                array = new Dictionary<int, LectureTransform>();
            }
            public void AddTransform(Lecture lecture, Venue venue1, Venue venue2, int time1, int time2)
            {
                int lectureNo = lecture.GetNumber();
                LectureTransform transform = new LectureTransform(lectureNo, venue1.GetNumber(), venue2.GetNumber(), time1, time2);
                array[lectureNo] = transform;
            }

            public int[] GetTransform(Lecture lecture)
            {
                return array[lecture.GetNumber()].GetTransform();
            }
            public void ChangeTransform(Lecture lecture, int no, Venue venue, int time)
            {
                array[lecture.GetNumber()].ChangeVariable(no, venue, time);
            }
            public bool Contains(Lecture lecture)
            {
                if (array.ContainsKey(lecture.GetNumber())) return true;
                else return false;
            }
        }
    }
}
