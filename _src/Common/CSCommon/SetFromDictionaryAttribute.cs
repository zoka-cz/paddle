using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZWS.Common
{
	/// <summary>Attribute marking the property, which says, that this property is set and serialized into the Dictionary</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class SetFromDictionaryAttribute : Attribute
	{
		/// <summary>The default value of the property in case, its definition is not found in the dictionary</summary>
		public string										DefaultValue { get; set; }

		/// <summary>Empty constructor</summary>
		public SetFromDictionaryAttribute() : base()
		{ }

		/// <summary>Constructor taking the default value of the property</summary>
		public SetFromDictionaryAttribute(string _default_value) : base()
		{
			DefaultValue = _default_value;
		}
	}
}
