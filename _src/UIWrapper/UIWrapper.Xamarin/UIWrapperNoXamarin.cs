using System;
using System.Collections.Generic;
using ita_mobile_terminal;

namespace Zoka.Paddle.UIWrapper.Xamarin
{

	/// <summary>Class which wraps the UI - in case the app uses no window</summary>
	public class UIWrapperNoXamarin : IUIWrapper
	{
		private List<IPageUIEventsAware>					m_PageUIEventReceivers = new List<IPageUIEventsAware>();
		private List<IAppUIEventsAware>						m_AppUIEventReceivers = new List<IAppUIEventsAware>();

		/// <summary>Constructor</summary>
		public UIWrapperNoXamarin()
		{

		}

		/// <summary>Returns the instance of the Xamarin.Forms.Application</summary>
		public global::Xamarin.Forms.Application			XamarinApplication => null;

		/// <summary>Will navigate the browser to the page</summary>
		public void											NavigateToUrl(string _url)
		{
			// nothing to do
		}

		/// <inheritdoc />
		public void											RegisterForPageUIEvents(IPageUIEventsAware _page_ui_events)
		{
			m_PageUIEventReceivers.Add(_page_ui_events);
		}

		/// <inheritdoc />
		public void											RegisterForAppUIEvents(IAppUIEventsAware _app_ui_events)
		{
			m_AppUIEventReceivers.Add(_app_ui_events);
		}

		/// <inheritdoc />
		public ESystemTheme									Theme => ESystemTheme.NotSpecified;
	}
}
