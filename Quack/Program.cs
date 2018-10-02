using System;
using Ninject;
using Quack.File;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;
using Quack.Lexer.TokenDefinitions.BracesAndParentheses;
using Quack.Parser;
using Quack.Parser.Brackets;
using Quack.SemanticValidation;
using Quack.Transpiler;

namespace Quack
{
	public class Program
	{
		static void Main(string[] args)
		{
			var kernel = Kernel();

			var compiler = kernel.Get<ICompiler>();

			compiler.Compile(args[0]);

			Console.ReadKey();
		}

		private static IKernel Kernel()
		{
			var kernel = new StandardKernel();

			kernel.Bind<ICompiler>().To<Compiler>();
			kernel.Bind<IFileReader>().To<FileReader>();
			kernel.Bind<IFileWriter>().To<FileWriter>();
			kernel.Bind<ISourceSanitizer>().To<SourceSanitizer>();
			kernel.Bind<ILexer>().To<Lexer.Lexer>();

			kernel.Bind<IParser>().To<Parser.Parser>();
			kernel.Bind<IExpressionParser>().To<ExpressionParser>();
			kernel.Bind<IBracketService>().To<BracketService>();

			kernel.Bind<ISemanticValidatorService>().To<SemanticValidatorService>();
			kernel.Bind<ITranspiler>().To<JavascriptTranspiler>();
			BindTokenDefinitions(kernel);

			return kernel;
		}

		private static void BindTokenDefinitions(IKernel kernel)
		{
			// keywords
			kernel.Bind<ITokenDefinition>().To<FuncDeclareTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<DeclareTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<IfTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<ElseTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<WhileTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<PrintTokenDefinition>();

			// symbols
			kernel.Bind<ITokenDefinition>().To<AssignTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<ArithmeticOperatorTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<BooleanOperatorTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<StatementEndTokenDefinition>();

			// brackets
			kernel.Bind<ITokenDefinition>().To<OpenParenthesisTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<CloseParenthesisTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<OpenBracesTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<CloseBracesTokenDefinition>();

			// labels/values
			kernel.Bind<ITokenDefinition>().To<NumberTokenDefinition>();
			kernel.Bind<ITokenDefinition>().To<LabelTokenDefinition>();
		}

	}
}
