using System;
using Android.Media;
using Zoka.Paddle.SoundPlayer.Abstraction;
using Zoka.Paddle.SoundPlayer.Android;

namespace SoundPlayer.Android
{
	/// <summary>Implementation of ISoundPlayer which uses MediaPlayer to play sounds</summary>
	public class MediaSoundPlayer : ISoundPlayer
    {
		private MediaPlayer									m_Player = null;

		/// <inheritdoc />
		public int?											Volume { get; set; }

		/// <summary></summary>
		public void											PlaySound(string _filename)
	    {
			if (m_Player != null)
				throw new InvalidOperationException("Already playing");
			m_Player = new MediaPlayer();
			m_Player.SetDataSource(_filename);
			
			m_Player.Prepare();
			m_Player.Completion += Mp_Completion;
			m_Player.Error += M_Player_Error;
			m_Player.Start();
	    }

		private void										M_Player_Error(object sender, MediaPlayer.ErrorEventArgs e)
		{
			m_Player.Dispose();
			m_Player = null;
		}

		/// <summary></summary>
		public void											PlaySound(byte[] _file_bytes)
	    {
			if (m_Player != null)
				throw new InvalidOperationException("Already playing");
		    m_Player = new MediaPlayer();
			m_Player.SetDataSource(new StreamMediaDataSource(_file_bytes));
			m_Player.Prepare();
			m_Player.Completion += Mp_Completion;
			m_Player.Error += M_Player_Error;
			m_Player.Start();
	    }

		private void										Mp_Completion(object sender, EventArgs e)
		{
			m_Player.Dispose();
			m_Player = null;
		}
	}
}
