using DataLibrary.Entities;
using DataLibrary.Model.DTO.Request.TableRequest;
using DataLibrary.Model.DTO.Response;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace DataLibrary.Helper.Notification
{
    public class FirebaseNotification : IFirebaseNotification
    {
        static FirebaseApp? app;

        public FirebaseNotification()
        {
            ReadFireBaseAdminSdk();
        }

        private static void ReadFireBaseAdminSdk()
        {
            if (FirebaseMessaging.DefaultInstance == null)
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("Resources/admin_sdk.json")
                });
            }
            else
            {
                app = FirebaseApp.DefaultInstance;
            }
        }

        public async Task SendMeetingNotification(GetMeetingGroupsResponse meeting, List<NOTIFICATION_TOKENS> tokens)
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

        public async Task SendGroupNotification(GROUPS group, List<NOTIFICATION_TOKENS> tokens)
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
                try
                {
                    await FirebaseMessaging.DefaultInstance.SendAsync(obj);
                }
                catch { }
            }
        }

        public async Task SendMessageNotificationAsync(GetMeetingGroupsResponse meeting, GetMessageRequest getMessageRequest, List<NOTIFICATION_TOKENS> tokens)
        {
            if (getMessageRequest.IDUSER != meeting.IdAuthor)
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
                                title = "Ktoś właśnie odpowiedział na twoje zaproszenie do spotkania!";

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
                    try
                    {
                        FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
                        await messaging.SendAsync(obj);
                        //await FirebaseMessaging.DefaultInstance.SendAsync(obj);
                    }
                    catch { }
                }
            }
        }

        public async Task SendNotificationToUserTeamAsync(string? teamName, List<NOTIFICATION_TOKENS> tokens)
        {

            var androidNotificationObj = new Dictionary<string, string>
            {
                { "Team", "4" }
            };
            string title;
            string body;
            foreach (var token in tokens.GroupBy(x => x.TOKEN)
                .Select(g => g.First())
                .ToList())
            {
                if (teamName != null)
                {
                    title = "Zostałeś dodany do drużyny!";
                    body = "Organizator właśnie dodał cię do drużyny " + teamName;
                }
                else
                {
                    title = "Zostałeś usuniety z drużyny!";
                    body = "Organizator właśnie usunął cie z druzyny i przeniósł do rezerwy";
                }

                var obj = new Message
                {
                    Token = token.TOKEN,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = title,
                        Body = body
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
    }


}
