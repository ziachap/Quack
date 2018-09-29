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
				case TokenType.DECLARE:
					return Declare(node);
				case TokenType.PRINT:
					return Print(node);
				case TokenType.ASSIGN:
					return Assign(node);
				default:
					throw new TranspilerException("AstNode type not supported");
			}
		}

		private string Declare(AstNode node)
		{
			var label = node.Children.Single().Value;
			return $"var {label}";
		}

		private string Print(AstNode node)
		{
			var value = EvaluatedValue(node.Children.Single());
			return $"console.log({value})";
		}

		private string Assign(AstNode node)
		{
			var label = node.Children.First().Value;
			var value = EvaluatedValue(node.Children.ElementAt(1));
			return $"{label} = {value}";
		}

		private string EvaluatedValue(AstNode node)
		{
			return node.Type == TokenType.ARITHMETIC_OPERATOR 
				? ArithmeticOperation(node) 
				: $"{node.Value}";
		}

		private string ArithmeticOperation(AstNode node)
		{
			var left = node.Children.First().Value;
			var op = node.Value;
			var right = EvaluatedValue(node.Children.ElementAt(1));
			return $"{left} {op} {right}";
		}

		private void StatementEnd()
		{
			_output.Append(";\n");
		}
	}
}