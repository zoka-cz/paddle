using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.UIWrapper.Xamarin
{
	/// <summary>The wrapper of the UI</summary>
	public interface IUIWrapper
	{
		/// <summary>Returns the instance of the Xamarin.Forms.Application or null in case the Xamarin is not used</summary>
		global::Xamarin.Forms.Application					XamarinApplication { get; }

		/// <summary>Will navigate the browser to the page</summary>
		void NavigateToUrl(string _url);

		/// <summary>Will register interface for receiving Page (with browser) events</summary>
		void RegisterForPageUIEvents(IPageUIEventsAware _page_ui_events);

		/// <summary>Will register interface for receiving Application events</summary>
		void RegisterForAppUIEvents(IAppUIEventsAware _app_ui_events);

		/// <summary>Theme of the system</summary>
		ESystemTheme Theme { get; }
	}
}
