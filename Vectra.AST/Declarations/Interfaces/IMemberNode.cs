using Vectra.AST.Models;

namespace Vectra.AST.Declarations.Interfaces;

/// <summary>
/// Represents a member node in the abstract syntax tree (AST).
/// </summary>
/// <remarks>
/// A member node defines a named component within a class or similar structure in the AST.
/// Implementations of this interface are used to represent specific types of members, such as
/// methods, properties, fields, or other constructs within a class or type declaration.
/// </remarks>
public interface IMemberNode : IAstNode
{
    /// Represents the name of a member associated with the node.
    /// This property is used to uniquely identify a member within the Abstract Syntax Tree (AST).
    /// Implemented as a part of the IMemberNode interface and can be used to retrieve the string identifier
    /// for nodes such as methods, fields, or properties in the AST structure.
    string Name { get; }
}