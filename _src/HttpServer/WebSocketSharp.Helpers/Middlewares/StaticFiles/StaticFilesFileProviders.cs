using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zoka.Paddle.FileProviders.Abstraction;

namespace Zoka.Paddle.WebSocketSharp.Helpers.Middlewares.StaticFiles
{
	/// <summary></summary>
	public class StaticFilesFileProviders
	{
		private readonly List<IAssetsFilesProvider>				m_FileProviders;

		/// <summary></summary>
		public StaticFilesFileProviders(IEnumerable<IAssetsFilesProvider> _file_providers)
		{
			m_FileProviders = new List<IAssetsFilesProvider>(_file_providers);
		}

		/// <summary></summary>
		public IEnumerable<IAssetsFilesProvider>					FileProviders => m_FileProviders;

		/// <summary>Will search all the file providers and returns the file info if available</summary>
		public async Task<AssetFileInfo>					GetFileInfoByPathAsync(Uri _path)
		{
			foreach (var files_provider in m_FileProviders)
			{
				var file_info = await files_provider.GetFileInfoByPathAsync(_path);
				if (file_info != null)
				{
					return file_info;
				}
			}

			return null;
		}
	}
}
