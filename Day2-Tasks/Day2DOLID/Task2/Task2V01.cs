using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class Task2V01
    {
        public interface IFile
        {
            string FilePath { get; }
            string FileText { get; set; }
            string LoadText();
            void SaveText();
        }

        public class SqlFile : IFile
        {
            public string FilePath { get; private set; }
            public string FileText { get; set; }

            public SqlFile(string filePath)
            {
                FilePath = filePath;
            }

            public virtual string LoadText()
            {
                if (!File.Exists(FilePath))
                    throw new FileNotFoundException("File not found.");

                FileText = File.ReadAllText(FilePath);
                return FileText;
            }

            public virtual void SaveText()
            {
                File.WriteAllText(FilePath, FileText);
            }
        }

        public class SqlFileManager
        {
            private readonly IEnumerable<IFile> _files;

            public SqlFileManager(IEnumerable<IFile> files )
            {
                _files = files;
            }

            public string GetTextFromFiles()
            {
                StringBuilder sb = new StringBuilder();
                foreach (var file in _files)
                {
                    sb.Append(file.LoadText());
                }
                return sb.ToString();
            }

            public void SaveTextIntoFiles()
            {
                foreach (var file in _files)
                {
                    file.SaveText();  
                }
            }
        }

    }
}
