﻿using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public class Parser : IParser
	{
		private readonly IExpressionParser _expressionParser;
		private readonly IBracketService _bracketService;

		public Parser(IExpressionParser expressionParser, IBracketService bracketService)
		{
			_expressionParser = expressionParser;
			_bracketService = bracketService;
		}

		public AstNode Parse(Queue<Token> tokens)
		{
			Console.WriteLine("--- PARSER ---");

			var remainingTokens = new Queue<Token>(tokens);

			var rootNode = Statements(remainingTokens);
			
			return rootNode;
		}
		
		private AstNode Statements(Queue<Token> tokens)
		{
			var node = new AstNode(AstNodeType.STATEMENTS);

			while (tokens.Any())
			{
				var statement = Statement(tokens);
				if (statement.Type != AstNodeType.STATEMENT_END)
				{
					node.Children.Add(statement);
				}
			}

			return node;
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
				case TokenType.IF:
					return If(tokens);
				default:
					throw new ParseException($"Unexpected token '{TokenTypeName(nextToken.Type)}'");
			}
		}

		private AstNode If(Queue<Token> tokens)
		{
			var ifElseNode = new AstNode(AstNodeType.IF_ELSE);

			Skip(tokens, TokenType.IF);
			Skip(tokens, TokenType.OPEN_PARENTHESES);

			var boolExpTokens = _bracketService.TakeTokensUntilCloseParentheses(tokens);
			var boolExpNode = _expressionParser.ParseExpression(boolExpTokens);
			ifElseNode.Children.Add(boolExpNode);

			Skip(tokens, TokenType.OPEN_BRACES);

			var ifStatementTokens = _bracketService.TakeTokensUntilCloseBraces(tokens);
			var ifStatementsNode = Statements(ifStatementTokens);
			ifElseNode.Children.Add(ifStatementsNode);
			
			if (tokens.Any() && tokens.Peek().Type == TokenType.ELSE)
			{
				Skip(tokens, TokenType.ELSE);
				Skip(tokens, TokenType.OPEN_BRACES);
				var elseStatementTokens = _bracketService.TakeTokensUntilCloseBraces(tokens);
				var elseStatementsNode = Statements(elseStatementTokens);
				ifElseNode.Children.Add(elseStatementsNode);
			}

			return ifElseNode;
		}

		private AstNode Declare(Queue<Token> tokens)
		{
			var declareNode = new AstNode(AstNodeType.DECLARE);

			Skip(tokens, TokenType.DECLARE);
			
			if (tokens.ElementAt(1).Type == TokenType.ASSIGN)
			{
				declareNode.Children.Add(Assign(tokens));
			}
			else
			{
				var labelToken = tokens.Dequeue();
				declareNode.Children.Add(Label(labelToken));
			}
			
			return declareNode;
		}

		private AstNode Assign(Queue<Token> tokens)
		{
			var assignNode = new AstNode(AstNodeType.ASSIGN);

			var assignmentTarget = tokens.Dequeue();
			AssertTypeOrThrow(assignmentTarget, TokenType.LABEL);
			assignNode.Children.Add(Label(assignmentTarget));

			Skip(tokens, TokenType.ASSIGN);

			assignNode.Children.Add(_expressionParser.ParseExpression(tokens));
			
			return assignNode;
		}

		private AstNode Print(Queue<Token> tokens)
		{
			var printNode = new AstNode(AstNodeType.PRINT);

			Skip(tokens, TokenType.PRINT);
			
			printNode.Children.Add(_expressionParser.ParseExpression(tokens));

			return printNode;
		}

		private AstNode StatementEnd(Queue<Token> tokens)
		{
			var statementEndNode = new AstNode(AstNodeType.STATEMENT_END);
			Skip(tokens, TokenType.STATEMENT_END);
			return statementEndNode;
		}

		private static AstNode Label(Token token)
		{
			AssertTypeOrThrow(token, TokenType.LABEL);
			return new AstNode(AstNodeType.LABEL, token.Value);
		}

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