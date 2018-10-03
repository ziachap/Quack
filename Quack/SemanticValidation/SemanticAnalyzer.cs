using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Parser;
using Quack.SemanticValidation.Exceptions;

namespace Quack.SemanticValidation
{
	public class SemanticAnalyzer : ISemanticAnalyzer
	{
		private readonly HashSet<IDefinition> _definitions;

		public SemanticAnalyzer()
		{
			_definitions = new HashSet<IDefinition>();
		}

		public SemanticAnalyzerResult Analyze(AstNode node)
		{
			AnalyzeNode(node);
			
			foreach (var child in node.Children)
			{
				Analyze(child);
			}

			return new SemanticAnalyzerResult(node, _definitions.ToList());
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
				case AstNodeType.FUNC_CALL:
					Label(node);
					break;
			}
		}

		private void Declaration(AstNode node)
		{
			var label = node.Value;
			if (_definitions.Any(i => i.ValueEquals(node.Value)))
			{
				throw new DuplicateDeclarationException(label);
			}
			_definitions.Add(MakeDefinition(node));
		}

		private void Label(AstNode node)
		{
			if (!_definitions.Any(i => i.ValueEquals(node.Value)))
			{
				throw new LabelNotDeclaredException(node.Value);
			}
		}

		private static IDefinition MakeDefinition(AstNode node)
		{
			if (node.Type == AstNodeType.DECLARE)
			{
				return new VariableDefinition(node.Value);
			}
			if (node.Type == AstNodeType.FUNC_DEF)
			{
				return new FunctionDefinition(node.Value, node.Children.Single());
			}
			throw new Exception($"AstNode {node} is not a function or variable declaration");
		}
	}
}