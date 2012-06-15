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
		NetworkCredential _credentials;
		bool _useJson;

		public HttpJsonPost(Dictionary<string, string> message, NetworkCredential credentials = null, bool useJson = true) {
			_message = message;
			_credentials = credentials;
			_useJson = useJson;
		}

		private HttpWebRequest InitializeRequest(string url, string method) {
			var request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.Method = method;
			request.Timeout = 15000;
			request.ReadWriteTimeout = 15000;
			request.KeepAlive = false;
			if (_credentials != null)
				request.Credentials = _credentials;

			return request;
		}

		public void Send(string url, string method, Action<Result> callback, bool processResponse = true) {
			var request = InitializeRequest(url, method);

			using (var stream = request.GetRequestStream()) {
				WriteMessage(stream);
			}

			if (processResponse)
				ProcessResponse(() => request.GetResponse(), callback);
			else if (callback != null)
				callback(new Result() { Success = true });
		}

		public void SendAsync(string url, string method, Action<Result> callback, bool processResponse = true) {
			var request = InitializeRequest(url, method);

			var state = new RequestState() { 
				Request = request,
				Callback = callback,
				ProcessResponse = processResponse
			};
			request.BeginGetRequestStream(new AsyncCallback(GetRequestStream), state);
		}

        private void GetRequestStream(IAsyncResult result) {
			var state = (RequestState)result.AsyncState;
			using (var postStream = state.Request.EndGetRequestStream(result)) {
				WriteMessage(postStream);
			}
			if(state.ProcessResponse)
				state.Request.BeginGetResponse(GetResponseStream, state);
			else if (state.Callback != null)
				state.Callback(new Result() { Success = true });
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

		private void WriteMessage(Stream stream) {
			if (_useJson) {
				JsonSerializer.SerializeToStream(_message, stream);
			}
			else {
				byte[] data = new System.Text.UTF8Encoding().GetBytes(string.Join(" ", _message.Select(m => String.Format("{0}={1}", m.Key, m.Value))) + "\r\n");
				stream.Write(data, 0, data.Length);
			}
		}
	}
}