[![.github/workflows/release-bytecode.yaml](https://github.com/DuncanMcPherson/vectra-bytecode/actions/workflows/release-bytecode.yaml/badge.svg)](https://github.com/DuncanMcPherson/vectra-bytecode/actions/workflows/release-bytecode.yaml)

# Vectra Bytecode

## Overview

Vectra Bytecode is a .NET-based project focused on bytecode manipulation and abstract syntax tree (AST) processing. The project is built using .NET 9.0 and C# 13.0, providing modern and efficient tools for bytecode analysis and manipulation.

## Project Structure

The repository consists of several key components:

- **Vectra.AST**: Core library for Abstract Syntax Tree manipulation
- **Vectra.AST.Tests**: Unit tests for the AST functionality
- **Vectra.Bytecode**: Main bytecode processing and manipulation library
- **Vectra.Bytecode.Tests**: Unit tests for the bytecode functionality

## Requirements

- .NET 9.0 SDK or later
- Compatible IDE (recommended: Visual Studio 2022 or JetBrains Rider)

## Getting Started

1. Clone the repository:
```bash
git clone https://github.com/duncanmcpherson/vectra-bytecode.git
```
2. Open the solution file
```bash
cd vectra-bytecode
dotnet restore
```
3. Build the project
```bash
dotnet build 
```
4. Run the tests:
```bash
dotnet test 
```

## Usage

[Note: Add specific usage examples based on the actual API and functionality]

## Testing

The project includes comprehensive test suites for both the AST and Bytecode components. Tests can be run using the standard .NET testing commands or through your IDE's test runner.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes following the code style guidelines
4. Commit your changes using the Semantic Versioning commit message format:
   - `feat(scope): description` - for new features
   - `fix(scope): description` - for bug fixes
   - `docs(scope): description` - for documentation changes
   - `style(scope): description` - for formatting changes
   - `refactor(scope): description` - for code refactoring
   - `perf(scope): description` - for performance improvements
   - `test(scope): description` - for adding or modifying tests
   - `chore(scope): description` - for maintenance tasks

   Examples:
   ```
   feat(ast): add support for lambda expressions
   fix(bytecode): resolve null reference in instruction decoder
   docs(readme): update installation instructions
   ```

   Breaking changes should be noted with a `!` and a footer:
   ```
   feat(api)!: change method signature in IParser

   BREAKING CHANGE: IParser.parse() now takes two arguments instead of one
   ```

5. Push to the branch (`git push origin feature/amazing-feature`)
6. Open a Pull Request

> **Note**: Following the semantic commit message format is required as it's used to automatically determine version numbers and generate changelogs through our CI/CD pipeline.

## License

[Add appropriate license information]

## Contact

[Add appropriate contact information]

## Acknowledgments

- List any third-party libraries or tools used
- Credits to contributors and maintainers
