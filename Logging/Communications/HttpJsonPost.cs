using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using ServiceStack.Text;

namespace Logging.Communications {
	public class HttpJsonPost {

		Dictionary<string,string> _message;

		public HttpJsonPost(Dictionary<string, string> message) {
			_message = message;
		}

		public void Send(string url, string method, Action<Result> callback) {
			var request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Method = method;
			request.Timeout = 15000;
			request.ReadWriteTimeout = 15000;
			request.KeepAlive = false;

			using (var stream = request.GetRequestStream()) {
				JsonSerializer.SerializeToStream(_message, stream);
			}

			ProcessResponse(() => request.GetResponse(), callback);
		}

		public void SendAsync(string url, string method, Action<Result> callback) {
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
			ProcessResponse(() => state.Request.EndGetResponse(result), state.Callback);
		}

		private void ProcessResponse(Func<WebResponse> getResponse, Action<Result> callback) {
			try {
				using (var response = getResponse()) {
					if (callback != null) {
						var responseResult = new Result() { Success = true };
						using (var reader = new StreamReader(response.GetResponseStream())) {
							responseResult.RawContent = reader.ReadToEnd();
						}
						callback(responseResult);
					}
				}
			}
			catch (Exception exc) {
				if (callback != null)
					callback(new ErrorResult(exc));
			}
		}
	}
}