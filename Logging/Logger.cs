using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging.Communications;

namespace Logging {
	public class Logger {

		private ILogProvider _logProvider;
		private static Logger _current;

		public Logger(ILogProvider logProvider) {
			_logProvider = logProvider;
		}

		public void LogMessage(object message, Action<Result> callback) {
			_logProvider.Log(message, callback);
		}

		public static void SetDefaultLogger(ILogProvider logProvider) {
			_current = new Logger(logProvider);
		}

		public static void Log(object message, Action<Result> callback) {
			if (_current != null) {
				_current.LogMessage(message, callback);
			}
			else {
				throw new Exception("Default logger not setup");
			}
		}
	}
}
