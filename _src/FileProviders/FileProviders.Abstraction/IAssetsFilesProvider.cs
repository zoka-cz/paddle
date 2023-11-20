using System;
using System.IO;
using System.Threading.Tasks;

namespace Zoka.Paddle.FileProviders.Abstraction
{
	/// <summary>The interface responsible for providing the file streams according to the path relative to the server</summary>
	public interface IAssetsFilesProvider
	{
		/// <summary>Will return the file stream according to the relative path passed</summary>
		/// <returns>If successful, it returns the stream. null is returned in case the file was not found</returns>
		Task<AssetFileInfo> GetFileInfoByPathAsync(Uri _path);
	}
}
