namespace Vectra.Bytecode.Models;

public enum OpCode : byte
{
    Nop = 0x00,
    LoadConst = 0x01,     // operand: constant pool index
    LoadLocal = 0x02,     // operand: local variable index
    StoreLocal = 0x03,    // operand: local variable index

    // Arithmetic
    Add = 0x10,
    Sub = 0x11,
    Mul = 0x12,
    Div = 0x13,

    // Method calls
    Call = 0x20,          // operand: function index or ID
    Ret = 0x21,           // return from function
}