using Depender.Types.FlatRides;
using System;
using UnityEngine;

namespace Depender.AnimationMotors
{
    [ExecuteInEditMode]
    [Serializable]
    public class MoverHP : motor
    {

        private enum State
        {
            RUNNING,
            STOPPED
        }
        public string axisPath;
        public Transform axis;
        public Vector3 originalRotationValue;
        private Vector3 fromPosition;
        public Vector3 toPosition;
        public float duration = 10f;


        private MoverHP.State currentState = MoverHP.State.STOPPED;


        private float currentPosition = 1f;


        private int direction = -1;

        public override void GetAxis(GameObject GO)
        {
            axis = GO.transform.FindChild(axisPath);
            base.GetAxis(GO);
        }
        public override void Reset()
        {

            currentPosition = 1f;
            direction = -1;
            if (axis)
                axis.localPosition = originalRotationValue;
            base.Reset();
        }
        public override string EventName
        {
            get
            {
                return "Mover";
            }
        }

        public override void Enter()
        {

            this.currentPosition = 1f;

            direction = -1;
            if (axis)
                originalRotationValue = axis.localPosition;
            Initialize(axis, axis.localPosition, toPosition, duration);
            base.Enter();
        }
        public void Initialize(Transform axis, Vector3 fromPosition, Vector3 toPosition, float duration)
        {
            this.axis = axis;
            this.fromPosition = fromPosition;
            this.toPosition = toPosition;
            this.duration = duration;
            this.setPosition();
        }

        public bool startFromTo()
        {
            if (this.direction != 1)
            {
                this.direction = 1;
                this.currentPosition = 0f;
                this.currentState = MoverHP.State.RUNNING;
                return true;
            }
            return false;
        }

        public bool startToFrom()
        {
            if (this.direction != -1)
            {
                this.direction = -1;
                this.currentPosition = 0f;
                this.currentState = MoverHP.State.RUNNING;
                return true;
            }
            return false;
        }

        public bool reachedTarget()
        {
            return this.currentState == MoverHP.State.STOPPED && this.currentPosition >= 1f;
        }

        public void tick(float dt)
        {
            this.currentPosition += dt * 1f / this.duration;
            if (this.currentPosition >= 1f)
            {
                this.currentPosition = 1f;
                this.currentState = MoverHP.State.STOPPED;
            }
            this.setPosition();
        }

        private void setPosition()
        {
            Vector3 a;
            Vector3 b;
            if (this.direction == 1)
            {
                a = this.fromPosition;
                b = this.toPosition;
            }
            else
            {
                a = this.toPosition;
                b = this.fromPosition;
            }
            this.axis.localPosition = Vector3.Lerp(a, b, Mathfx.Hermite(0f, 1f, this.currentPosition));
        }
    }
}
