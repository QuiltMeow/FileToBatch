using System;
using System.IO;
using System.Text;

namespace FileToBatch
{
    public static class BatchCreator
    {
        private const string EXTENSION_PLACEHOLDER = "%extension%";

        private static readonly string batch = new StringBuilder()
            .AppendLine("@echo off")
            .AppendLine(":path")
            .AppendLine($"set \"file=%tmp%\\%RANDOM%%RANDOM%{EXTENSION_PLACEHOLDER}\"")
            .AppendLine("if exist \"%file%\" goto :path")
            .AppendLine("certutil -decode \"%~f0\" \"%file%\" >nul 2>&1")
            .AppendLine("start \"\" \"%file%\"")
            .Append("exit /b").ToString();

        public static string createBatch(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("找不到指定檔案");
            }

            string extension = Path.GetExtension(file);
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new InvalidOperationException("無效的檔案名稱");
            }

            return new StringBuilder()
                .AppendLine(batch.Replace(EXTENSION_PLACEHOLDER, extension))
                .Append(Util.toBase64String(file)).ToString();
        }

        public static void writeConfuseBatchFile(string output, string batch)
        {
            using (FileStream fs = new FileStream(output, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    byte[] header = Util.confuseHeader;
                    sw.BaseStream.Write(header, 0, header.Length);
                    sw.WriteLine();

                    sw.Write(batch);
                }
            }
        }
    }
}