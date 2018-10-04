using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Parser;
using Quack.SemanticAnalysis.Exceptions;
using Quack.SemanticValidation;

namespace Quack.SemanticAnalysis
{
	public class SemanticAnalyzer : ISemanticAnalyzer
	{
		private readonly DeclarationStore _declarations;

		public SemanticAnalyzer()
		{
			_declarations = new DeclarationStore();
		}

		public SemanticAnalyzerResult Analyze(AstNode node)
		{
			_declarations.PushContext(new DeclarationContext("__GLOBAL__"));

			DoAnalyze(node);

			return new SemanticAnalyzerResult(node, _declarations.PopContext().Declarations);
		}

		private void DoAnalyze(AstNode node)
		{
			AnalyzeNode(node);

			foreach (var child in node.Children)
			{
				DoAnalyze(child);
			}
			
			if (node.Type == AstNodeType.FUNC_DEF)
			{
				_declarations.PopContext();
			}
		}

		private void AnalyzeNode(AstNode node)
		{
			switch (node.Type)
			{
				case AstNodeType.DECLARE:
				case AstNodeType.FUNC_DEF:
					Declaration(node);
					break;
				case AstNodeType.LABEL:
					VerifyDeclarationExists(node);
					break;
				case AstNodeType.FUNC_CALL:
					VerifyDeclarationExists(node);
					VerifyHasRequiredParameters(node);
					break;
			}
		}

		private void Declaration(AstNode node)
		{
			var label = node.Value;
			if (_declarations.ExistsInScope(label))
			{
				throw new DuplicateDeclarationException(label);
			}
			var declaration = MakeDefinition(node);
			_declarations.AddToCurrentContext(declaration);
		}

		private void VerifyDeclarationExists(AstNode node)
		{
			if (!_declarations.ExistsInScope(node.Value))
			{
				throw new LabelNotDeclaredException(node.Value);
			}
		}

		private static IDeclaration MakeDefinition(AstNode node)
		{
			switch (node.Type)
			{
				case AstNodeType.DECLARE:
				case AstNodeType.FUNC_PARAM:
					return new VariableDeclaration(node.Value);
				case AstNodeType.FUNC_DEF:
					return new FunctionDeclaration(node.Value, node.Children.Last())
					{
						Params = new HashSet<IDeclaration>(node.Children.Take(node.Children.Count - 1).Select(MakeDefinition))
					};
			}

			throw new Exception($"AstNode {node} is not a function or variable declaration");
		}

		private void VerifyHasRequiredParameters(AstNode functionCallNode)
		{
			var function = (FunctionDeclaration)_declarations.FindDeclaration(functionCallNode.Value);
			if (function.Params.Count != functionCallNode.Children.Count)
			{
				throw new InvalidFunctionCallException(function);
			}
		}
	}
}