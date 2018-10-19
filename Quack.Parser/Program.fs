namespace Quack.Parser
module internal Program =
    open System
    open Types
    open Parser

    let mutable Indentation : int = 0

    (* ENTRY POINT *)
    let rec PrintNode (node:AstNode) = 
        printfn "%s[%s: %s]" (String.replicate Indentation " ") node.Type node.Value
        Indentation <- Indentation + 2
        node.Children |> List.iter (PrintNode)
        Indentation <- Indentation - 2

    //[<EntryPoint>]
    let main argv =
        let testTokens = [ 
            {Type = "DECLARE"; Value = "declare"};
            {Type = "IDENTIFIER"; Value = "apples"};
            {Type = "ASSIGN_OP"; Value = "="}; 
            {Type = "IDENTIFIER"; Value = "oranges"};
            {Type = "ARITHMETIC_OP"; Value = "+"};
            {Type = "OPEN_PARENTHESES"; Value = "("}; 
            {Type = "IDENTIFIER"; Value = "oranges"};
            {Type = "ARITHMETIC_OP"; Value = "+"};
            {Type = "NUMBER"; Value = "4"};
            {Type = "CLOSE_PARENTHESES"; Value = ")"};
            {Type = "ARITHMETIC_OP"; Value = "+"};
            {Type = "IDENTIFIER"; Value = "pears"};
            {Type = "ST_END"; Value = ";"}; 
            {Type = "DECLARE"; Value = "declare"};
            {Type = "IDENTIFIER"; Value = "apples"};
            {Type = "ASSIGN_OP"; Value = "="}; 
            {Type = "IDENTIFIER"; Value = "pears"};
            {Type = "ST_END"; Value = ";"}; 
        ]
        //let parsed = Statements testTokens
        //PrintNode parsed

        Console.ReadKey() |> ignore

        0 // Return an integer exit code

