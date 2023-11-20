namespace Zoka.Paddle.SoundPlayer.Abstraction
{
	/// <summary>Interface for playing the sounds</summary>
	public class NoSoundPlayer : ISoundPlayer
	{
		/// <inheritdoc />
		public int? Volume { get; set; }
		/// <inheritdoc />
		public void PlaySound(string _filename)
		{
			
		}

		/// <inheritdoc />
		public void PlaySound(byte[] _file_bytes)
		{
		}
	}
}
