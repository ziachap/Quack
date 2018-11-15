using System;
using System.Collections.Generic;
using Quack.Lexer;

namespace Quack.Parser
{
	public class AstNode
	{
		public AstNode(AstNodeType type, DebugInfo info, string value = null, string typeIdentifier = null)
		{
			Type = type;
			Info = info;
			TypeIdentifier = typeIdentifier;
			Value = value;
			Children = new List<AstNode>();
		}

		// Backward compatibility
		public AstNode(AstNodeType type, string value, List<AstNode> children)
		{
			Type = type;
			Value = value;
			Children = children;
			TypeIdentifier = null;
		}

		public AstNodeType Type { get; }
		public string Value { get; }
		public string TypeIdentifier { get; }
		public DebugInfo Info { get; }
		public List<AstNode> Children { get; set; }
		public AstNode Parent { get; set; }
		
		public override string ToString()
		{
			var type = Enum.GetName(typeof(AstNodeType), Type);
			var value = Value != null ? " : " + Value : string.Empty;
			var typeIdentifier = TypeIdentifier != null ? $" <{TypeIdentifier}>": string.Empty;
			return $"[{type}{typeIdentifier}{value}]";
		}
	}

	public enum AstNodeType
	{
		NO_OP,
		STATEMENT_BLOCK,
		STATEMENT,
		DECLARE,
		ASSIGN,
		ARITHMETIC_OPERATOR,
		BOOLEAN_RELATIONAL_OPERATOR,
		BOOLEAN_EQUALITY_OPERATOR,
		BOOLEAN_LOGIC_OPERATOR,
		BOOLEAN_UNARY_OPERATOR,
		STATEMENT_END,
		PRINT,
		NUMBER,
		BOOLEAN_CONSTANT,
		IDENTIFIER,
		FACTOR,
		IF_ELSE,
		WHILE,
		FUNC_DEF,
		FUNC_INVOKE,
		FUNC_PARAM, // LEGACY
		FUNC_RETURN
	}
}