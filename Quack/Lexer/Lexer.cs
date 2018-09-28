using System;
using System.Collections.Generic;
using System.Linq;
using Quack.Lexer.TokenDefinitions;

namespace Quack.Lexer
{
	public class Lexer : ILexer
	{
		private readonly IEnumerable<ITokenDefinition> _tokenDefinitions;

		public Lexer(IEnumerable<ITokenDefinition> tokenDefinitions)
		{
			_tokenDefinitions = tokenDefinitions;
		}

		public Queue<Token> Tokenise(string input)
		{
			Console.WriteLine("--- LEXER START ---");

			var splitInput = input.Split(' ');

			var tokenQueue = new Queue<Token>();

			foreach (var term in splitInput)
			{
				var tokenDefinition = _tokenDefinitions.FirstOrDefault(x => x.IsMatch(term));

				if (tokenDefinition == null)
				{
					throw new TermNotSupportedException($"The term {term} could not be tokenised");
				}
				
				tokenQueue.Enqueue(tokenDefinition.GetToken(term));
			}
			
			Console.WriteLine("--- LEXER END ---");

			return tokenQueue;
		}
	}
}