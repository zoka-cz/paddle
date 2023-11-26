namespace Zoka.Paddle.UIWrapper.Xamarin
{
	/// <summary>Interface for receiving events regarding Page UI</summary>
	public interface IPageUIEventsAware
	{
		/// <summary>Called before the page is about to be appearing</summary>
		void OnAppearing();

		/// <summary>Called when the back button has been pressed in UI.</summary>
		/// <returns>True, when the back button has been processed and the further processing should be prevented. False otherwise</returns>
		bool OnBackButtonPressed();
	}
}
