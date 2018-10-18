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

			var result = Parser.Parse(ListModule.OfSeq(tokens.Select(AsParserToken)));

			return AstNode(result);
		}

		private Types.Token AsParserToken(Token token) 
			=> new Types.Token(TokenType(token.Type), token.Value);

		private string TokenType(Enum type) => Enum.GetName(typeof(TokenType), type);

		private AstNode AstNode(Types.AstNode node)
			=> new AstNode(ParseAstNodeType(node.Type), node.Value, node.Children.Select(AstNode).ToList());

		private AstNodeType ParseAstNodeType(string type) => (AstNodeType)Enum.Parse(typeof(AstNodeType), type);
	}
}