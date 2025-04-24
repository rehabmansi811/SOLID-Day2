using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class Task2V03
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
            public List<SqlFile> lstSqlFiles { get; set; }

            public string GetTextFromFiles()
            {
                StringBuilder objStrBuilder = new StringBuilder();
                foreach (var objFile in lstSqlFiles)
                {
                    objStrBuilder.Append(objFile.LoadText());
                }
                return objStrBuilder.ToString();
            }

            public void SaveTextIntoFiles()
            {
                foreach (var objFile in lstSqlFiles)
                {
                     if (objFile is not ReadOnlySqlFile)
                    {
                        objFile.SaveText();
                    }
                    else
                    {
                        Console.WriteLine($"Skipping Save for Read-Only File: {objFile.FilePath}");
                    }
                }
            }
        }

        ////////////////////////////////////////////////////

        public class ReadOnlySqlFile : SqlFile
        {
            public ReadOnlySqlFile(string filePath) : base(filePath) { }

            public override void SaveText()
            {
                throw new IOException($"Cannot save: File is read-only - {FilePath}");
            }
        }

    }
    ///////////////////////////////////////////////////////
    


}
