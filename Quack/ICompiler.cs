using System;
using Quack.File;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;
using Quack.Parser;
using Quack.SemanticValidation;
using Quack.Transpiler;

namespace Quack
{
	public interface ICompiler
	{
		void Compile(string filePath);
	}

	public class Compiler : ICompiler
	{
		private readonly IFileReader _reader;
		private readonly ISourceSanitizer _sanitizer;
		private readonly ILexer _lexer;
		private readonly IParser _parser;
		private readonly ISemanticAnalyzer _semanticValidator;
		private readonly ITranspiler _transpiler;
		private readonly IFileWriter _writer;

		public Compiler(IFileReader reader,
			ISourceSanitizer sanitizer,
			ILexer lexer,
			IParser parser, 
			ISemanticAnalyzer semanticValidator,
			ITranspiler transpiler,
			IFileWriter writer)
		{
			_reader = reader;
			_sanitizer = sanitizer;
			_lexer = lexer;
			_parser = parser;
			_semanticValidator = semanticValidator;
			_transpiler = transpiler;
			_writer = writer;
		}

		public void Compile(string filePath)
		{
			//var input = "declare a ; a = 5 ; print a ;";
			
			var input = _reader.LoadFromFile(filePath);

			Console.WriteLine("Loaded from " + filePath);
			Console.WriteLine("Source code:");
			Console.WriteLine(input);

			var sanitizedInput = _sanitizer.Sanitize(input);

			Console.WriteLine("Sanitized source code:");
			Console.WriteLine(sanitizedInput);

			var tokens = _lexer.Tokenise(sanitizedInput);
			foreach (var token in tokens)
			{
				Console.WriteLine($"    {token}");
			}

			var ast = _parser.Parse(tokens);
			PrintAstTree(ast);

			var analyzedAst = _semanticValidator.Analyze(ast);

			var transpiledCode = _transpiler.Transpile(analyzedAst);
			Console.WriteLine(transpiledCode);

			var outputPath = "C:/working.git/Quack/TestScripts";
			var outputFilename = "Output.js";

			_writer.WriteToFile(transpiledCode, outputPath, outputFilename);

			Console.WriteLine($"Written to {outputPath}/{outputFilename}");
			Console.WriteLine($"\nPress any key to exit..");
		}

		private void PrintAstTree(AstNode node, int level = 1)
		{
			Console.WriteLine(new String(' ', 2 * level) + AstNodeText(node));
			node.Children.ForEach(n => PrintAstTree(n, level + 1));
		}

		private string AstNodeText(AstNode node)
		{
			var type = Enum.GetName(typeof(AstNodeType), node.Type);
			var valueString = node.Value != null ? " : " + node.Value : string.Empty;
			return $"[{type}{valueString}]";
		}
	}
}
