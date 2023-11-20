using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Zoka.Paddle.UIWrapper.Xamarin.QRScanner
{
	/// <summary>The interface to provide UI for QR scanning support</summary>
	public interface IQRScannerUI
	{
		/// <summary>Will start scanning</summary>
		void StartScanning(Rectangle _position, bool _use_front_camera);
		/// <summary>Will stop scanning</summary>
		void StopScanning();
		/// <summary>Will move the scanner to new position</summary>
		void MoveScannerTo(Point _position);
		/// <summary>Returns whether we are scanning currently</summary>
		bool IsScanning { get; }
	}
}
