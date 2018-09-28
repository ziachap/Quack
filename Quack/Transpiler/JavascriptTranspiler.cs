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
			foreach (var node in rootNode.Children)
			{
				Statement(node);
			}

			return _output.ToString();
		}

		private void Statement(AstNode node)
		{
			switch (node.Type)
			{
				case TokenType.DECLARE:
					Declare(node);
					break;
				case TokenType.PRINT:
					Print(node);
					break;
				case TokenType.ASSIGN:
					Assign(node);
					break;
				default:
					throw new TranspilerException("AstNode type not supported");
			}
		}

		private void Declare(AstNode node)
		{
			_output.Append("var ");
			var label = node.Children.Single().Value;
			_output.Append(label);
			StatementEnd();
		}

		private void Print(AstNode node)
		{
			var label = node.Children.Single().Value;
			_output.Append($"console.log({label})");
			StatementEnd();
		}

		private void Assign(AstNode node)
		{
			var target = node.Children.First().Value;
			var value  = node.Children.ElementAt(1).Value;
			_output.Append(target);
			_output.Append(" = ");
			_output.Append(value);
			StatementEnd();
		}

		private void StatementEnd()
		{
			_output.Append(";\n");
		}
	}
}