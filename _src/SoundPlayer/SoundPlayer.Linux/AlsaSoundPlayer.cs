//using System;
//using System.IO;
//using Alsa.Net;
//using Microsoft.Extensions.Logging;
//using Zoka.Paddle.SoundPlayer.Abstraction;

//namespace Zoka.Paddle.SoundPlayer.Linux
//{
//	/// <summary>Implementation of Sound player using Alsa.NET library (linux implementation)</summary>
//	public class AlsaSoundPlayer : ISoundPlayer
//	{
//		private readonly ILogger<AlsaSoundPlayer>			m_Logger;
//		private ISoundDevice								m_SoundDevice;
//		private (long, long)?								m_VolumeRange = null;

//		/// <summary>Constructor</summary>
//		public AlsaSoundPlayer(ILogger<AlsaSoundPlayer> _logger)
//		{
//			m_Logger = _logger;
//			var sd = new SoundDeviceSettings();
//			m_SoundDevice = AlsaDeviceBuilder.Create(sd);
//			try
//			{
//				m_VolumeRange = m_SoundDevice.PlaybackVolumeRange;
//				m_Logger.LogTrace($"Volume range: {m_VolumeRange.Value.Item1}, {m_VolumeRange.Value.Item2}");
//			}
//			catch (Exception ex)
//			{
//				m_Logger.LogError(ex.ToStringAllExceptionDetails());
//			}
//		}

//		/// <inheritdoc />
//		public int?											Volume 
//		{ 
//			get
//			{
//				try
//				{
//					var one_per = (float)(m_VolumeRange.Value.Item2 - m_VolumeRange.Value.Item1) / 100;
//					return (int)Math.Round((m_SoundDevice.PlaybackVolume - m_VolumeRange.Value.Item1) / one_per);
//				}
//				catch (Exception ex)
//				{
//					m_Logger.LogError($"Error getting volume. Error: {ex.ToStringAllExceptionDetails()}");
//					return null;
//				}
//			}
//			set
//			{
//				if (value == null || m_VolumeRange == null)
//					return;
//				try
//				{
//					var one_per = (float)(m_VolumeRange.Value.Item2 - m_VolumeRange.Value.Item1) / 100;
//					var val = (int)Math.Round(one_per * value.Value + m_VolumeRange.Value.Item1);
//					m_SoundDevice.PlaybackVolume = val;
//				}
//				catch (Exception ex)
//				{
//					m_Logger.LogError($"Error settings volume. Error: {ex.ToStringAllExceptionDetails()}");
//				}
//			}
//		}

//		/// <inheritdoc />
//		public void											PlaySound(string _filename)
//		{
//			m_SoundDevice.Play(_filename);
//		}

//		private MemoryStream ms = null;

//		/// <inheritdoc />
//		public void											PlaySound(byte[] _file_bytes)
//		{
//			m_Logger.LogTrace($"Volume: {Volume}");
//			ms = new MemoryStream(_file_bytes);
//			m_SoundDevice.Play(ms);
//		}
//	}
//}
