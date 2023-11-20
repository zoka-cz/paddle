using System;
using System.Collections.Generic;

namespace Zoka.Paddle.CSCommon
{
	/// <summary>Helpers for the Type</summary>
	public static class TypeExtensions
	{
		/// <summary>Will search and find the type in all the loaded assemblies</summary>
		public static Type									GetTypeInAllAssemblies(string _typename)
		{
			foreach (System.Reflection.Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{

				var type = a.GetType(_typename);
				if (type != null)
					return type;
			}

			return null;
		}

		/// <summary>Will return enumerable of types from all loaded assemblies</summary>
		public static IEnumerable<Type>						GetTypesFromAllAssemblies()
		{
			var x = new List<Type>();
			foreach (System.Reflection.Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					foreach (var type in a.GetTypes())
					{
						x.Add(type);
					}
				}
				catch {}
			}
			return x;
		}
	}
}
