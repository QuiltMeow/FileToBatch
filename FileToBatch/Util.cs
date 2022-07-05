using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileToBatch
{
    public static class Util
    {
        public static readonly byte[] confuseHeader = {
            0xFF, 0xFE
        };

        public static string wrapText(string text, int lineLength = 64)
        {
            return Regex.Replace(text, $"(.{{{lineLength}}})", $"$1{Environment.NewLine}");
        }

        public static string toBase64String(string fileName)
        {
            byte[] content = File.ReadAllBytes(fileName);

            return new StringBuilder()
                .AppendLine("-----BEGIN CERTIFICATE-----")
                .AppendLine(wrapText(Convert.ToBase64String(content)))
                .Append("-----END CERTIFICATE-----").ToString();
        }
    }
}
