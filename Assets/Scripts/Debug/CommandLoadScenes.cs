using System.Collections.Generic;
using UnityEditor;

namespace Deckfense
{
    public class CommandLoadScenes : ConsoleCommand
    {
        private enum Scenes
        {
            Splash,
            Loading,
            Title,
            Lobby,
            InGame,
            InGameLoading,
            MAX
        }

        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }

        public Dictionary<string, bool> buildInScences = new Dictionary<string, bool>();
        public CommandLoadScenes() 
        {
            Name = "LoadScenes";
            Command = "loadscenes";
            Description = $"{Name} - LoadScenes \"[불러올 씬]\"";

            extractBuildInScenes();
            AddCommandToConsole();
        }

        public override void RunCommand(string[] args)
        {
            int result;
            if (buildInScences.ContainsKey(args[0]))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(args[0]);
            }
            else if(int.TryParse(args[0], out result))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(result);
            }
        }

        public static CommandLoadScenes CreateCommand()
        {
            return new CommandLoadScenes();
        }

        private void extractBuildInScenes()
        {
            buildInScences.Clear();
#if UNITY_EDITOR
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                string scenes = EditorBuildSettings.scenes[i].path.Split('/')[2].Split('.')[0];
                buildInScences.Add(scenes, true);
            }
#else
            for (Scenes scenes = 0; scenes < Scenes.MAX; scenes++)
            {
                buildInScences.Add(scenes.ToString(), true);
            }
#endif
        }
    }
}
