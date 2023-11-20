using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Zoka.Paddle.WebSocketSharp.Helpers.Middlewares.WebAPI
{
	internal class ControllerActionParameterDescriptor
	{
		enum EParameterSource
		{
			Unknown,
			QueryString,
			BodyJson,
		}

		private readonly ParameterInfo						m_ParameterInfo;
		public string										Name => m_ParameterInfo.Name;

		private ControllerActionParameterDescriptor(ParameterInfo _parameter_info)
		{
			m_ParameterInfo = _parameter_info;
		}

		public Task<(bool, object)>							TryResolveParameter(WebSocketSharpMiddlewareContext _context)
		{

			var param_source = GetParameterSource(_context);

			switch (param_source)
			{
				case EParameterSource.QueryString:
					return ResolveFromQueryString(_context);
				case EParameterSource.BodyJson:
					return ResolveFromJsonBody(_context);
				default:
					return Task.FromResult((false, (object)null));
			}
		}

		public static ControllerActionParameterDescriptor	FromParameterInfo(ParameterInfo _parameter_info)
		{
			var param_desc = new ControllerActionParameterDescriptor(_parameter_info);
			return param_desc;
		}

		private Task<(bool, object)>						ResolveFromQueryString(WebSocketSharpMiddlewareContext _context)
		{
			object param_value = null;
			var values = HttpUtility.ParseQueryString(_context.OriginalUrl.Query);
			if (!values.AllKeys.Any(k => k == Name))
				return Task.FromResult((false, param_value));
			var sval = values.Get(Name);
			var type_converter = TypeDescriptor.GetConverter(m_ParameterInfo.ParameterType);
			param_value = type_converter.ConvertFromString(sval);

			return Task.FromResult((true, param_value));
		}

		private async Task<(bool, object)>					ResolveFromJsonBody(WebSocketSharpMiddlewareContext _context)
		{
			object param_value = null;
			var content_type = new ContentType(_context.ContentType);
			if (content_type.MediaType != "application/json")
				return (false, param_value);
			string json_str;
			using (var sr = new StreamReader(_context.ContentStream))
				json_str = await sr.ReadToEndAsync();
			param_value = Newtonsoft.Json.JsonConvert.DeserializeObject(json_str, m_ParameterInfo.ParameterType);

			return (true, param_value);
		}


		private EParameterSource							GetParameterSource(WebSocketSharpMiddlewareContext _context)
		{
			if (_context.RequestMethod.In(HttpMethod.Get.Method, HttpMethod.Delete.Method))
			{
				return EParameterSource.QueryString;
			}
			else if (_context.RequestMethod.In(HttpMethod.Post.Method, HttpMethod.Put.Method))
			{
				var attrs = m_ParameterInfo.GetCustomAttributes(typeof(FromUriAttribute));
				if (attrs.Any())
					return EParameterSource.QueryString;
				return EParameterSource.BodyJson;
			}
			else
			{
				return EParameterSource.Unknown;
			}
		}
	}
}
