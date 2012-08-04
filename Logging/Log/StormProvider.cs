using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Logging.Communications;

namespace Logging.Log {
	public class StormProvider : ILogProvider {

		private string _baseUrl;
		private string _accessToken;
		private string _projectId;
		private bool _sendAsync;

		public StormProvider(string baseUrl, string accessToken, string projectId, bool sendAsync = true) {
			_baseUrl = baseUrl;
			_accessToken = accessToken;
			_projectId = projectId;
			_sendAsync = sendAsync;
		}

		public string FullUrl {
			get {
				return string.Format("{0}?index={1}&sourcetype=AppData", _baseUrl, _projectId);
			}
		}

		public void Log(Dictionary<string, string> message, Action<Communications.Result> callback) {
			var credentials = new NetworkCredential(_accessToken, "x");
			var request = new HttpJsonPost(message, credentials, false);
			if (_sendAsync)
				request.SendAsync(FullUrl, "POST", callback);
			else
				request.Send(FullUrl, "POST", callback);
		}
	}
}
