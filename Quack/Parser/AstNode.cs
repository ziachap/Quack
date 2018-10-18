using System;
using System.Collections.Generic;

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
		
		public override string ToString()
		{
			var type = Enum.GetName(typeof(AstNodeType), Type);
			var valueString = Value != null ? " : " + Value : string.Empty;
			return $"[{type}{valueString}]";
		}
	}

	public enum AstNodeType
	{
		NO_OP,
		STATEMENTS,
		STATEMENT,
		DECLARE,
		ASSIGN,
		ARITHMETIC_OPERATOR,
		BOOLEAN_OPERATOR,
		STATEMENT_END,
		PRINT,
		NUMBER,
		LABEL,
		FACTOR,
		IF_ELSE,
		WHILE,
		FUNC_DEF,
		FUNC_INVOKE,
		FUNC_PARAM,
		FUNC_RETURN
	}
}