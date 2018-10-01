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
				statements += Indentation() + Statement(childNode);
			}
			return statements;
		}

		private string Statement(AstNode node)
		{
			switch (node.Type)
			{
				case AstNodeType.DECLARE:
					return Declare(node) + StatementEnd();
				case AstNodeType.PRINT:
					return Print(node) + StatementEnd();
				case AstNodeType.ASSIGN:
					return Assign(node) + StatementEnd();
				case AstNodeType.IF_ELSE:
					return IfElse(node) + "\n";
				case AstNodeType.WHILE:
					return While(node) + "\n";
				default:
					throw new TranspilerException("AstNodeType not supported");
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
			var ifStatements = Indented(() => Statements(node.Children.ElementAt(1)));
			var ifElseOutput = $"if ({boolExp}) {{\n{ifStatements}{Indentation()}}}";

			if (HasElse())
			{
				var elseChild = node.Children.ElementAt(2);;
				if (IsElseChainedToIf(elseChild))
				{
					ifElseOutput += $"\nelse {IfElse(elseChild)}";
				}
				else
				{
					var elseStatements = Indented(() => Statements(node.Children.ElementAt(2)));
					ifElseOutput += $"\nelse {{\n{elseStatements}{Indentation()}}}";
				}
			}

			return ifElseOutput;

			bool HasElse() => node.Children.Count > 2;
			bool IsElseChainedToIf(AstNode elseChild) => elseChild.Type == AstNodeType.IF_ELSE;
		}

		private string While(AstNode node)
		{
			var boolExp = Expression(node.Children.First());
			var whileStatements = Indented(() => Statements(node.Children.ElementAt(1)));
			return $"while ({boolExp}) {{\n{whileStatements}{Indentation()}}}";
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

		private string Indented(Func<string> func)
		{
			IndentationLevel++;
			var output = func();
			IndentationLevel--;
			return output;
		}
	}
}