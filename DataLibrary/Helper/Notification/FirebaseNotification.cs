using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request;
using DataLibrary.Model.DTO.Response;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace DataLibrary.Helper.Notification
{
    public class FirebaseNotification : IFirebaseNotification
    {
        public FirebaseNotification()
        {
            ReadFireBaseAdminSdk();
        }

        private static void ReadFireBaseAdminSdk()
        {

            if (FirebaseMessaging.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("Resources/admin_sdk.json")
                });
            }
        }

        public async void SendMeetingNotification(GetMeetingGroupsResponse meeting, List<NOTIFICATION_TOKENS> tokens)
        {

            var androidNotificationObj = new Dictionary<string, string>
            {
                { "Meeting", "1" }
            };
            foreach (var token in tokens.GroupBy(x => x.TOKEN)
                .Select(g => g.First())
                .ToList())
            {
                var obj = new Message
                {
                    Token = token.TOKEN,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = "Nowe zaproszenie do spotkania w grupie " + meeting.Name,
                        Body = meeting.DateMeeting?.ToString("dd-MM-yyyy HH:mm") + " " + meeting.Place + " " + meeting.Description
                    },
                    Data = androidNotificationObj
                };
                try
                {
                    await FirebaseMessaging.DefaultInstance.SendAsync(obj);
                }
                catch { }
            }
        }

        public async void SendGroupNotification(GROUPS group, List<NOTIFICATION_TOKENS> tokens)
        {

            var androidNotificationObj = new Dictionary<string, string>
            {
                { "Group", "2" }
            };
            foreach (var token in tokens.GroupBy(x => x.TOKEN)
                .Select(g => g.First())
                .ToList())
            {
                var obj = new Message
                {
                    Token = token.TOKEN,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = "Ktoś właśnie wysłał ci zaproszenie do grupy!",
                        Body = "Nowe zaproszenie do grupy " + group.NAME,
                    },
                    Data = androidNotificationObj
                };
                await FirebaseMessaging.DefaultInstance.SendAsync(obj);
            }
        }

        public async void SendMessageNotificationAsync(GetMeetingGroupsResponse meeting, GetMessageRequest getMessageRequest, List<NOTIFICATION_TOKENS> tokens)
        {
            var androidNotificationObj = new Dictionary<string, string>
            {
                { "Message", "3" }
            };
            string title;
            foreach (var token in tokens.GroupBy(x => x.TOKEN)
                .Select(g => g.First())
                .ToList())
            {
                switch (getMessageRequest.ANSWER)
                {
                    case "yes":
                        {
                            title = "Ktoś właśnie zaakceptował twoje zaproszenie do spotkania!";
                        }
                        break;
                    case "no":
                        {
                            title = "Ktoś właśnie odrzucił twoje zaproszenie do spotkania!";
                        }
                        break;
                    default:
                        {
                            title = "Ktoś właśnie odpowiedział na twoje zaproszenie do grupy!";

                        };
                        break;
                }
                var obj = new Message
                {
                    Token = token.TOKEN,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = title,
                        Body = meeting.DateMeeting?.ToString("dd-MM-yyyy HH:mm") + " " + meeting.Place + " " + meeting.Description
                    },
                    Data = androidNotificationObj
                };
                await FirebaseMessaging.DefaultInstance.SendAsync(obj);
            }
        }
    }


}
