using System;
using UnityEngine;
using System.Linq;
using Depender.Types.FlatRides;
using Depender.AnimationMotors;


namespace Depender.AnimationEvents
{
    [Serializable]
    public class ToFromRot : RideAnimationEvent
    {
        public RotateBetweenHP rotator;
        float lastTime;

        public override void Check(CustomFlatRide RA)
        {
            foreach (RotateBetweenHP R in RA.motors.OfType<RotateBetweenHP>().ToList())
                if (R.Identifier == identifierMotor)
                    rotator = R;

            base.Check(RA);
        }
        public override string EventName
        {
            get
            {
                return "To-From Rot";
            }
        }

        public override void Enter()
        {
            lastTime = Time.realtimeSinceStartup;

            rotator.startToFrom();
            base.Enter();
        }
        public override void Run()
        {

            if (rotator)
            {


                rotator.tick(Time.deltaTime);
                lastTime = Time.realtimeSinceStartup;
                if (rotator.isStopped())
                {
                    done = true;
                }
                base.Run();
            }

        }
    }
}
