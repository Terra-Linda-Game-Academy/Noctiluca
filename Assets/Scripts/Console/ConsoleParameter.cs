using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public static class ParameterConversionExtensions
{


    // public static ConsoleArgument<Vector3> ConsoleConvert(this Vector3 v, string[] args) {
    //     //                                                  used 3 args  |                    |                    |   so 3
    //     return new ConsoleArgument<Vector3>(new Vector3(float.Parse(args[0]),float.Parse(args[1]),float.Parse(args[2])), 3);
    // }

    // public static ConsoleArgument<int> ConsoleConvert(this int i, string[] args) {
    //     return new ConsoleArgument<int>(int.Parse(args[0]), 1);
    // }

    // public static ConsoleArgument<string> ConsoleConvert(this string s, string[] args) {


    //     string content = string.Join(" ", args);
    //     string output = "";
    //     int index = 0;

    //     int secondQuotationIndex = content.Substring(1).IndexOf('"');

    //     if(content.StartsWith("\"") && secondQuotationIndex != -1) {
    //         output = content.Substring(1,secondQuotationIndex);
    //         index = output.Split(" ").Length;
    //     } else {
    //         output = args[0];
    //         index++;
    //     }

    //     return new ConsoleArgument<string>(output, index);
    // }
}

// public static class ParameterFormatExtensions
// {
//     // public static ParameterFormat ConsoleFormat(this Vector3 vector3) {

//     // }
// }

// public class ParameterFormat
// {
//     public string name;
//     public string format;
//     public ParameterFormat (string name, string format) {
//         this.name = name;
//         this.format = format;
//     }
// }

public class ConsoleArgument {
    public int lastIndexUsed;
    public object value;
    public ConsoleArgument(object value, int lastIndexUsed) {
        this.lastIndexUsed = lastIndexUsed;
        this.value = value;
    }
}

public interface CustomConsoleParameter
{
    public static string ConsoleFormat {get;}
    public static ConsoleArgument ConsoleConvert(string[] args){return null;}
}


//ex
public class Person : CustomConsoleParameter {
    public static string ConsoleFormat {get {return "<name[string] age[int] quote[string]>";}}
    public static ConsoleArgument ConsoleConvert(string[] args) {
        
        return new ConsoleArgument(new Person(args[0], int.Parse(args[1]), args[2]), 3);
    }

    public Person(string name, int age, string quote) {
        Debug.Log(name+age+quote);
    }
}





// public class ConsoleVector3 : CustomConsoleParameter {
//     public string Name {get {return "Vector3";}}
//     public string Format {get {return "x y z";}}
//     public ConsoleArgument Convert(string[] args) {
//         return new ConsoleArgument(new Vector3(float.Parse(args[0]),float.Parse(args[1]),float.Parse(args[2])), 3);
//     }
// }

