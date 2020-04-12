using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading;

namespace Eir.DevTools
{
    public static class FileTools
    {
        public static bool WriteModifiedText(String path, String text)
        {
            bool SaveText()
            {
                if (File.Exists(path) == false)
                    return true;
                String originalText = File.ReadAllText(path);
                return String.CompareOrdinal(text, originalText) != 0;
            }

            if (SaveText() == false)
                return false;

            WriteAllText(path, text);
            return true;
        }

        public static void WriteAllText(String path, String text)
        {
            for (Int32 i = 0; i < 5; i++)
            {
                try
                {
                    File.WriteAllText(path, text);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Thread.Sleep(100);
                }
            }
            throw new Exception("File write failed");
        }
    }
}
