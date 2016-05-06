using System;
using UnityEngine;
using System.Linq;
using Depender.Types.FlatRides;
using Depender.AnimationMotors;


namespace Depender.AnimationEvents
{
    [Serializable]
    public class FromToMove : RideAnimationEvent
    {

        public MoverHP rotator;

        float lastTime;

        public override void Check(CustomFlatRide RA)
        {
            foreach (MoverHP R in RA.motors.OfType<MoverHP>().ToList())
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
                return "From-To Move";
            }
        }


        public override void Enter()
        {
            lastTime = Time.realtimeSinceStartup;

            rotator.startFromTo();
            base.Enter();
        }
        public override void Run()
        {

            if (rotator)
            {


                rotator.tick(Time.deltaTime);
                lastTime = Time.realtimeSinceStartup;
                if (rotator.reachedTarget())
                {
                    done = true;
                }
                base.Run();
            }

        }
    }
}
