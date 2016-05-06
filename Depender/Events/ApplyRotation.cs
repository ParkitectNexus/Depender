
using System;
using UnityEngine;
using System.Linq;
using Depender.Types.FlatRides;
using Depender.AnimationMotors;

namespace Depender.AnimationEvents
{
    [ExecuteInEditMode]
    [Serializable]
    public class ApplyRotation : RideAnimationEvent
    {

        public MultipleRotationsHP rotator;

        float lastTime;

        public override void Check(CustomFlatRide RA)
        {
            foreach (MultipleRotationsHP R in RA.motors.OfType<MultipleRotationsHP>().ToList())
                if (R.Identifier == identifierMotor)
                {
                    rotator = R;
                }
            base.Check(RA);
        }
        public override string EventName
        {
            get
            {
                return "ApplyRptations";
            }
        }



        public override void Enter()
        {

        }
        public override void Run()
        {
            if (rotator != null)
            {
                rotator.tick(Time.deltaTime);
                lastTime = Time.realtimeSinceStartup;
                done = true;
                base.Run();
            }

        }
    }
}
