﻿using System;

namespace Quack.Parser
{
	public class ParseException : Exception
	{
		public ParseException(string message) : base(message)
		{
		}


		public ParseException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}