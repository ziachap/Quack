using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Parser;
using Quack.SemanticAnalysis.Exceptions;

namespace Quack.SemanticAnalysis
{
	public class SemanticAnalyzer : ISemanticAnalyzer
	{
		private readonly DeclarationStore _declarations;
		private readonly IExpressionEvaluator _expressionEvaluator;
		private readonly ITypeComparer _typeComparer;

		public SemanticAnalyzer(DeclarationStore declarations, IExpressionEvaluator expressionEvaluator, ITypeComparer typeComparer)
		{
			_declarations = declarations;
			_expressionEvaluator = expressionEvaluator;
			_typeComparer = typeComparer;
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

			if (node.Type == AstNodeType.STATEMENT_BLOCK)
			{
				_declarations.PopContext();
			}
		}

		private void AnalyzeNode(AstNode node)
		{
			switch (node.Type)
			{
				case AstNodeType.STATEMENT_BLOCK:
					NewContext(node);
					break;
				case AstNodeType.DECLARE:
				case AstNodeType.FUNC_DEF:
					Declaration(node);
					break;
				case AstNodeType.IDENTIFIER:
					_declarations.AssertDeclarationExists(node);
					break;
				case AstNodeType.FUNC_INVOKE:
					_declarations.AssertDeclarationExists(node);
					AssertHasRequiredParameters(node);
					break;
				case AstNodeType.ASSIGN:
					AssertValidTypeforAssignment(node);
					break;
				case AstNodeType.IF_ELSE:
					AssertBranchExpressionIsBoolean(node);
					break;
				case AstNodeType.BOOLEAN_RELATIONAL_OPERATOR:
				case AstNodeType.BOOLEAN_EQUALITY_OPERATOR:
				case AstNodeType.BOOLEAN_LOGIC_OPERATOR:
				case AstNodeType.ARITHMETIC_OPERATOR:
				case AstNodeType.BOOLEAN_UNARY_OPERATOR:
					_expressionEvaluator.Type(node);
					break;
			}
		}

		private void NewContext(AstNode statementBlockNode)
		{
			var parent = statementBlockNode.Parent;
			if (parent != null && parent.Type == AstNodeType.FUNC_DEF)
			{
				// if this is a statement block for a function, add the function's params to the new context
				var parameters = parent.Children.Skip(1).Select(MakeDeclaration);
				_declarations.PushContext(new DeclarationContext("FUNC_" + parent.Value, new HashSet<IDeclaration>(parameters)));
			}
			else
			{
				_declarations.PushBlankContext();
			}
		}

		private void Declaration(AstNode node)
		{
			var identifier = node.Value;
			if (_declarations.ExistsInScope(identifier))
			{
				throw new DuplicateDeclarationException(node.Info, identifier);
			}
			var declaration = MakeDeclaration(node);
			_declarations.AddToCurrentContext(declaration);
		}

		private IDeclaration MakeDeclaration(AstNode node)
		{
			switch (node.Type)
			{
				case AstNodeType.DECLARE:
				//case AstNodeType.FUNC_PARAM:
					return new VariableDeclaration(node.Value, node.TypeIdentifier);
				case AstNodeType.FUNC_DEF:
					var parameters = node.Children.Skip(1).ToList();
					AssertExplicitlyTypedParameters(parameters.Select(p => p.TypeIdentifier));
					return new FunctionDeclaration(node.Value, node.Children.First())
					{
						Params = new HashSet<IDeclaration>(parameters.Select(MakeDeclaration))
					};
				default:
					throw new Exception($"AstNode {node} is not a function or variable declaration");
			}
		}

		private void AssertHasRequiredParameters(AstNode functionCallNode)
		{
			var function = (FunctionDeclaration)_declarations.FindDeclaration(functionCallNode.Value);
			var functionParams = function.Params.ToArray();
			if (functionParams.Length != functionCallNode.Children.Count)
			{
				throw new InvalidFunctionCallException(functionCallNode.Info, function);
			}

			// check types
			for (var i = 0; i < functionCallNode.Children.Count; i++)
			{
				var parameterNode = functionCallNode.Children[i];
				var parameterNodeType = _expressionEvaluator.Type(parameterNode);
				var expectedParameterNode = (VariableDeclaration)functionParams[i];
				if (!expectedParameterNode.IsImplicitlyTyped)
				{
					// TODO: Lets think about what 'any' actually means and whether it belongs in function params
					_typeComparer.AssertType(parameterNode.Info, parameterNodeType, expectedParameterNode.TypeIdentifier);
				}
			}
		}

		private void AssertBranchExpressionIsBoolean(AstNode node)
		{
			var expr = node.Children.First();
			if (expr.TypeIdentifier != LanguageConstants.ValueTypes.BOOL)
			{
				throw new InvalidTypeException(expr.Info, LanguageConstants.ValueTypes.BOOL, expr.TypeIdentifier);
			}
		}

		private void AssertValidTypeforAssignment(AstNode node)
		{
			var identifier = node.Children.First();
			_declarations.AssertDeclarationExists(identifier);
			var declaration = (VariableDeclaration)_declarations.FindDeclaration(identifier.Value);

			var expr = node.Children.ElementAt(1);
			var exprTypeIdentifier = _expressionEvaluator.Type(expr);
			if (exprTypeIdentifier != declaration.TypeIdentifier && !declaration.IsImplicitlyTyped)
			{
				throw new InvalidAssignmentTypeException(identifier.Info, declaration, exprTypeIdentifier);
			}
			if (declaration.IsImplicitlyTyped)
			{
				declaration.ImplicitSetType(exprTypeIdentifier);
			}
		}
		
		private void AssertExplicitlyTypedParameters(IEnumerable<string> parameterTypes)
		{
			// TODO: this could be way nicer
			foreach (var type in parameterTypes)
			{
				if (type == LanguageConstants.ValueTypes.ANY)
				{
					throw new Exception($"Can not use <any> in function parameter");
				}
			}
		}
	}
}