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

    public static List<object> commandList = new List<object>();

    public List<string> commandHistory = new List<string>();
    private int commandHistoryIndex = -1;
    private const int MAX_COMMAND_HISTORY = 30;

    public List<string> consoleLog = new List<string>();

    public static Dictionary<Type,(MethodInfo, string)> baseConsoleParameters = new Dictionary<Type,(MethodInfo, string)>();
    //public static List<BaseConsoleParameter> baseConsoleParameters = new List<BaseConsoleParameter>();

    [ConsoleCommand("test","ok")]
    public string TestFunction(string s, int i, string x) {
        return s + i + x;
    }

    public void OnToggleDebug()
    {
        showConsole = !showConsole;
        commandHistoryIndex = -1;
    }

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
            Type parameterType = pi.ParameterType;
            string format = parameterType.Name;
            try
            {
                if (typeof(CustomConsoleParameter).IsAssignableFrom(parameterType))
                {
                    format = (string)parameterType.GetField("ConsoleFormat", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                }
                else if (baseConsoleParameters.ContainsKey(parameterType))
                {
                    format = baseConsoleParameters[parameterType].Item2;
                }
            } catch (Exception ex) { Debug.LogError(ex); }
            

            output += " <"+ format + ">";
        }

        return output;
    }

    [ConsoleCommand("person", "creates person")]
    public string PersonF(Person person, int num) {
        return "There are "+num+""+person.name+"s";
    }

    public static ConsoleArgument StringToArgument(string[] arguments, Type parameterType)
    {
        ConsoleArgument ca = null;
        if (typeof(CustomConsoleParameter).IsAssignableFrom(parameterType))
        {
            ca = (ConsoleArgument)parameterType.GetMethod("ConsoleConvert").Invoke(null, new object[] { arguments });
        }
        else if (baseConsoleParameters.ContainsKey(parameterType))
        {
            ca = (ConsoleArgument)baseConsoleParameters[parameterType].Item1.Invoke(null, new object[] { arguments });
        }
        return ca;
    }
    
    public object[] ParametersFromString(string[] arguments, ParameterInfo[] parameterInfo) {
        List<object> objects = new List<object>();
        Debug.Log("Arguments " + arguments.Length);
        try {
            if(arguments.Length == 0) {
                foreach(ParameterInfo pi in parameterInfo) {
                    objects.Add(null);
                }
                return objects.ToArray();
            }

            int argumentIndex = 0;
            foreach(ParameterInfo pi in parameterInfo) {
                Type parameterType = pi.ParameterType;
                Debug.Log("Current Index " + argumentIndex);
                ConsoleArgument consoleArgument = StringToArgument(arguments[argumentIndex..arguments.Length], parameterType);
                if(consoleArgument != null)
                {
                    argumentIndex += consoleArgument.lastIndexUsed;
                    objects.Add(consoleArgument.value);
                } else {
                    objects.Add(null);
                }
                // }else if(parameterType == typeof(int)) {
                //     objects.Add(int.Parse(arguments[argumentIndex]));
                //     argumentIndex++;
                // } else if(parameterType == typeof(string)) {
                //     string argument = arguments[argumentIndex];
                //     if(argument.StartsWith('"')) {
                //         string total = argument.Substring(1);
                //         if(total.EndsWith('"')) {
                //             objects.Add(total);
                //             break;
                //         }
                //         argumentIndex++;
                //         while (!(total.EndsWith('"'))) {
                //             total += " " + arguments[argumentIndex];
                //             argumentIndex++;
                //         }
                //         total=total.Substring(0,total.Length-1);
                //         objects.Add(total);
                        
                //     } else {
                //         objects.Add(arguments[argumentIndex]);
                //         argumentIndex++;
                //     }
                    
                // }else if(parameterType == typeof(Vector3)) {
                //     objects.Add(new Vector3(float.Parse(arguments[argumentIndex]),float.Parse(arguments[argumentIndex+1]),float.Parse(arguments[argumentIndex+2])));
                //     argumentIndex+=3;
                // }
                //  else {

                // }
            }
        } catch(Exception e) {
            Debug.Log(e.Message);
        }
        
        return objects.ToArray();
    }


    public void Awake()
    {
        long startTime = DateTime.Now.Millisecond;

        Assembly assembly =  Assembly.GetExecutingAssembly();

        System.Type[] types = assembly.GetTypes();

        foreach(Type type in types) {

            BindingFlags flags = BindingFlags.Public | BindingFlags.Static| BindingFlags.NonPublic | BindingFlags.Instance;
            MemberInfo[] members = type.GetMembers(flags);

            foreach (MemberInfo member in members) {

                if(member.GetCustomAttributes(false).Length > 0) {
                    ConsoleCommand attribute = member.GetCustomAttribute<ConsoleCommand>();
                    if(attribute != null) {
                        if(member.MemberType == MemberTypes.Method) {
                            MethodInfo methodInfo = (MethodInfo) member;
                            ParameterInfo[] parameterInfo = methodInfo.GetParameters();
                            Type classType = member.DeclaringType;

                            //take parameter types, use function to convert string into those type and pass it through
                            commandList.Add(new DebugCommand<string[]>(attribute.id, attribute.description, GenerateFormat(attribute.id, parameterInfo), (x) => {
                                List<string> arguments = new List<string>(x);
                                arguments.RemoveAt(0);
                                object[] parameters = ParametersFromString(arguments.ToArray(), parameterInfo);
                                object script = null;
                                try{if(classType.IsSubclassOf(typeof(UnityEngine.Object))) script = GameObject.FindObjectOfType(classType);} catch(Exception e) {}
                                if (script == null && !methodInfo.IsStatic)
                                {
                                    script = Activator.CreateInstance(type, null);
                                } else if(methodInfo.IsStatic) {
                                    script = null;
                                }
                                AddToConsoleLog(methodInfo.Invoke(script, parameters)+"");}));
                        }
                        
                    }
                }
            }
        }

        foreach(MethodInfo methodInfo in typeof(BaseConsoleParameters).GetMethods()) {
            BaseConsoleParameter baseConsoleParameter = methodInfo.GetCustomAttribute<BaseConsoleParameter>();
            if(baseConsoleParameter != null) {
                baseConsoleParameters.Add(baseConsoleParameter.type, (methodInfo, baseConsoleParameter.format));
            }
        }
        Debug.Log("Took: " + (DateTime.Now.Millisecond - startTime)+" milliseconds");
    }





    private void AddToConsoleLog(string log, bool timestamp = true, bool fromUser = false)
    {
        string[] lines = log.Split(
            new string[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );
        foreach(string line in lines) {
            if(line =="") continue;
            consoleLog.Add("(" + (fromUser ? "U" : "C") + ") " + (timestamp ? ($"[{System.DateTime.Now.ToLongTimeString()}] ") : "") + line);
            scroll.y = 20 * (consoleLog.Count - 1);
        }
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

        Event e = Event.current;
        if (!e.isKey) return;

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

