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
					VerifyDeclarationExists(node);
					break;
				case AstNodeType.FUNC_INVOKE:
					VerifyDeclarationExists(node);
					VerifyHasRequiredParameters(node);
					break;
				case AstNodeType.ASSIGN:
					VerifyValidTypeforAssignment(node);
					break;
				case AstNodeType.IF_ELSE:
					VerifyBranchExpressionIsBoolean(node);
					break;
				case AstNodeType.BOOLEAN_OPERATOR:
				case AstNodeType.ARITHMETIC_OPERATOR:
				case AstNodeType.BOOLEAN_UNARY_OPERATOR:
					ExpressionType(node);
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
				_declarations.PushContext();
			}
		}

		private void Declaration(AstNode node)
		{
			var identifier = node.Value;
			if (_declarations.ExistsInScope(identifier))
			{
				throw new DuplicateDeclarationException(identifier);
			}
			var declaration = MakeDeclaration(node);
			_declarations.AddToCurrentContext(declaration);
		}

		private void VerifyDeclarationExists(AstNode node)
		{
			if (!_declarations.ExistsInScope(node.Value))
			{
				throw new IdentifierNotDeclaredException(node.Value);
			}
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
			}

			throw new Exception($"AstNode {node} is not a function or variable declaration");
		}

		private void VerifyHasRequiredParameters(AstNode functionCallNode)
		{
			var function = (FunctionDeclaration)_declarations.FindDeclaration(functionCallNode.Value);
			var functionParams = function.Params.ToArray();
			if (functionParams.Length != functionCallNode.Children.Count)
			{
				throw new InvalidFunctionCallException(function);
			}

			// check types
			for (var i = 0; i < functionCallNode.Children.Count; i++)
			{
				var parameterNode = functionCallNode.Children[i];
				var parameterNodeType = ExpressionType(parameterNode);
				var expectedParameterNode = (VariableDeclaration)functionParams[i];
				if (!expectedParameterNode.IsImplicitlyTyped)
				{
					// TODO: Lets think about what 'any' actually means and whether it belongs in function params
					AssertType(parameterNodeType, expectedParameterNode.TypeIdentifier);
				}
			}
		}

		private void VerifyBranchExpressionIsBoolean(AstNode node)
		{
			var expr = node.Children.First();
			if (expr.TypeIdentifier != LanguageConstants.ValueTypes.BOOL)
			{
				throw new InvalidTypeException(LanguageConstants.ValueTypes.BOOL, expr.TypeIdentifier);
			}
		}

		private void VerifyValidTypeforAssignment(AstNode node)
		{
			var identifier = node.Children.First();
			VerifyDeclarationExists(identifier);
			var declaration = (VariableDeclaration)_declarations.FindDeclaration(identifier.Value);

			var expr = node.Children.ElementAt(1);
			var exprTypeIdentifier = ExpressionType(expr);
			if (exprTypeIdentifier != declaration.TypeIdentifier && !declaration.IsImplicitlyTyped)
			{
				throw new InvalidAssignmentTypeException(declaration, exprTypeIdentifier);
			}
			if (declaration.IsImplicitlyTyped)
			{
				declaration.ImplicitSetType(exprTypeIdentifier);
			}
		}

		private string LookupIdentifierType(AstNode identifier)
		{
			VerifyDeclarationExists(identifier);
			var declaration = (VariableDeclaration)_declarations.FindDeclaration(identifier.Value);
			return declaration.TypeIdentifier;
		}

		// TODO: Move this expression type evaluation out somewhere
		private string ExpressionType(AstNode expr)
		{
			switch (expr.Type)
			{
				case AstNodeType.ARITHMETIC_OPERATOR:
					AssertTypes(expr.Children.Select(ExpressionType), LanguageConstants.ValueTypes.INT);
					return expr.TypeIdentifier;
				case AstNodeType.BOOLEAN_OPERATOR:
					// TODO: Need seperation between relational, equality and logic boolean operators to type check here
					//AssertTypes(expr.Children.Select(ExpressionType), LanguageConstants.ValueTypes.BOOL);
					return expr.TypeIdentifier;
				case AstNodeType.BOOLEAN_UNARY_OPERATOR:
					AssertType(ExpressionType(expr.Children.Single()), LanguageConstants.ValueTypes.BOOL);
					return expr.TypeIdentifier;
				case AstNodeType.BOOLEAN_CONSTANT:
				case AstNodeType.NUMBER:
					return expr.TypeIdentifier;
				case AstNodeType.FACTOR:
					return ExpressionType(expr.Children.Single());
				case AstNodeType.IDENTIFIER:
					return LookupIdentifierType(expr);
				default:
					throw new Exception("Unexpected AstNodeType in expression");
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

		private void AssertTypes(IEnumerable<string> actual, string expected)
		{
			foreach (var type in actual)
			{
				AssertType(type, expected);
			}
		}

		private void AssertType(string actual, string expected)
		{
			if (expected != actual)
			{
				throw new InvalidTypeException(expected, actual);
			}
		}
	}
}