using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Logging;

namespace LoggingTests {
	
	[TestFixture]
	public class StormProviderIntegrationTests {

		[Test]
		public void Log_BasicObjectSynchronously_ExecutesHttpPostSuccessfully() {
			string url = SensitiveSettings.SettingsManager.Settings["Storm.BaseURL"];
			string token = SensitiveSettings.SettingsManager.Settings["Storm.AccessToken"];
			string projectId = SensitiveSettings.SettingsManager.Settings["Storm.ProjectId"];

			var message = new Dictionary<string, string> { 
				{ "Type", "UnitTest" },
				{ "TimeStamp", DateTime.UtcNow.ToString() },
				{"Method", "Log_BasicObjectSynchronously_ExecutesHttpPostSuccessfully"}
			};
			bool wasSuccess = false;
			var logger = new StormProvider(url, token, projectId, false);

			logger.Log(message, (result) => wasSuccess = result.Success);

			Assert.IsTrue(wasSuccess);
		}
	}
}
