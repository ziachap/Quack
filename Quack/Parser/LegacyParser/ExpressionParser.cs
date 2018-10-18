using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;
using Quack.Parser.LegacyParser.Brackets;

namespace Quack.Parser.LegacyParser
{
	public class ExpressionParser : IExpressionParser
	{
		private readonly IBracketService _bracketService;

		public ExpressionParser(IBracketService bracketService)
		{
			_bracketService = bracketService;
		}

		public AstNode ParseExpression(TokenQueue tokens)
		{
			if (tokens.IsNextType(TokenType.OPEN_PARENTHESES))
			{
				return Factor(tokens);
			}
			if (tokens.IsNextType(TokenType.ARITHMETIC_OPERATOR, 1))
			{
				return Operation(tokens, AstNodeType.ARITHMETIC_OPERATOR);
			}
			if (tokens.IsNextType(TokenType.BOOLEAN_OPERATOR, 1))
			{
				return Operation(tokens, AstNodeType.BOOLEAN_OPERATOR);
			}
			if (tokens.IsNextType(TokenType.LABEL) || tokens.IsNextType(TokenType.NUMBER))
			{
				return LabelOrConstant(tokens);
			}

			throw new ParseException("Unexpected token: " + TokenTypeName(tokens.Peek().Type));
		}

		private AstNode Factor(TokenQueue tokens)
		{
			var enclosedTokens = _bracketService.TakeEnclosedTokens(tokens, BracketSets.Parentheses);
			var children = new[] { ParseExpression(enclosedTokens) }.ToList();
			var factor = new AstNode(AstNodeType.FACTOR, null, children);

			if (tokens.Any())
			{
				switch (tokens.Peek().Type)
				{
					case TokenType.ARITHMETIC_OPERATOR:
						return Operation(tokens, AstNodeType.ARITHMETIC_OPERATOR, factor);
					case TokenType.BOOLEAN_OPERATOR:
						return Operation(tokens, AstNodeType.BOOLEAN_OPERATOR, factor);
				}
			}

			return factor;
		}

		private AstNode Operation(TokenQueue tokens, AstNodeType operationType) 
			=> Operation(tokens, operationType, LabelOrConstant(tokens));

		private AstNode Operation(TokenQueue tokens, AstNodeType operationType, AstNode leftNode)
		{
			var op = tokens.Dequeue();
			var children = new List<AstNode> { leftNode, ParseExpression(tokens) };
			return new AstNode(operationType, op.Value, children);
		}

		private AstNode LabelOrConstant(TokenQueue tokens)
		{
			var token = tokens.Dequeue();
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