using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Metrics {
	public class MetricStore {

		private static MetricStore _current;
		private IMetricProvider _provider;

		public IMetricProvider Metrics { get { return _provider; } }

		public MetricStore(IMetricProvider provider) {
			_provider = provider;
		}

		public static void SetDefaultMetrics(IMetricProvider logProvider) {
			_current = new MetricStore(logProvider);
		}

		private static MetricStore GetDefaultLogger() {
			if (_current != null) {
				return _current;
			}
			else {
				throw new Exception("Default metrics not setup");
			}
		}

		public static void Store(string tag) {
			GetDefaultLogger().Metrics.Store(new Metric() { MetricType = MetricType.Counter, Name = tag, Value = 1 }, null);
		}

		public static void Store(Metric metric) {
			GetDefaultLogger().Metrics.Store(metric, null);
		}

		public static void Store(IEnumerable<Metric> metrics) {
			GetDefaultLogger().Metrics.Store(metrics, null);
		}

	}
}
