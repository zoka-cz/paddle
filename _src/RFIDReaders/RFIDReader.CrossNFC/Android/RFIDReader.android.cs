using System;
using Android.App;
using Android.Content;
using Plugin.NFC;
using Zoka.Paddle.RFIDReader.Abstraction;

namespace iTATerminal.Helpers.NFCManager.CrossNFC
{
	/// <summary></summary>
	public partial class CrossNFCRFIDReader : IRFIDReader
	{
		/// <summary></summary>
		public CrossNFCRFIDReader(Activity _activity, out Action _on_activity_resume_callback, out Action<Intent> _on_activity_new_intent_callback)
		{
			Plugin.NFC.CrossNFC.Init(_activity);
			_on_activity_resume_callback = OnActivityResume;
			_on_activity_new_intent_callback = OnActivityNewIntent;
		}

		/// <summary>Should be called from MainActivity in OnResume</summary>
		public void	OnActivityResume()
		{
			Plugin.NFC.CrossNFC.OnResume();
		}

		/// <summary>Should be called from MainActivity in OnNewIntent</summary>
		public void OnActivityNewIntent(Intent _intent)
		{
			Plugin.NFC.CrossNFC.OnNewIntent(_intent);
		}

	}
}