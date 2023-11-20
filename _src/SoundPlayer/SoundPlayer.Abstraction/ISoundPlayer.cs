namespace Zoka.Paddle.SoundPlayer.Abstraction
{
	/// <summary>Interface for playing the sounds</summary>
	public interface ISoundPlayer
	{
		/// <summary>Will sets or gets the actual volume (0-100%)</summary>
		int? Volume { get; set; }

		/// <summary>Will play sound from the passed filename</summary>
		void PlaySound(string _filename);
		/// <summary>Will play sound from the passed byte array</summary>
		void PlaySound(byte[] _file_bytes);
	}
}
