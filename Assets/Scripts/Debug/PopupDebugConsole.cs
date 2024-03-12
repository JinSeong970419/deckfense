using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deckfense
{
    public class PopupDebugConsole : Popup
    {
        public static PopupDebugConsole Instance { get; private set; }
        private static Dictionary<string, ConsoleCommand> commands { get; set; }

        [Header("Console UI")]
        [SerializeField] private TextMeshProUGUI consoleText;
        [SerializeField] private TMP_InputField inputText;
        [SerializeField] private TMP_InputField consoleInput;
        [SerializeField] private ScrollRect scrollRect;

        [Header("InputReader")]
        [SerializeField] private InputReader inputReader;

        protected override void OnEnable()
        {
            base.OnEnable();
            Application.logMessageReceived += HandleLog;
            inputReader.enterAction += InputEnter;
        }

        protected override void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
            inputReader.enterAction -= InputEnter;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = this;
            commands = new Dictionary<string, ConsoleCommand>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
            CreateCommands();
        }

        private void InputEnter()
        {
            if (inputText.text != "")
            {
                AddMessageToConsole(inputText.text);
                ParseInput(inputText.text);
                consoleInput.Select();
                consoleInput.text = "";
            }
        }

        public void CreateCommands()
        {
            CommandQuit.CreateCommand();
            CommandClear.CreateCommand();
            CommandLoadScenes.CreateCommand();
        }

        public void AddMessageToConsole(string msg, LogType type = LogType.Log)
        {
            string typeMsg;
            switch (type)
            {
                case LogType.Warning:
                    typeMsg = $"<color=yellow>{msg} : </color>";
                    break;

                case LogType.Error:
                    typeMsg = $"<color=red>{msg} : </color>";
                    break;

                default:
                    typeMsg = msg;
                    break;

            }
            consoleText.text += typeMsg + '\n';
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void ClearConsole()
        {
            consoleText.text = "";
        }

        public static void AddCommandsToConsole(string _name, ConsoleCommand _command)
        {
            if (!commands.ContainsKey(_name))
            {
                commands.Add(_name, _command);
            }
        }

        private void ParseInput(string input)
        {
            string[] _input = input.Split(' ');

            if(_input.Length == 0 || _input == null)
                return;

            if (!commands.ContainsKey(_input[0]))
            {
                List<string> args = _input.ToList();
                if (args[0].Contains("-help"))
                {
                    List<string> keys = commands.Keys.ToList();

                    AddMessageToConsole("==============================");
                    for (int i = 0; i < keys.Count; i++)
                    {
                        AddMessageToConsole($"{commands[keys[i]].Command} - {commands[keys[i]].Description}");
                    }
                    AddMessageToConsole("==============================");

                    return;
                }

                Debug.LogWarning("Not FInd Command");
            }
            else
            {
                List<string> args = _input.ToList();
                args.RemoveAt(0);

                if (args.Contains("-help"))
                {
                    AddMessageToConsole("==============================");
                    AddMessageToConsole(commands[_input[0]].Description);
                    AddMessageToConsole("==============================");

                    return;
                }

                commands[_input[0]].RunCommand(args.ToArray());
            }
        }
        
        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            string _message = $"[{DateTime.Now.ToString(("HH:mm:ss"))} {type.ToString()}] - {logString}";
            AddMessageToConsole(_message, type);
        }
    }
}
