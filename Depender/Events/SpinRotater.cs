using System;
using UnityEngine;
using System.Linq;
using Depender.Types.FlatRides;
using Depender.AnimationMotors;


namespace Depender.AnimationEvents
{
    [Serializable]
    public class SpinRotater : RideAnimationEvent
    {

        public RotatorHP rotator;
        public bool spin = false;
        public float spins = 1;
        float lastTime;

        public override void Check(CustomFlatRide RA)
        {
            foreach (RotatorHP R in RA.motors.OfType<RotatorHP>().ToList())
                if (R.Identifier == identifierMotor)
                    rotator = R;
            base.Check(RA);
        }




        public override string EventName
        {
            get
            {
                return "SpinRotator";
            }
        }


        public override void Enter()
        {
            rotator.resetRotations();
            base.Enter();
        }
        public override void Run()
        {
            if (rotator != null)
            {


                rotator.tick(Time.deltaTime);
                if (spin)
                {
                    if (rotator.getRotationsCount() >= spins)
                    {
                        done = true;
                    }
                }
                else
                { done = true; }

                base.Run();
            }

        }
    }
}
