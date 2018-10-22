namespace Quack.Parser
module Atoms =
    open Types

    (* Token Recognizers *)
    let (|IDENTIFIER|_|) (token:Token) = if token.Type = "IDENTIFIER" then Some(token) else None
    let (|NUMBER|_|) (token:Token) = if token.Type = "NUMBER" then Some(token) else None
    let (|BOOLEAN_CONSTANT|_|) (token:Token) = if token.Type = "BOOLEAN_CONSTANT" then Some(token) else None

    let (|VAR_DECLARE|_|) (token:Token) = if token.Type = "VAR_DECLARE" then Some() else None
    let (|PRINT|_|) (token:Token) = if token.Type = "PRINT" then Some() else None
    let (|ASSIGN|_|) (token:Token) = if token.Type = "ASSIGN" then Some(token) else None

    let (|BOOLEAN_RELATIONAL_OPERATOR|_|) (token:Token) = if token.Type = "BOOLEAN_RELATIONAL_OPERATOR" then Some(token) else None
    let (|BOOLEAN_EQUALITY_OPERATOR|_|) (token:Token) = if token.Type = "BOOLEAN_EQUALITY_OPERATOR" then Some(token) else None
    let (|BOOLEAN_LOGIC_OPERATOR|_|) (token:Token) = if token.Type = "BOOLEAN_LOGIC_OPERATOR" then Some(token) else None
    let (|BOOLEAN_UNARY_OPERATOR|_|) (token:Token) = if token.Type = "BOOLEAN_UNARY_OPERATOR" then Some(token) else None

    let (|ARITHMETIC_OPERATOR|_|) (token:Token) = if token.Type = "ARITHMETIC_OPERATOR" then Some(token) else None
    let (|STATEMENT_END|_|) (token:Token) = if token.Type = "STATEMENT_END" then Some() else None
    let (|OPEN_PARENTHESES|_|) (token:Token) = if token.Type = "OPEN_PARENTHESES" then Some() else None
    let (|CLOSE_PARENTHESES|_|) (token:Token) = if token.Type = "CLOSE_PARENTHESES" then Some() else None
    let (|OPEN_BRACES|_|) (token:Token) = if token.Type = "OPEN_BRACES" then Some() else None
    let (|CLOSE_BRACES|_|) (token:Token) = if token.Type = "CLOSE_BRACES" then Some() else None
    let (|IF|_|) (token:Token) = if token.Type = "IF" then Some() else None
    let (|ELSE|_|) (token:Token) = if token.Type = "ELSE" then Some() else None
    let (|WHILE|_|) (token:Token) = if token.Type = "WHILE" then Some() else None
    let (|FUNC_DECLARE|_|) (token:Token) = if token.Type = "FUNC_DECLARE" then Some() else None
    let (|RETURN|_|) (token:Token) = if token.Type = "RETURN" then Some() else None
    let (|LAMBDA_OPERATOR|_|) (token:Token) = if token.Type = "LAMBDA_OPERATOR" then Some() else None
    let (|PARAM_DELIMITER|_|) (token:Token) = if token.Type = "PARAM_DELIMITER" then Some() else None
    let (|INT|_|) (token:Token) = if token.Type = "INT" then Some(token) else None
    let (|BOOL|_|) (token:Token) = if token.Type = "BOOL" then Some(token) else None

    (* Ast Nodes *)
    let NumberNode (number:Token) : AstNode =
        { Type = "NUMBER"; Value = number.Value; TypeIdentifier = "int"; Children = [] }

    let BooleanConstantNode (boolean:Token) : AstNode =
        { Type = "BOOLEAN_CONSTANT"; Value = boolean.Value; TypeIdentifier = "bool"; Children = [] }

    let IdentifierNode (identifier:Token) : AstNode =
        { Type = "IDENTIFIER"; Value = identifier.Value; TypeIdentifier = null; Children = [] }
        
    let TypeIdentifierNode (identifier:Token) : AstNode =
        { Type = "TYPE_IDENTIFIER"; Value = identifier.Value; TypeIdentifier = null; Children = [] }

    let DeclareNode (child:AstNode, typeNode:AstNode) : AstNode =
        { Type = "DECLARE"; Value = child.Value; TypeIdentifier = typeNode.Value; Children = [child] }

    let ArithmeticNode (left:AstNode, op:Token, right:AstNode) : AstNode =
        { Type = "ARITHMETIC_OPERATOR"; Value = op.Value; TypeIdentifier = "int"; Children = [left; right] }
        
    let BooleanNode (left:AstNode, op:Token, right:AstNode) : AstNode =
        { Type = "BOOLEAN_OPERATOR"; Value = op.Value; TypeIdentifier = "bool"; Children = [left; right] }

    let BooleanUnaryNode (op:Token, node:AstNode) : AstNode =
        { Type = "BOOLEAN_UNARY_OPERATOR"; Value = op.Value; TypeIdentifier = "bool"; Children = [node] }

    let FactorNode (exp:AstNode) : AstNode =
        { Type = "FACTOR"; Value = null; TypeIdentifier = null; Children = [exp] }
        
    let PrintNode (exp:AstNode) : AstNode =
        { Type = "PRINT"; Value = null; TypeIdentifier = null; Children = [exp] }

    let AssignNode (identifier:AstNode, exp:AstNode) : AstNode =
        { Type = "ASSIGN"; Value = identifier.Value; TypeIdentifier = null; Children = [identifier; exp] }
        
    let StatementBlockNode (statements:List<AstNode>) : AstNode =
        { Type = "STATEMENT_BLOCK"; Value = null; TypeIdentifier = null; Children = statements }
                    
    let IfNode (exp:AstNode, ifStatements:AstNode) : AstNode =
        { Type = "IF_ELSE"; Value = null; TypeIdentifier = null; Children = [exp; ifStatements] }
        
    let IfElseNode (exp:AstNode, ifStatements:AstNode, elseStatements:AstNode) : AstNode =
        { Type = "IF_ELSE"; Value = null; TypeIdentifier = null; Children = [exp; ifStatements;  elseStatements] }

    let WhileNode (exp:AstNode, whileStatements:AstNode) : AstNode =
        { Type = "WHILE"; Value = null; TypeIdentifier = null; Children = [exp; whileStatements] }

    let FuncDefNode (identifier:AstNode, funcStatements:AstNode, funcParams:List<AstNode>) : AstNode =
        { Type = "FUNC_DEF"; Value = identifier.Value; TypeIdentifier = null; Children = List.append [funcStatements] funcParams }
                
    let FuncInvokeNode (identifier:AstNode, funcParams:List<AstNode>) : AstNode =
        { Type = "FUNC_INVOKE"; Value = identifier.Value; TypeIdentifier = null; Children = funcParams }

    let FuncReturnExpNode (exp:AstNode) : AstNode =
        { Type = "FUNC_RETURN"; Value = null; TypeIdentifier = null; Children = [exp] }

    let FuncReturnNode () : AstNode =
        { Type = "FUNC_RETURN"; Value = null; TypeIdentifier = null; Children = [] }