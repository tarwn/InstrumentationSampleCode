using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Logging.Log;

namespace SampleSiteWithLogging.Controllers {
	public class HomeController : Controller {
		//
		// GET: /Home/

		public ActionResult Index() {
			return View();
		}

		public ActionResult ShortOperation() {
			using (var log = Logger.CaptureElapsedTime(new Dictionary<string, string> { { "Type", "SiteHit" }, { "Area", "HomeController" }, { "Method", "ShortOperation" } }, null)) {
				return View("Index");
			}
		}

		public ActionResult LongOperation() {
			using (var log = Logger.CaptureElapsedTime(new Dictionary<string, string> { { "Type", "SiteHit" }, { "Area", "HomeController" }, { "Method", "LongOperation" } }, null)) {
				Thread.Sleep((int)(3000 * new Random().NextDouble()));
				return View("Index");
			}
		}

	}
}
