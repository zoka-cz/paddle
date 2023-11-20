using Microsoft.Extensions.Logging;
using NAudio.Wave;
using Zoka.Paddle.SoundPlayer.Abstraction;

namespace Zoka.Paddle.SoundPlayer.Windows
{
	/// <summary>Implementation of ISoundPlayer using NAudio</summary>
	public class NAudioSoundPlayer : ISoundPlayer
	{
		private readonly ILogger<NAudioSoundPlayer>			m_Logger;
		private int											m_Volume = -1;

		/// <summary>Constructor</summary>
		public NAudioSoundPlayer(ILogger<NAudioSoundPlayer> _logger)
		{
			var x = Volume;
			m_Logger = _logger;
		}

		/// <inheritdoc />
		public int? Volume
		{
			get
			{
				if (m_Volume == -1)
				{
					using (var waveOut = new WaveOutEvent())
						m_Volume = (int)Math.Round(waveOut.Volume * 100);
				}

				if (m_Volume < 0)
					m_Volume = 0;
				if (m_Volume > 100)
					m_Volume = 100;

				return m_Volume;
			}
			set
			{
				if (value == null)
					return;
				if (value < 0)
					m_Volume = 0;
				else if (value > 100)
					m_Volume = 100;
				else
					m_Volume = value.Value;
			}
		}

		/// <inheritdoc />
		public void PlaySound(string _filename)
		{
			using (var waveOut = new WaveOutEvent())
			using (var wavReader = new WaveFileReader(_filename))
			{
				waveOut.Init(wavReader);
				waveOut.Volume = (float)m_Volume / 100;
				waveOut.Play();
			}
		}

		/// <inheritdoc />
		public void PlaySound(byte[] _file_bytes)
		{
			using (var waveOut = new WaveOutEvent())
			using (var ms = new MemoryStream(_file_bytes))
			using (var wavReader = new WaveFileReader(ms))
			{
				waveOut.Init(wavReader);
				waveOut.Volume = (float)m_Volume / 100;
				waveOut.Play();
				while (waveOut.PlaybackState == PlaybackState.Playing)
				{
					Thread.Sleep(100);
				}
			}
		}
	}
}