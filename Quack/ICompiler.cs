using System;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack
{
	public interface ICompiler
	{
		void Compile();
	}

	public class Compiler : ICompiler
	{
		private readonly ILexer _lexer;

		public Compiler(ILexer lexer)
		{
			_lexer = lexer;
		}

		public void Compile()
		{
			var input = "declare a ; a = 5 ; print a ;";

			var tokens = _lexer.Tokenise(input);

			foreach (var token in tokens)
			{
				PrintToken(token);
			}
		}

		public void PrintToken(Token token)
		{
			var type = Enum.GetName(typeof(TokenType), token.Type);
			var valueString = token.Value != null ? " : " + token.Value : string.Empty;
			Console.WriteLine($"    [{type}]{valueString}");

		}
	}
}
