using Logging.Communications;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Log
{
    public class ScalyrProvider : ILogProvider
    {
        private string _baseUrl;
        private string _accessToken;
        private bool _sendAsync;

        public ScalyrProvider(string baseUrl, string accessToken, bool sendAsync = true)
        {
            _baseUrl = baseUrl;
            _accessToken = accessToken;
            _sendAsync = sendAsync;
        }

        public string FullUrl
        {
            get
            {
                return _baseUrl;
            }
        }

        public void Log(Dictionary<string, string> message, Action<Communications.Result> callback)
        {
            string session = Guid.NewGuid().ToString();
            if (message.ContainsKey("SessionId"))
                session = message["SessionId"];

            var evt = new ScalyrEvent(DateTime.UtcNow);
            foreach (var pair in message.Where(kvp => !kvp.Key.Equals("SessionId")))
                evt.attrs[pair.Key] = pair.Value;

            var messageObject = new ScalyrMessage()
            {
                token = _accessToken,
                session = session
            };
            messageObject.events.Add(evt);

            string messageString = JsonSerializer.SerializeToString(messageObject);

            var request = new HttpJsonPost(messageString, null, true);
            if (_sendAsync)
                request.SendAsync(FullUrl, "POST", callback);
            else
                request.Send(FullUrl, "POST", callback);
        }

        private class ScalyrMessage
        {
            public string token { get; set; }
            public string session { get; set; }
            public ScalyrSessionInfo sessionInfo { get; set; }
            public List<ScalyrEvent> events { get; private set; }

            public ScalyrMessage()
            {
                sessionInfo = new ScalyrSessionInfo();
                events = new List<ScalyrEvent>();
            }
        }

        private class ScalyrSessionInfo
        {
            public string serverType { get; set; }
            public string serverId { get; set; }

            public ScalyrSessionInfo()
            {
                serverType = "NoTypeSpecified";
                serverId = "NotSpecified";
            }
        }

        private class ScalyrEvent
        {
            public string ts { get; private set; }
            public Dictionary<string, string> attrs { get; set; }

            public ScalyrEvent(DateTime timestamp)
            {
                ts = (timestamp.Subtract(DateTime.Parse("1/1/1970")).TotalMilliseconds * 1000000).ToString("F0");
                attrs = new Dictionary<string, string>();
            }
        }
    }
}
