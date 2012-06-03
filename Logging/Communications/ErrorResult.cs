using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using ServiceStack.Text;

namespace Logging.Communications {
	public class ErrorResult : Result {

		public Exception InnerException { get; set; }

		public ErrorResult(Exception exc)
			: base() {
			Success = false;
			RawContent = exc.ToString();
			InnerException = exc;
		}
	}
}
