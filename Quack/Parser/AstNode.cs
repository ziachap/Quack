using System.Collections.Generic;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public class AstNode
	{
		public AstNode(AstNodeType type, string value = null)
		{
			Type = type;
			Value = value;
			Children = new List<AstNode>();
		}

		public AstNode(AstNodeType type, string value, List<AstNode> children)
		{
			Type = type;
			Value = value;
			Children = children;
		}

		public AstNodeType Type { get; }
		public string Value { get; }
		public List<AstNode> Children { get; set; }
	}

	public enum AstNodeType
	{
		ROOT,
		DECLARE,
		ASSIGN,
		ARITHMETIC_OPERATOR,
		STATEMENT_END,
		PRINT,
		NUMBER,
		LABEL,
		FACTOR
	}
}