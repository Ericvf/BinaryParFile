namespace BinaryParFile
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// File Description packet
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketFileDescBodyStructure
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        internal byte[] fileId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        internal byte[] fileHash;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        internal byte[] fileBlockHash;

        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        internal ulong fileLength;
    }
}
