using System;
using UnityEngine;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Depender.Types.FlatRides
{
    //============[ The component added to the flatRide gameobject ]============;
    public class CustomFlatRide : FlatRide
    {
        public Phase currentPhase;
        int phaseNum;
        public bool animating;
        public List<Phase> phases = new List<Phase>();
        public List<motor> motors = new List<motor>();
        private bool _show;
        private Rect _windowPosition = new Rect(20, 20, 350, 320);
        private Vector2 motorsScrollPos;

        public override void onOpenInfoWindow()
        {
            if (!GetComponent<BuildableObject>().isPreview && !UIUtility.isMouseOverUIElement() && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)))
                _show = true;
            base.onOpenInfoWindow();
        }
        private void OnGUI()
        {
            if (_show)
            {
                _windowPosition = GUI.Window(Mathf.RoundToInt(transform.position.x * transform.position.z), _windowPosition, FlatRideWindow, getName());

                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    _show = false;
                }
            }
        }
        private void FlatRideWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0, 0, 310, 30));
            if (GUI.Button(new Rect(320, 5, 20, 20), "x"))
            {
                _show = false;
            }
            motorsScrollPos = GUILayout.BeginScrollView(motorsScrollPos);
            foreach (motor M in motors)
            {
                GUILayout.BeginVertical("box");
                GUILayout.BeginVertical("box");
                GUILayout.Label(M.GetType().Name);
                GUILayout.EndVertical();
                M.DrawGUI();
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void onStartRide()
        {
            foreach (motor m in motors)
            {
                m.GetAxis(this.gameObject);
            }

            base.onStartRide();
            foreach (motor m in motors)
            {
                m.Enter();
            }
            if (phases.Count <= 0)
            {
                animating = false;
                return;
            }
            foreach (motor m in motors)
            {
                m.Enter();
            }

            animating = true;
            phaseNum = 0;
            currentPhase = phases[phaseNum];
            currentPhase.running = true;
            currentPhase.Enter();
            currentPhase.Run();
        }
        public override void tick(StationController stationController)
        {
            if (currentPhase != null && animating)
            {
                currentPhase.Run();
                if (!currentPhase.running)
                {
                    NextPhase();
                }
            }

        }
        void NextPhase()
        {

            currentPhase.Exit();
            currentPhase.running = false;
            phaseNum++;
            if (phases.Count > phaseNum)
            {
                currentPhase = phases[phaseNum];
                currentPhase.running = true;
                currentPhase.Enter();
                currentPhase.Run();
                return;
            }
            animating = false;

        }
        public override bool shouldLetGuestsOut()
        {
            return !animating;
        }
    }


    public class Phase : MonoBehaviour
    {

        public List<RideAnimationEvent> Events = new List<RideAnimationEvent>();
        public bool running = false;
        bool done = false;


        public void Enter()
        {
            foreach (RideAnimationEvent RAE in Events)
            {
                RAE.Enter();
            }
        }
        public void Run()
        {
            foreach (RideAnimationEvent RAE in Events)
            {
                RAE.Run();
            }
            done = true;
            foreach (RideAnimationEvent RAE in Events)
            {
                if (!RAE.done)
                {
                    running = true;
                    done = false;
                    break;
                }
            }
            if (done)
            {
                running = false;
            }

        }
        public void Exit()
        {
            foreach (RideAnimationEvent RAE in Events)
            {
                RAE.Exit();
            }
        }

    }
    public class RideAnimationEvent : MonoBehaviour
    {
        public bool done = false;
        public bool showSettings;
        public bool isPlaying;

        public string identifierMotor;
        public Color ColorIdentifier;
        public virtual string EventName { set; get; }
        public virtual void Check(CustomFlatRide RA)
        {

        }

        public virtual void DrawGUI()
        {

        }
        public virtual void Enter()
        {
            isPlaying = true;
        }
        public virtual void Run()
        {

        }
        public virtual void Exit()
        {

            isPlaying = false;
            done = false;
        }

    }
    public class motor : MonoBehaviour
    {
        public bool showSettings;
        public string Identifier = "";
        public virtual string EventName { set; get; }

        public virtual void DrawGUI()
        {
        }
        public virtual void Enter()
        {

        }
        public virtual void GetAxis(GameObject GO)
        {

        }
        public virtual void Reset()
        {

        }
    }
}
