namespace Zoka.Paddle.QRScanner.Abstraction
{
	/// <summary>The permission status (taken from Xamarin)</summary>
	public enum EPermissionState
	{
		/// <summary>The permission hasn't been granted or requested and is in an unknown state.</summary>
		Unknown,
		/// <summary>The user has denied the permission.</summary>
		Denied,
		/// <summary>The permission is disabled for the app.</summary>
		Disabled,
		/// <summary>The user has granted permission.</summary>
		Granted,
		/// <summary>The permission is in a restricted state.</summary>
		Restricted,

	}
}
