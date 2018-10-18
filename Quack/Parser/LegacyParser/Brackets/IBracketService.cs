using Quack.Lexer;

namespace Quack.Parser.LegacyParser.Brackets
{
	public interface IBracketService
	{
		TokenQueue TakeEnclosedTokens(TokenQueue tokens, BracketSet bracketSet);
	}
}
