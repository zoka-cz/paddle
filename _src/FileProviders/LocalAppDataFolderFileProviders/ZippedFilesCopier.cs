using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Zoka.Paddle.PlatformConfiguration.Abstraction;

namespace Zoka.Paddle.FileProviders.LocalAppDataFolderFileProviders
{
	/// <summary>Provides functitonality to extract zip files into the AppDataFolder</summary>
	public static class ZippedFilesCopier
	{
		/// <summary>Will extract the zip file in the passed stream into the LocalApplicationData directory</summary>
		public static void									ExtractZipIntoAppDataFolder(Stream _zip_file_stream, IPlatformConfiguration _platform_configuration)
		{
			var target_dir = new DirectoryInfo(_platform_configuration.LocalAppFolder);
			target_dir.Create();

			ExtractZip(_zip_file_stream, target_dir);

			//var fst_zip = new FastZip();
			//fst_zip.ExtractZip(_zip_file_stream, target_dir.FullName, FastZip.Overwrite.Always, null, null, null, true, true);
		}

		private static void									ExtractZip(Stream _zip_file_stream, DirectoryInfo _target_info)
		{
			using (var zf = new ZipFile(_zip_file_stream))
			{
				foreach (ZipEntry zipEntry in zf)
				{
					if (!zipEntry.IsFile)
					{
						// Ignore directories
						continue;
					}
					String entryFileName = zipEntry.Name;
					// to remove the folder from the entry:
					//entryFileName = Path.GetFileName(entryFileName);
					// Optionally match entrynames against a selection list here
					// to skip as desired.
					// The unpacked length is available in the zipEntry.Size property.

					// Manipulate the output filename here as desired.
					var fullZipToPath = Path.Combine(_target_info.FullName, entryFileName);
					var directoryName = Path.GetDirectoryName(fullZipToPath);
					if (directoryName.Length > 0)
					{
						Directory.CreateDirectory(directoryName);
					}

					// 4K is optimum
					var buffer = new byte[4096];

					// Unzip file in buffered chunks. This is just as fast as unpacking
					// to a buffer the full size of the file, but does not waste memory.
					// The "using" will close the stream even if an exception occurs.
					using (var zipStream = zf.GetInputStream(zipEntry))
					using (Stream fsOutput = File.Create(fullZipToPath))
					{
						StreamUtils.Copy(zipStream, fsOutput, buffer);
					}
				}
			}
		}
	}
}
