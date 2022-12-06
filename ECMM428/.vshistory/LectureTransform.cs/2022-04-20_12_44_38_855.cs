using System;
using System.Collections.Generic;
using System.Text;

namespace ECMM428
{
    public class LectureTransform
    {
        int lectureNo;
        int startVenueNo;
        int finalVenueNo;
        int startTime;
        int finalTime;
        public LectureTransform(int lectureNo, int venueNo1, int venueNo2, int time1, int time2)
        {
            this.lectureNo = lectureNo;
            this.startVenueNo = venueNo1;
            this.finalVenueNo = venueNo2;
            this.startTime = time1;
            this.finalTime = time2;
        }
        public int[] GetTransform()
        {
            return new int[] { lectureNo, startVenueNo, finalVenueNo, startTime, finalTime };
        }
    }
}
