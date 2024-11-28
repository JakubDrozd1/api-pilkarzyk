using DataLibrary.Entities;
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

        public async Task SendMeetingNotification(List<NOTIFICATION_TOKENS> tokens, string title, string body, int idMeeting)
        {
            var androidNotificationObj = new Dictionary<string, string>
            {
                { "MeetingNotificationId", Convert.ToString(idMeeting) }
            };

            if (idMeeting == 0 || String.IsNullOrEmpty(body) || String.IsNullOrEmpty(title)) throw new Exception();

            foreach (var token in tokens.GroupBy(x => x.TOKEN)
                    .Select(g => g.First())
                    .ToList())
            {

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

        public async Task SendNotification(List<NOTIFICATION_TOKENS> tokens, string title, string body)
        {
            var androidNotificationObj = new Dictionary<string, string>
                    {
                        { "NotificationId", "1" }
                    };

            if (String.IsNullOrEmpty(body) || String.IsNullOrEmpty(title)) throw new Exception();

            foreach (var token in tokens.GroupBy(x => x.TOKEN)
                .Select(g => g.First())
                .ToList())
            {
                var obj = new Message
                {
                    Token = token.TOKEN,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = title,
                        Body = body,
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

        public async Task SendTeamNotification(List<NOTIFICATION_TOKENS> tokens, string title, string body, int idMeeting)
        {

            var androidNotificationObj = new Dictionary<string, string>
                {
                    { "TeamNotificationId", Convert.ToString(idMeeting) }
                };

            if (idMeeting == 0 || String.IsNullOrEmpty(body) || String.IsNullOrEmpty(title)) throw new Exception();

            foreach (var token in tokens.GroupBy(x => x.TOKEN)
                .Select(g => g.First())
                .ToList())
            {
                var obj = new Message
                {
                    Token = token.TOKEN,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = title,
                        Body = body,
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

        public async Task SendGroupNotification(List<NOTIFICATION_TOKENS> tokens, string title, string body, int idGroup)
        {
            var androidNotificationObj = new Dictionary<string, string>
                {
                    { "GroupId", Convert.ToString(idGroup) }
                };

            if (idGroup == 0 || String.IsNullOrEmpty(body) || String.IsNullOrEmpty(title)) throw new Exception();

            foreach (var token in tokens.GroupBy(x => x.TOKEN)
                .Select(g => g.First())
                .ToList())
            {
                var obj = new Message
                {
                    Token = token.TOKEN,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = title,
                        Body = body,
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