using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zoka.Paddle.FileProviders.Abstraction
{
	/// <summary>Basic info about asset file</summary>
	public class AssetFileInfo
	{
		/// <summary>Constructor</summary>
		public AssetFileInfo(Stream _file_stream, ulong? _file_length)
		{
			FileStream = _file_stream;
			FileLength = _file_length;
		}

		/// <summary>The stream of the file data</summary>
		public Stream FileStream { get; }
		/// <summary>The length of the file. If the value is null, than the FileLength was not known</summary>
		public ulong? FileLength { get; }
	}
}
