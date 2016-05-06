using Depender.Types.FlatRides;
using System;
using UnityEngine;

namespace Depender.AnimationMotors
{
    [Serializable]
    public class RotatorHP : motor
    {
        public enum State
        {
            STARTING,
            RUNNING,
            PAUSING,
            REQUEST_STOP,
            STOPPING
        }
        public string axisPath;
        public Quaternion originalRotationValue;
        [SerializeField]
        public float accelerationSpeed = 12f;
        public float maxSpeed = 180f;
        public Vector3 rotationAxis = Vector3.up;
        public int rotationAxisIndex = 1;
        public float minRotationSpeedPercent = 0.3f;
        public Quaternion initialRotation;


        public RotatorHP.State currentState = RotatorHP.State.STOPPING;


        public float currentSpeed;


        public float currentRotation;


        public int direction = 1;

        public override void DrawGUI()
        {
            base.DrawGUI();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max Speed = " + maxSpeed);
            if (GUILayout.Button("+")) { maxSpeed *= 2; }
            if (GUILayout.Button("-")) { maxSpeed *= .5f; }
            GUILayout.EndHorizontal();
        }
        public override void Reset()
        {
            if (axis)
                axis.localRotation = originalRotationValue;
            base.Reset();
        }
        public override string EventName
        {
            get
            {
                return "RotatorHP";
            }
        }

        public Transform axis
        {
            get;
            set;
        }
        public override void GetAxis(GameObject GO)
        {
            axis = GO.transform.FindChild(axisPath);
            base.GetAxis(GO);
        }
        public override void Enter()
        {
            originalRotationValue = axis.localRotation;
            resetRotations();
            this.currentRotation = 0;
            currentSpeed = 0;
            changeState(State.STARTING);
            Initialize(axis, accelerationSpeed, maxSpeed, rotationAxis);
            base.Enter();
        }

        public void Initialize(Transform axis, float accelerationSpeed, float maxSpeed)
        {
            this.Initialize(axis, accelerationSpeed, maxSpeed, Vector3.up);
        }

        public void Initialize(Transform axis, float accelerationSpeed, float maxSpeed, Vector3 rotationAxis)
        {
            this.axis = axis;
            this.accelerationSpeed = accelerationSpeed;
            this.maxSpeed = maxSpeed;
            this.setRotationAxis(rotationAxis);
            this.setInitialRotation(axis.localRotation);
            axis.Rotate(rotationAxis, this.currentRotation);
        }

        public void setInitialRotation(Quaternion initialLocalRotation)
        {
            this.initialRotation = initialLocalRotation;
        }

        public void setMinRotationSpeedPercent(float minRotationSpeedPercent)
        {
            this.minRotationSpeedPercent = minRotationSpeedPercent;
        }

        private void setRotationAxis(Vector3 rotationAxis)
        {
            this.rotationAxis = rotationAxis;
            if (rotationAxis.x != 0f)
            {
                this.rotationAxisIndex = 0;
            }
            else if (rotationAxis.y != 0f)
            {
                this.rotationAxisIndex = 1;
            }
            else if (rotationAxis.z != 0f)
            {
                this.rotationAxisIndex = 2;
            }
        }

        public bool start()
        {
            if (this.currentState != RotatorHP.State.STARTING && this.currentState != RotatorHP.State.RUNNING)
            {
                this.changeState(RotatorHP.State.STARTING);
                this.currentSpeed = 0f;
                this.currentRotation = 0f;
                return true;
            }
            return false;
        }

        public void stop()
        {
            this.changeState(RotatorHP.State.REQUEST_STOP);
        }

        public void pause()
        {
            this.changeState(RotatorHP.State.PAUSING);
        }

        public bool isStopped()
        {
            return this.currentState == RotatorHP.State.STOPPING && Mathf.Approximately(this.currentSpeed, 0f);
        }

        public RotatorHP.State getState()
        {
            return this.currentState;
        }

        public void resetRotations()
        {
            this.currentRotation = 0f;
        }

        public float getRotationsCount()
        {
            return Mathf.Abs(this.currentRotation) / 360f;
        }

        public int getCompletedRotationsCount()
        {
            return Mathf.FloorToInt(this.getRotationsCount());
        }

        public bool isInAngleRange(float fromAngle, float toAngle)
        {
            fromAngle %= 360f;
            toAngle %= 360f;
            float num = this.axis.localEulerAngles[this.rotationAxisIndex];
            if (fromAngle >= toAngle)
            {
                return num >= fromAngle || num <= toAngle;
            }
            return num < toAngle && num > fromAngle;
        }

        public bool reachedFullSpeed()
        {
            return this.currentState != RotatorHP.State.STARTING;
        }

        public float getCurrentSpeed()
        {
            return this.currentSpeed;
        }

        public float getMaxSpeed()
        {
            return this.maxSpeed;
        }

        public void setDirection(int direction)
        {
            this.direction = direction;
        }

        public int getDirection()
        {
            return this.direction;
        }

        public void changeState(RotatorHP.State newState)
        {
            this.currentState = newState;

        }

        public virtual void tick(float dt)
        {
            float num = this.currentSpeed * dt;
            this.currentRotation += num;
            if (this.currentState == RotatorHP.State.STARTING || this.currentState == RotatorHP.State.RUNNING || this.currentState == RotatorHP.State.PAUSING)
            {
                this.axis.Rotate(this.rotationAxis, num * (float)this.direction);
            }
            if (this.currentState == RotatorHP.State.STARTING)
            {
                if (this.currentSpeed < this.maxSpeed)
                {
                    this.currentSpeed += dt * this.accelerationSpeed;
                }
                else
                {
                    this.changeState(RotatorHP.State.RUNNING);
                }
            }
            else if (this.currentState == RotatorHP.State.PAUSING)
            {
                this.currentSpeed -= dt * this.accelerationSpeed;
                if (this.currentSpeed < 0f)
                {
                    this.currentSpeed = 0f;
                }
            }
            else if (this.currentState == RotatorHP.State.REQUEST_STOP)
            {
                this.currentSpeed -= dt * this.accelerationSpeed;
                this.currentSpeed = Mathf.Max(this.maxSpeed * this.minRotationSpeedPercent - 0.01f, this.currentSpeed);
                if (this.currentSpeed < this.maxSpeed * this.minRotationSpeedPercent)
                {
                    float num2 = this.axis.localEulerAngles[this.rotationAxisIndex] - this.initialRotation.eulerAngles[this.rotationAxisIndex] + 180f;
                    float num3 = num2 - 360f * Mathf.Round(num2 / 360f);
                    if ((num3 > 0f && this.direction > 0) || (num3 < 0f && this.direction < 0))
                    {
                        this.changeState(RotatorHP.State.STOPPING);
                    }
                }
                this.axis.Rotate(this.rotationAxis, num * (float)this.direction);
            }
            else if (this.currentState == RotatorHP.State.STOPPING && this.currentSpeed != 0f)
            {
                float b = Quaternion.Angle(this.axis.localRotation, this.initialRotation);
                this.currentSpeed = Mathf.Min(this.currentSpeed, b);
                this.axis.localRotation = Quaternion.RotateTowards(this.axis.localRotation, this.initialRotation, Mathf.Max(1f, this.currentSpeed) * dt);
                float num4 = this.axis.localEulerAngles[this.rotationAxisIndex] - this.initialRotation.eulerAngles[this.rotationAxisIndex];
                float num5 = num4 - 360f * Mathf.Round(num4 / 360f);
                if ((num5 > 0f && this.direction > 0) || (num5 < 0f && this.direction < 0))
                {
                    this.axis.localRotation = this.initialRotation;
                    this.currentSpeed = 0f;
                }
            }
        }
    }
}
