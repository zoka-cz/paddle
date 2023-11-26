using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Xamarin.Forms.Internals.GIFBitmap;
using Rectangle = System.Drawing.Rectangle;

namespace ita_mobile_terminal
{
	internal delegate void OnPageEventHandler(ContentPage _content_page);
	internal delegate bool OnBackButtonPressedHandler();

	/// <summary>Main page containing the browser window</summary>
	public partial class MainPage : ContentPage
	{
		/// <summary>Constructor</summary>
		public MainPage()
		{
			InitializeComponent();
			BindingContext = this;
			Browser.Navigated += Browser_Navigated;
		}

		/// <inheritdoc />
		protected override bool OnBackButtonPressed()
		{
			return RaiseOnBackButtonPressed();
		}

		private void Browser_Navigated(object sender, WebNavigatedEventArgs e)
		{
			
		}

		internal void										NavigateTo(string _url)
		{
			Browser.Source = new UrlWebViewSource() { Url = _url };
		}

		internal void										Refresh()
		{
			Browser.Reload();
		}

		/// <summary>Called when page is about to be displayed</summary>
		protected override void								OnAppearing()
		{
			Debug.WriteLine("OnAppearing raised");
			RaiseOnPageAppearing(this);
			base.OnAppearing();
		}

		/// <summary></summary>
		protected override void								OnDisappearing()
		{
			Debug.WriteLine("OnDisappearing raised");
			base.OnDisappearing();
		}


		internal event OnPageEventHandler					OnPageAppearing;
		private void										RaiseOnPageAppearing(ContentPage __content_page)
		{
			OnPageAppearing?.Invoke(__content_page);
		}

		internal event OnBackButtonPressedHandler			OnBackButtonPressedEvent;
		private bool										RaiseOnBackButtonPressed()
		{
			return OnBackButtonPressedEvent?.Invoke() ?? false;
		}
	}
}
