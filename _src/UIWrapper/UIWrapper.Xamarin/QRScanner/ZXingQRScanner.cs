using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Zoka.Paddle.QRScanner.Abstraction;

namespace Zoka.Paddle.UIWrapper.Xamarin.QRScanner
{
    /// <summary>
    /// 
    /// </summary>
    public class ZXingQRScanner : IQRScanner
    {
	    private readonly QRScannerUIProvider				m_QRScannerUIProvider;

		/// <summary>Constructor</summary>
		public ZXingQRScanner(QRScannerUIProvider _qr_scanner_ui_provider)
	    {
		    m_QRScannerUIProvider = _qr_scanner_ui_provider;
			m_QRScannerUIProvider.QRReadCallback += QRReadCallback;
	    }

		private void QRReadCallback(Guid _scanned_guid)
		{
			RaiseQRCodeRead(_scanned_guid);
		}

		/// <summary>Returns the status of the camera permission (not known, granted, denied, ...)</summary>
		public async Task<EPermissionState>					GetCameraPermissionState()
		{
			var state = await Permissions.CheckStatusAsync<Permissions.Camera>();
			var should_show = Permissions.ShouldShowRationale<Permissions.Camera>();
			switch (state)
			{
				case PermissionStatus.Unknown:
					return EPermissionState.Unknown;
				case PermissionStatus.Denied:
					if (should_show == true)
						return EPermissionState.Denied;
					else
						return EPermissionState.Unknown; // probably, the user has selected "this time only", so we should ask again
				case PermissionStatus.Disabled:
					return EPermissionState.Disabled;
				case PermissionStatus.Granted:
					return EPermissionState.Granted;
				case PermissionStatus.Restricted:
					return EPermissionState.Restricted;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <inheritdoc />
		public Task<bool>									GetIsAvailable() => Task.FromResult(true);
		/// <inheritdoc />
		public Task<bool>									GetIsRunning() => Task.FromResult(m_QRScannerUIProvider.QRScannerUI?.IsScanning ?? false);
		
        /// <inheritdoc />
        public Task Start(Rectangle _rectangle, bool _front_camera)
        {
            m_QRScannerUIProvider.QRScannerUI?.StartScanning(_rectangle, _front_camera);
			return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task Stop()
        {
            m_QRScannerUIProvider.QRScannerUI?.StopScanning();
			return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task MovePreviewTo(Point _position)
        {
			m_QRScannerUIProvider.QRScannerUI?.MoveScannerTo(_position);
			return Task.CompletedTask;
        }

		/// <inheritdoc />
		public event QRCodeReadHandler						QRCodeRead;
		private void										RaiseQRCodeRead(Guid _scanned_guid)
		{
			QRCodeRead?.Invoke(_scanned_guid);
		}
    }
}
