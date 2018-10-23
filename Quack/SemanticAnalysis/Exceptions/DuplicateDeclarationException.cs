using System;
using Quack.Lexer;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class DuplicateDeclarationException : BaseLanguageException
	{
		public DuplicateDeclarationException(DebugInfo info, string identifier) 
			: base(info, $"Identifier '{identifier}' has already been declared")
		{
		}
	}
}