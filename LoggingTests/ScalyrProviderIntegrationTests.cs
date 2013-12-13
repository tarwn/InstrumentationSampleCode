using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Logging;
using Logging.Log;

namespace LoggingTests {
	
	[TestFixture]
	public class ScalyrProviderIntegrationTests {

		[Test, Explicit]
		public void Log_BasicObjectSynchronously_ExecutesHttpPostSuccessfully() {
			string url = SensitiveSettings.SettingsManager.Settings["Scalyr.BaseURL"];
            string token = SensitiveSettings.SettingsManager.Settings["Scalyr.WriteLogsToken"];

			var message = new Dictionary<string, string> { 
				{ "Type", "UnitTest" },
				{ "Time", DateTime.UtcNow.ToString() },
				{"Method", "Log_BasicObjectSynchronously_ExecutesHttpPostSuccessfully"}
			};
			bool wasSuccess = false;
			var logger = new ScalyrProvider(url, token, false);

			logger.Log(message, (result) => wasSuccess = result.Success);

			Assert.IsTrue(wasSuccess);
		}
	}
}
