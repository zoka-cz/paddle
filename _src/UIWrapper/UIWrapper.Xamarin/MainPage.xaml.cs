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
using Zoka.Paddle.QRScanner.Abstraction;
using Zoka.Paddle.UIWrapper.Xamarin.QRScanner;
using ZXing;
using ZXing.Net.Mobile.Forms;
using static Xamarin.Forms.Internals.GIFBitmap;
using Rectangle = System.Drawing.Rectangle;

namespace ita_mobile_terminal
{
	internal delegate void OnPageEventHandler(ContentPage _content_page);
	internal delegate bool OnBackButtonPressedHandler();

	/// <summary>Main page containing the browser window</summary>
	public partial class MainPage : ContentPage, IQRScannerUI
	{
		private readonly QRScannerUIProvider				m_QRScannerUIProvider;
		private global::ZXing.Net.Mobile.Forms.ZXingScannerView zxing = null;

		//private readonly IQRScanner							m_QRScanner;

		/// <summary>Constructor</summary>
		public MainPage(QRScannerUIProvider _qr_scanner_ui_provider)
		{
			m_QRScannerUIProvider = _qr_scanner_ui_provider;
			//m_QRScanner = _qr_scanner;
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
			m_QRScannerUIProvider.QRScannerUI = this;
			//zxing.OnScanResult += ZxingOnOnScanResult;
			RaiseOnPageAppearing(this);
			base.OnAppearing();
			//zxing.Options.UseFrontCameraIfAvailable = true;
		}

		/// <summary></summary>
		protected override void								OnDisappearing()
		{
			Debug.WriteLine("OnDisappearing raised");
			m_QRScannerUIProvider.QRScannerUI = null;
			IsScanning = false;
			base.OnDisappearing();
		}


		private string myStringProperty;
		/// <summary></summary>
		public string MyStringProperty
		{
			get { return myStringProperty; }
			set
			{
				myStringProperty = value;
				OnPropertyChanged(nameof(MyStringProperty)); // Notify that there was a change on this property
			}
		}

		private bool m_IsScanning = false;

		/// <summary></summary>
		public bool IsScanning
		{
			get { return m_IsScanning; }
			set
			{
				m_IsScanning = value;
				OnPropertyChanged(nameof(IsScanning));
			}
		}

		private void ZxingOnOnScanResult(Result _result)
		{
			if (_result != null)
			{
				MyStringProperty = _result.Text;
				Debug.WriteLine($"Scanned {_result.Text}");
				if (Guid.TryParse(_result.Text, out var scanned_guid))
				{
					if (m_QRScannerUIProvider.QRReadCallback != null)
						m_QRScannerUIProvider.QRReadCallback(scanned_guid);
				}
			}
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

		//private void ToggleCamera_OnClicked(object _sender, EventArgs _e)
		//{
		//	var rnd = new Random();

		//	if (ToggleCamera.Text == "Start")
		//	{
		//		ToggleCamera.Text = "Stop";
		//		m_QRScanner.Start(new Rectangle(rnd.Next(0, 200), rnd.Next(0, 200), rnd.Next(200, 300), rnd.Next(400, 500)), false);
		//	}
		//	else
		//	{
		//		ToggleCamera.Text = "Start";
		//		m_QRScanner.Stop();
		//	}

		//	//if (ToggleCamera.Text == "Start")
		//	//{
		//	//	Device.BeginInvokeOnMainThread(() =>
		//	//	{
		//	//		IsScanning = true;
		//	//		CameraLayout.IsVisible = true;
		//	//		ToggleCamera.Text = "Stop";
		//	//	});
		//	//}
		//	//else
		//	//{
		//	//	Device.BeginInvokeOnMainThread(() =>
		//	//	{
		//	//		ToggleCamera.Text = "Start";
		//	//		CameraLayout.IsVisible = false;
		//	//		IsScanning = false;
		//	//	});
		//	//}
		//}

		#region IQRScannerUI implementation

		/// <inheritdoc />
		public void StartScanning(Rectangle _position, bool _use_front_camera)
		{
			var rect = new Xamarin.Forms.Rectangle(_position.X, _position.Y, _position.Width, _position.Height);
			Device.BeginInvokeOnMainThread(() =>
			{
				AbsoluteLayout.SetLayoutBounds(CameraLayout, rect);
				AbsoluteLayout.SetLayoutFlags(CameraLayout, AbsoluteLayoutFlags.None);
				if (zxing == null)
				{
					zxing = new ZXingScannerView();
					zxing.VerticalOptions = LayoutOptions.FillAndExpand;
					zxing.OnScanResult += this.ZxingOnOnScanResult;
					zxing.BindingContext = this;
					zxing.SetBinding(ZXingScannerView.IsScanningProperty, nameof(IsScanning));
					CameraLayout.Children.Add(zxing);
				}
				zxing.Options.UseFrontCameraIfAvailable = _use_front_camera;
				CameraLayout.IsVisible = true;
				IsScanning = true;
			});
		}

		/// <inheritdoc />
		public void StopScanning()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				CameraLayout.IsVisible = false;
				IsScanning = false;
			});
		}

		/// <summary>Will move the scanner to new position</summary>
		public void MoveScannerTo(System.Drawing.Point _position)
		{
			if (!IsScanning)
				return;

			Device.BeginInvokeOnMainThread(() =>
			{
				var rect = AbsoluteLayout.GetLayoutBounds(CameraLayout);
				rect.X = _position.X;
				rect.Y = _position.Y;
				AbsoluteLayout.SetLayoutBounds(CameraLayout, rect);
			});
		}


		#endregion // IQRScannerUI implementation

		//private void MoveCamera_OnClicked(object _sender, EventArgs _e)
		//{
		//	m_QRScanner.MovePreviewTo(new System.Drawing.Point(0, 0));
		//}
	}
}
