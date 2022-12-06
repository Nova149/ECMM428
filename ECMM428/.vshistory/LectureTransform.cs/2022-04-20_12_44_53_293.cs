﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ECMM428
{
    public class LectureTransform
    {
        int lectureNo;
        int startVenueNo;
        int finalVenueNo;
        int time1;
        int time2;
        public LectureTransform(int lectureNo, int venueNo1, int venueNo2, int time1, int time2)
        {
            this.lectureNo = lectureNo;
            this.startVenueNo = venueNo1;
            this.finalVenueNo = venueNo2;
            this.time1 = time1;
            this.time2 = time2;
        }
        public int[] GetTransform()
        {
            return new int[] { lectureNo, startVenueNo, finalVenueNo, time1, time2 };
        }
    }
}
