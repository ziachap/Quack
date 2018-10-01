using Quack.Lexer;

namespace Quack.Parser
{
	public interface IExpressionParser
	{
		AstNode ParseExpression(TokenQueue tokens);
	}
}
