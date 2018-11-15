using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quack.Parser;
using Quack.SemanticAnalysis;

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

		public string Transpile(SemanticAnalyzerResult analysedAst)
		{
			Console.WriteLine("--- Transpiler (JavaScript) ---");

			_output.Append(Statements(analysedAst.EntryNode));

			return _output.ToString();
		}

		// TODO: Perhaps lets get rid of this now?
		private string AllFunctionDeclarations(IEnumerable<IDeclaration> declarations)
		{
			var functionOutputs = declarations
				.OfType<FunctionDeclaration>()
				.Select(FuncDef)
				.ToList();

			return string.Join(BracedEnd(), functionOutputs) + BracedEnd();

			string FuncDef(FunctionDeclaration funcDef)
			{
				var statements = Indented(() => Statements(funcDef.Statements));
				var parameters = FunctionParameters(funcDef.Params.Select(d => d.Value));
				return $"function {funcDef.Value}({parameters}){{\n{statements}{Indentation()}}}";
			} 
		}

		private string FunctionParameters(IEnumerable<string> parameters) => string.Join(", ", parameters);

		private string Statements(AstNode node)
		{
			return string.Join("", node.Children.Select(st => Indentation() + Statement(st)));
		}

		private string Statement(AstNode node)
		{
			switch (node.Type)
			{
				case AstNodeType.STATEMENT_BLOCK:
					return Statements(node);
				case AstNodeType.DECLARE:
					return Declare(node) + StatementEnd();
				case AstNodeType.PRINT:
					return Print(node) + StatementEnd();
				case AstNodeType.ASSIGN:
					return Assign(node) + StatementEnd();
				case AstNodeType.IF_ELSE:
					return IfElse(node) + BracedEnd();
				case AstNodeType.WHILE:
					return While(node) + BracedEnd();
				case AstNodeType.FUNC_DEF:
					return FunctionDeclaration(node) + BracedEnd();
				case AstNodeType.FUNC_INVOKE:
					return FunctionCall(node) + StatementEnd();
				case AstNodeType.FUNC_RETURN:
					return FunctionReturn(node) + StatementEnd();
				case AstNodeType.NO_OP:
					return string.Empty;
				default:
					throw new TranspilerException("AstNodeType not supported");
			}
		}
		
		private string FunctionDeclaration(AstNode node)
		{
			var identifier = node.Value;
			var statements = Indented(() => Statements(node.Children.First()));
			var parameters = FunctionParameters(node.Children.Skip(1).Select(d => d.Value));

			return $"function {identifier}({parameters}){{\n{statements}{Indentation()}}}";
		}

		private string FunctionCall(AstNode node)
		{
			var parameters = node.Children.Select(Expression);
			return $"{node.Value}({string.Join(", ", parameters)})";
		}

		private string FunctionReturn(AstNode node)
		{
			return node.Children.Any() 
				? "return " + Expression(node.Children.Single()) 
				: "return";
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
			var identifier = node.Children.First().Value;
			var value = Expression(node.Children.ElementAt(1));
			return $"{identifier} = {value}";
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
			switch (node.Type)
			{
				case AstNodeType.FACTOR:
					return $"({Expression(node.Children.Single())})";
				case AstNodeType.FUNC_INVOKE:
					return FunctionCall(node);
				case AstNodeType.ARITHMETIC_OPERATOR:
				case AstNodeType.BOOLEAN_RELATIONAL_OPERATOR:
				case AstNodeType.BOOLEAN_EQUALITY_OPERATOR:
				case AstNodeType.BOOLEAN_LOGIC_OPERATOR:
					return Operation(node);
				case AstNodeType.BOOLEAN_UNARY_OPERATOR:
					return UnaryOperation(node);
				default:
					return $"{node.Value}";
			}
		}

		private string Operation(AstNode node)
		{
			var left = Expression(node.Children.First());
			var op = node.Value;
			var right = Expression(node.Children.ElementAt(1));
			return $"{left} {op} {right}";
		}

		private string UnaryOperation(AstNode node)
		{
			var op = node.Value;
			var right = Expression(node.Children.Single());
			return $"{op}{right}";
		}

		private string StatementEnd() => ";\n";
		private string BracedEnd() => "\n";

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