using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class Task2v02
    {
        public interface IFile
        {
            string FilePath { get; set; }
            string FileText { get; set; }
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
            public string FilePath { get; set; }
            public string FileText { get; set; }
        }


        public class SqlFileReader : IFileReader
        {
            public virtual string LoadText(IFile file)
            {
                if (!File.Exists(file.FilePath))
                    throw new FileNotFoundException("File not found.");

                file.FileText = File.ReadAllText(file.FilePath);
                return file.FileText;
            }
        }

        public class SqlFileWriter : IFileWriter
        {
            public virtual  void SaveText(IFile file)
            {
                File.WriteAllText(file.FilePath, file.FileText);
            }
        }
        public class SqlFileManager
        {
            private readonly IEnumerable<IFile> _files;
            private readonly IFileReader _reader;
            private readonly IFileWriter _writer;

            public SqlFileManager(IEnumerable<IFile> files, IFileReader reader, IFileWriter writer)
            {
                _files = files;
                _reader = reader;
                _writer = writer;
            }

            public virtual string GetTextFromFiles()
            {
                StringBuilder sb = new StringBuilder();

                foreach (var file in _files)
                {
                    sb.Append(_reader.LoadText(file));
                }

                return sb.ToString();
            }

            public virtual void SaveTextIntoFiles()
            {
                foreach (var file in _files)
                {
                    _writer.SaveText(file);
                }
            }
        }


        ////////////////////////////////////////////////////////////new feature ///////

        public class ReadOnlySqlFile : SqlFile
        {
            public override void SaveText()
            {
                // Throw an exception when trying to save a read-only file
                throw new IOException("Can't Save: File is read-only.");
            }
        }

        public class SqlFileWriter : IFileWriter
        {
            public virtual void SaveText(IFile file)
            {
                // Ensure that we can only save files that are not read-only
                if (file is ReadOnlySqlFile)
                {
                    throw new IOException("Can't Save: File is read-only.");
                }

                File.WriteAllText(file.FilePath, file.FileText);
            }
        }

    }
}

