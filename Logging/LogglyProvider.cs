using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging.Communications;

namespace Logging {
	public class LogglyProvider : ILogProvider {

		string _baseUrl;

		public LogglyProvider(string baseUrl) {
			_baseUrl = baseUrl;
		}

		public void Log(Dictionary<string, string> message, Action<Communications.Result> callback) {
			var request = new HttpJsonPost(message);
			request.SendAsync(_baseUrl, "POST", callback);
		}

	}
}
