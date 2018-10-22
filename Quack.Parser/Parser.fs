namespace Quack.Parser

module public rec Parser = 
    open Types
    open Atoms
    open Expressions

    let (|TypeIdentifier|_|) (token:Token) =
        match token with
        | INT node | BOOL node -> Some(TypeIdentifierNode node)
        | VAR_DECLARE -> Some(TypeIdentifierNode(token))
        | Identifier node -> Some(node)
        | _ -> None

    (* Statements *)  
    let (|AssignStatement|_|) (stream:List<Token>) =
        match stream with
        | Identifier id :: ASSIGN _ :: (Expression(exp, tail)) -> Some(AssignNode(id, exp), tail)
        | _ -> None

    let (|DeclareStatement|_|) (stream:List<Token>) =
        match stream with
        | TypeIdentifier typeNode :: AssignStatement(node, tail) -> Some(DeclareNode(node, typeNode), tail)
        | TypeIdentifier typeNode :: Identifier(node) :: tail -> Some(DeclareNode(node, typeNode), tail)
        | _ -> None

    let (|PrintStatement|_|) (stream:List<Token>) =
        match stream with
        | PRINT :: (Expression(exp, tail)) -> Some(PrintNode(exp), tail)
        | _ -> None
       
    let (|ReturnStatement|_|) (stream:List<Token>) =
        match stream with
        | RETURN :: (Expression(exp, tail)) -> Some(FuncReturnExpNode(exp), tail)
        | RETURN :: tail -> Some(FuncReturnNode(), tail)
        | _ -> None

    let rec (|StatementBlock|_|) (stream:List<Token>)  =
        match stream with
        | Statements(nodes, tail) -> Some(StatementBlockNode(nodes), tail)
        | _ -> None
      
    and (|Statements|_|) (stream:List<Token>)  =
        match stream with
        | Statement(node, []) -> Some([node], [])
        // TODO: Is there a way to get the close braces out of here and into EnclosedStatements?
        | Statement(node, CLOSE_BRACES :: tail) -> Some([node], tail)
        | Statement(node, Statements(next, tail)) -> Some(List.append [node] next, tail)
        | _ -> failwith ("Cannot parse statement starting: " + stream.Head.Type)
        
    and (|Statement|_|) (stream:List<Token>)  =
        match stream with
        | FunctionDeclaration (node, tail) -> Some(node,  tail)
        | IfStatement (node, tail) -> Some(node,  tail)
        | WhileStatement (node, tail) -> Some(node,  tail)
        | DeclareStatement (node, STATEMENT_END :: tail) -> Some(node,  tail)
        | PrintStatement (node, STATEMENT_END :: tail) -> Some(node,  tail)
        | ReturnStatement (node, STATEMENT_END :: tail) -> Some(node,  tail)
        | AssignStatement (node, STATEMENT_END :: tail) -> Some(node,  tail)
        | FunctionInvoke (node, STATEMENT_END :: tail) -> Some(node,  tail)
        | _ -> None
    
    // CONTROL FLOW
    let rec (|IfStatement|_|) (stream:List<Token>) =
        match stream with
        | IF :: (EnclosedBooleanExpression(exp, EnclosedStatements(ifStmts, ElseStatement(elseStmts, tail)))) ->
            Some(IfElseNode(exp, ifStmts, elseStmts), tail)
        | IF :: (EnclosedBooleanExpression(exp, EnclosedStatements(ifStmts, tail))) -> 
            Some(IfNode(exp, ifStmts), tail)
        | _ -> None
        
    and (|ElseStatement|_|) (stream:List<Token>) =
        match stream with
        | ELSE :: EnclosedStatements(elseStatementsNode, tail) -> Some(elseStatementsNode, tail)
        | _ -> None 

    and (|WhileStatement|_|) (stream:List<Token>) =
        match stream with
        | WHILE :: (EnclosedBooleanExpression(exp, EnclosedStatements(whileStmts, tail))) -> Some(WhileNode(exp, whileStmts), tail)
        | _ -> None

    and (|EnclosedBooleanExpression|_|) (stream:List<Token>)  =
        match stream with
        | OPEN_PARENTHESES :: (Expression(node, CLOSE_PARENTHESES :: tail)) -> Some(node, tail)
        | _ -> None

    and (|EnclosedStatements|_|) (stream:List<Token>)  =
        match stream with
        | OPEN_BRACES :: CLOSE_BRACES :: tail -> Some(StatementBlockNode([]), tail)
        | OPEN_BRACES :: (StatementBlock(node, tail)) -> Some(node, tail)
        | _ -> None
    
    // FUNCTIONS
    let rec (|FunctionDeclaration|_|) (stream:List<Token>) =
        match stream with
        | FUNC_DECLARE :: Identifier id :: OPEN_PARENTHESES :: CLOSE_PARENTHESES:: FunctionStatements(funcStmts, tail) -> 
            Some(FuncDefNode(id, funcStmts, []), tail)
        | FUNC_DECLARE :: Identifier id :: OPEN_PARENTHESES :: (FunctionDeclarationParameters(parametersNode, CLOSE_PARENTHESES :: FunctionStatements(funcStmts, tail))) -> 
            Some(FuncDefNode(id, funcStmts, parametersNode), tail)
        | _ -> None
    
    and (|FunctionStatements|_|) (stream:List<Token>) =
        match stream with
        | LAMBDA_OPERATOR :: Statement(node, tail) -> Some(StatementBlockNode([node]), tail)
        | EnclosedStatements(node, tail) -> Some(node, tail)
        | _ -> None

    and (|FunctionDeclarationParameters|_|) (stream:List<Token>)  =
        match stream with
        | (DeclareStatement(node, PARAM_DELIMITER :: FunctionDeclarationParameters(nextDeclarations, tail))) -> 
            Some(List.append [node] nextDeclarations, tail)
        | (DeclareStatement(node, tail)) -> 
            Some([node], tail)
        | _ -> None

    let Parse (stream:List<Token>) =
        match stream with
        | StatementBlock(node, _) -> node
        | _ -> failwith "Cannot parse statements"
