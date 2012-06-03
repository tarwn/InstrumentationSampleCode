﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging.Communications;
using System.Diagnostics;

namespace Logging {
	public class Logger {

		private ILogProvider _logProvider;
		private static Logger _current;

		public Logger(ILogProvider logProvider) {
			_logProvider = logProvider;
		}

		public void LogMessage(Dictionary<string, string> message, Action<Result> callback) {
			_logProvider.Log(message, callback);
		}

		public static void SetDefaultLogger(ILogProvider logProvider) {
			_current = new Logger(logProvider);
		}

		private static Logger GetDefaultLogger() {
			if (_current != null) {
				return _current;
			}
			else {
				throw new Exception("Default logger not setup");
			}
		}

		public static void Log(Dictionary<string, string> message, Action<Result> callback) {
			GetDefaultLogger().LogMessage(message, callback);
		}

		public static LoggerWithElapsedTime CaptureElapsedTime(Dictionary<System.String, System.String> message, Action<Result> callback) {
			return new LoggerWithElapsedTime(GetDefaultLogger(), message, callback);
		}
	}

	public class LoggerWithElapsedTime : IDisposable {

		private Dictionary<string, string> _message;
		private Stopwatch _timer;
		private Action<Result> _callback;
		private Logger _logger;

		public LoggerWithElapsedTime(Logger logger, Dictionary<string, string> initialMessage, Action<Result> callback){
			_message = initialMessage;
			_message.Add("StartTime", DateTime.UtcNow.ToString());
			_callback = callback;
			_logger = logger;
			_timer = new Stopwatch();
			_timer.Start();
		}

		public void Dispose() {
			_message.Add("ElapsedTime", _timer.ElapsedMilliseconds.ToString());
			_logger.LogMessage(_message, _callback);
		}
	}
}
