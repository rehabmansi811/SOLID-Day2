using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Task2
{
    internal class Task2v04
    {
        public interface IFile
        {
            string FilePath { get; }
            string FileText { get; set; }
            string LoadText();
            void SaveText();
        }

        public interface IFileReader
        {
            string LoadText(IFile file);
        }

        public interface IFileWriter
        {
            void SaveText(IFile file);
        }

        public class SqlFile : IFile
        {
            public string FilePath { get; private set; }
            public string FileText { get; set; }

            public SqlFile(string filePath)
            {
                FilePath = filePath;
            }

            public string LoadText()
            {
                if (!File.Exists(FilePath))
                    throw new FileNotFoundException("File not found.");

                FileText = File.ReadAllText(FilePath);
                return FileText;
            }

            public virtual void SaveText()
            {
                if (File.Exists(FilePath))
                {
                    File.WriteAllText(FilePath, FileText);
                }
                else
                {
                    throw new FileNotFoundException("File not found.");
                }
            }
        }

        public class ReadOnlySqlFile : SqlFile
        {
            public ReadOnlySqlFile(string filePath) : base(filePath) { }

            public override void SaveText()
            {
                throw new IOException($"Cannot save: File is read-only - {FilePath}");
            }
        }

        public class SqlFileReader : IFileReader
        {
            public string LoadText(IFile file)
            {
                return file.LoadText();
            }
        }

        public class WritableFileWriter : IFileWriter
        {
            public void SaveText(IFile file)
            {
                file.SaveText();
            }
        }

        public class SqlFileManager
        {
            private readonly IEnumerable<IFile> _files;
            private readonly IFileReader _fileReader;
            private readonly IFileWriter _fileWriter;

            public SqlFileManager(IEnumerable<IFile> files, IFileReader fileReader, IFileWriter fileWriter)
            {
                _files = files;
                _fileReader = fileReader;
                _fileWriter = fileWriter;
            }

            public string GetTextFromFiles()
            {
                StringBuilder objStrBuilder = new StringBuilder();
                foreach (var file in _files)
                {
                    objStrBuilder.Append(_fileReader.LoadText(file));
                }
                return objStrBuilder.ToString();
            }

            public void SaveTextIntoFiles()
            {
                foreach (var file in _files)
                {
                    if (file is ReadOnlySqlFile)
                    {
                        Console.WriteLine($"Skipping Save for Read-Only File: {file.FilePath}");
                        continue;
                    }

                    _fileWriter.SaveText(file);
                }
            }
        }

        public static void Main(string[] args)
        {
            var file1 = new SqlFile("file1.sql") { FileText = "SELECT * FROM Users;" };
            var file2 = new ReadOnlySqlFile("file2.sql") { FileText = "SELECT * FROM Products;" };

            var files = new List<IFile> { file1, file2 };
            var fileReader = new SqlFileReader();
            var fileWriter = new WritableFileWriter();
            var fileManager = new SqlFileManager(files, fileReader, fileWriter);

            string allText = fileManager.GetTextFromFiles();
            Console.WriteLine("Text from all files: ");
            Console.WriteLine(allText);

            fileManager.SaveTextIntoFiles();
        }
    }
}
