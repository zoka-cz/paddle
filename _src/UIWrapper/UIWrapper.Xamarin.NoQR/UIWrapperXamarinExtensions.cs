using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Zoka.Paddle.UIWrapper.Xamarin
{
	/// <summary>DI extensions to the UIWrapper.Xamarin</summary>
	public static class UIWrapperXamarinExtensions
	{
		/// <summary>Will register the UIWrapperXamarin as IUIWrapper implementation</summary>
		public static IServiceCollection					UseXamarinUI(this IServiceCollection _service_collection)
		{
			_service_collection.AddSingleton<IUIWrapper, UIWrapperXamarin>();
			return _service_collection;
		}

		/// <summary>Will register the UIWrapperNoXamarin as IUIWrapper implementation</summary>
		public static IServiceCollection					UseNoXamarinUI(this IServiceCollection _service_collection)
		{
			_service_collection.AddSingleton<IUIWrapper, UIWrapperNoXamarin>();
			return _service_collection;
		}
	}
}
