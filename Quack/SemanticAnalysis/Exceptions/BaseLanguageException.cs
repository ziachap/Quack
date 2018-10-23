using System;
using System.Linq;
using Quack.Lexer;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class BaseLanguageException : Exception
	{
		public BaseLanguageException(DebugInfo info, string message)
			: base($"[Line:{info.LineNumber}] > {info.Line}\n{message}")
		{
		}

		private static string Function(FunctionDeclaration function)
			=> $"{function.Value}({string.Join(", ", function.Params.Select(p => p.Value))})";
	}
}