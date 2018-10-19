using System;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class DuplicateDeclarationException : Exception
	{
		public DuplicateDeclarationException(string identifier) 
			: base($"Identifier '{identifier}' has already been declared")
		{
		}
	}
}