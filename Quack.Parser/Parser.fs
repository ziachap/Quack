﻿namespace Quack.Parser
module public Parser = 
    open Types
    open Atoms
    open Expressions

    (* Statements *)  
    let (|AssignStatement|_|) (stream:List<Token>) =
        match stream with
        | Identifier id :: ASSIGN _ :: (Expression(exp, tail)) -> Some(AssignNode(id, exp), tail)
        | _ -> None

    let (|DeclareStatement|_|) (stream:List<Token>) =
        match stream with
        | VAR_DECLARE :: AssignStatement(node, tail) -> Some(DeclareNode(node), tail)
        | VAR_DECLARE :: Identifier(node) :: tail -> Some(DeclareNode(node), tail)
        | _ -> None

    let (|PrintStatement|_|) (stream:List<Token>) =
        match stream with
        | PRINT :: (Expression(exp, tail)) -> Some(PrintNode(exp), tail)
        | _ -> None
       

    let rec (|Statements|_|) (stream:List<Token>)  =
        match stream with
        | Statement(node, []) -> Some(FinalStatementNode(node), [])
        // TODO: Is there a way to get the close braces out of here and into EnclosedStatements?
        | Statement(node, CLOSE_BRACES :: tail) -> Some(FinalStatementNode(node), tail)
        | Statement(node, Statements(next, tail)) -> Some(StatementNode(node, next), tail)
        | _ -> failwith ("Cannot parse statement starting: " + stream.Head.Type)
     
        
    and (|Statement|_|) (stream:List<Token>)  =
        match stream with
        | FunctionDeclaration (node, tail) -> Some(node,  tail)
        | IfStatement (node, tail) -> Some(node,  tail)
        | WhileStatement (node, tail) -> Some(node,  tail)
        | DeclareStatement (node, STATEMENT_END :: tail) -> Some(node,  tail)
        | PrintStatement (node, STATEMENT_END :: tail) -> Some(node,  tail)
        | AssignStatement (node, STATEMENT_END :: tail) -> Some(node,  tail)
        | FunctionInvoke (node, STATEMENT_END :: tail) -> Some(node,  tail)
        | _ -> None
    
    // CONTROL FLOW
    and (|IfStatement|_|) (stream:List<Token>) =
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
        | OPEN_PARENTHESES :: (BooleanExpression(node, CLOSE_PARENTHESES :: tail)) -> Some(node, tail)
        | _ -> None

    and (|EnclosedStatements|_|) (stream:List<Token>)  =
        match stream with
        | OPEN_BRACES :: (Statements(node, tail)) -> Some(node, tail)
        | _ -> None
    
    // FUNCTIONS
    and (|FunctionDeclaration|_|) (stream:List<Token>) =
        match stream with
        | FUNC_DECLARE :: Identifier id :: OPEN_PARENTHESES :: CLOSE_PARENTHESES:: EnclosedStatements(funcStmts, tail) -> 
            Some(FuncDefNode(id, funcStmts, []), tail)
        | FUNC_DECLARE :: Identifier id :: OPEN_PARENTHESES :: (Declarations(parametersNode, CLOSE_PARENTHESES :: EnclosedStatements(funcStmts, tail))) -> 
            Some(FuncDefNode(id, funcStmts, parametersNode), tail)
        | _ -> None

    and (|Declarations|_|) (stream:List<Token>)  =
        match stream with
        | (DeclareStatement(node, PARAM_DELIMITER :: Declarations(nextDeclarations, tail))) -> 
            Some(List.append [node] nextDeclarations, tail)
        | (DeclareStatement(node, tail)) -> 
            Some([node], tail)
        | _ -> None
        
    and (|FunctionInvoke|_|) (stream:List<Token>) =
        match stream with
        | Identifier id :: OPEN_PARENTHESES :: CLOSE_PARENTHESES :: tail -> Some(FuncInvokeNode(id, []), tail)
        | Identifier id :: OPEN_PARENTHESES :: FunctionParams(parameterNodes, CLOSE_PARENTHESES :: tail) -> 
            Some(FuncInvokeNode(id, parameterNodes), tail)
        | _ -> None 

    and (|FunctionParams|_|) (stream:List<Token>)  =
        match stream with
        | (Expression(node, PARAM_DELIMITER :: FunctionParams(next, tail))) -> Some(List.append [node] next, tail)
        | (Expression(node, tail)) -> Some([node], tail)
        | _ -> None

    let Parse (stream:List<Token>) =
        match stream with
        | Statements(node, _) -> node
        | _ -> failwith "Cannot parse statements"