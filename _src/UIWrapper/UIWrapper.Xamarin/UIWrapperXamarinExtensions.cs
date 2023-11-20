using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Zoka.Paddle.QRScanner.Abstraction;
using Zoka.Paddle.UIWrapper.Xamarin.QRScanner;

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

		/// <summary>Will register ZXingQRScanner as the IQRScanner implementation</summary>
		public static IServiceCollection					UseQRScanner(this IServiceCollection _service_collection)
		{
			_service_collection.AddSingleton<IQRScanner, ZXingQRScanner>();
			_service_collection.AddSingleton<QRScannerUIProvider>();
			return _service_collection;
		}

		/// <summary>Will register NoQRScanner as the IQRScanner implementation</summary>
		public static IServiceCollection					UseNoQRScanner(this IServiceCollection _service_collection)
		{
			_service_collection.AddSingleton<IQRScanner, NoQRScanner>();
			return _service_collection;
		}
	}
}
