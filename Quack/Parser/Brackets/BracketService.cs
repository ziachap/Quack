using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser.Brackets
{
	public class BracketService : IBracketService
	{
		public TokenQueue TakeEnclosedTokens(TokenQueue tokens, BracketSet bracketSet)
		{
			tokens.Skip(bracketSet.OpenType);
			var parenthesisStack = new Stack<TokenType>(new[] { bracketSet.OpenType });
			var enclosedTokens = new TokenQueue();

			while (parenthesisStack.Any())
			{
				if (!tokens.Any())
				{
					throw new ParseException("Could not find closing parentheses/braces");
				}

				var nextToken = tokens.Dequeue();
				if (nextToken.Type == bracketSet.OpenType)
				{
					parenthesisStack.Push(bracketSet.OpenType);
					enclosedTokens.Enqueue(nextToken);
				}
				else if (nextToken.Type == bracketSet.CloseType)
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