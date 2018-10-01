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

		public TokenQueue Tokenise(string input)
		{
			Console.WriteLine("--- LEXER ---");

			var splitInput = input.Split(' ');

			var tokenQueue = new TokenQueue();

			foreach (var term in splitInput)
			{
				var tokenDefinition = _tokenDefinitions.FirstOrDefault(x => x.IsMatch(term));

				if (tokenDefinition == null)
				{
					throw new TermNotSupportedException($"The term '{term}' could not be tokenised");
				}
				
				tokenQueue.Enqueue(tokenDefinition.GetToken(term));
			}

			return tokenQueue;
		}
	}
}