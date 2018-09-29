using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public class Parser : IParser
	{
		private readonly IArithmeticExpressionParser _arithmeticExpressionParser;

		public Parser(IArithmeticExpressionParser arithmeticExpressionParser)
		{
			_arithmeticExpressionParser = arithmeticExpressionParser;
		}

		public AstNode Parse(Queue<Token> tokens)
		{
			Console.WriteLine("--- PARSER ---");

			var remainingTokens = new Queue<Token>(tokens);

			var rootNode = new AstNode(AstNodeType.ROOT);

			while (remainingTokens.Any())
			{
				var statementNode = Statement(remainingTokens);
				if (statementNode.Type != AstNodeType.STATEMENT_END)
				{
					rootNode.Children.Add(statementNode);
				}
			}

			return rootNode;
		}

		private AstNode Statement(Queue<Token> tokens)
		{
			var nextToken = tokens.Peek();
			switch (nextToken.Type)
			{
				case TokenType.DECLARE:
					return Declare(tokens);
				case TokenType.LABEL:
					return Assign(tokens);
				case TokenType.PRINT:
					return Print(tokens);
				case TokenType.STATEMENT_END:
					return StatementEnd(tokens);
				default:
					throw new ParseException($"Unexpected token '{TokenTypeName(nextToken.Type)}'");
			}
		}

		private AstNode StatementEnd(Queue<Token> tokens)
		{
			var statementEndNode = new AstNode(AstNodeType.STATEMENT_END);
			Skip(tokens, TokenType.STATEMENT_END);
			return statementEndNode;
		}

		private AstNode Declare(Queue<Token> tokens)
		{
			var declareNode = new AstNode(AstNodeType.DECLARE);

			Skip(tokens, TokenType.DECLARE);
			
			var labelToken = tokens.Dequeue();
			AssertTypeOrThrow(labelToken, TokenType.LABEL);
			var labelNode = new AstNode(AstNodeType.LABEL, labelToken.Value);

			declareNode.Children.Add(labelNode);

			return declareNode;
		}

		private AstNode Assign(Queue<Token> tokens)
		{
			var assignNode = new AstNode(AstNodeType.ASSIGN);

			var assignmentTarget = tokens.Dequeue();
			AssertTypeOrThrow(assignmentTarget, TokenType.LABEL);
			assignNode.Children.Add(Label(assignmentTarget));

			Skip(tokens, TokenType.ASSIGN);

			assignNode.Children.Add(_arithmeticExpressionParser.ParseExpression(tokens));
			
			return assignNode;
		}

		private AstNode Print(Queue<Token> tokens)
		{
			var printNode = new AstNode(AstNodeType.PRINT);

			Skip(tokens, TokenType.PRINT);
			
			printNode.Children.Add(_arithmeticExpressionParser.ParseExpression(tokens));

			return printNode;
		}
		
		private static AstNode Label(Token token) => new AstNode(AstNodeType.LABEL, token.Value);
		
		private static void Skip(Queue<Token> tokens, TokenType expectedType)
		{
			var token = tokens.Dequeue();
			AssertTypeOrThrow(token, expectedType);
		}

		private static void AssertTypeOrThrow(Token token, TokenType expectedType)
		{
			if (token.Type != expectedType)
			{
				throw new ParseException($"Expected [{TokenTypeName(expectedType)}] but got {token}");
			}
		}

		private static string TokenTypeName(TokenType type) => Enum.GetName(typeof(TokenType), type);
	}
}