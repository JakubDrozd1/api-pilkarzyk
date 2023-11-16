using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Entities;

namespace DataLibrary.IRepository
{
    internal interface IMeetingsRepository
    {
        List<Meeting> GetAllMeetings();
        Meeting? GetMeetingById(int meetingId);
        void AddMeeting(Meeting meeting);
        void UpdateMeeting(Meeting meeting);
        void DeleteMeeting(int meetingId);
    }
}
