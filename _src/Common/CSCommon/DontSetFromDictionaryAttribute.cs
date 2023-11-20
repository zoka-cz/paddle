using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZWS.Common
{
	/// <summary>Attribute marking the property, which says, that this property is set and serialized into the Dictionary</summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class DontSetFromDictionaryAttribute : Attribute
	{
	}
}
