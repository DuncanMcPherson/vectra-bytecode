namespace Vectra.Bytecode;

internal static class Extensions
{
    public static void WriteString(this BinaryWriter writer, string value)
    {
        writer.Write(value.Length);
        writer.Write(value);
    }
}