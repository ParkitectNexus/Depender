using UnityEngine;

namespace Depender
{
    public class Main : IMod
    {
        string name = "Depender";
        string description = "The mod that manage all the mods made with the mod spark.";
        public void onEnabled()
        {
            Registar._hider = new GameObject("DependerGO");
        }

        public void onDisabled()
        {
            Registar.UnRegister();
          
        }

        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public string Path { get; set; }
        public string Identifier { get; set; }
    }
}
