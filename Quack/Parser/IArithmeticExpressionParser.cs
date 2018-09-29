using System.Collections.Generic;
using Quack.Lexer;

namespace Quack.Parser
{
	public interface IArithmeticExpressionParser
	{
		AstNode ParseExpression(Queue<Token> tokens);
	}
}
