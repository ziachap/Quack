using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public class ExpressionParser : IExpressionParser
	{
		private readonly IBracketService _bracketService;

		public ExpressionParser(IBracketService bracketService)
		{
			_bracketService = bracketService;
		}

		public  AstNode ParseExpression(Queue<Token> tokens)
		{
			var currentToken = tokens.Dequeue();

			if (currentToken.Type == TokenType.OPEN_PARENTHESES)
			{
				return Factor(tokens);
			}
			else if (tokens.Count > 1 && tokens.Peek().Type == TokenType.ARITHMETIC_OPERATOR)
			{
				return Operation(tokens, LabelOrConstant(currentToken), AstNodeType.ARITHMETIC_OPERATOR);
			}
			else if (tokens.Count > 1 && tokens.Peek().Type == TokenType.BOOLEAN_OPERATOR)
			{
				return Operation(tokens, LabelOrConstant(currentToken), AstNodeType.BOOLEAN_OPERATOR);
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
			var enclosedTokens = _bracketService.TakeTokensUntilCloseParentheses(tokens);
			var children = new[] { ParseExpression(enclosedTokens) }.ToList();
			var factor = new AstNode(AstNodeType.FACTOR, null, children);

			if (tokens.Any())
			{
				switch (tokens.Peek().Type)
				{
					case TokenType.ARITHMETIC_OPERATOR:
						return Operation(tokens, factor, AstNodeType.ARITHMETIC_OPERATOR);
					case TokenType.BOOLEAN_OPERATOR:
						return Operation(tokens, factor, AstNodeType.BOOLEAN_OPERATOR);
				}
			}

			return factor;
		}

		private AstNode Operation(Queue<Token> tokens, AstNode leftNode, AstNodeType operationType)
		{
			var op = tokens.Dequeue();
			var children = new List<AstNode> { leftNode, ParseExpression(tokens) };

			return new AstNode(operationType, op.Value, children);
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
		
		private static string TokenTypeName(TokenType type) => Enum.GetName(typeof(TokenType), type);
	}
}