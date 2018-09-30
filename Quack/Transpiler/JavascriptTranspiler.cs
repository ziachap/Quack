using System;
using System.Linq;
using System.Text;
using Quack.Parser;

namespace Quack.Transpiler
{
	public class JavascriptTranspiler : ITranspiler
	{
		private readonly StringBuilder _output;
		private int IndentationLevel { get; set; }

		public JavascriptTranspiler()
		{
			_output = new StringBuilder();
			IndentationLevel = 0;
		}

		public string Transpile(AstNode rootNode)
		{
			Console.WriteLine("--- Transpiler (JavaScript) ---");

			_output.Append(Statements(rootNode));

			return _output.ToString();
		}

		private string Statements(AstNode node)
		{
			var statements = string.Empty;
			foreach (var childNode in node.Children)
			{
				statements += Indentation() + Statement(childNode) + StatementEnd();
			}
			return statements;
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
				case AstNodeType.IF_ELSE:
					return IfElse(node);
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

		private string IfElse(AstNode node)
		{
			var boolExp = Expression(node.Children.First());
			IndentationLevel++;
			var ifStatements = Statements(node.Children.ElementAt(1));
			IndentationLevel--;
			return $"if ({boolExp}) {{\n{ifStatements}}}";
		}

		private string Expression(AstNode node)
		{
			if (node.Type == AstNodeType.FACTOR)
			{
				return $"({Expression(node.Children.Single())})";
			}

			return node.Type == AstNodeType.ARITHMETIC_OPERATOR || node.Type == AstNodeType.BOOLEAN_OPERATOR
				? Operation(node) 
				: $"{node.Value}";
		}

		private string Operation(AstNode node)
		{
			var left = Expression(node.Children.First());
			var op = node.Value;
			var right = Expression(node.Children.ElementAt(1));
			return $"{left} {op} {right}";
		}

		private string StatementEnd() => ";\n";

		private string Indentation() => new string(' ', 2 * IndentationLevel);
	}
}