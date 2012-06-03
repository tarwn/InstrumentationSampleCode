using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using ServiceStack.Text;

namespace Logging.Communications {
	public class HttpJsonPost {

		object _message;

		public HttpJsonPost(object message) {
			_message = message;
		}

		public void Send(string url, string method, Action<Result> callback) {
			var request = (HttpWebRequest) HttpWebRequest.Create(url);
			request.Method = method;
			request.Timeout = 15000;
			request.ReadWriteTimeout = 15000;
			request.KeepAlive = false;

			var state = new RequestState() { 
				Request = request,
				Callback = callback
			};
			request.BeginGetRequestStream(new AsyncCallback(GetRequestStream), state);
		}

		private void GetRequestStream(IAsyncResult result) {
			var state = (RequestState)result.AsyncState;
			using (var postStream = state.Request.EndGetRequestStream(result)) {
				JsonSerializer.SerializeToStream(_message, postStream);
			}
			state.Request.BeginGetResponse(GetResponseStream, state);
		}

		private void GetResponseStream(IAsyncResult result) {
			var state = (RequestState)result.AsyncState;
			try {
				using (var response = (HttpWebResponse)state.Request.EndGetResponse(result)) {
					if (state.Callback != null) {
						var responseResult = new Result() { Success = true };
						using (var reader = new StreamReader(response.GetResponseStream())) {
							responseResult.RawContent = reader.ReadToEnd();
						}
						state.Callback(responseResult);
					}
				}
			}
			catch (Exception exc) {
				if (state.Callback != null)
					state.Callback(new ErrorResult(exc));
			}
		}	
	}
}