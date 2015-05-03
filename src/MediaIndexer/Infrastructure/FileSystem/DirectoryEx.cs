using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pihalve.MediaIndexer.Infrastructure.FileSystem
{
    public static class DirectoryEx
    {
        public static IEnumerable<string> EnumerateFiles(string path, string[] extensions, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(path, "*", searchOption).Where(file => HasAllowedExtension(file, extensions));
        }

        private static bool HasAllowedExtension(string filePath, string[] extensions)
        {
            var extension = Path.GetExtension(filePath);
            return extensions.Any(ext => ext.Equals(extension));
        }
    }
}
