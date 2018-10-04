﻿using System;
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
				case TokenType.FUNC_DECLARE:
					return FuncDeclare(tokens);
				case TokenType.VAR_DECLARE:
					return VarDeclare(tokens);
				case TokenType.LABEL:
					return Label(tokens);
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

		private AstNode FuncDeclare(TokenQueue tokens)
		{
			tokens.Skip(TokenType.FUNC_DECLARE);

			var label = tokens.Dequeue();

			var funcNode = new AstNode(AstNodeType.FUNC_DEF, label.Value);


			var paramTokens = _bracketService.TakeEnclosedTokens(tokens, BracketSets.Parentheses);
			while (paramTokens.Any())
			{
				if (paramTokens.IsNextType(TokenType.VAR_DECLARE))
				{
					var declare = VarDeclare(paramTokens, false);
					funcNode.Children.Add(new AstNode(AstNodeType.FUNC_PARAM, declare.Value));
					if (paramTokens.IsNextType(TokenType.PARAM_DELIMITER))
					{
						paramTokens.Skip(TokenType.PARAM_DELIMITER);
					}
				}
				else throw new ParseException($"Expected variable declaration for params but got {paramTokens.Peek()}");
			}

			var enclosedStatements = BracedStatements(tokens);
			funcNode.Children.Add(enclosedStatements);

			return funcNode;
		}

		private AstNode VarDeclare(TokenQueue tokens, bool allowInLineAssignment = true)
		{
			var declareNode = new AstNode(AstNodeType.DECLARE, tokens.ElementAt(1).Value);
			
			tokens.Skip(TokenType.VAR_DECLARE);

			if (allowInLineAssignment && tokens.IsNextType(TokenType.ASSIGN, 1))
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

		private AstNode Label(TokenQueue tokens)
		{
			return IsFunctionCall() ? FunctionCall(tokens) : Assign(tokens);
			bool IsFunctionCall() => tokens.IsNextType(TokenType.OPEN_PARENTHESES, 1);
		}

		private AstNode FunctionCall(TokenQueue tokens)
		{
			var funcLabel = tokens.Dequeue().Value;
			var funcCallNode = new AstNode(AstNodeType.FUNC_CALL, funcLabel);

			var paramTokens = _bracketService.TakeEnclosedTokens(tokens, BracketSets.Parentheses);

			while (paramTokens.Any())
			{
				var nextExpression = _expressionParser.ParseExpression(paramTokens);
				funcCallNode.Children.Add(nextExpression);
				if (paramTokens.IsNextType(TokenType.PARAM_DELIMITER))
				{
					paramTokens.Skip(TokenType.PARAM_DELIMITER);
				}
			}

			return funcCallNode;
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