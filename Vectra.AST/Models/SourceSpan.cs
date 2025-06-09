namespace Vectra.AST.Models;

/// Represents a range within a source file, defined by its start and end line and column positions.
/// This provides a way to track specific portions of source code, such as for error reporting or debugging.
public struct SourceSpan(int startLine, int startColumn, int endLine, int endColumn)
{
    /// <summary>
    /// Gets the starting line number of the source span.
    /// </summary>
    /// <remarks>
    /// Represents the line number where the source span begins.
    /// The value is immutable and is initialized at the time of constructing the <see cref="SourceSpan"/>.
    /// </remarks>
    public int StartLine { get; } = startLine;

    /// Gets the starting column position of the span.
    /// This property represents the column where the source span begins
    /// within its respective starting line.
    /// It is a read-only integer property initialized during the construction
    /// of the SourceSpan instance.
    public int StartColumn { get; } = startColumn;

    /// <summary>
    /// Gets the zero-based line number where the source span ends.
    /// </summary>
    /// <remarks>
    /// This property indicates the line in the source code where the represented span concludes.
    /// The value corresponds to the zero-based index of the end line.
    /// </remarks>
    public int EndLine { get; } = endLine;

    /// Gets the zero-based index of the column where the span ends.
    /// This property represents the column number at which the source span concludes within its ending line.
    /// It is useful for determining the exact character position of the end boundary of a span,
    /// particularly in parsing or analyzing sections of textual data.
    public int EndColumn { get; } = endColumn;
}