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
using System.Text.RegularExpressions;
using System.Collections.Specialized;


public class ConsoleController : MonoBehaviour
{
    bool showConsole;
    string input = "";
    public static bool CheatsEnabled = false;


    public static List<string> consoleLog = new List<string>();
    public static List<ConsoleCommandHolder> commandList = new List<ConsoleCommandHolder>();
    public static List<string> commandHistory = new List<string>();
    private int commandHistoryIndex = -1;
    public int MAX_COMMAND_HISTORY = 30;
    public static Dictionary<Type,(MethodInfo, string)> baseConsoleParameters = new Dictionary<Type,(MethodInfo, string)>();


    

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

            HandleInput();
           
            input = "";
        }
    }
    public static string GetFormatOfParameter(Type parameterType) {
        string format = parameterType.Name;
        if (typeof(CustomConsoleParameter).IsAssignableFrom(parameterType))
        {
            format = (string)parameterType.GetField("ConsoleFormat", BindingFlags.Public | BindingFlags.Static).GetValue(null);
        }
        else if (baseConsoleParameters.ContainsKey(parameterType))
        {
            format = baseConsoleParameters[parameterType].Item2;
        } else
        {
            foreach (ConstructorInfo constructorInfo in parameterType.GetConstructors())
            {
                bool isUsableConstructor = true;
                foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                {
                    if (!IsConvertableType(parameterInfo.ParameterType))
                    {
                        isUsableConstructor = false;
                        break;
                    }
                }
                if (!isUsableConstructor)
                    continue;
                format = "";
                foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                {
                    format += $"{parameterInfo.Name} - <{GetFormatOfParameter(parameterInfo.ParameterType)}>, ";
                }
                format = format.Substring(0, format.Length - 2);

            }
        }
        return format;
    }

    public string GenerateFormat(string commandName, ParameterInfo[] parameterInfo) {
        string output = commandName;
        
        foreach(ParameterInfo pi in parameterInfo) {
            Type parameterType = pi.ParameterType;
            string format = parameterType.Name;

            format = GetFormatOfParameter(parameterType);
            

            output += " <"+ format + ">";
        }

        return output;
    }


    public static bool IsConvertableType(Type type)
    {
        return baseConsoleParameters.ContainsKey(type) || typeof(CustomConsoleParameter).IsAssignableFrom(type);
    }

    public static ConsoleArgument StringToArgument(string[] arguments, Type parameterType)
    {
        ConsoleArgument ca = new ConsoleArgument(null, 0);
        if (arguments.Length == 0 && parameterType != typeof(GameObject))
        {
            return ca;
        }
        else if (typeof(CustomConsoleParameter).IsAssignableFrom(parameterType))
        {
            ca = (ConsoleArgument)parameterType.GetMethod("ConsoleConvert").Invoke(null, new object[] { arguments });
        }
        else if (baseConsoleParameters.ContainsKey(parameterType))
        {
            ca = (ConsoleArgument)baseConsoleParameters[parameterType].Item1.Invoke(null, new object[] { arguments });
        } else
        {
            foreach(ConstructorInfo constructorInfo in parameterType.GetConstructors())
            {
                bool isUsableConstructor = true;
                foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                {
                    if (!IsConvertableType(parameterInfo.ParameterType))
                    {
                        isUsableConstructor = false;
                        break;
                    }
                }
                if (!isUsableConstructor)
                    continue;

                List<object> constructorArguments = new List<object>();
                int index = 0;
                foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                {
                    ConsoleArgument constructParam = StringToArgument(arguments[index..arguments.Length], parameterInfo.ParameterType);
                    index += constructParam.lastIndexUsed;
                    constructorArguments.Add(constructParam.value);
                }
                return new ConsoleArgument(Activator.CreateInstance(parameterType, constructorArguments.ToArray()), index);

            }
        }
        return ca;
    }
    
    public object[] ParametersFromString(string[] arguments, ParameterInfo[] parameterInfo) {
        List<object> objects = new List<object>();
        Debug.Log("Arguments " + arguments.Length);

        Type parameterType = null;
        int argumentIndex = 0;
        try {
            if(arguments.Length == 0) {
                foreach(ParameterInfo pi in parameterInfo) {
                    objects.Add(StringToArgument(new string[0], pi.ParameterType).value);
                }
                Debug.Log("O" + objects[0]);
                return objects.ToArray();
            }

            
            foreach(ParameterInfo pi in parameterInfo) {
                parameterType = pi.ParameterType;
                ConsoleArgument consoleArgument = StringToArgument(arguments[argumentIndex..arguments.Length], parameterType);
                
                if(consoleArgument != null)
                {
                    argumentIndex += consoleArgument.lastIndexUsed;
                    objects.Add(consoleArgument.value);
                } else {
                    objects.Add(null);
                }
            }
        } catch(Exception e) {
            Debug.Log(e.Message);
            AddToConsoleLog($"There was an issue when converting {String.Join(" ", arguments[argumentIndex..arguments.Length])} into type {parameterType.Name}");
        }
        
        return objects.ToArray();
    }


    public void Awake()
    {
        long startTime = DateTime.Now.Millisecond;

        Assembly assembly =  Assembly.GetExecutingAssembly();

        System.Type[] types = assembly.GetTypes();

        foreach(MethodInfo methodInfo in typeof(BaseConsoleParameters).GetMethods()) {
            BaseConsoleParameter baseConsoleParameter = methodInfo.GetCustomAttribute<BaseConsoleParameter>();
            if(baseConsoleParameter != null) {
                baseConsoleParameters.Add(baseConsoleParameter.type, (methodInfo, baseConsoleParameter.format));
            }
        }

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
                            commandList.Add(new ConsoleCommandHolder(attribute.id, attribute.description, GenerateFormat(attribute.id, parameterInfo), attribute.executionValue, attribute.requiresCheats, (x) => {
                                List<string> arguments = new List<string>(x);
                                arguments.RemoveAt(0);
                                object[] parameters = ParametersFromString(arguments.ToArray(), parameterInfo);
                                object script = null;
                                if (classType.IsSubclassOf(typeof(UnityEngine.Object))) script = GameObject.FindObjectOfType(classType);
                                if (script == null && !methodInfo.IsStatic)
                                {
                                    script = Activator.CreateInstance(type, null);
                                } else if(methodInfo.IsStatic) {
                                    script = null;
                                }
                                string output = methodInfo.Invoke(script, parameters) + "";
                                if (output == "")
                                {
                                    if (attribute.executionValue == "")
                                    {
                                        AddToConsoleLog(attribute.id + " executed succesfully");
                                    }
                                    else
                                    {
                                        //replaces {variables} with their parameter value
                                        Regex re = new Regex(@"\{([^\}]+)\}", RegexOptions.Compiled);
                                        string input = attribute.executionValue;
                                        StringDictionary fields = new StringDictionary();

                                        for(int i = 0; i < parameters.Length; i++) {
                                            fields.Add(parameterInfo[i].Name, parameters[i].ToString());
                                        }

                                        output = re.Replace(input, delegate (Match match) {
                                            return fields[match.Groups[1].Value];
                                        });

                                        AddToConsoleLog(output);
                                    }

                                }
                                else
                                {
                                    AddToConsoleLog(output);
                                }

                            }));
                        }
                        
                    }
                }
            }
        }

        Debug.Log("Took: " + (DateTime.Now.Millisecond - startTime)+" milliseconds");
    }





    public void AddToConsoleLog(string log, bool timestamp = true, bool fromUser = false)
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

    private Vector2 scroll;

    public static GameObject SelectedGameObject = null;


    // int suggestionIndex = 0;
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
            string[] suggestions = GetCommandSuggestions(input.Split(" ")[0]);

            foreach (string suggestion in suggestions)
            {
                GUI.contentColor = new Color(255, 255, 255, 255);
                GUI.backgroundColor = new Color(255, 0, 0, 255);
                if (GUI.Button(new Rect(0, y, Screen.width, 20), new GUIContent(suggestion))) {
                 input = suggestion.Split(" ")[0];
                }
                y += 20;
            }
        }

        if(SelectedGameObject != null) {
            GUI.color = new Color(0, 0, 0, 255);
            Rect hoveredRect = new Rect(5, y, viewport.width - 100, 20);
            GUI.Label(hoveredRect, $"{SelectedGameObject.name} - <{SelectedGameObject.GetInstanceID()}>");
        }
        

        Event e = Event.current;
        if(e.isMouse && e.type == EventType.MouseDown) {
            //Get Current Hovered Gameobject
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                SelectedGameObject = hit.transform.gameObject;
                
            } else {
                SelectedGameObject = null;
            }
        }

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
                input = commandSuggestions[0].Split(" ")[0];
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
        if (UnityEngine.Input.GetKeyDown(KeyCode.BackQuote))
        {
            OnToggleDebug();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.F1))
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
            ConsoleCommandHolder command = commandList[i];

            if (properties[0] == (command.commandId))
            {
                if (command.requiresCheats && !CheatsEnabled)
                {
                    AddToConsoleLog("This command requires cheats to be enabled, use \"cheats true\" to enable");
                    return;
                }
                command.Invoke(properties);


            }
        }

    }

    private string[] GetCommandSuggestions(string command)
    {
        string[] properties = input.Split(' ');

        List<string> suggestions = new List<string>();

        for (int i = 0; i < commandList.Count; i++)
        {
            ConsoleCommandHolder commandBase = commandList[i] as ConsoleCommandHolder;

            if (commandBase.commandId.Contains(command))
            {
                suggestions.Add(commandBase.commandFormat);
            }
        }

        return suggestions.ToArray();
    }

}

public class ConsoleCommandHolder
{
    private string _commandId;
    private string _commandDescription;
    private string _commandFormat;
    private string _executionValue;
    private bool _requiresCheats;
    private Action<string[]> command;

    public string commandId { get { return _commandId; } }
    public string commandDescription { get { return _commandDescription; } }
    public string commandFormat { get { return _commandFormat; } }
    public string executionValue { get { return _executionValue; } }
    public bool requiresCheats { get { return _requiresCheats; } }

    public void Invoke(string[] args)
    {
        command.Invoke(args);
    }


    public ConsoleCommandHolder(string id, string description, string format, string executionValue, bool requiresCheats, Action<string[]> command)
    {
        _commandId = id;
        _commandDescription = description;
        _commandFormat = format;
        _requiresCheats = requiresCheats;
        _executionValue= executionValue;
        this.command = command;
    }
}




[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommand : System.Attribute {
    public string id;
    public string description;
    public string executionValue;
    public bool requiresCheats;

    public ConsoleCommand(string id, string description, bool requiresCheats = false, string executionValue = "") {
        this.id=id;
        this.description=description;
        this.executionValue = executionValue;
        this.requiresCheats = requiresCheats;
    }

}

