using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public interface IBracketService
	{
		Queue<Token> TakeTokensUntilCloseParentheses(Queue<Token> tokens);
		Queue<Token> TakeTokensUntilCloseBraces(Queue<Token> tokens);
		Queue<Token> TakeTokensUntilClose(Queue<Token> tokens, TokenType openType, TokenType closeType);
	}

	public class BracketService : IBracketService
	{
		public Queue<Token> TakeTokensUntilCloseParentheses(Queue<Token> tokens) 
			=> TakeTokensUntilClose(tokens, TokenType.OPEN_PARENTHESES, TokenType.CLOSE_PARENTHESES);

		public Queue<Token> TakeTokensUntilCloseBraces(Queue<Token> tokens)
			=> TakeTokensUntilClose(tokens, TokenType.OPEN_BRACES, TokenType.CLOSE_BRACES);

		public Queue<Token> TakeTokensUntilClose(Queue<Token> tokens, TokenType openType, TokenType closeType)
		{
			var parenthesisStack = new Stack<TokenType>(new[] { openType });
			var enclosedTokens = new Queue<Token>();

			while (parenthesisStack.Any())
			{
				if (!tokens.Any())
				{
					throw new ParseException("Could not find closing parentheses/braces");
				}

				var nextToken = tokens.Dequeue();
				if (nextToken.Type == openType)
				{
					parenthesisStack.Push(openType);
					enclosedTokens.Enqueue(nextToken);
				}
				else if (nextToken.Type == closeType)
				{
					parenthesisStack.Pop();
					if (parenthesisStack.Any())
					{
						enclosedTokens.Enqueue(nextToken);
					}
				}
				else
				{
					enclosedTokens.Enqueue(nextToken);
				}
			}

			return enclosedTokens;
		}
	}
}
