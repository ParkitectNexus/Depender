using System;
using System.Collections.Generic;
using UnityEngine;

namespace Depender
{
    public class HPDebug : MonoBehaviour
    {
        static bool EnableDebug = true;
        static List<Message> Messages;
        static int MaxSavedMessages = 50;
        public Rect windowRect = new Rect(20, 20, 120, 50);
        private Vector2 scrollPosition;

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
                Debug.Log("[HP Registar] ===(" + message + ")===");
            }
            //AddMessage(message, Message.Types.Log);
        }
        public static void LogError(string message)
        {
            if (EnableDebug)
            {
                Debug.Log("[HP Registar] (ERROR) ===(" + message + ")===");
            }
            //AddMessage(message, Message.Types.Error);
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
