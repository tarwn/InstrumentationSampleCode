using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging.Communications;

namespace Logging {
	public interface ILogProvider {
		void Log(Dictionary<string, string> message, Action<Result> callback);
	}
}
