namespace Quack.Parser
module Types = 

    type Token = 
        {
            Type:string
            Value:string
        }

    type AstNode = 
        {
            Type:string
            Value:string
            TypeIdentifier: string
            Children:List<AstNode>
        }