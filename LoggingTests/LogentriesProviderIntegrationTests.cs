using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Logging;
using Logging.Log;

namespace LoggingTests {
	
	[TestFixture]
	public class LogentriesProviderIntegrationTests {

		[Test, Explicit]
		public void Log_BasicObjectSynchronously_ExecutesHttpPostSuccessfully() {
			string url = SensitiveSettings.SettingsManager.Settings["logentries.BaseURL"];
			string api = SensitiveSettings.SettingsManager.Settings["logentries.AccountKey"];
			string host = SensitiveSettings.SettingsManager.Settings["logentries.Host"];
			string log = SensitiveSettings.SettingsManager.Settings["logentries.Log"];

			var message = new Dictionary<string, string> { 
				{ "Type", "UnitTest" },
				{ "TimeStamp", DateTime.UtcNow.ToString() },
				{"Method", "Log_BasicObjectSynchronously_ExecutesHttpPostSuccessfully"}
			};
			bool wasSuccess = false;
			var logger = new LogentriesProvider(url, api, host, log, false);

			logger.Log(message, (result) => wasSuccess = result.Success);

			Assert.IsTrue(wasSuccess);
		}
	}
}
