﻿using Microsoft.Extensions.Logging;
using ReferenceEngine.Bibtex.Abstractions;
using ReferenceEngine.Bibtex.Enumerations;
using ReferenceEngine.Bibtex.Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReferenceEngine.Bibtex
{
    /// <summary>
    /// Represents a bibliography built by an <see cref="IBibliographyBuilder"/>.
    /// </summary>
    public interface IBibliography
    {
        /// <summary>
        /// Path to the target .bbl file to write to. This must be specified before calling the <see cref="Write"/> method.
        /// </summary>
        string TargetPath { get; set; }

        /// <summary>
        /// Path to the target .aux file to write \bibcite data to. This must be specified before calling the <see cref="Write"/> method.
        /// </summary>
        string TargetAuxPath { get; set; }

        /// <summary>
        /// Method of ordering.
        /// </summary>
        BibliographyOrder OrderBy { get; }

        /// <summary>
        /// The collection of styled Bibitems.
        /// </summary>
        ICollection<Bibitem> Bibitems { get; set; }

        /// <summary>
        /// The collection of styled Bibitems.
        /// </summary>
        ICollection<string> Preambles { get; set; }

        /// <summary>
        /// Writes the bibliography to the <see cref="TargetPath"/> .bbl file and adds the \bibcite entries to the aux file.
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
        public string TargetAuxPath { get; set; }

        /// <inheritdoc />
        public BibliographyOrder OrderBy { get; }

        /// <inheritdoc />
        public ICollection<Bibitem> Bibitems { get; set; }

        /// <inheritdoc />
        public ICollection<string> Preambles { get; set; }

        /// <summary>
        /// Constructs a new Bibliography instance initialised with empty <see cref="Bibitems"/> and <see cref="Preambles"/> collections, and assigns the relevant fields with the provided values.
        /// </summary>
        /// <param name="fileManager"><see cref="IFileManager"/> instance used to write the bibliography.</param>
        /// <param name="logger"><see cref="ILogger{TCategoryName}"/> instance used to log messages.</param>
        /// <param name="orderBy"><see cref="BibliographyOrder"/> associated with the bibliography, defaults to Appearance.</param>
        public Bibliography(IFileManager fileManager, ILogger<Bibliography> logger, BibliographyOrder orderBy = BibliographyOrder.Appearance)
        {
            _fileManager = fileManager;
            _logger = logger;
            Bibitems = new List<Bibitem>();
            Preambles = new List<string>();
            OrderBy = orderBy;
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException ">Thrown when <see cref="TargetPath"/> or <see cref="TargetAuxPath"/> is null.</exception>
        public void Write()
        {
            if (TargetPath == null)
            {
                throw new InvalidOperationException("TargetPath property cannot be null!");
            }
            else if (TargetAuxPath == null)
            {
                throw new InvalidOperationException("TargetAuxPath property cannot be null!");
            }

            _fileManager.ThrowIfFileDoesNotExist(TargetAuxPath);

            _logger.LogTrace("Starting write to .aux file.");
            _fileManager.WriteStream(TargetAuxPath, append: true, write: writer =>
            {
                foreach (var bibitem in Bibitems)
                {
                    writer.WriteLine($"\\bibcite{{{bibitem.CitationKey}}}{{{bibitem.Index}}}");
                }
            });
            _logger.LogTrace("Finished write to .aux file.");

            _logger.LogTrace("Starting write of .bbl file.");
            _fileManager.DeleteIfExists(TargetPath);
            _fileManager.WriteStream(TargetPath, writer =>
            {
                foreach (var preamble in Preambles)
                {
                    writer.WriteLine(preamble);
                }

                writer.WriteLine("\\begin{thebibliography}{1}\r\n");
                foreach (var bibitem in Bibitems)
                {
                    writer.WriteLine($"{bibitem}\r\n");
                }
                writer.WriteLine(@"\end{thebibliography}");
            });

            _logger.LogTrace("Finished writing .bbl file.");
        }
    }
}