using System.Collections.Generic;

namespace Quack.Lexer
{
	public interface ILexer
	{
		Queue<Token> Tokenise(string input);
	}
}