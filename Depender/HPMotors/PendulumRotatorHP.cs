
using UnityEngine;

namespace Depender.AnimationMotors
{
    public class PendulumRotatorHP : RotatorHP
    {

        public float armLength;

        public float gravity;

        public float angularFriction;

        public bool pendulum;

        public void setActAsPendulum(bool pendulum)
        {
            this.pendulum = pendulum;
        }

        public override void tick(float dt)
        {

            if (!this.pendulum)
            {
                base.tick(dt);
                return;
            }
            float num = -1f * this.gravity * Mathf.Sin(base.axis.localEulerAngles[this.rotationAxisIndex] * 0.0174532924f) / this.armLength * 157.29578f;
            num = Mathf.Clamp(num, -this.accelerationSpeed, this.accelerationSpeed);
            this.currentSpeed += num * dt;
            this.currentRotation += num * dt;
            this.currentSpeed -= this.currentSpeed * this.angularFriction * dt;
            this.currentSpeed = Mathf.Clamp(this.currentSpeed, -this.maxSpeed, this.maxSpeed);
            Vector3 localEulerAngles = base.axis.localEulerAngles;
            int rotationAxisIndex;
            int expr_C6 = rotationAxisIndex = this.rotationAxisIndex;
            float num2 = localEulerAngles[rotationAxisIndex];
            localEulerAngles[expr_C6] = num2 + this.currentSpeed * dt;
            base.axis.localEulerAngles = localEulerAngles;
            if (this.currentState == RotatorHP.State.REQUEST_STOP && Mathf.Abs(this.currentSpeed) <= 0.5f && Mathf.Abs(num) <= 0.3f)
            {
                base.changeState(RotatorHP.State.STOPPING);
                base.axis.localRotation = this.initialRotation;
                this.currentSpeed = 0f;
            }
        }
    }
}