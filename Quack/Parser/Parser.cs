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
				var parsedStatement = Statement(remainingTokens);
				if (parsedStatement.Node.Type != TokenType.STATEMENT_END)
				{
					rootNode.Children.Add(parsedStatement.Node);
				}
				remainingTokens = parsedStatement.RemainingTokens;
			}

			return rootNode;
		}

		private StatementParseResult Statement(Queue<Token> tokens)
		{
			var nextToken = tokens.First();
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

		private StatementParseResult StatementEnd(Queue<Token> tokens)
		{
			var statementEndNode = new AstNode(TokenType.STATEMENT_END);
			tokens.Dequeue();
			return new StatementParseResult(statementEndNode, tokens);
		}

		private StatementParseResult Declare(Queue<Token> tokens)
		{
			var declareNode = new AstNode(TokenType.DECLARE);

			var declareToken = tokens.Dequeue();
			AssertTypeOrThrow(declareToken, TokenType.DECLARE);
			
			var labelToken = tokens.Dequeue();
			AssertTypeOrThrow(labelToken, TokenType.LABEL);
			var labelNode = new AstNode(TokenType.LABEL, labelToken.Value);

			declareNode.Children.Add(labelNode);

			return new StatementParseResult(declareNode, tokens);
		}

		private StatementParseResult Assign(Queue<Token> tokens)
		{
			var assignNode = new AstNode(TokenType.ASSIGN);

			var assignmentTarget = tokens.Dequeue();
			AssertTypeOrThrow(assignmentTarget, TokenType.LABEL);

			var assignmentOperator = tokens.Dequeue();
			AssertTypeOrThrow(assignmentOperator, TokenType.ASSIGN);

			var valueToken = tokens.Dequeue();

			assignNode.Children.Add(Label(assignmentTarget));
			assignNode.Children.Add(LabelOrConstant(valueToken));

			return new StatementParseResult(assignNode, tokens);
		}

		private StatementParseResult Print(Queue<Token> tokens)
		{
			var printNode = new AstNode(TokenType.PRINT);

			var printToken = tokens.Dequeue();
			AssertTypeOrThrow(printToken, TokenType.PRINT);

			var valueToken = tokens.Dequeue();
			var valueNode = LabelOrConstant(valueToken);

			printNode.Children.Add(valueNode);

			return new StatementParseResult(printNode, tokens);
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

		private AstNode Label(Token token) => new AstNode(TokenType.LABEL, token.Value);
		private AstNode Number(Token token) => new AstNode(TokenType.NUMBER, token.Value);


		private void AssertTypeOrThrow(Token token, TokenType expectedType)
		{
			if (token.Type != expectedType)
			{
				throw new ParseException($"Expected [{TokenTypeName(expectedType)}] but got {token}");
			}
		}

		private string TokenTypeName(TokenType type) => Enum.GetName(typeof(TokenType), type);
	}
}