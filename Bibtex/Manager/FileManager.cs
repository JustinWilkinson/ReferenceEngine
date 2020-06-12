using System.IO;

namespace Bibtex.Manager
{
    public interface IFileManager
    {
        /// <summary>
        /// Throws an exception if the file at the provided path does not exist.
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        void ThrowIfFileDoesNotExist(string path);

        /// <summary>
        /// Returns the original path with the extension replaced by the new extension.
        /// </summary>
        /// <param name="path">Original file path</param>
        /// <param name="newExtension">Extension to replace with</param>
        /// <returns>Original path with new extensions</returns>
        string ReplaceExtension(string path, string newExtension);
    }

    public class FileManager : IFileManager
    {
        /// <inheritdoc />
        public void ThrowIfFileDoesNotExist(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);

            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Could not find directory: '{directory}'");
            }
            else if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find file: '{fileName}' in '{directory}'");
            }
        }

        /// <inheritdoc />
        public string ReplaceExtension(string path, string newExtension) => Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.{newExtension}");
    }
}