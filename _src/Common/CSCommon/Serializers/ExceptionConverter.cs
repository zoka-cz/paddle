using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zoka.CSCommon.Serializers
{
	/// <summary>Converter, which serialize the Exception type to be serialized with TypeNameHandling set to All, so it can be deserialized later to proper type</summary>
	public class ExceptionConverter : JsonConverter
	{
		/// <inheritdoc />
		public override void								WriteJson(JsonWriter   writer, object value, JsonSerializer serializer)
		{
			var ser = serializer.DeepCopy();
			ser.Converters.Remove(this);
			ser.TypeNameHandling = TypeNameHandling.All;
			ser.Serialize(writer, value);
		}

		/// <inheritdoc />
		public override object								ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new InvalidOperationException("Not to be implemented");
		}

		/// <inheritdoc />
		/// <remarks>This converter is used only for writing</remarks>
		public override bool								CanRead { get; } = false;

		/// <inheritdoc />
		public override bool CanConvert(Type        objectType)
		{
			return typeof(Exception).IsAssignableFrom(objectType);
		}
	}
}
