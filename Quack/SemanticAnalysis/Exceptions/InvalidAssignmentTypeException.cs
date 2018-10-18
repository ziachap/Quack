﻿using System;

namespace Quack.SemanticAnalysis.Exceptions
{
	public class InvalidAssignmentTypeException : Exception
	{
		public InvalidAssignmentTypeException(VariableDeclaration declaration, string actual)
			: base($"Attempted to assign type <{actual}> to variable of type <{declaration.TypeIdentifier}> for '{declaration.Value}'")
		{
		}
	}
}