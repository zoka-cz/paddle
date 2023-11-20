namespace Zoka.Paddle.RFIDReader.Abstraction
{
	/// <summary>The RFID has been scanned</summary>
	public delegate void RFIDReadHandler(long _rfid);
	
	/// <summary>Interface which specifies the basic operations to work with NFC</summary>
	public interface IRFIDReader
	{
		/// <summary>Will initialize the NFCManager</summary>
		void Initialize();

		/// <summary>Will start listening for RFID</summary>
		void StartListening();

		/// <summary>Will stop listening for RFID</summary>
		void StopListening();

		/// <summary>Called when the RFID tag is read by the manager</summary>
		event RFIDReadHandler OnRFIDRead;

	}
}
