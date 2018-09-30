using System;
using System.Linq;
using System.Text;
using Quack.Lexer.TokenDefinitions;
using Quack.Parser;

namespace Quack.Transpiler
{
	public class JavascriptTranspiler : ITranspiler
	{
		private readonly StringBuilder _output;

		public JavascriptTranspiler()
		{
			_output = new StringBuilder();
		}

		public string Transpile(AstNode rootNode)
		{
			Console.WriteLine("--- Transpiler (JavaScript) ---");

			foreach (var node in rootNode.Children)
			{
				_output.Append(Statement(node));
				StatementEnd();
			}

			return _output.ToString();
		}

		private string Statement(AstNode node)
		{
			switch (node.Type)
			{
				case AstNodeType.DECLARE:
					return Declare(node);
				case AstNodeType.PRINT:
					return Print(node);
				case AstNodeType.ASSIGN:
					return Assign(node);
				default:
					throw new TranspilerException("AstNode type not supported");
			}
		}

		private string Declare(AstNode node)
		{
			var child = node.Children.Single();

			var declarationExpression = child.Type == AstNodeType.ASSIGN 
				? Assign(child) 
				: child.Value;

			return $"var {declarationExpression}";
		}

		private string Print(AstNode node)
		{
			var value = Expression(node.Children.Single());
			return $"console.log({value})";
		}

		private string Assign(AstNode node)
		{
			var label = node.Children.First().Value;
			var value = Expression(node.Children.ElementAt(1));
			return $"{label} = {value}";
		}

		private string Expression(AstNode node)
		{
			if (node.Type == AstNodeType.FACTOR)
			{
				return $"({Expression(node.Children.Single())})";
			}

			return node.Type == AstNodeType.ARITHMETIC_OPERATOR 
				? ArithmeticOperation(node) 
				: $"{node.Value}";
		}

		private string ArithmeticOperation(AstNode node)
		{
			var left = Expression(node.Children.First());
			var op = node.Value;
			var right = Expression(node.Children.ElementAt(1));
			return $"{left} {op} {right}";
		}

		private void StatementEnd()
		{
			_output.Append(";\n");
		}
	}
}