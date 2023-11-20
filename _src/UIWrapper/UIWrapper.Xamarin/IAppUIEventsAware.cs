namespace Zoka.Paddle.UIWrapper.Xamarin
{
	/// <summary>Interface for receiving events regarding Xamarin Application</summary>
	public interface IAppUIEventsAware
	{
		/// <summary>Called when application is started</summary>
		void OnStart();

		/// <summary>Called when application is going to sleep</summary>
		void OnSleep();

		/// <summary>Called when application is resumed</summary>
		void OnResume();
	}
}
