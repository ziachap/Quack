using System;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class DuplicateDeclarationException : Exception
	{
		public DuplicateDeclarationException(string label) 
			: base($"Identifier '{label}' has already been declared")
		{
		}
	}
}