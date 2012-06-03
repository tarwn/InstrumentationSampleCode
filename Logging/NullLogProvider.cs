using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging {
	public class NullLogProvider : ILogProvider {
		public void Log(Dictionary<string, string> message, Action<Communications.Result> callback) {
			if(callback != null)
				callback(new Communications.Result { Success = true, RawContent = "{ success: true; }" });
		}
	}
}
