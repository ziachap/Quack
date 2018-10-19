using System;
using System.Linq;
using Microsoft.FSharp.Collections;
using Quack.Lexer;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Parser
{
	public class FunctionalParser : IParser
	{
		public AstNode Parse(TokenQueue tokens)
		{
			Console.WriteLine("--- PARSER ---");

			Types.AstNode result;
			try
			{
				result = Parser.Parse(ListModule.OfSeq(tokens.Select(AsParserToken)));
			}
			catch(ParseException ex)
			{
				throw new ParseException("There was a problem parsing the source file", ex);
			}

			return AstNode(result);
		}

		private Types.Token AsParserToken(Token token) 
			=> new Types.Token(TokenType(token.Type), token.Value);

		private string TokenType(Enum type) => Enum.GetName(typeof(TokenType), type);

		private AstNode AstNode(Types.AstNode node, AstNode parent = null)
		{
			var astNode = new AstNode(ParseAstNodeType(node.Type), node.Value, node.TypeIdentifier);
			astNode.Children = node.Children.Select(n => AstNode(n, astNode)).ToList();
			astNode.Parent = parent;
			return astNode;
		}

		private AstNodeType ParseAstNodeType(string type) => (AstNodeType)Enum.Parse(typeof(AstNodeType), type);
	}
}