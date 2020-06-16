using Microsoft.Extensions.Logging;
using ReferenceEngine.Bibtex.Manager;
using System;
using System.Collections.Generic;

namespace ReferenceEngine.Bibtex.Abstractions
{
    /// <summary>
    /// Represents a bibliography built by an <see cref="IBibliographyBuilder"/>.
    /// </summary>
    public interface IBibliography
    {
        /// <summary>
        /// Path to the target .bbl file to write to. This must be specified before calling the Write method.
        /// </summary>
        string TargetPath { get; set; }

        /// <summary>
        /// The collection of styled Bibitems.
        /// </summary>
        ICollection<Bibitem> Bibitems { get; set; }

        /// <summary>
        /// Writes the bibliography to the <see cref="TargetPath"/> .bbl file.
        /// </summary>
        void Write();
    }

    /// <summary>
    /// An implementation of the <see cref="IBibliography"/> interface.
    /// </summary>
    public class Bibliography : IBibliography
    {
        private readonly IFileManager _fileManager;
        private readonly ILogger<Bibliography> _logger;

        /// <inheritdoc />
        public string TargetPath { get; set; }

        /// <inheritdoc />
        public ICollection<Bibitem> Bibitems { get; set; }

        /// <summary>
        /// Constructs a new Bibliography instance initialised with an empty <see cref="Bibitems"/> collection.
        /// </summary>
        public Bibliography()
        {
            Bibitems = new List<Bibitem>();
        }

        /// <summary>
        /// Constructs a new Bibliography instance initialised with an empty <see cref="Bibitems"/> collection, and assigns the relevant fields with the provided values.
        /// </summary>
        /// <param name="fileManager"><see cref="IFileManager"/> instance used to write the bibliography.</param>
        /// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance used to log messages.</param>
        public Bibliography(IFileManager fileManager, ILogger<Bibliography> logger) : this()
        {
            _fileManager = fileManager;
            _logger = logger;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException ">Thrown when <see cref="TargetPath"/> is null.</exception>
        public void Write()
        {
            if (TargetPath == null)
            {
                throw new InvalidOperationException("TargetPath property cannot be null!");
            }

            _logger.LogTrace("Starting write of .bbl file.");

            _fileManager.DeleteIfExists(TargetPath);
            _fileManager.WriteStream(TargetPath, writer =>
            {
                writer.WriteLine("\\begin{thebibliography}{1}\r\n");
                foreach (var bibitem in Bibitems)
                {
                    writer.WriteLine($"{bibitem}\r\n");
                }
                writer.WriteLine("\\end{thebibliography}");
            });

            _logger.LogTrace("Finished writing .bbl file.");
        }
    }
}