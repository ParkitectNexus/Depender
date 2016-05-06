using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class ParkitectObject
{

    public GameObject gameObject;
    public enum ObjType
    {
        none,
        _,
        deco,
        trashbin,
        seating,
        seatingAuto,
        lamp,
        fence,
        FlatRide
    }
    public float XSize = 1;
    public float ZSize = 1;
    public string inGameName;
    public int price;
    public bool grid;
    public int heightDelta;
    public bool recolorable;
    public Color color1 = new Color(0.95f, 0, 0);
    public Color color2 = new Color(0.32f, 1, 0);
    public Color color3 = new Color(0.11f, .05f, 1);
    public Color color4 = new Color(1, 0, 1);

    public bool snapCenter = true;
    public bool snap;
    public ObjType type = ObjType.none;

    public List<Waypoint> waypoints = new List<Waypoint>();
    public RideAnimation Animation = new RideAnimation();
    internal string ObjName;
}
[Serializable]
[ExecuteInEditMode]
public class RideAnimation : FlatRide
{

    public List<motor> motors = new List<motor>();
    public List<Phase> phases = new List<Phase>();
    public Phase currentPhase;

    int phaseNum;
    public bool animating;
    public override void Start()
    {
        base.Start();
        
    }
    public override void onStartRide()
    {
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
        

        currentPhase = null;
    }
    public override void tick(StationController stationController)
    {
        if (currentPhase != null)
        {
            currentPhase.Run();
            if (!currentPhase.running)
            {
                NextPhase();
            }
        }

    }
    public override bool shouldLetGuestsOut()
    {
        return !animating;
    }
}


[ExecuteInEditMode]
[Serializable]
public class Phase
{
    [SerializeField]
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

[ExecuteInEditMode]
[Serializable]
public class RideAnimationEvent : ScriptableObject
{
    public bool done = false;
    public bool showSettings;
    public bool isPlaying;
    public string identifierMotor;
    public Color ColorIdentifier;
    public virtual void Check(RideAnimation RA)
    {

    }
    public virtual string EventName { set; get; }

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
[Serializable]
public class motor : ScriptableObject
{
    public bool showSettings;
    public string Identifier = "";
    public Color ColorIdentifier;

    public virtual string EventName { set; get; }
    public void Awake()
    {
        ColorIdentifier = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
    }
    public virtual void DrawGUI()
    {
    }
    public virtual void Enter()
    {

    }
    
    public virtual void Reset()
    {

    }
}