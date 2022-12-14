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
        public LectureTransform(Lecture lecture, Venue venue1, Venue venue2, int time1, int time2)
        {
            this.lectureNo = lecture.GetNumber();
            if (venue1 != null) { 
                this.venueNo1 = venue1.GetNumber(); 
            } else
            {
                this.venueNo1 = -1;
            }
            if (venue2 != null)
            {
                this.venueNo2 = venue2.GetNumber();
            }
            else
            {
                this.venueNo2 = -1;
            }
            this.time1 = time1;
            this.time2 = time2;
        }
        public int[] GetTransform()
        {
            return new int[] { lectureNo, venueNo1, venueNo2, time1, time2 };
        }
        public int GetNumber()
        {
            return lectureNo;
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
        public void PerformTransform(Timetable timetable, int finalNo)
        {
            Lecture lecture = timetable.GetLecture(lectureNo);
            timetable.UnassignLecture(lecture);
            if (finalNo == 1)
            {
                if (venueNo1 == -1)
                {

                }
            }
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
            LectureTransform transform = new LectureTransform(lecture, venue1, venue2, time1, time2);
            array[lectureNo] = transform;
        }
        public void AddTransform(LectureTransform transform)
        {
            int lectureNo = transform.GetNumber();
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
