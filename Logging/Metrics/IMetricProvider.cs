using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging.Communications;

namespace Logging.Metrics {
	public interface IMetricProvider {
		void Store(Metric metric, Action<Result> callback);
		void Store(IEnumerable<Metric> metrics, Action<Result> callback);
	}
}
