using Depender.Types.FlatRides;
using System;
using UnityEngine;

namespace Depender.AnimationMotors
{
    [Serializable]
    public class RotateBetweenHP : motor
    {
        public string axisPath;
        public Transform axis;
        public Quaternion fromRotation;
        public Vector3 rotationAxis = Vector3.up;
        public Quaternion toRotation;
        public Quaternion originalRotationValue;
        public float duration = 1f;


        private float currentPosition;


        private float direction;
        public override string EventName
        {
            get
            {
                return "Rotator";
            }
        }
        public override void GetAxis(GameObject GO)
        {
            axis = GO.transform.FindChild(axisPath);
            base.GetAxis(GO);
        }
        public override void Reset()
        {
            if (axis)
                axis.localRotation = originalRotationValue;
            currentPosition = 0f;


            base.Reset();
        }
        public override void Enter()
        {
            originalRotationValue = axis.localRotation;
            Initialize(axis, axis.localRotation, Quaternion.Euler(axis.localEulerAngles + rotationAxis), duration);
        }
        public void Initialize(Transform axis, Quaternion fromRotation, Quaternion toRotation, float duration)
        {
            this.axis = axis;
            this.fromRotation = fromRotation;
            this.toRotation = toRotation;
            this.duration = duration;
            axis.localRotation = Quaternion.Lerp(fromRotation, toRotation, 0);
        }

        public bool startFromTo()
        {
            if (this.direction != 1f)
            {
                this.direction = 1f;
                return true;
            }
            return false;
        }

        public bool startToFrom()
        {
            if (this.direction != -1f)
            {
                this.direction = -1f;
                return true;
            }
            return false;
        }

        public bool isStopped()
        {
            if (this.direction == 1f)
            {
                return this.currentPosition >= 0.99f;
            }
            return this.currentPosition <= 0.01f;
        }

        public void tick(float dt)
        {
            this.currentPosition += dt * this.direction * 1f / this.duration;
            this.currentPosition = Mathf.Clamp01(this.currentPosition);
            this.axis.localRotation = Quaternion.Lerp(this.fromRotation, this.toRotation, Mathfx.Hermite(0f, 1f, this.currentPosition));
        }
    }
}
