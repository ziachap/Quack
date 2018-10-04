using System;
using System.Linq;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class InvalidFunctionCallException : Exception
	{
		public InvalidFunctionCallException(FunctionDeclaration function)
			: base($"Function call '{Function(function)}' has too few or too many required parameters")
		{
		}

		private static string Function(FunctionDeclaration function) 
			=> $"{function.Value}({string.Join(", ", function.Params.Select(p => p.Value))})";
	}
}