using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Zoka.Paddle.FileProviders.Abstraction;

namespace FileProviders.UWP
{
	/// <inheritdoc />
	public class UwpStorageAssetsFilesProvider : IAssetsFilesProvider
	{
		private readonly List<string> m_SearchInDirectories;

		/// <summary>Constructor</summary>
		public UwpStorageAssetsFilesProvider(IEnumerable<string> _search_in_directories)
		{
			m_SearchInDirectories = new List<string>(_search_in_directories);
		}


		/// <inheritdoc />
		public async Task<AssetFileInfo> GetFileInfoByPathAsync(Uri _path)
		{
			Windows.Storage.StorageFolder assets_folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");

			foreach (var directory in m_SearchInDirectories)
			{
				StorageFolder curr_dir;
				try
				{
					curr_dir = await assets_folder.GetFolderAsync(directory);
				}
				catch (Exception)
				{
					continue;
				}

				for (int seg_idx = 1; seg_idx < _path.Segments.Length - 1; seg_idx++)
				{
					try
					{
						curr_dir = await curr_dir.GetFolderAsync(_path.Segments[seg_idx].TrimCharacters('/'.Yield()));
					}
					catch (Exception)
					{
						break;
					}
				}

				try
				{
					var file = await curr_dir.GetFileAsync(_path.Segments.Last());
					var stream = await file.OpenStreamForReadAsync();
					return new AssetFileInfo(stream, (ulong)stream.Length);
				}
				catch (Exception)
				{
					continue;
				}
			}

			return null;
		}
	}
}
