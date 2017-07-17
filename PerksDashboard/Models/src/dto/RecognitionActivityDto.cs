using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerksDashboard.Models.src.dto
{
    public class RecognitionActivityDto: ActivityDto
    {
        Int32 _recognitionId;
        string _recognitionReason;

        public Int32 RecognitionId
        {
            get { return _recognitionId; }
            set { _recognitionId = value; }
        }
        public string RecognitionReason
        {
            get { return _recognitionReason; }
            set { _recognitionReason = value; }
        }
    }
}