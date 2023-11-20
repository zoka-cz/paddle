using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zoka.Paddle.QRScanner.Abstraction;
using Zoka.Paddle.UIWrapper.Xamarin;
using Zoka.Paddle.UIWrapper.Xamarin.QRScanner;

namespace ita_mobile_terminal
{
	internal delegate void OnAppEventHandler(Application _application);

	/// <summary>Xamarin application</summary>
	public partial class App : Application
	{
		private readonly MainPage							m_MainPage;

		/// <summary>Constructor</summary>
		public App(IServiceProvider _service_provider)
		{
			InitializeComponent();
			m_MainPage = new MainPage(_service_provider.GetRequiredService<QRScannerUIProvider>()/*, _service_provider.GetRequiredService<IQRScanner>()*/);
			m_MainPage.OnPageAppearing += MainPage_OnPageAppearing;
			m_MainPage.OnBackButtonPressedEvent += MainPage_OnBackButtonPressedEvent;
			MainPage = m_MainPage;
		}

		internal void										NavigateTo(string _url)
		{
			m_MainPage.NavigateTo(_url);
		}

		internal void										Refresh()
		{
			m_MainPage.Refresh();
		}


		/// <inheritdoc />
		protected override void OnStart()
		{
			Debug.WriteLine("OnStart raised");
			RaiseOnAppStart(this);
		}

		/// <inheritdoc />
		protected override void OnSleep()
		{
			Debug.WriteLine("OnSleep raised");
			RaiseOnAppSleep(this);
		}

		/// <inheritdoc />
		protected override void OnResume()
		{
			Debug.WriteLine("OnResume raised");
			RaiseOnAppResume(this);
		}

		private void MainPage_OnPageAppearing(ContentPage _content_page)
		{
			RaiseOnPageAppearing(_content_page);
		}

		private bool MainPage_OnBackButtonPressedEvent()
		{
			return RaiseOnBackButtonPressed();
		}


		/// <summary></summary>
		internal event OnAppEventHandler					OnAppStart;
		private void										RaiseOnAppStart(Application __application)
		{
			OnAppStart?.Invoke(__application);
		}

		internal event OnAppEventHandler					OnAppSleep;
		private void										RaiseOnAppSleep(Application __application)
		{
			OnAppSleep?.Invoke(__application);
		}

		internal event OnAppEventHandler					OnAppResume;

		private void										RaiseOnAppResume(Application __application)
		{
			OnAppResume?.Invoke(__application);
		}

		internal event OnPageEventHandler					OnPageAppearing;
		private void										RaiseOnPageAppearing(ContentPage __content_page)
		{
			OnPageAppearing?.Invoke(__content_page);
		}

		internal event OnBackButtonPressedHandler			OnBackButtonPressed;
		private bool										RaiseOnBackButtonPressed()
		{
			return OnBackButtonPressed?.Invoke() ?? false;
		}
	}
}
