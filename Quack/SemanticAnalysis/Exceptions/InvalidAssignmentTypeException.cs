using System;
using Quack.Lexer;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class InvalidAssignmentTypeException : BaseLanguageException
	{
		public InvalidAssignmentTypeException(DebugInfo info, VariableDeclaration declaration, string actual)
			: base(info, $"Attempted to assign type <{actual}> to variable of type <{declaration.TypeIdentifier}> for '{declaration.Value}'")
		{
		}
	}

	public class InvalidTypeException : BaseLanguageException
	{
		public InvalidTypeException(DebugInfo info, string expected, string actual)
			: base(info, $"Expected type <{expected}> but got type <{actual}>")
		{
		}
	}
}