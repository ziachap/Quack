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

		public TokenQueue Tokenise(string[] sanitizedLines, string[] sourceLines)
		{
			Console.WriteLine("--- LEXER ---");


			var tokenQueue = new TokenQueue();

			for (var i = 0; i < sanitizedLines.Count(); i++)
			{
				var line = sanitizedLines[i];

				if (string.IsNullOrWhiteSpace(line)) continue;

				var splitInput = line.Split(' ');

				foreach (var term in splitInput)
				{
					var tokenDefinition = _tokenDefinitions.FirstOrDefault(x => x.IsMatch(term));

					if (tokenDefinition == null)
					{
						throw new TermNotSupportedException($"The term '{term}' could not be tokenised");
					}

					var info = new DebugInfo()
					{
						Line = sourceLines[i],
						LineNumber = i + 1
					};

					tokenQueue.Enqueue(tokenDefinition.MakeToken(term, info));
				}
			}

			return tokenQueue;
		}
	}
}