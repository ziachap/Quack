using System.Collections.Generic;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public class AstNode
	{
		public AstNode(TokenType type, string value = null)
		{
			Type = type;
			Value = value;
			Children = new List<AstNode>();
		}

		public AstNode(TokenType type, string value, List<AstNode> children)
		{
			Type = type;
			Value = value;
			Children = children;
		}

		public TokenType Type { get; }
		public string Value { get; }
		public List<AstNode> Children { get; set; }
	}

	public class StatementParseResult
	{
		public StatementParseResult(AstNode node, Queue<Token> remainingTokens)
		{
			Node = node;
			RemainingTokens = remainingTokens;
		}

		public AstNode Node { get; }
		public Queue<Token> RemainingTokens { get; }
	}
}