using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Logging.Log;
using SampleSiteWithLogging.Attributes;

namespace SampleSiteWithLogging.Controllers {
	public class HomeController : Controller {
		//
		// GET: /Home/

		public ActionResult Index() {
			return View();
		}

		[TimedAction]
		public ActionResult ShortOperation() {
			return View("Index");
		}

		[TimedAction]
		public ActionResult LongOperation() {
			Thread.Sleep((int)(3000 * new Random().NextDouble()));
			return View("Index");
		}

	}
}
