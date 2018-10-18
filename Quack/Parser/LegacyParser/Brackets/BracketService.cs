using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser.LegacyParser.Brackets
{
	public class BracketService : IBracketService
	{
		public TokenQueue TakeEnclosedTokens(TokenQueue tokens, BracketSet bracketSet)
		{
			tokens.Skip(bracketSet.OpenType);
			var bracketStack = new Stack<TokenType>(new[] { bracketSet.OpenType });
			var enclosedTokens = new TokenQueue();

			while (bracketStack.Any())
			{
				if (!tokens.Any())
				{
					throw new ParseException("Could not find closing parentheses/braces");
				}

				var nextToken = tokens.Dequeue();
				if (nextToken.Type == bracketSet.OpenType)
				{
					bracketStack.Push(bracketSet.OpenType);
					enclosedTokens.Enqueue(nextToken);
				}
				else if (nextToken.Type == bracketSet.CloseType)
				{
					bracketStack.Pop();
					if (bracketStack.Any())
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