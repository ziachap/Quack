using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public interface IBracketService
	{
		TokenQueue TakeTokensUntilCloseParentheses(TokenQueue tokens);
		TokenQueue TakeTokensUntilCloseBraces(TokenQueue tokens);
		TokenQueue TakeTokensUntilClose(TokenQueue tokens, TokenType openType, TokenType closeType);
	}

	public class BracketService : IBracketService
	{
		public TokenQueue TakeTokensUntilCloseParentheses(TokenQueue tokens) 
			=> TakeTokensUntilClose(tokens, TokenType.OPEN_PARENTHESES, TokenType.CLOSE_PARENTHESES);

		public TokenQueue TakeTokensUntilCloseBraces(TokenQueue tokens)
			=> TakeTokensUntilClose(tokens, TokenType.OPEN_BRACES, TokenType.CLOSE_BRACES);

		public TokenQueue TakeTokensUntilClose(TokenQueue tokens, TokenType openType, TokenType closeType)
		{
			var parenthesisStack = new Stack<TokenType>(new[] { openType });
			var enclosedTokens = new TokenQueue();

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
