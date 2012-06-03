using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using ServiceStack.Text;

namespace Logging.Communications {
	public class RequestState {
		public HttpWebRequest Request { get; set; }
		public Action<Result> Callback { get; set; }
	}
}
