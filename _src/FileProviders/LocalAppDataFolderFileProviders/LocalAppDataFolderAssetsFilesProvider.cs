using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Zoka.Paddle.FileProviders.Abstraction;
using Zoka.Paddle.PlatformConfiguration.Abstraction;

namespace Zoka.Paddle.FileProviders.LocalAppDataFolderFileProviders
{
	/// <summary>This file provider takes and serves files from the LocalAppData folder</summary>
	public class LocalAppDataFolderAssetsFilesProvider : FileProviders.Abstraction.IAssetsFilesProvider
	{
		private readonly IPlatformConfiguration				m_PlatformConfiguration;
		private readonly List<string>						m_SearchInDirectories;
		
		
		/// <summary>Constructor</summary>
		public LocalAppDataFolderAssetsFilesProvider(IPlatformConfiguration _platform_configuration, IEnumerable<string> _search_in_directories)
		{
			m_PlatformConfiguration = _platform_configuration;
			m_SearchInDirectories = new List<string>(_search_in_directories);
		}
		
		
		/// <inheritdoc/>
		public Task<AssetFileInfo>							GetFileInfoByPathAsync(Uri _path)
		{
			var dest_dir = m_PlatformConfiguration.LocalAppFolder;
			
			foreach (var directory in m_SearchInDirectories)
			{
				var fn = Path.Combine(dest_dir, directory, _path.AbsolutePath.TrimCharacters("/"));

				var fi = new FileInfo(fn);
				if (fi.Exists)
				{
					return Task.FromResult(new AssetFileInfo(fi.OpenRead(), (ulong)fi.Length));
				}
			}
			
			return Task.FromResult<AssetFileInfo>(null);
		}
	}
}