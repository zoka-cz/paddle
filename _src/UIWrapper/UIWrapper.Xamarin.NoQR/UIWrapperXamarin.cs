using System;
using System.Collections.Generic;
using ita_mobile_terminal;
using Xamarin.Forms;

namespace Zoka.Paddle.UIWrapper.Xamarin
{

	/// <summary>Class which wraps the UI - the window with Browser, running the angular UI application</summary>
	public class UIWrapperXamarin : IUIWrapper
	{
		private readonly IServiceProvider					m_ServiceProvider;
		private App											m_Application;

		private List<IPageUIEventsAware>					m_PageUIEventReceivers = new List<IPageUIEventsAware>();
		private List<IAppUIEventsAware>						m_AppUIEventReceivers = new List<IAppUIEventsAware>();

		/// <summary>Constructor</summary>
		public UIWrapperXamarin(IServiceProvider _service_provider)
		{
			m_ServiceProvider = _service_provider;
		}

		/// <summary>Returns the instance of the Xamarin.Forms.Application</summary>
		public global::Xamarin.Forms.Application			XamarinApplication => Application;

		private App											Application
		{
			get
			{
				if (m_Application != null)
					return m_Application;
				m_Application = new App(m_ServiceProvider);
				m_Application.OnAppStart += Application_OnAppStart;
				m_Application.OnAppSleep += Application_OnAppSleep;
				m_Application.OnAppResume += Application_OnAppResume;
				m_Application.OnPageAppearing += Application_OnPageAppearing;
				m_Application.OnBackButtonPressed += Application_OnBackButtonPressed;
				return m_Application;
			}
		}

		private bool Application_OnBackButtonPressed()
		{
			var res = false;
			foreach (var page_ui_event_receiver in m_PageUIEventReceivers)
			{
				res |= page_ui_event_receiver.OnBackButtonPressed();
			}
			return res;
		}

		private void Application_OnPageAppearing(global::Xamarin.Forms.ContentPage _content_page)
		{
			foreach (var page_ui_event_receiver in m_PageUIEventReceivers)
			{
				page_ui_event_receiver.OnAppearing();
			}
		}

		private void Application_OnAppResume(global::Xamarin.Forms.Application _application)
		{
			foreach (var app_ui_event_receiver in m_AppUIEventReceivers)
			{
				app_ui_event_receiver.OnResume();
			}
		}

		private void Application_OnAppSleep(global::Xamarin.Forms.Application _application)
		{
			foreach (var app_ui_event_receiver in m_AppUIEventReceivers)
			{
				app_ui_event_receiver.OnSleep();
			}
		}

		private void Application_OnAppStart(global::Xamarin.Forms.Application _application)
		{
			foreach (var app_ui_event_receiver in m_AppUIEventReceivers)
			{
				app_ui_event_receiver.OnStart();
			}
		}

		/// <summary>Will navigate the browser to the page</summary>
		public void											NavigateToUrl(string _url)
		{
			Application.NavigateTo(_url);
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
		public ESystemTheme									Theme
		{
			get { return XamarinTheme2SystemTheme(m_Application?.RequestedTheme ?? OSAppTheme.Unspecified); }
		}

		private ESystemTheme								XamarinTheme2SystemTheme(OSAppTheme _x_theme)
		{
			switch (_x_theme)
			{
				case OSAppTheme.Unspecified:
					return ESystemTheme.NotSpecified;
				case OSAppTheme.Light:
					return ESystemTheme.Light;
				case OSAppTheme.Dark:
					return ESystemTheme.Dark;
				default:
					throw new ArgumentOutOfRangeException(nameof(_x_theme), _x_theme, null);
			}
		}
	}
}
