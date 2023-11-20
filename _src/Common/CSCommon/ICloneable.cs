using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoka.CSCommon
{
	/// <summary>ICloneable interface which supports cloning of the object (typed)</summary>
	public interface ICloneable<T>
	{
		/// <summary>Returns the deep clone of the object</summary>
		T DeepClone();
	}
}
