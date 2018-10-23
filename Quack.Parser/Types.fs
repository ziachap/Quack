namespace Quack.Parser
module Types = 

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