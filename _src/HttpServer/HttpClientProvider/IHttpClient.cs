using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zoka.Paddle.HttpClientProvider
{
	/// <summary>The application http client helper</summary>
	public interface IHttpClient
	{
		/// <summary>For the next request sent it uses the provided username and password</summary>
		IHttpClient WithSingleUseBasicAuth(string _username, string _password);

		/// <summary>For next request sent it uses the provided headers</summary>
		IHttpClient WithSingleUseHeaders(string _name, IEnumerable<string> _values);

		/// <summary>Will sends the GET request and returns decoded TOut typed data</summary>
		Task<TOut> GetAsync<TOut>(string _api_url);

		/// <summary>Will download file with GET request</summary>
		Task<ContentFileInfo> GetFileAsync(string _api_url);

		/// <summary>Will sends the POST request with TIn typed data and returns TOut typed data</summary>
		Task<TOut> PostAsync<TOut, TIn>(string _api_url, TIn _data_to_send);

		/// <summary>Will send the POST request with no content, and do not expect any content</summary>
		Task PostAsync(string _api_url);

		/// <summary>Will send the POST request with passed model and also passed files (multipart content)</summary>
		Task<TOut> PostAsyncDataAndFiles<TOut, TIn>(string _api_url, TIn _data_to_send, string _data_section_name, IEnumerable<ContentFileInfo> _files);

		/// <summary>Will sends the PUT request with TIn typed data</summary>
		Task PutAsync<TIn>(string _api_url, TIn _data_to_send);

		/// <summary>Will send the PUT request with passed model and also passed files (multipart content)</summary>
		Task<TOut> PutAsyncDataAndFiles<TOut, TIn>(string _api_url, TIn _data_to_send, string _data_section_name, IEnumerable<ContentFileInfo> _files);

		/// <summary>
		/// Will send the DELETE request
		/// </summary>
		/// <param name="_api_url"></param>
		/// <returns></returns>
		Task DeleteAsync(string _api_url);

	}
}
