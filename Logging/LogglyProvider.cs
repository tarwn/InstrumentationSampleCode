using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging.Communications;

namespace Logging {
	public class LogglyProvider : ILogProvider {

		string _baseUrl;
		bool _async;

		public LogglyProvider(string baseUrl, bool sendAsync = true) {
			_baseUrl = baseUrl;
			_async = sendAsync;
		}

		public void Log(Dictionary<string, string> message, Action<Communications.Result> callback) {
			var request = new HttpJsonPost(message);
			if(_async)
				request.SendAsync(_baseUrl, "POST", callback);
			else
				request.Send(_baseUrl, "POST", callback);
		}

	}
}
