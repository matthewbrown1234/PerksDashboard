using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerksDashboard.Models.src.dto
{
    public enum ActivityType { Sales, Recognition}

    public class ActivityDto
    {
        Int32 _id;
        char _type;
        string _handleName;
        string _description;
        Boolean _verified;
        DateTimeOffset _dateTime;

        public char Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public Int32 Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string HandleName
        {
            get { return _handleName; }
            set { _handleName = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public Boolean Verified
        {
            get { return _verified; }
            set { _verified = value; }
        }
        public DateTimeOffset DateTime
        {
            get { return _dateTime; }
            set { _dateTime = value; }
        }
    }
}