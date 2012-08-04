using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging.Communications;
using System.Net;

namespace Logging.Metrics {
	public class LibratoMetricsProvider : IMetricProvider {

		string _username;
		string _password;
		bool _sendAsync;
		string _url;

		public LibratoMetricsProvider(string URL, string username, string password, bool sendAsync) {
			_url = URL;
			_username = username;
			_password = password;
			_sendAsync = sendAsync;
		}

		public string FullUrl { get { return _url; } }

		public void Store(Metric metric, Action<Communications.Result> callback) {
			SendMessage(new Metric[] { metric }, callback);
		}

		public void Store(IEnumerable<Metric> metrics, Action<Communications.Result> callback) {
			SendMessage(metrics, callback);
		}

		private void SendMessage(IEnumerable<Metric> metrics, Action<Communications.Result> callback) {
			var message = GetMessages(metrics);
			var credentials = new NetworkCredential(_username, _password);
			var request = new HttpJsonPost(message, credentials, true);
			if (_sendAsync)
				request.SendAsync(FullUrl, "POST", callback);
			else
				request.Send(FullUrl, "POST", callback);
		}

		private string GetMessages(IEnumerable<Metric> metrics) {
			var result = new Dictionary<string, object>();

			var data = Labels.GroupJoin(metrics, lbl => lbl.MetricType, m => m.MetricType, 
								(lbl, ms) => String.Format("\"{0}\":[ {1} ]",
												lbl.Name,
												String.Join(",", ms.Select(m =>String.Format("{{ \"name\": \"{0}\", \"value\": {1}}}", m.Name, m.Value)))
											))
							.ToArray();

			return "{" + String.Join(",", data) + "}";
		}

		private DataLabel[] Labels {
			get {
				return new DataLabel[] { 
					new DataLabel(){ MetricType= MetricType.Gauge, Name="gauges"},
					new DataLabel(){ MetricType= MetricType.Counter, Name="counters"},
				};
			}
		}

		private class DataLabel {
			public string Name { get; set; }
			public MetricType MetricType { get; set; }
		}

	}

}
