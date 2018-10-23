using System.Linq;
using Quack.Lexer;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class InvalidFunctionCallException : BaseLanguageException
	{
		public InvalidFunctionCallException(DebugInfo info, FunctionDeclaration function)
			: base(info, $"Function call '{Function(function)}' has too few or too many required parameters")
		{
		}

		private static string Function(FunctionDeclaration function) 
			=> $"{function.Value}({string.Join(", ", function.Params.Select(p => p.Value))})";
	}
}