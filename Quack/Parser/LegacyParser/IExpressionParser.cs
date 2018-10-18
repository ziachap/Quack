using Quack.Lexer;

namespace Quack.Parser.LegacyParser
{
	public interface IExpressionParser
	{
		AstNode ParseExpression(TokenQueue tokens);
	}
}
