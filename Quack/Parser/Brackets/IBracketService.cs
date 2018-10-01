using Quack.Lexer;

namespace Quack.Parser.Brackets
{
	public interface IBracketService
	{
		TokenQueue TakeEnclosedTokens(TokenQueue tokens, BracketSet bracketSet);
	}
}
