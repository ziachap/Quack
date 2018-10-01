﻿using System;

namespace Quack.SemanticValidation.Exceptions
{
	public class LabelNotDeclaredException : Exception
	{
		public LabelNotDeclaredException(string label)
			: base($"Identifier '{label}' has not been declared")
		{
		}
	}
}