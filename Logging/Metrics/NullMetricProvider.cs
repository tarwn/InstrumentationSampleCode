using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Metrics {
	public class NullMetricProvider : IMetricProvider {
		public void Store(Metric metric, Action<Communications.Result> callback) {
			if (callback != null)
				callback(new Communications.Result { Success = true, RawContent = "{ success: true; }" });
		}

		public void Store(IEnumerable<Metric> metrics, Action<Communications.Result> callback) {
			if (callback != null)
				callback(new Communications.Result { Success = true, RawContent = "{ success: true; }" });
		}
	}
}
