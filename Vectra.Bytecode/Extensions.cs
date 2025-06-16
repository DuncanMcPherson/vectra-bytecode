using System.Text;

namespace Vectra.Bytecode;

internal static class Extensions
{
    public static void WriteString(this BinaryWriter writer, string value)
    {
        writer.Write(value.Length);
        writer.Write(value);
    }

    public static string ReadString(this BinaryReader reader, int length)
    {
        var builder = new StringBuilder();
        for (var i = 0; i < length; i++)
        {
            builder.Append((char)reader.ReadByte());
        }
        
        return builder.ToString();
    }
}