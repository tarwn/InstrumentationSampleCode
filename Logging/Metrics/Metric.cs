using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Metrics {
	public class Metric {
		public virtual MetricType MetricType { get; set; }
		public double Value { get; set; }
		public string Source { get; set; }
		public string Name { get; set; }
	}

	public class MultiSampleGauge : Metric {
		public override MetricType MetricType { get { return MetricType.Gauge; } set { } }

		public int Count { get; set; }
		public double Sum { get; set; }
		public double Max { get; set; }
		public double Min { get; set; }
		public double SumSquares { get; set; }
	}

	public enum MetricType {
		Gauge,
		Counter
	}
}

