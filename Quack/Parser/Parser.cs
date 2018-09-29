using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public class Parser : IParser
	{
		public AstNode Parse(Queue<Token> tokens)
		{
			Console.WriteLine("--- PARSER ---");

			var remainingTokens = new Queue<Token>(tokens);

			var rootNode = new AstNode(TokenType.ROOT);

			while (remainingTokens.Any())
			{
				var statementNode = Statement(remainingTokens);
				if (statementNode.Type != TokenType.STATEMENT_END)
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
			var statementEndNode = new AstNode(TokenType.STATEMENT_END);
			Skip(tokens, TokenType.STATEMENT_END);
			return statementEndNode;
		}

		private AstNode Declare(Queue<Token> tokens)
		{
			var declareNode = new AstNode(TokenType.DECLARE);

			Skip(tokens, TokenType.DECLARE);
			
			var labelToken = tokens.Dequeue();
			AssertTypeOrThrow(labelToken, TokenType.LABEL);
			var labelNode = new AstNode(TokenType.LABEL, labelToken.Value);

			declareNode.Children.Add(labelNode);

			return declareNode;
		}

		private AstNode Assign(Queue<Token> tokens)
		{
			var assignNode = new AstNode(TokenType.ASSIGN);

			var assignmentTarget = tokens.Dequeue();
			AssertTypeOrThrow(assignmentTarget, TokenType.LABEL);
			assignNode.Children.Add(Label(assignmentTarget));

			Skip(tokens, TokenType.ASSIGN);

			assignNode.Children.Add(EvaluatedValue(tokens));
			
			return assignNode;
		}

		private AstNode Print(Queue<Token> tokens)
		{
			var printNode = new AstNode(TokenType.PRINT);

			Skip(tokens, TokenType.PRINT);
			
			printNode.Children.Add(EvaluatedValue(tokens));

			return printNode;
		}

		private AstNode EvaluatedValue(Queue<Token> tokens)
		{
			var valueToken = tokens.Dequeue();

			return tokens.Peek().Type == TokenType.ARITHMETIC_OPERATOR 
				? ArithmeticOperation(tokens, valueToken) 
				: LabelOrConstant(valueToken);
		}

		private AstNode ArithmeticOperation(Queue<Token> tokens, Token previousTerm)
		{
			var arithmeticOperator = tokens.Dequeue();
			var children = new List<AstNode> { LabelOrConstant(previousTerm), EvaluatedValue(tokens) };

			return new AstNode(TokenType.ARITHMETIC_OPERATOR, arithmeticOperator.Value, children);
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

		private static AstNode Label(Token token) => new AstNode(TokenType.LABEL, token.Value);
		private static AstNode Number(Token token) => new AstNode(TokenType.NUMBER, token.Value);

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