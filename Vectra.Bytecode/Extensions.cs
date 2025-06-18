using System.Text;

namespace Vectra.Bytecode;

internal static class Extensions
{
    public static void WriteString(this BinaryWriter writer, string value)
    {
        // Convert the string to a byte array
        var bytes = Encoding.UTF8.GetBytes(value);
        // Write the length of the string
        writer.Write(bytes.Length);
        // Write the string bytes
        writer.Write(bytes);
    }

    public static string ReadStringValue(this BinaryReader reader)
    {
        var length = reader.ReadInt32();
        var builder = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            builder.Append((char)reader.ReadByte());
        }
        
        return builder.ToString();
    }
}