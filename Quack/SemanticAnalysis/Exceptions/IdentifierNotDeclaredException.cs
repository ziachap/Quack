using System;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class IdentifierNotDeclaredException : Exception
	{
		public IdentifierNotDeclaredException(string identifier)
			: base($"Identifier '{identifier}' has not been declared")
		{
		}
	}
}