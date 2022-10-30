using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using JetBrains.Annotations;
using UnityEditor;

public class DebugController : MonoBehaviour
{
    bool showConsole;

    string input = "";

    public static DebugCommand QUIT;
    public static DebugCommand<int> RANDOM_NUMBER;
    public static DebugCommand PING;
    public static DebugCommand TEST;
    public static DebugCommand HELP;

    public static List<object> commandList;

    public List<string> commandHistory = new List<string>();
    private int commandHistoryIndex = -1;
    private const int MAX_COMMAND_HISTORY = 30;

    public List<string> consoleLog = new List<string>();

    public List<ParameterConverter> paramConverters = new List<ParameterConverter>();

    


    //private GameObject portalCamera;
    //private GameObject portalScreen;

    //example of how to create console command

    [ConsoleCommand("wow","ok")]
    public string TestFunction(string s, int i, string x) {
        Debug.Log("Now: ");
        Debug.Log("IT WOrked!"+i);
        return s + i + x;// <----- That is what it will print to in game console
    }

    

    public void OnToggleDebug()
    {
        showConsole = !showConsole;
        //SetButtonsActive(!showConsole);
        commandHistoryIndex = -1;
    }

    // private void SetButtonsActive(bool active)
    // {
    //     Button[] components = GameObject.FindObjectsOfType<Button>();
    //     for (int i = 0; i < components.Length; i++)
    //         components[i].enabled = active;
    // }

    public void OnReturn()
    {
        if (showConsole && input != "")
        {
            AddToConsoleLog(input, true, true);
            AddToConsoleHistory(input);
            try
            {
                HandleInput();
            }
            catch (Exception ex) { }
            input = "";
        }
    }

    public string GenerateFormat(string commandName, ParameterInfo[] parameterInfo) {
        string output = commandName;
        
        foreach(ParameterInfo pi in parameterInfo) {
            output += " <"+pi.ParameterType.Name + ">";
        }

        return output;
    }
    
    public object[] ParametersFromString(string[] arugments, ParameterInfo[] parameterInfo) {
        List<object> objects = new List<object>();
        try {
            
            
            int argumentIndex = 0;
            foreach(ParameterInfo pi in parameterInfo) {
                Type parameterType = pi.ParameterType;
                if(parameterType == typeof(int)) {
                    objects.Add(int.Parse(arugments[argumentIndex]));
                    argumentIndex++;
                    //Debug.Log("added int: " + int.Parse(arugments[argumentIndex]));
                } else if(parameterType == typeof(string)) {
                    string argument = arugments[argumentIndex];
                    if(argument.StartsWith('"')) {
                        string total = argument.Substring(1);
                        if(total.EndsWith('"')) {
                            //Debug.Log("added string: " + total);
                            objects.Add(total);
                            break;
                        }
                        argumentIndex++;
                        while (!(total.EndsWith('"'))) {
                            total += " " + arugments[argumentIndex];
                            argumentIndex++;
                        }
                        //Debug.Log("added string: " + total);
                        total=total.Substring(0,total.Length-1);
                        objects.Add(total);
                        
                    } else {
                        //Debug.Log("added string: " + arugments[argumentIndex]);
                        objects.Add(arugments[argumentIndex]);
                        argumentIndex++;
                    }
                }else if(parameterType == typeof(Vector3)) {
                    objects.Add(new Vector3(float.Parse(arugments[argumentIndex]),float.Parse(arugments[argumentIndex+1]),float.Parse(arugments[argumentIndex+2])));
                    argumentIndex+=3;
                    //Debug.Log("added vector: ");
                }
                else {
                    objects.Add(null);
                }
            }
        } catch(Exception e) {
            Debug.Log(e.Message);
        }
        return objects.ToArray();
    }


    public void Awake()
    {
        

        QUIT = new DebugCommand("quit", "quits", "quit", () =>
        {
            Application.Quit();
        });

        TEST = new DebugCommand("test", "Runs a test function", "test", () =>
        {
            //Test code here
            AddToConsoleLog("Finished Test Function");
        });

        PING = new DebugCommand("ping", "Gets your ping to the server.", "ping", () =>
        {
            AddToConsoleLog("pong");
        });

        //Example
        RANDOM_NUMBER = new DebugCommand<int>("random_number", "Prints a random number up to your input", "random_number <max>", (max) =>
        {
            AddToConsoleLog("You got: " + UnityEngine.Random.Range(0,max));
        });


        HELP = new DebugCommand("help", "Displays a list of a the commands", "help", () =>
        {
            for (int i = 0; i < commandList.Count; i++)
            {

                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string line = $"{command.commandFormat} - {command.commandDescription}";

                AddToConsoleLog(line, false);

            }
            AddToConsoleLog("---", false);
        });

        commandList = new List<object>()
        {
            QUIT,
            PING,
            RANDOM_NUMBER,
            TEST,
            HELP,
        };


        long startTime = DateTime.Now.Millisecond;

        //Adds the Attributed Commands
        //Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        Assembly assembly =  Assembly.GetExecutingAssembly();

        //foreach(Assembly assembly in assemblies) {

        System.Type[] types = assembly.GetTypes();

        foreach(Type type in types) {

            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            MemberInfo[] members = type.GetMembers(flags);

            foreach (MemberInfo member in members) {

                if(member.GetCustomAttributes(false).Length > 0) {
                    ConsoleCommand attribute = member.GetCustomAttribute<ConsoleCommand>();
                    if(attribute != null) {
                        if(member.MemberType == MemberTypes.Method) {
                            MethodInfo methodInfo = (MethodInfo) member;
                            ParameterInfo[] parameterInfo = methodInfo.GetParameters();
                            Type classType = member.DeclaringType;
                            Debug.Log("tryin to run 0" + attribute.id);
                            //take parameter types, use function to convert string into those type and pass it through
                            commandList.Add(new DebugCommand<string[]>(attribute.id, attribute.description, GenerateFormat(attribute.id, parameterInfo), (x) => {
                                Debug.Log("tryin to run 1");
                                List<string> arguments = new List<string>(x);
                                arguments.RemoveAt(0);
                                Debug.Log("tryin to run 2");
                                object[] parameters = ParametersFromString(arguments.ToArray(), parameterInfo);
                                Debug.Log("parameters: " + parameters.Length);
                                AddToConsoleLog(methodInfo.Invoke(GameObject.FindObjectOfType(classType),parameters)+"");}));
                            // if(parameterInfo.Length==1) {
                            //     Type parameterType = parameterInfo[0].ParameterType;
                            //     if(parameterType == typeof(int))
                            //         commandList.Add(new DebugCommand<int>(attribute.id, attribute.description, GenerateFormat(attribute.id, parameterInfo), (x) => {AddToConsoleLog(methodInfo.Invoke(GameObject.FindObjectOfType(classType), new object[1]{x})+"");}));
                            // } else {
                            //     commandList.Add(new DebugCommand(attribute.id, attribute.description, GenerateFormat(attribute.id, parameterInfo), () => {AddToConsoleLog(methodInfo.Invoke(GameObject.FindObjectOfType(classType),null)+"");}));
                            // }
                        }
                        
                    } 
                }
            }
            //}

        }
        Debug.Log("Took: " + (DateTime.Now.Millisecond - startTime)+" milliseconds");
    }





    private void AddToConsoleLog(string line, bool timestamp = true, bool fromUser = false)
    {
        consoleLog.Add("(" + (fromUser ? "U" : "C") + ") " + (timestamp ? ($"[{System.DateTime.Now.ToLongTimeString()}] ") : "") + line);
        scroll.y = 20 * (consoleLog.Count - 1);
    }

    private void AddToConsoleHistory(string line)
    {
        commandHistory.Insert(0, input);
        if (commandHistory.Count > MAX_COMMAND_HISTORY)
        {
            commandHistory.RemoveAt(commandHistory.Count - 1);
        }
    }

    Vector2 scroll;


    int suggestionIndex = 0;
    public void OnGUI()
    {

        if (!showConsole) return;

        float y = 0f;

        //Console Log

        GUI.Box(new Rect(0, y, Screen.width, 100), "");

        Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * consoleLog.Count);

        scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

        for (int i = 0; i < consoleLog.Count; i++)
        {

            string line = consoleLog[i];

            Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

            GUI.Label(labelRect, line);

        }

        GUI.EndScrollView();

        y += 100;


        //Delete old logs
        while (consoleLog.Count > 100)
        {
            consoleLog.RemoveAt(0);
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

        y += 30;

        if (input != "")
        {
            string[] suggestions = GetCommandSuggestions(input);

            foreach (string suggestion in suggestions)
            {
                GUI.Box(new Rect(0, y, Screen.width, 20), "");
                GUI.backgroundColor = new Color(0, 0, 0, 0);
                GUI.TextArea(new Rect(0, y, Screen.width, 20), suggestion);
                y += 20;
            }


        }

        // if (GUI.Button(new Rect(Screen.width - 200f, y + 5f, 125f, 20f), "Enter"))
        // {
        //   OnReturn();
        //}

        Event e = Event.current;
        if (!e.isKey) return;

        //if (e.keyCode == KeyCode.T || e.keyCode == KeyCode.BackQuote)
        //{
        //    OnToggleDebug();
        //}

        if (e.keyCode == KeyCode.Return)
        {
            OnReturn();
        }

        if(e.keyCode == KeyCode.Tab)
        {
            string[] commandSuggestions = GetCommandSuggestions(input);
            if(commandSuggestions.Length > 0)
            {
                input = commandSuggestions[0];
            }
        }


        if (e.keyCode == KeyCode.UpArrow)
        {
            commandHistoryIndex += 1;
            if (commandHistoryIndex >= commandHistory.Count) { commandHistoryIndex = 0; }
            input = commandHistory.ToArray()[commandHistoryIndex];
        }

        else if (e.keyCode == KeyCode.DownArrow)
        {
            commandHistoryIndex -= 1;
            if (commandHistoryIndex < 0) { commandHistoryIndex = commandHistory.Count - 1; }
            input = commandHistory.ToArray()[commandHistoryIndex];
        }


    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            OnToggleDebug();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Cursor.visible == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }



    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                } 
                else if (commandList[i] as DebugCommand<string[]> != null)
                {
                    (commandList[i] as DebugCommand<string[]>).Invoke(properties);
                }
            }
        }

    }

    private string[] GetCommandSuggestions(string command)
    {
        string[] properties = input.Split(' ');

        List<string> suggestions = new List<string>();

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (commandBase.commandId.Contains(command))
            {
                suggestions.Add(commandBase.commandFormat);
            }
        }

        return suggestions.ToArray();
    }

}

public class DebugCommandBase
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;

    public string commandId { get { return _commandId; } }
    public string commandDescription { get { return _commandDescription; } }
    public string commandFormat { get { return _commandFormat; } }

    public DebugCommandBase(string id, string description, string format)
    {
        _commandId = id;
        _commandDescription = description;
        _commandFormat = format;
    }
}

public class DebugCommand : DebugCommandBase
{
    private Action command;

    public DebugCommand(string id, string description, string format, Action command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class DebugCommandTest : DebugCommandBase
{
    private Action<string> command;

    public DebugCommandTest(string id, string description, string format, Action<string> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(string value)
    {
        command.Invoke(value);
    }
}

public class DebugCommand<T1> : DebugCommandBase
{
    private Action<T1> command;

    public DebugCommand(string id, string description, string format, Action<T1> command) : base(id, description, format)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}


[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommand : System.Attribute {
    public string id;
    public string description; 
    public ConsoleCommand(string id, string description) {
        this.id=id;
        this.description=description;
        }
}




public class ParameterConverter {
    public Type type;
    public Action<string> convert;
    public ParameterConverter(Type type, Action<string> convert) {
        this.type = type;
        this.convert = convert;
    }
}

