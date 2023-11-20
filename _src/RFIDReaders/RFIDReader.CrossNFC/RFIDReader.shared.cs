using System;
using System.Diagnostics;
using System.Linq;
using Plugin.NFC;
using Zoka.Paddle.RFIDReader.Abstraction;
using Zoka.Paddle.UIWrapper.Xamarin;

namespace iTATerminal.Helpers.NFCManager.CrossNFC
{

	/// <summary></summary>
	public partial class CrossNFCRFIDReader : IRFIDReader
	{
		/// <summary></summary>
		public void											Initialize()
		{
			SubscribeEvents();
		}

		/// <summary>Will start listening for RFID</summary>
		public void											StartListening()
		{
			try
			{
				Plugin.NFC.CrossNFC.Current.StartListening();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error starting RFID listening.");
				Debug.WriteLine(ex.ToStringAllExceptionDetails());
			}
		}

		/// <summary>Will stop listening for RFID</summary>
		public void											StopListening()
		{
			Plugin.NFC.CrossNFC.Current.StopListening();
		}


		#region GUI Callbacks


		#endregion // GUI Callbacks

		#region Events

		/// <summary>Called when the RFID tag is read by the manager</summary>
		public event RFIDReadHandler						OnRFIDRead;
		private void										RaiseOnRFIDRead(long _rfid)
		{
			var tmp = OnRFIDRead;
			if (tmp != null)
				tmp(_rfid);
		}

		#endregion // Events

		#region CrosNFC events

		private void										SubscribeEvents()
		{
			// Event raised when a ndef message is received.
			Plugin.NFC.CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
			// Event raised when a ndef message has been published.
			Plugin.NFC.CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
			// Event raised when a tag is discovered. Used for publishing.
			Plugin.NFC.CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;
			// Event raised when NFC listener status changed
			Plugin.NFC.CrossNFC.Current.OnTagListeningStatusChanged += Current_OnTagListeningStatusChanged;

			// Android Only:
			// Event raised when NFC state has changed.
			Plugin.NFC.CrossNFC.Current.OnNfcStatusChanged += Current_OnNfcStatusChanged;

			// iOS Only: 
			// Event raised when a user cancelled NFC session.
			Plugin.NFC.CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;
		}

		private void										UnsubscribeEvents()
		{
			// Event raised when a ndef message is received.
			Plugin.NFC.CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
			// Event raised when a ndef message has been published.
			Plugin.NFC.CrossNFC.Current.OnMessagePublished -= Current_OnMessagePublished;
			// Event raised when a tag is discovered. Used for publishing.
			Plugin.NFC.CrossNFC.Current.OnTagDiscovered -= Current_OnTagDiscovered;
			// Event raised when NFC listener status changed
			Plugin.NFC.CrossNFC.Current.OnTagListeningStatusChanged -= Current_OnTagListeningStatusChanged;

			// Android Only:
			// Event raised when NFC state has changed.
			Plugin.NFC.CrossNFC.Current.OnNfcStatusChanged -= Current_OnNfcStatusChanged;

			// iOS Only: 
			// Event raised when a user cancelled NFC session.
			Plugin.NFC.CrossNFC.Current.OniOSReadingSessionCancelled -= Current_OniOSReadingSessionCancelled;
		}

		private void Current_OniOSReadingSessionCancelled(object _sender, EventArgs _e)
		{
			Debug.WriteLine($"NFCCallback: Current_OniOSReadingSessionCancelled");
		}

		private void Current_OnNfcStatusChanged(bool _isenabled)
		{
			Debug.WriteLine($"NFCCallback: Current_OnNfcStatusChanged");
		}

		private void Current_OnTagListeningStatusChanged(bool _islistening)
		{
			Debug.WriteLine($"NFCCallback: Current_OnTagListeningStatusChanged IsListening:{_islistening}");
		}

		private void Current_OnTagDiscovered(ITagInfo _taginfo, bool _format)
		{
			Debug.WriteLine($"NFCCallback: Current_OnTagDiscovered");
		}

		private void Current_OnMessagePublished(ITagInfo _taginfo)
		{
			Debug.WriteLine($"NFCCallback: Current_OnMessagePublished");
		}

		private void Current_OnMessageReceived(ITagInfo _taginfo)
		{
			byte[] id_bytes = new byte[8];
			for (int i = 0; i < id_bytes.Length; i++)
			{
				id_bytes[i] = 0;
				if (i < _taginfo.Identifier.Length)
					id_bytes[i] = _taginfo.Identifier[_taginfo.Identifier.Length - i - 1];
			}
			var rfid = BitConverter.ToInt64(id_bytes, 0);
			Debug.WriteLine($"NFCCallback: Current_OnMessageReceived ({_taginfo.SerialNumber} | {rfid})");
			RaiseOnRFIDRead(rfid);
		}

		#endregion // CrosNFC events

		///// <inheritdoc />
		//public void OnStart()
		//{
			
		//}

		///// <inheritdoc />
		//public void OnSleep()
		//{
			
		//}

		///// <inheritdoc />
		//public void OnResume()
		//{
			
		//}
	}
}
