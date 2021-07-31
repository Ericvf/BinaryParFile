namespace BinaryParFile
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Main packet
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PacketMainBodyStructure
    {
        [MarshalAs(UnmanagedType.U8, SizeConst = 8)]
        public ulong sliceSize;

        [MarshalAs(UnmanagedType.U4, SizeConst = 4)]
        public uint numberOfFiles;
    }
}
