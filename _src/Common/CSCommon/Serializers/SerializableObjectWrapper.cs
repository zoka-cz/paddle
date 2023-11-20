using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoka.CSCommon.Serializers
{
	/// <summary>Wrapper for object (of any type) which could be serialized</summary>
	[Newtonsoft.Json.JsonConverter(typeof(SerializableObjectWrapperJSONConverter))]
	public class SerializableObjectWrapper : ICloneable
	{
		private object										m_Data;

		/// <summary>Data</summary>
		public object										Data
		{
			get { return m_Data; }
			set
			{
				m_Data = value;
				if (m_Data == null)
					DataType = typeof(object).AssemblyQualifiedName;
				else
					DataType = m_Data.GetType().AssemblyQualifiedName;
			}
		}

		/// <summary>Type of data</summary>
		public string										DataType { get; set; }

		#region Construction

		/// <summary>Empty constructor</summary>
		public SerializableObjectWrapper()
		{
			
		}

		/// <summary>Constructor taking the object</summary>
		public SerializableObjectWrapper(object _data)
		{
			Data = _data;
		}


		/// <inheritdoc />
		public object										Clone()
		{
			var ret = new SerializableObjectWrapper() {
				Data = this.Data is ICloneable ? (this.Data as ICloneable).Clone() : this.Data
			};

			return ret;
		}

		#endregion // Construction
	}
}
