using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zoka.Web.Helper.JSON
{
	/// <summary>JSON Converter which enables to create objects</summary>
	public abstract class AJsonCreationConverter<T> : JsonConverter where T : class
	{
		/// <summary>Will create object of some type</summary>
		protected abstract T Create(Type objectType, JObject jsonObject);

		/// <summary></summary>
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		/// <summary> </summary>
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		/// <summary> </summary>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
				return null;
			var jsonObject = JObject.Load(reader);
			var target = Create(objectType, jsonObject);
			serializer.Populate(jsonObject.CreateReader(), target);
			return target;
		}

		/// <summary> </summary>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new InvalidOperationException();
		}
	}
}
