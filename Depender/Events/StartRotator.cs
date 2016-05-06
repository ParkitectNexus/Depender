using System;
using UnityEngine;
using System.Linq;
using Depender.Types.FlatRides;
using Depender.AnimationMotors;


namespace Depender.AnimationEvents
{
    [ExecuteInEditMode]
    [Serializable]
    public class StartRotator : RideAnimationEvent
    {

        public RotatorHP rotator;
        float lastTime;
        public override void Check(CustomFlatRide RA)
        {
            foreach (RotatorHP R in RA.motors.OfType<RotatorHP>().ToList())
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
                return "StartRotator";
            }
        }


        public override void Enter()
        {
            ;

            rotator.start();
            base.Enter();
        }
        public override void Run()
        {
            if (rotator)
            {

                rotator.tick(Time.deltaTime);
                if (rotator.reachedFullSpeed())
                {
                    done = true;
                }
                base.Run();
            }

        }
    }
}
