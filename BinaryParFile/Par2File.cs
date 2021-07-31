namespace BinaryParFile
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Par2File
    {
        public readonly ReadOnlyCollection<Packet> Packets;

        private Par2File(List<Packet> packets)
        {
            this.Packets = new ReadOnlyCollection<Packet>(packets);
        }

        public static Par2File OpenFile(string filePath)
        {
            List<Packet> packets = new List<Packet>();
            int structSize;

            using (var md5 = MD5.Create())
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var binaryReader = new BinaryReader(stream);

                    while (binaryReader.BaseStream.NotEndOfStream())
                    {
                        var packetBeginPosition = binaryReader.BaseStream.Position;

                        var packetHeader = binaryReader.ReadStruct<PacketHeaderStructure>(out structSize);
                        long currentBytesRead = structSize;
                        Packet packet;

                        switch (packetHeader.Type)
                        {
                            case "PAR 2.0\0Main\0\0\0\0":
                                var mainBody = binaryReader.ReadStruct<PacketMainBodyStructure>(out structSize);
                                currentBytesRead += structSize;
                                currentBytesRead += mainBody.numberOfFiles * 16;

                                var packetMain = new PacketMain();
                                packetMain.Body = mainBody;
                                packetMain.Files = new List<string>();

                                for (int i = 0; i < mainBody.numberOfFiles; i++)
                                {
                                    packetMain.Files.Add(binaryReader.ReadBytes(16).ToHexString());
                                }

                                packet = packetMain;
                                break;

                            case "PAR 2.0\0FileDesc":
                                var fileDescBody = binaryReader.ReadStruct<PacketFileDescBodyStructure>(out structSize);
                                currentBytesRead += structSize;

                                var fileNameBytes = binaryReader.ReadBytes((int)packetHeader.length - (int)currentBytesRead);
                                currentBytesRead += fileNameBytes.Length;

                                var packetFileDesc = new PacketFileDescription();
                                packetFileDesc.Body = fileDescBody;
                                packetFileDesc.FileName = Encoding.ASCII.GetString(fileNameBytes);
                                packet = packetFileDesc;
                                break;

                            case "PAR 2.0\0Creator\0":
                                var creatorBytes = binaryReader.ReadBytes((int)packetHeader.length - (int)currentBytesRead);
                                currentBytesRead += creatorBytes.Length;

                                var packetCreator = new PacketCreator();
                                packetCreator.ClientCreator = Encoding.ASCII.GetString(creatorBytes);
                                packet = packetCreator;
                                break;

                            case "PAR 2.0\0IFSC\0\0\0\0":
                                var fileId = binaryReader.ReadBytes(16).ToHexString();
                                currentBytesRead += 16;

                                var packetFileSliceChecksum = new PacketFileSliceChecksum();
                                packetFileSliceChecksum.FileId = fileId;
                                packet = packetFileSliceChecksum;
                                break;

                            default:
                                packet = new Packet();
                                break;
                        }

                        if (!packetHeader.Type.StartsWith("PAR 2.0"))
                        {
                            break;
                        }

                        var packetEndPosition = binaryReader.BaseStream.Position + (long)packetHeader.length - currentBytesRead;

                        binaryReader.BaseStream.Position = packetBeginPosition + 32;

                        var packetTotalBytes = binaryReader.ReadBytes((int)packetEndPosition - (int)packetBeginPosition - 32);

                        var packetHash = md5.ComputeHash(packetTotalBytes).ToHexString();
                        if (packetHeader.PacketHash == packetHash)
                        {
                            packet.Header = packetHeader;
                            packets.Add(packet);
                        }
                    }
                }
            }

            return new Par2File(packets);
        }
    }
}
