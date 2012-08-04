using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Logging.Log;

namespace SampleSiteWithLogging {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication {
		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			ILogProvider provider = GetProviderFromSettings();
			Logger.SetDefaultLogger(provider);
			Logger.Log(new Dictionary<string, string>() { { "Type", "ApplicationStartup" }, { "Time", DateTime.UtcNow.ToString() } }, null);
		}

		protected void Application_BeginRequest() {
			Logger.Log(new Dictionary<string, string>() { { "Type", "ApplicationRequest" }, { "UserAgent", Request.UserAgent }, { "Time", DateTime.UtcNow.ToString() } }, null);
		}

		protected void Application_Error(Object sender, System.EventArgs e) {
			System.Web.HttpContext context = HttpContext.Current;
			System.Exception exc = Context.Server.GetLastError();

			Logger.Log(new Dictionary<string, string>() { { "Type", "ApplicationError" }, { "Exception", exc.ToString() }, { "Time", DateTime.UtcNow.ToString() } }, null);
		}

		protected ILogProvider GetProviderFromSettings() {
			SensitiveSettings.SettingsManager.LoadFrom(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"bin\"));
			string provider = "null";
			if (SensitiveSettings.SettingsManager.Settings.ContainsKey("LogProvider")) {
				provider = SensitiveSettings.SettingsManager.Settings["LogProvider"];
			}

			switch (provider.ToUpper()) { 
				case "LOGGLY":
					return new LogglyProvider(SensitiveSettings.SettingsManager.Settings["Loggly.BaseURL"]);
				case "STORM":
					return new StormProvider(SensitiveSettings.SettingsManager.Settings["Storm.BaseURL"], SensitiveSettings.SettingsManager.Settings["Storm.AccessToken"], SensitiveSettings.SettingsManager.Settings["Storm.ProjectId"]);
				case "LOGENTRIES":
					return new LogentriesProvider(SensitiveSettings.SettingsManager.Settings["logentries.BaseURL"], SensitiveSettings.SettingsManager.Settings["logentries.AccountKey"], SensitiveSettings.SettingsManager.Settings["logentries.Host"], SensitiveSettings.SettingsManager.Settings["logentries.Log"]);
				default:
					return new NullLogProvider();
			}
			
		}
	}
}