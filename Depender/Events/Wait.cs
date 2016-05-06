using System;
using UnityEngine;
using System.Linq;
using Depender.Types.FlatRides;
using Depender.AnimationMotors;


namespace Depender.AnimationEvents
{
    [Serializable]
    public class Wait : RideAnimationEvent
    {

        public float seconds;

        float timeLimit;
        float time;
        public override string EventName
        {
            get
            {
                return "Wait";
            }
        }


        public override void Enter()
        {
            timeLimit = seconds;
            time = 0;
            base.Enter();
        }
        public override void Run()
        {

            time += Time.deltaTime;
            if (time > timeLimit)
            {

                done = true;
            }
            else
            {

            }
            base.Run();
        }

    }
}
