using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zoka.CSCommon
{
	/// <summary>Extension for Json.net which copies the JsonSerializer with all its passed settings</summary>
	public static class JsonSerializerExtensions
	{
		/// <summary>Will deep copy the serializer with all its settings</summary>
		public static JsonSerializer						DeepCopy(this JsonSerializer serializer)
		{
			var copiedSerializer = new JsonSerializer
			{
				Context = serializer.Context,
				Culture = serializer.Culture,
				ContractResolver = serializer.ContractResolver,
				ConstructorHandling = serializer.ConstructorHandling,
				CheckAdditionalContent = serializer.CheckAdditionalContent,
				DateFormatHandling = serializer.DateFormatHandling,
				DateFormatString = serializer.DateFormatString,
				DateParseHandling = serializer.DateParseHandling,
				DateTimeZoneHandling = serializer.DateTimeZoneHandling,
				DefaultValueHandling = serializer.DefaultValueHandling,
				EqualityComparer = serializer.EqualityComparer,
				FloatFormatHandling = serializer.FloatFormatHandling,
				Formatting = serializer.Formatting,
				FloatParseHandling = serializer.FloatParseHandling,
				MaxDepth = serializer.MaxDepth,
				MetadataPropertyHandling = serializer.MetadataPropertyHandling,
				MissingMemberHandling = serializer.MissingMemberHandling,
				NullValueHandling = serializer.NullValueHandling,
				ObjectCreationHandling = serializer.ObjectCreationHandling,
				PreserveReferencesHandling = serializer.PreserveReferencesHandling,
				ReferenceResolver = serializer.ReferenceResolver,
				ReferenceLoopHandling = serializer.ReferenceLoopHandling,
				StringEscapeHandling = serializer.StringEscapeHandling,
				TraceWriter = serializer.TraceWriter,
				TypeNameHandling = serializer.TypeNameHandling,
				SerializationBinder = serializer.SerializationBinder,
				TypeNameAssemblyFormatHandling = serializer.TypeNameAssemblyFormatHandling
			};
			foreach (var converter in serializer.Converters)
			{
				copiedSerializer.Converters.Add(converter);
			}
			return copiedSerializer;
		}
	}
}
