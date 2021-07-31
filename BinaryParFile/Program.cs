namespace BinaryParFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    public class Program
    {
        [STAThread]
        internal static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                global::System.Windows.Forms.MessageBox.Show("You must specify a single filepath.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            var inputFileName = args[0];
            var folderPath = Path.GetDirectoryName(inputFileName);
            var parFile = Par2File.OpenFile(inputFileName);

            var md5FileTable = GetFileBlockHashTable(folderPath);

            var packetFileDescriptions = parFile.Packets
                .Where(p => p is PacketFileDescription)
                .Cast<PacketFileDescription>();

            foreach (var fileDescription in packetFileDescriptions)
            {
                var fileBlockHash = fileDescription.FileBlockHash;

                if (md5FileTable.ContainsKey(fileBlockHash))
                {
                    var oldFilePath = md5FileTable[fileBlockHash];
                    var oldFileName = Path.GetFileName(oldFilePath);
                    var newFileName = fileDescription.FileName.TrimEnd('\0');

                    if (!string.Equals(oldFileName, newFileName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        File.Move(oldFilePath, Path.Combine(folderPath, newFileName));
                        md5FileTable.Remove(fileBlockHash);
                    }
                }
            }
        }

        private static Dictionary<string, string> GetFileBlockHashTable(string folderPath)
        {
            var md5FileTable = new Dictionary<string, string>();

            using (var md5 = MD5.Create())
            {
                foreach (var filePath in Directory.GetFiles(folderPath))
                {
                    if (filePath.EndsWith(".par2", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        var binaryReader = new BinaryReader(fileStream);
                        var blockBytes = binaryReader.ReadBytes(16 * 1024);

                        byte[] data = md5.ComputeHash(blockBytes);
                        md5FileTable.Add(data.ToHexString(), filePath);
                    }
                }
            }

            return md5FileTable;
        }
    }
}
