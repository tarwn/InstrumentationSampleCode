using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Logging.Metrics;

namespace LoggingTests {
	[TestFixture]
	public class LibratoMetricsTests {

		[Test, Explicit]
		public void LibratoTest() {
			string url = SensitiveSettings.SettingsManager.Settings["librato.URL"];
			string token = SensitiveSettings.SettingsManager.Settings["librato.Token"];
			string username = SensitiveSettings.SettingsManager.Settings["librato.Email"];

			var metric = new Metric() {
				MetricType = MetricType.Gauge,
				Name = "UnitTest1",
				Value = new Random().NextDouble()
			};

			bool wasSuccess = false;
			var store = new LibratoMetricsProvider(url, username, token, false);

			store.Store(metric, (r) => wasSuccess = r.Success);

			Assert.IsTrue(wasSuccess);
		}

		[Test, Explicit]
		public void LibratoTest2() {
			string url = SensitiveSettings.SettingsManager.Settings["librato.URL"];
			string token = SensitiveSettings.SettingsManager.Settings["librato.Token"];
			string username = SensitiveSettings.SettingsManager.Settings["librato.Email"];

			var rnd = new Random();
			var metrics = new Metric[]{
				new Metric() { MetricType = MetricType.Gauge,  Name = "UnitTest2",		 Value = rnd.NextDouble()			},
				new Metric() { MetricType = MetricType.Gauge,  Name = "UnitTest3",		 Value = rnd.NextDouble()			},
				new Metric() { MetricType = MetricType.Counter,Name = "UnitTestCounter1",Value = (int)(100 * rnd.NextDouble())			},
				new Metric() { MetricType = MetricType.Counter,Name = "UnitTestCounter2",Value = (int)(100 * rnd.NextDouble())			}
			};

			bool wasSuccess = false;
			var store = new LibratoMetricsProvider(url, username, token, false);

			store.Store(metrics, (r) => wasSuccess = r.Success);

			Assert.IsTrue(wasSuccess);
		}
	}
}
