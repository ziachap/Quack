using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public class ArithmeticExpressionParser : IArithmeticExpressionParser
	{
		public  AstNode ParseExpression(Queue<Token> tokens)
		{
			var currentToken = tokens.Dequeue();

			if (currentToken.Type == TokenType.OPEN_PARENTHESES)
			{
				return Factor(tokens);
			}
			else if (tokens.Count > 1 && tokens.Peek().Type == TokenType.ARITHMETIC_OPERATOR)
			{
				return ArithmeticOperation(tokens, LabelOrConstant(currentToken));
			}
			else if (currentToken.Type == TokenType.LABEL || currentToken.Type == TokenType.NUMBER)
			{
				return LabelOrConstant(currentToken);
			}
			else
			{
				throw new ParseException("Unexpected token: " + TokenTypeName(currentToken.Type));
			}
		}

		private AstNode Factor(Queue<Token> tokens)
		{
			var enclosedTokens = TakeTokensUntilCloseParentheses(tokens);
			var nextToken = tokens.Any() ? tokens.Peek() : null;

			var children = new[] { ParseExpression(enclosedTokens) }.ToList();
			var factor = new AstNode(AstNodeType.FACTOR, null, children);

			return nextToken != null && nextToken.Type == TokenType.ARITHMETIC_OPERATOR
				? ArithmeticOperation(tokens, factor)
				: factor;
		}

		private AstNode ArithmeticOperation(Queue<Token> tokens, AstNode leftNode)
		{
			var arithmeticOperator = tokens.Dequeue();
			var children = new List<AstNode> { leftNode, ParseExpression(tokens) };

			return new AstNode(AstNodeType.ARITHMETIC_OPERATOR, arithmeticOperator.Value, children);
		}

		private AstNode LabelOrConstant(Token token)
		{
			switch (token.Type)
			{
				case TokenType.LABEL:
					return Label(token);
				case TokenType.NUMBER:
					return Number(token);
				default:
					throw new ParseException("Expected a label or constant");
			}
		}

		private static AstNode Label(Token token) => new AstNode(AstNodeType.LABEL, token.Value);
		private static AstNode Number(Token token) => new AstNode(AstNodeType.NUMBER, token.Value);
		
		private Queue<Token> TakeTokensUntilCloseParentheses(Queue<Token> tokens)
		{
			var parenthesisStack = new Stack<string>(new[] { "(" });
			var enclosedTokens = new Queue<Token>();

			while (parenthesisStack.Any())
			{
				var nextToken = tokens.Dequeue();
				if (nextToken.Type == TokenType.OPEN_PARENTHESES)
				{
					parenthesisStack.Push("(");
					enclosedTokens.Enqueue(nextToken);
				}
				else if (nextToken.Type == TokenType.CLOSE_PARENTHESES)
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

		private static string TokenTypeName(TokenType type) => Enum.GetName(typeof(TokenType), type);
	}
}