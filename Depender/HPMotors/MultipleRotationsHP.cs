using Depender.Types.FlatRides;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Depender.AnimationMotors
{

    [Serializable]
    public class MultipleRotationsHP : motor
    {

        public string axisPath;
        public List<string> AxissPath = new List<string>();

        public Transform mainAxis;

        public List<Transform> Axiss = new List<Transform>();


        public override void Reset()
        {
            if (mainAxis)
            {
                foreach (Transform T in Axiss)
                {
                    T.localRotation = mainAxis.localRotation;
                }
            }
        }
        public override void GetAxis(GameObject GO)
        {
            mainAxis = GO.transform.Find(axisPath);
            foreach (string path in AxissPath)
            {
                Axiss.Add(GO.transform.Find(path));
            }
            base.GetAxis(GO);
        }
        public void tick(float dt)
        {
            if (mainAxis)
            {
                foreach (Transform T in Axiss)
                {
                    T.localRotation = mainAxis.localRotation;
                }
            }
        }
    }
}
