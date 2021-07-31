namespace BinaryParFile
{
    using System.IO;
    using System.Runtime.InteropServices;

    public static class SteamExtensions
    {
        /// <summary>
        /// Read bytes from a BinaryReader and Marshals the byte array into a Struct
        /// - This code starts reading at the current position of the stream
        /// </summary>
        /// <typeparam name="T">The type of Struct</typeparam>
        /// <param name="binaryReader">The BinaryReader</param>
        /// <param name="structSize">Returns the size of the struct</param>
        /// <returns>A struct of type T</returns>
        internal static T ReadStruct<T>(this BinaryReader binaryReader, out int structSize)
            where T : struct
        {
            // Determine the size of the struct
            structSize = Marshal.SizeOf(typeof(T));

            // Read the bytes
            byte[] bytes = binaryReader.ReadBytes(structSize);

            // Allocate the bytes and returns a IntPtr
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);

            // Convert the IntPtr to the structure
            T outputStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));

            // Release the handle to the allocated memory
            handle.Free();

            // Return the structure
            return outputStructure;
        }

        /// <summary>
        /// Returns true if the Stream position is not at the end
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <returns>Boolean to represent if the stream is at the end</returns>
        internal static bool NotEndOfStream(this Stream stream)
        {
            return stream.Position < stream.Length;
        }
    }
}
