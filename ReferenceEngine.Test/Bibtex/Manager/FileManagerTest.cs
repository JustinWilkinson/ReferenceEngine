using Bibtex.Manager;
using NUnit.Framework;

namespace Test.Bibtex.Manager
{
    [TestFixture]
    public class FileManagerTest
    {
        private readonly IFileManager _fileManager = new FileManager();

        [TestCase("RootFile.txt", "test", ExpectedResult = "RootFile.test")]
        [TestCase(@"C:\SomeFolder\SomeFile.tex", "aux", ExpectedResult = @"C:\SomeFolder\SomeFile.aux")]
        [TestCase(@"C:\SomeFolder\SomeFile", "txt", ExpectedResult = @"C:\SomeFolder\SomeFile.txt")]
        public string ReplaceExtension_FilePathWithExtension_Successful(string path, string newExtension)
        {
            return _fileManager.ReplaceExtension(path, newExtension);
        }
    }
}