namespace BinaryParFile
{
    public class PacketFileDescription : Packet
    {
        public string FileName { get; set; }

        public string FileId
        {
            get
            {
                return this.Body.fileId.ToHexString();
            }
        }

        public string FileHash
        {
            get
            {
                return this.Body.fileHash.ToHexString();
            }
        }

        public string FileBlockHash
        {
            get
            {
                return this.Body.fileBlockHash.ToHexString();
            }
        }

        internal PacketFileDescBodyStructure Body { get; set; }
    }
}
