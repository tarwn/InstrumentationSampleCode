using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging.Communications;
using System.Net.Sockets;
using System.Net.Security;

namespace Logging {
	public class LogentriesProvider : ILogProvider {

		private string _baseUrl;
		private string _apiKey;
		private string _hostKey;
		private string _logkey;
		private bool _sendAsync;

		public LogentriesProvider(string baseUrl, string apiKey, string hostKey, string logKey, bool sendAsync = true) {
			_baseUrl = baseUrl;
			_apiKey = apiKey;
			_hostKey = hostKey;
			_logkey = logKey;
			_sendAsync = sendAsync;
		}

		public string FullUrl {
			get {
				return String.Format("{0}/{1}/hosts/{2}/{3}?realtime=1", _baseUrl, _apiKey, _hostKey, _logkey);
			}
		}

		public void Log(Dictionary<string, string> message, Action<Communications.Result> callback) {
			var request = new HttpJsonPost(message, useJson: false);
			if (_sendAsync)
				request.SendAsync(FullUrl, "PUT", callback, false);
			else
				request.Send(FullUrl, "PUT", callback, false);
		}
	}
}
