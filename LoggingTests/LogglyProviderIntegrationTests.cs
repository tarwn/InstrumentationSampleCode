using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Logging;

namespace LoggingTests {

	[TestFixture]
	public class LogglyProviderIntegrationTests {

		[Test, Explicit]
		public void Log_BasicObjectSynchronously_ExecutesHttpPostSuccessfully() 
		{
			string url = SensitiveSettings.SettingsManager.Settings["Loggly.BaseURL"];
			var message = new Dictionary<string, string> { 
				{ "Type", "UnitTest" },
				{ "TimeStamp", DateTime.UtcNow.ToString() },
				{"Method", "Log_BasicObjectSynchronously_ExecutesHttpPostSuccessfully"}
			};
			bool wasSuccess = false;
			var logger = new LogglyProvider(url, false);

			logger.Log(message, (result) => wasSuccess = result.Success);

			Assert.IsTrue(wasSuccess);
		}
	}
}
