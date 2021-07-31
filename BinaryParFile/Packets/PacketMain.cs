namespace BinaryParFile
{
    using System.Collections.Generic;

    public class PacketMain : Packet
    {
        public PacketMainBodyStructure Body { get; set; }

        public List<string> Files { get; set; }
    }
}
