using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using Logging.Log;
using Logging.Metrics;

namespace SampleSiteWithLogging.Attributes {
	public class TimedActionAttribute : ActionFilterAttribute {

		private readonly Stopwatch _stopwatch;
		private DateTime _startTime;

		public TimedActionAttribute() {
			_stopwatch = new Stopwatch();
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext) {
			_startTime = DateTime.UtcNow;
			_stopwatch.Start();
		}



		public override void OnActionExecuted(ActionExecutedContext filterContext) {
			_stopwatch.Stop();

			string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
			string action = filterContext.ActionDescriptor.ActionName;
			var elapsed = _stopwatch.ElapsedMilliseconds;

			Dictionary<string,string> values = new Dictionary<string,string>(){
				{"Time", _startTime.ToString()},
				{"Controller", controller},
				{"Action", action},
				{"Elapsed", elapsed.ToString()}
			};
			Logger.Log(values, null);

			var metrics = new Metric []{
				new Metric(){ MetricType=MetricType.Counter, Name=String.Format("Hit.{0}.{1}", controller, action), Value=1 },
				new Metric(){ MetricType=MetricType.Gauge, Name=String.Format("Elapsed.{0}.{1}", controller, action), Value=elapsed }
			};
			MetricStore.Store(metrics);
		}

	}
}