using System;
using System.Threading.Tasks;

namespace Zoka.Paddle.QRScanner.Abstraction
{

	/// <summary>The RFID has been scanned</summary>
	public delegate void QRCodeReadHandler(Guid _scanned_guid);


	/// <summary>Interface for QR code scanning feature</summary>
	public interface IQRScanner
	{
		/// <summary>Returns the status of the camera permission (not known, granted, denied, ...)</summary>
		Task<EPermissionState> GetCameraPermissionState();
		/// <summary>Returns, whether the QR code scanning is available (camera present, permission granted, ...)</summary>
		Task<bool> GetIsAvailable();
		/// <summary>Returns whether the scanning is running at current moment</summary>
		Task<bool> GetIsRunning();
		/// <summary>Will start scanning of the QR codes</summary>
		/// <remarks>
		/// Will open the window with camera preview overlay on the specified position at specified size
		/// and starts processing of images to find out any QR code
		/// </remarks>
		Task Start(System.Drawing.Rectangle _rectangle, bool _front_camera);
		/// <summary>Will stop the scanning of the QR codes</summary>
		Task Stop();
		/// <summary>Will move camera preview overlay to the specified position</summary>
		Task MovePreviewTo(System.Drawing.Point _position);

		/// <summary>Raised when the QR code is read</summary>
		event QRCodeReadHandler QRCodeRead;
	}
}
