namespace BinaryParFile
{
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Packet Header
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketHeaderStructure
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] magic;

        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public ulong length;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] packetHash;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] recoverySetId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] type;

        public string PacketHash
        {
            get
            {
                return this.packetHash.ToHexString();
            }
        }

        public string Magic
        {
            get
            {
                return Encoding.ASCII.GetString(this.magic);
            }
        }

        public string Type
        {
            get
            {
                return Encoding.ASCII.GetString(this.type);
            }
        }
    }
}
