using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.UIWrapper.Xamarin.QRScanner
{
	/// <summary>Class which provides the IQRScannerUI when available</summary>
	public class QRScannerUIProvider
	{
		/// <summary>Provides current instance of the IQRScannerUI or null, if it is not currently available</summary>
		public IQRScannerUI									QRScannerUI { get; internal set; }
		/// <summary>Gets/sets the callback to be called when the QR code is scanned</summary>
		public Action<Guid>									QRReadCallback { get; set; }
	}
}
