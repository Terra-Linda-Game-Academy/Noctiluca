using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class BaseConsoleParameters {

    [BaseConsoleParameter(typeof(Vector3), "vector3[x(float) y(float) x(float)]")]
    public static ConsoleArgument ConsoleConvertVector3(string[] args) {
        //                                         used 3 args  |                    |                    |   so 3
        return new ConsoleArgument(new Vector3(float.Parse(args[0]),float.Parse(args[1]),float.Parse(args[2])), 3);
    }

    [BaseConsoleParameter(typeof(int), "int")]
    public static ConsoleArgument ConsoleConvertInt(string[] args) {
        return new ConsoleArgument(int.Parse(args[0]), 1);
    }

    [BaseConsoleParameter(typeof(float), "float")]
    public static ConsoleArgument ConsoleConvertFloat(string[] args) {
        return new ConsoleArgument(float.Parse(args[0]), 1);
    }

    [BaseConsoleParameter(typeof(bool), "bool")]
    public static ConsoleArgument ConsoleConvertBool(string[] args) {
        return new ConsoleArgument(bool.Parse(args[0]), 1);
    }


    [BaseConsoleParameter(typeof(string), "string")]
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

    [BaseConsoleParameter(typeof(Color), "color[r(int) g(int) b(int)]")]
    public static ConsoleArgument ConsoleConvertColor(string[] args)
    {
        //Color color = (Color)typeof(Color).GetProperty(args[0].ToLowerInvariant()).GetValue(null, null);
        float r = int.Parse(args[0]) / 255.0f;
        float g = int.Parse(args[1]) / 255.0f;
        float b = int.Parse(args[2]) / 255.0f;
        return new ConsoleArgument(new Color(r,g,b), 3);
    }

    [BaseConsoleParameter(typeof(GameObject), "selected gameobject")]
    public static ConsoleArgument ConsoleConvertGameObject(string[] args)
    {
        return new ConsoleArgument(DebugController.SelectedGameObject, 0);
    }
//spawnmob bob 1.123 31 5 50 100 12

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
    public string format;
    public BaseConsoleParameter(Type type, string format) {
        this.type = type;
        this.format = format;
    }
}