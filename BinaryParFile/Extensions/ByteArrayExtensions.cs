namespace BinaryParFile
{
    using System.Text;

    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Converts a byte array into a hexadecimal string 
        /// </summary>
        /// <param name="bytes">The byte array</param>
        /// <param name="upperCase">Specifies if upper casing should be used</param>
        /// <returns>String that represents the byte array in hexadecimal</returns>
        public static string ToHexString(this byte[] bytes, bool upperCase = false)
        {
            // Create a new fixed length stringbuilder
            var result = new StringBuilder(bytes.Length * 2);

            // Determine the format (outside loop)
            var format = upperCase ? "X2" : "x2";

            // Iterate over all bytes
            for (int i = 0; i < bytes.Length; i++)
            {
                // Append a formatted byte
                result.Append(bytes[i].ToString(format));
            }

            // Return the output string
            return result.ToString();
        }
    }
}
