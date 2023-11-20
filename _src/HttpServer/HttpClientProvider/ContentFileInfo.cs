using System;
using System.Collections.Generic;
using System.Text;

namespace Zoka.Paddle.HttpClientProvider
{
	/// <summary>Information about file to be added as content into HTTP request/response</summary>
	public class ContentFileInfo
	{
		/// <summary>Name of the file</summary>
		public string										Filename { get; set; }
		/// <summary>Mime type</summary>
		public string										MimeType { get; set; }
		/// <summary>Content of the file</summary>
		public byte[]										FileBytes { get; set; }
	}
}
