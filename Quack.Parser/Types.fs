namespace Quack.Parser
module Types = 
    open System

    type DebugInfo = 
        {
            Line:string
            LineNumber:int
        }

    type Token = 
        {
            Type:string
            Value:string
            Info:DebugInfo
        }

    type AstNode = 
        {
            Type:string
            Value:string
            TypeIdentifier: string
            Children:List<AstNode>
            Info:DebugInfo
        }

    type ParseException (token:Token) = 
        inherit Exception("Cannot parse statement starting: " + token.Type 
        + "\n\t[line:" + string(token.Info.LineNumber) + "] " + token.Info.Line)