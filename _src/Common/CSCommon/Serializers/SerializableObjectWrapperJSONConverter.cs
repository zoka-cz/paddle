using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoka.Paddle.CSCommon;

namespace Zoka.CSCommon.Serializers
{
	/// <summary>The JSON serializer for SerializableObjectWrapper</summary>
	public class SerializableObjectWrapperJSONConverter : JsonConverter
	{
		/// <inheritdoc />
		public override bool CanConvert(Type objectType)
		{
			return typeof(SerializableObjectWrapper).IsAssignableFrom(objectType);
		}

		/// <inheritdoc />
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		/// <inheritdoc/>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
				return (SerializableObjectWrapper)null;
			var jsonObject = JObject.Load(reader);
			if (jsonObject["Data"].Type == JTokenType.Null)
				return (SerializableObjectWrapper)null;
			var data_type_string = jsonObject["DataType"].Value<string>();
			var data_type = FindType(data_type_string);
			var target = Activator.CreateInstance(data_type);
			if (data_type.IsPrimitive)
			{
				target = (jsonObject["Data"] as JValue).Value;
				return new SerializableObjectWrapper(target);
			}
			else
			{
				serializer.Populate(jsonObject["Data"].CreateReader(), target);
				return new SerializableObjectWrapper(target);
			}
		}

		private Type										FindType(string _data_type_string)
		{
			var data_type = Type.GetType(_data_type_string);
			if (data_type == null)
			{
				var comma_idx = _data_type_string.IndexOf(',');
				if (comma_idx != -1)
				{
					_data_type_string = _data_type_string.Substring(0, comma_idx);
					data_type = TypeExtensions.GetTypeInAllAssemblies(_data_type_string);
					return data_type;
				}
			}
			return data_type;
		}

		/// <inheritdoc />
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new InvalidOperationException();
		}

	}
}
