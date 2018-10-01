using System;

namespace Quack.SemanticValidation.Exceptions
{
	public class DuplicateDeclarationException : Exception
	{
		public DuplicateDeclarationException(string label) 
			: base($"Identifier '{label}' has already been declared")
		{
		}
	}
}