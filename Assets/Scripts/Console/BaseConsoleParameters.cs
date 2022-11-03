using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class BaseConsoleParameters {

    [BaseConsoleParameter(typeof(Vector3))]
    public static ConsoleArgument ConsoleConvertVector3(string[] args) {
        //                                         used 3 args  |                    |                    |   so 3
        return new ConsoleArgument(new Vector3(float.Parse(args[0]),float.Parse(args[1]),float.Parse(args[2])), 3);
    }

    [BaseConsoleParameter(typeof(int))]
    public static ConsoleArgument ConsoleConvertInt(string[] args) {
        return new ConsoleArgument(int.Parse(args[0]), 1);
    }

    [BaseConsoleParameter(typeof(string))]
    public static ConsoleArgument ConsoleConvertString(string[] args) {
        string content = string.Join(" ", args);
        string output = "";
        int index = 0;

        int secondQuotationIndex = content.Substring(1).IndexOf('"');

        if(content.StartsWith("\"") && secondQuotationIndex != -1) {
            output = content.Substring(1,secondQuotationIndex);
            index = output.Split(" ").Length;
        } else {
            output = args[0];
            index++;
        }

        return new ConsoleArgument(output, index);
    }
    // }
}

// public interface BaseConsoleParameter : CustomConsoleParameter
// {
//     public static Type type {get;}
// }

// public class IntConsoleParameter : BaseConsoleParameter {
//     public static string ConsoleFormat {get {return "<name[string] age[int] quote[string]>";}}
//     public static ConsoleArgument ConsoleConvert(string[] args) {
        
//         return new ConsoleArgument(new Person(args[0], int.Parse(args[1]), args[2]), 3);
//     }
// }

[AttributeUsage(AttributeTargets.Method)]
public class BaseConsoleParameter : System.Attribute {
    public Type type;
    public BaseConsoleParameter(Type type) {
        this.type = type;
    }
}