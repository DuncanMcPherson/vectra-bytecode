using System.Diagnostics.CodeAnalysis;
using System.Text;
using Vectra.Bytecode.Models;

namespace Vectra.Bytecode;

[ExcludeFromCodeCoverage]
public class BytecodeReader
{
    /// <summary>
    /// Reads and parses a Vectra Bytecode (VBC) file from the specified file path.
    /// This method constructs and returns a <c>VbcProgram</c> object based on the file's content.
    /// </summary>
    /// <param name="path">The file path of the VBC file to be read and parsed.</param>
    /// <returns>A <c>VbcProgram</c> representing the parsed content of the VBC file.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the file at the specified path does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the current user does not have permission to access the specified file.</exception>
    /// <exception cref="ArgumentException">Thrown when the provided path is null, empty, or invalid.</exception>
    /// <exception cref="InvalidDataException">Thrown when the provided file contains invalid or corrupt bytecode.</exception>
    public static VbcProgram Read(string path)
    {
        using var stream = GetStream(path);
        using var reader = new BinaryReader(stream);
        
        ReadAndValidateHeader(reader);
        var entryInfo = ReadEntryInfo(reader);
        var constants = ReadConstants(reader);
        var rootSpace = ReadSpace(reader);

        return new VbcProgram
        {
            Dependencies = [],
            EntryPointMethod = entryInfo,
            ModuleName = path,
            ModuleType = entryInfo == null ? VbcModuleType.Library : VbcModuleType.Executable,
            RootSpace = rootSpace,
            Constants = constants
        };
    }
    
    /// <summary>
    /// Opens a file stream for reading from the specified file path.
    /// This method is used to provide a stream for reading the file's content.
    /// </summary>
    /// <param name="path">The file path of the file to be opened for reading.</param>
    /// <returns>A <c>FileStream</c> initialized for reading from the specified file.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the file at the specified path does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown when the current user does not have permission to access the specified file.</exception>
    /// <exception cref="ArgumentException">Thrown when the provided path is invalid.</exception>
    private static FileStream GetStream(string path)
    {
        return new FileStream(path, FileMode.Open, FileAccess.Read);
    }
    
    /// <summary>
    /// Reads and validates the header of a bytecode file to ensure compatibility and integrity.
    /// This method checks for a proper magic number and verifies the bytecode version.
    /// If the magic number or version is invalid, an exception will be thrown.
    /// </summary>
    /// <param name="reader">The binary reader used to read the bytecode file's header.</param>
    /// <exception cref="NotSupportedException">Thrown when the bytecode version is unsupported or the magic number is invalid.</exception>
    private static void ReadAndValidateHeader(BinaryReader reader)
    {
        var magicNumber = reader.ReadChars(3); // Always 3 chars and should always be "VBC"
        var version = reader.ReadByte(); // Currently 1, with future versions we may increment this value
        if (version != 1) throw new NotSupportedException($"Bytecode version {version} is not supported.");
        if (magicNumber[0] != 'V' || magicNumber[1] != 'B' || magicNumber[2] != 'C')
        {
            throw new NotSupportedException("Invalid bytecode file.");
        }
    }
    
    /// <summary>
    /// Reads the entry information from the provided binary reader.
    /// This method reads the length of the entry point, processes the associated bytes, and returns the resulting string.
    /// If the length is zero, the method returns null. If the length is zero, the module is considered to be a library.
    /// If the length is greater than zero, the module is considered to be an executable.
    /// </summary>
    /// <param name="reader">The binary reader from which the entry information is read.</param>
    /// <returns>A string representing the entry information if available; otherwise, null.</returns>
    private static string? ReadEntryInfo(BinaryReader reader)
    {
        var entryPointLength = (int)reader.ReadByte();
        if (entryPointLength == 0) return null;
        var builder = new StringBuilder();
        for (var i = 0; i < entryPointLength; i++)
        {
            builder.Append((char)reader.ReadByte());
        }
        return builder.ToString();
    }
    
    /// <summary>
    /// Reads a collection of constants from the provided binary reader.
    /// The method reads the number of constants, processes each using a helper method, and returns the resulting list of values.
    /// </summary>
    /// <param name="reader">The binary reader from which to read the constants.</param>
    /// <returns>A list of objects representing the constants, which may include integers, booleans, or strings.</returns>
    private static List<object> ReadConstants(BinaryReader reader)
    {
        var constantsCount = reader.ReadInt32();
        var constants = new List<object>();
        for (var i = 0; i < constantsCount; i++)
        {
            constants.Add(ReadConstant(reader));
        }
        return constants;
    }
    
    /// <summary>
    /// Reads a constant value from the provided binary reader.
    /// The method interprets the constant type based on a type code and returns the corresponding value.
    /// </summary>
    /// <param name="reader">The binary reader from which to read the constant value.</param>
    /// <returns>An object representing the constant value, such as an integer, boolean, or string.</returns>
    private static object ReadConstant(BinaryReader reader)
    {
        var typeCode = (int)reader.ReadByte();
        return typeCode switch
        {
            1 => reader.ReadInt32(),
            2 => reader.ReadBoolean(),
            3 => reader.ReadStringValue(),
            _ => throw new NotSupportedException($"Constant type {typeCode} is not supported.")
        };
    }
    
    /// <summary>
    /// Reads a <see cref="VbcSpace"/> from the provided binary reader.
    /// This method recursively reads all subspaces and types within the space.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    private static VbcSpace ReadSpace(BinaryReader reader)
    {
        var spaceName = reader.ReadStringValue();
        var classesCount = reader.ReadInt32();
        var types = new List<VbcType>();
        for (var i = 0; i < classesCount; i++)
        {
            types.Add(ReadClass(reader));
        }
        
        var subspacesCount = reader.ReadInt32();
        var subspaces = new List<VbcSpace>();
        for (var i = 0; i < subspacesCount; i++)
        {
            subspaces.Add(ReadSpace(reader));
        }

        return new VbcSpace
        {
            Name = spaceName,
            Types = types,
            Subspaces = subspaces
        };
    }
    
    /// <summary>
    /// Reads a <see cref="VbcClass" /> from the provided binary reader.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> used to read the file</param>
    /// <returns>An instance of a <see cref="VbcClass"/></returns>
    private static VbcClass ReadClass(BinaryReader reader)
    {
        var className = reader.ReadStringValue();
        var methodCount = reader.ReadInt32();
        var methods = new List<VbcMethod>();
        for (var i = 0; i < methodCount; i++)
        {
            methods.Add(ReadMethod(reader));
        }

        return new VbcClass
        {
            Name = className,
            Methods = methods
        };
    }
    
    /// <summary>
    /// Reads a full method declaration from the provided binary reader.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> used to read the file</param>
    /// <returns>An instance of a <see cref="VbcMethod"/> that correlates to the binary representation</returns>
    private static VbcMethod ReadMethod(BinaryReader reader)
    {
        var methodName = reader.ReadStringValue();
        var paramCount = (int)reader.ReadByte();
        var parameters = new List<VbcParameter>();
        for (var i = 0; i < paramCount; i++)
        {
            parameters.Add(ReadParameter(reader));
        }
        
        var instructionCount = (int)reader.ReadByte();
        var instructions = new List<Instruction>();
        for (var i = 0; i < instructionCount; i++)
            instructions.Add(ReadInstruction(reader));
        return new VbcMethod
        {
            Name = methodName,
            Parameters = parameters,
            Instructions = instructions
        };
    }
    
    /// <summary>
    /// Reads a single VbcParameter from the provided binary reader.
    /// The parameter consists of a name and a type.
    /// </summary>
    /// <param name="reader">The binary reader to read the parameter from.
    /// It must be positioned at the beginning of the string length marker.</param>
    /// <returns>A <see cref="VbcParameter"/> instance containing the parameter name
    /// and type to be used for the corresponding method declaration</returns>
    private static VbcParameter ReadParameter(BinaryReader reader)
    {
        var paramName = reader.ReadStringValue();
        var paramType = reader.ReadStringValue();
        return new VbcParameter
        {
            Name = paramName,
            TypeName = paramType
        };
    }
    
    /// <summary>
    /// Reads a single instruction from the provided binary reader.
    /// The instruction consists of an OpCode and an operand,
    /// representing a low-level command in the bytecode system.
    /// </summary>
    /// <param name="reader">The binary reader to read the instruction data from.
    /// It must be positioned at the beginning of the instruction data.</param>
    /// <returns>An <see cref="Instruction"/> instance containing the OpCode
    /// and operand extracted from the binary stream.</returns>
    private static Instruction ReadInstruction(BinaryReader reader)
    {
        var opCode = (OpCode) reader.ReadByte();
        var operand = reader.ReadInt32();
        return new Instruction(opCode, operand);
    }
}