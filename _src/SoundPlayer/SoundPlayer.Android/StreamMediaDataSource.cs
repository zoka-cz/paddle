using System;
using Android.Media;

namespace Zoka.Paddle.SoundPlayer.Android
{

	internal class StreamMediaDataSource : MediaDataSource
	{
		private byte[] _data;

		public StreamMediaDataSource(byte[] data)
		{
			_data = data;
		}

		public override long Size
			=> _data.Length;

		public override void Close()
		{
			_data = null;
		}

		public override int ReadAt(long position, byte[] buffer, int offset, int size)
		{

			if (position >= _data.Length)
			{
				return -1;
			}

			if (position + size > _data.Length)
			{
				size -= (Convert.ToInt32(position) + size) - _data.Length;
			}
			Array.Copy(_data, position, buffer, offset, size);
			return size;
		}
	}
}
