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
        public LectureTransform(int lectureNo, int venueNo1, int finalVenueNo, int startTime, int finalTime)
        {
            this.lectureNo = lectureNo;
            this.startVenueNo = venueNo1;
            this.finalVenueNo = finalVenueNo;
            this.startTime = startTime;
            this.finalTime = finalTime;
        }
        public int[] GetTransform()
        {
            return new int[] { lectureNo, startVenueNo, finalVenueNo, startTime, finalTime };
        }
    }
}
