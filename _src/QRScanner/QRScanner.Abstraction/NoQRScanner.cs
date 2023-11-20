using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Zoka.Paddle.QRScanner.Abstraction
{
	/// <summary>Implementation of the IQRScanner, for no QR scanner available</summary>
	public class NoQRScanner : IQRScanner
	{
		/// <summary>Returns the status of the camera permission (not known, granted, denied, ...)</summary>
		public Task<EPermissionState>						GetCameraPermissionState() => Task.FromResult(EPermissionState.Unknown);

		/// <inheritdoc />
		public Task<bool>									GetIsAvailable() => Task.FromResult(false);
		
		/// <inheritdoc />
		public Task<bool>									GetIsRunning() => Task.FromResult(false);

		/// <inheritdoc />
		public Task											Start(Rectangle _rectangle, bool _fornt_camera)
		{
			throw new InvalidOperationException("No QR scanning available");
		}

		/// <inheritdoc />
		public Task											Stop()
		{
			throw new InvalidOperationException("No QR scanning available");
		}

		/// <inheritdoc />
		public Task											MovePreviewTo(Point _position)
		{
			throw new InvalidOperationException("No QR scanning available");
		}

		/// <inheritdoc />
		public event QRCodeReadHandler						QRCodeRead;
		private void										RaiseQRCodeRead(Guid _scanned_guid)
		{
			QRCodeRead?.Invoke(_scanned_guid);
		}

	}
}
