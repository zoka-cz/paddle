using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.Content.Res;
using Zoka.Paddle.FileProviders.Abstraction;

namespace Zoka.Paddle.FileProviders.Android
{
	/// <inheritdoc />
	public class AndroidAssetAssetsFilesProvider : IAssetsFilesProvider
	{
		private readonly AssetManager m_AssetsManager;
		private readonly List<string> m_SearchInDirectories;

		/// <summary>Constructor</summary>
		public AndroidAssetAssetsFilesProvider(AssetManager _assets_manager, IEnumerable<string> _search_in_directories)
		{
			m_AssetsManager = _assets_manager;
			m_SearchInDirectories = new List<string>(_search_in_directories);
		}

		public async Task<AssetFileInfo>					GetFileInfoByPathAsync(Uri _path)
		{
			foreach (var directory in m_SearchInDirectories)
			{
				var fn = Path.Join(directory, _path.AbsolutePath);
				try
				{
					var stream = m_AssetsManager.Open(fn, Access.Random);
					if (stream != null)
					{
						var ms = new MemoryStream();
						await stream.CopyToAsync(ms);
						ms.Position = 0;
						return new AssetFileInfo(ms, (ulong)ms.Length);
					}
				}
				catch (Exception)
				{

				}
			}
			return null;
		}
	}
}
