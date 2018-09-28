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
			_output.Append(target);
			_output.Append(" =");

			var valueNode  = node.Children.ElementAt(1);
			if (valueNode.Type == TokenType.ARITHMETIC_OPERATOR)
			{
				ArithmeticOperation(valueNode);
			}
			else
			{
				_output.Append(" " + valueNode.Value);

			}

			StatementEnd();
		}

		private void ArithmeticOperation(AstNode node)
		{
			_output.Append(" " + node.Children.First().Value);
			_output.Append($" {node.Value}");
			var rightValue = node.Children.ElementAt(1);
			if (rightValue.Type == TokenType.ARITHMETIC_OPERATOR)
			{
				ArithmeticOperation(rightValue);
			}
			else
			{
				_output.Append(" " + rightValue.Value);
			}
		}

		private void StatementEnd()
		{
			_output.Append(";\n");
		}
	}
}