namespace Quack.Parser
module Expressions = 
    open Types
    open Atoms

    (* Atoms *)
    let (|Identifier|_|) (token:Token)  =
        match token with
        | LABEL id -> Some(IdentifierNode id)
        | _ -> None

    let (|Number|_|) (token:Token)  =
        match token with
        | NUMBER n -> Some(NumberNode n)
        | _ -> None

    let (|AtomicValue|_|) (token:Token)  =
        match token with
        | Identifier node -> Some(node)
        | Number node -> Some(node)
        | _ -> None
 
    (* Expressions *)  
    let rec (|Expression|_|) (stream:List<Token>)  =
        match stream with
        | Arithmetic(node, tail) -> Some(node, tail)
        | Factor(node, tail) -> Some(node, tail)
        | AtomicValue node :: tail -> Some(node, tail)
        | _ -> None

    and (|Factor|_|) (stream:List<Token>)  =
        match stream with
        | OPEN_PARENTHESES :: (Expression(inner, CLOSE_PARENTHESES :: tail)) -> Some(FactorNode(inner), tail)
        | _ -> None

    and (|Arithmetic|_|) (stream:List<Token>)  =
        match stream with
        | AtomicValue left :: ARITHMETIC_OPERATOR op :: (Expression(right, tail)) -> Some(ArithmeticNode(left, op, right), tail)
        | Factor(left, ARITHMETIC_OPERATOR op :: (Expression(right, tail))) -> Some(ArithmeticNode(left, op, right), tail)
        | _ -> None

    and (|BooleanExpression|_|) (stream:List<Token>)  =
        match stream with
        | Expression(left, BOOLEAN_OPERATOR op :: (Expression(right, tail))) -> Some(BooleanNode(left, op, right), tail)
        | _ -> None