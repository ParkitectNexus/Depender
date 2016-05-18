using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Depender
{
    public class HPDebug : MonoBehaviour
    {
        static bool EnableDebug = true;
        static List<Message> Messages = new List<Message>();
        static int MaxSavedMessages = 50;
        public Rect windowRect = new Rect(20, 20, 120, 50);
        private Vector2 scrollPosition;
        public static string Path;

        public class Message
        {
            public string message;
            public enum Types
            { Log, Error }
            public Types Type;
        }
        public static void Log(string message)
        {
            if (EnableDebug)
            {
                MessageToLog("[HP Registar] ===(" + message + ")===");
            }
            //AddMessage(message, Message.Types.Log);
        }
        public static void LogError(System.Exception error)
        {
            if (EnableDebug)
            {
                MessageToLog("! [HP Registar] (ERROR) ===(" + error + ")===");
            }
            //AddMessage(message, Message.Types.Error);
        }
        private static void MessageToLog(String text)
        {
            string modPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            StreamWriter sw = File.AppendText(modPath + @"\mod.log");
            sw.WriteLine(text);

            sw.Flush();

            sw.Close();
        }
        public static void AddMessage(string message, Message.Types Type)
        {
            Message m = new Message();
            m.message = message;
            m.Type = Type;
            Messages.Add(m);
            if (Messages.Count >= MaxSavedMessages)
            {
                Messages.RemoveAt(0);
            }
        }

        void OnGUI()
        {
            windowRect = GUI.Window(92, windowRect, DoMyWindow, "HPRegistar Debug");
        }
        void DoMyWindow(int windowID)
        {
            GUILayout.BeginScrollView(scrollPosition);
            for (int i = Messages.Count; i < 0; i--)
            {
                Message M = Messages[i];
                GUILayout.BeginVertical("box");
                switch (M.Type)
                {
                    case Message.Types.Log:
                        GUI.color = Color.white;
                        break;
                    case Message.Types.Error:
                        GUI.color = Color.red;
                        break;
                    default:
                        GUI.color = Color.white;
                        break;
                }
                GUILayout.Label(M.message);
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }
    }
}
