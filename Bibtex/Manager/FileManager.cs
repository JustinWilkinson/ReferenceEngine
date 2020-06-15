using System;
using System.IO;

namespace Bibtex.Manager
{
    public interface IFileManager
    {
        /// <summary>
        /// Throws an exception if the file at the provided path does not exist.
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <exception cref="ArgumentException">Thrown if the path is null, empty or whitespace.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown if the directory for the path does not exist.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the file does not exist in the directory.</exception>
        void ThrowIfFileDoesNotExist(string path);

        /// <summary>
        /// Returns the original path with the extension replaced by the new extension.
        /// </summary>
        /// <param name="path">Original file path</param>
        /// <param name="newExtension">Extension to replace with</param>
        /// <returns>Original path with new extensions</returns>
        string ReplaceExtension(string path, string newExtension);

        /// <summary>
        /// Reads all the contents of a file.
        /// </summary>
        /// <param name="path">Path to the file to read.</param>
        /// <returns>A string containing the full contents of a file.</returns>
        string ReadFileContents(string path);

        /// <summary>
        /// Deletes a directory/file if it exists.
        /// </summary>
        /// <param name="path">Path to directory/file to delete.</param>
        void DeleteIfExists(string path);

        /// <summary>
        /// Writes to file using a custom StreamWriter action.
        /// </summary>
        /// <param name="path">Path to write to.</param>
        /// <param name="write">Custom write action.</param>
        void WriteStream(string path, Action<StreamWriter> write);
    }

    public class FileManager : IFileManager
    {
        /// <inheritdoc />
        public void ThrowIfFileDoesNotExist(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"Argument should not be null, empty or purely whitespace: {nameof(path)}");
            }

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

        /// <inheritdoc />
        public string ReadFileContents(string path) => File.ReadAllText(path);

        /// <inheritdoc />
        public void DeleteIfExists(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
            else if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <inheritdoc />
        public void WriteStream(string path, Action<StreamWriter> write)
        {
            using var writer = new StreamWriter(path);
            write(writer);
        }
    }
}