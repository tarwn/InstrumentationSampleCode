using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging.Communications;

namespace Logging {
	interface ILogProvider {
		void Log(object message, Action<Result> callback);
	}
}
