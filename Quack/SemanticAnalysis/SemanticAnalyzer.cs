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

			if (node.Type == AstNodeType.FUNC_DEF)
			{
				// Ignore parameter children
				DoAnalyze(node.Children.First());
			}
			else
			{
				foreach (var child in node.Children)
				{
					DoAnalyze(child);
				}
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
				case AstNodeType.FUNC_INVOKE:
					VerifyDeclarationExists(node);
					VerifyHasRequiredParameters(node);
					break;
				case AstNodeType.ASSIGN:
					VerifyValidTypeforAssignment(node);
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
				//case AstNodeType.FUNC_PARAM:
					return new VariableDeclaration(node.Value, node.TypeIdentifier);
				case AstNodeType.FUNC_DEF:
					return new FunctionDeclaration(node.Value, node.Children.First())
					{
						Params = new HashSet<IDeclaration>(node.Children.Skip(1).Select(MakeDefinition))
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

		private void VerifyValidTypeforAssignment(AstNode node)
		{
			var identifier = node.Children.First();
			VerifyDeclarationExists(identifier);
			var declaration = (VariableDeclaration)_declarations.FindDeclaration(identifier.Value);

			var expr = node.Children.ElementAt(1);
			var exprTypeIdentifier = ExpressionType(expr);
			if (exprTypeIdentifier != declaration.TypeIdentifier && declaration.TypeIdentifier != "any")
			{
				throw new InvalidAssignmentTypeException(declaration, exprTypeIdentifier);
			}
		}

		private string LookupIdentifierType(AstNode identifier)
		{
			VerifyDeclarationExists(identifier);
			var declaration = (VariableDeclaration)_declarations.FindDeclaration(identifier.Value);
			return declaration.TypeIdentifier;
		}

		private string ExpressionType(AstNode expr)
		{
			switch (expr.Type)
			{
				case AstNodeType.ARITHMETIC_OPERATOR:
				case AstNodeType.BOOLEAN_OPERATOR:
				case AstNodeType.NUMBER:
					return expr.TypeIdentifier;
				case AstNodeType.FACTOR:
					return ExpressionType(expr.Children.Single());
				case AstNodeType.LABEL:
					return LookupIdentifierType(expr);
				default:
					throw new Exception("Unexpected AstNodeType in expression");
			}
		}
	}
}