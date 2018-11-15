namespace Quack.Parser
module Expressions = 
    open Types
    open Atoms

    (* Atoms *)
    let (|Identifier|_|) (token:Token)  =
        match token with
        | IDENTIFIER id -> Some(IdentifierNode id)
        | _ -> None

    let (|Number|_|) (token:Token)  =
        match token with
        | NUMBER n -> Some(NumberNode n)
        | _ -> None
        
    let (|BooleanConstant|_|) (token:Token)  =
        match token with
        | BOOLEAN_CONSTANT n -> Some(BooleanConstantNode n)
        | _ -> None

    let (|AtomicValue|_|) (token:Token)  =
        match token with
        | BooleanConstant node -> Some(node)
        | Identifier node -> Some(node)
        | Number node -> Some(node)
        | _ -> None
 
    (* Expressions *)             
        
    let rec (|Expression|_|) (stream:List<Token>)  =
        match stream with
        | BooleanLogicExpression(node, tail) -> Some(node, tail)
        | _ -> None
    
    and (|BooleanLogicExpression|_|) (stream:List<Token>)  =
        match stream with
        | BooleanEqualityExpression(left, BOOLEAN_LOGIC_OPERATOR op :: (BooleanEqualityExpression(right, tail))) -> Some(BooleanLogicNode(left, op, right), tail)
        | BooleanEqualityExpression(node, tail) -> Some(node, tail)
        | _ -> None
    
    and (|BooleanEqualityExpression|_|) (stream:List<Token>)  =
        match stream with
        | BooleanRelationalExpression(left, BOOLEAN_EQUALITY_OPERATOR op :: (BooleanRelationalExpression(right, tail))) -> Some(BooleanEqualityNode(left, op, right), tail)
        | BooleanRelationalExpression(node, tail) -> Some(node, tail)
        | _ -> None

    and (|BooleanRelationalExpression|_|) (stream:List<Token>)  =
        match stream with
        | ArithmeticExpression(left, BOOLEAN_RELATIONAL_OPERATOR op :: (ArithmeticExpression(right, tail))) -> Some(BooleanRelationalNode(left, op, right), tail)
        | ArithmeticExpression(node, tail) -> Some(node, tail)
        | _ -> None
        
    and (|ArithmeticExpression|_|) (stream:List<Token>)  =
        match stream with
        | Factor(left, ARITHMETIC_OPERATOR op :: (Factor(right, tail))) -> Some(ArithmeticNode(left, op, right), tail)
        | Factor(node, tail) -> Some(node, tail)
        | _ -> None

    and (|Factor|_|) (stream:List<Token>)  =
        match stream with
        | BOOLEAN_UNARY_OPERATOR op :: Factor(node, tail) -> Some(BooleanUnaryNode(op, node), tail)
        | OPEN_PARENTHESES :: (Expression(inner, CLOSE_PARENTHESES :: tail)) -> Some(FactorNode(inner), tail)
        | FunctionInvoke (node, tail) -> Some(node, tail)
        | AtomicValue node :: tail -> Some(node, tail)
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
