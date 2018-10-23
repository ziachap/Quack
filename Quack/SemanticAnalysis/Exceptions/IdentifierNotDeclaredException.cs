using System;
using Quack.Lexer;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class IdentifierNotDeclaredException : BaseLanguageException
	{
		public IdentifierNotDeclaredException(DebugInfo info, string identifier)
			: base(info, $"Identifier '{identifier}' has not been declared")
		{
		}
	}
}