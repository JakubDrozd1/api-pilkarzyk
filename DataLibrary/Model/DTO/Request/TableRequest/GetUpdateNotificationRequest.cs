using System.Text.Json.Serialization;

namespace DataLibrary.Model.DTO.Request.TableRequest
{
    public class GetUpdateNotificationRequest
    {
        [JsonPropertyName("MeetingNotification")]
        public bool MEETING_NOTIFICATION { get; set; }

        [JsonPropertyName("GroupInvNotification")]
        public bool GROUP_INV_NOTIFICATION { get; set; }

        [JsonPropertyName("MeetingOrganizerNotification")]
        public bool MEETING_ORGANIZER_NOTIFICATION { get; set; }

        [JsonPropertyName("TeamNotofication")]
        public bool TEAM_NOTIFICATION { get; set; }

        [JsonPropertyName("UpdateMeetingNotification")]
        public bool UPDATE_MEETING_NOTIFICATION { get; set; }

        [JsonPropertyName("TeamOrganizerNotification")]
        public bool TEAM_ORGANIZER_NOTIFICATION { get; set; }

        [JsonPropertyName("GroupAddNotification")]
        public bool GROUP_ADD_NOTIFICATION { get; set; }

        [JsonPropertyName("MeetingCancelNotification")]
        public bool MEETING_CANCEL_NOTIFICATION { get; set; }
        public required string[] Column { get; set; }
    }
}
