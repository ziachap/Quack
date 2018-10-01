using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;
using Quack.Parser.Brackets;

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

		public AstNode Parse(TokenQueue tokens)
		{
			Console.WriteLine("--- PARSER ---");

			var remainingTokens = new TokenQueue(tokens);

			var rootNode = Statements(remainingTokens);
			
			return rootNode;
		}
		
		private AstNode Statements(TokenQueue tokens)
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
		
		private AstNode BracedStatements(TokenQueue tokens)
		{
			var statementTokens = _bracketService.TakeEnclosedTokens(tokens, BracketSets.Braces);
			return Statements(statementTokens);
		}

		private AstNode Statement(TokenQueue tokens)
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
				case TokenType.IF:
					return IfElse(tokens);
				case TokenType.WHILE:
					return While(tokens);
				case TokenType.STATEMENT_END:
					return StatementEnd(tokens);
				default:
					throw new ParseException($"Unexpected token '{TokenTypeName(nextToken.Type)}'");
			}
		}

		private AstNode IfElse(TokenQueue tokens)
		{
			var ifElseNode = new AstNode(AstNodeType.IF_ELSE);

			tokens.Skip(TokenType.IF);

			var boolExpTokens = _bracketService.TakeEnclosedTokens(tokens, BracketSets.Parentheses);
			var boolExpNode = _expressionParser.ParseExpression(boolExpTokens);
			ifElseNode.Children.Add(boolExpNode);
			ifElseNode.Children.Add(BracedStatements(tokens));
			
			if (tokens.IsNextType(TokenType.ELSE))
			{
				tokens.Skip(TokenType.ELSE);
				ifElseNode.Children.Add(tokens.IsNextType(TokenType.IF) 
					? IfElse(tokens) 
					: BracedStatements(tokens));
			}

			return ifElseNode;
		}

		private AstNode While(TokenQueue tokens)
		{
			tokens.Skip(TokenType.WHILE);

			var boolExpTokens = _bracketService.TakeEnclosedTokens(tokens, BracketSets.Parentheses);
			var boolExpNode = _expressionParser.ParseExpression(boolExpTokens);
			var children = new List<AstNode> {boolExpNode, BracedStatements(tokens)};

			return new AstNode(AstNodeType.WHILE, null, children);
		}

		private AstNode Declare(TokenQueue tokens)
		{
			var declareNode = new AstNode(AstNodeType.DECLARE, tokens.ElementAt(1).Value);
			
			tokens.Skip(TokenType.DECLARE);

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

		private AstNode Assign(TokenQueue tokens)
		{
			var assignNode = new AstNode(AstNodeType.ASSIGN);

			var assignmentTarget = tokens.Dequeue();
			assignmentTarget.AssertType(TokenType.LABEL);
			assignNode.Children.Add(Label(assignmentTarget));

			tokens.Skip(TokenType.ASSIGN);

			assignNode.Children.Add(_expressionParser.ParseExpression(tokens));
			
			return assignNode;
		}

		private AstNode Print(TokenQueue tokens)
		{
			var printNode = new AstNode(AstNodeType.PRINT);

			tokens.Skip(TokenType.PRINT);
			
			printNode.Children.Add(_expressionParser.ParseExpression(tokens));

			return printNode;
		}

		private AstNode StatementEnd(TokenQueue tokens)
		{
			var statementEndNode = new AstNode(AstNodeType.STATEMENT_END);
			tokens.Skip(TokenType.STATEMENT_END);
			return statementEndNode;
		}

		private static AstNode Label(Token token)
		{
			token.AssertType(TokenType.LABEL);
			return new AstNode(AstNodeType.LABEL, token.Value);
		}

		private static string TokenTypeName(TokenType type) 
			=> Enum.GetName(typeof(TokenType), type);
	}
}