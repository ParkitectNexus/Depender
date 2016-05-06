

using System.Collections.Generic;
using UnityEngine;
namespace Depender.Types.FlatRides
{
    public class FlatRideMod : ModdedObject
    {
        //Ratings
        public float Excitement;
        public float Intensity;
        public float Nausea;

        //Size
        public int XSize;
        public int ZSize;
        public Vector3 closedAngleRetraints;
        public List<Waypoint> waypoints = new List<Waypoint>();
        public RideAnimationMod Animation = new RideAnimationMod();
        public override void Decorate()
        {
            //Setup waypoints
            if (Object.GetComponent<Waypoints>())
                Object.GetComponent<Waypoints>().waypoints = waypoints;
            else
                Object.AddComponent<Waypoints>().waypoints = waypoints;

            Extra.FixSeats(Object);
            //Flat Ride sSettings
            CustomFlatRide FR = Object.AddComponent<CustomFlatRide>();
            FR.xSize = XSize;
            FR.zSize = ZSize;
            FR.excitementRating = Excitement;
            FR.intensityRating = Intensity;
            FR.nauseaRating = Nausea;

            //Restraints
            RestraintRotationController controller = Object.AddComponent<RestraintRotationController>();
            controller.closedAngles = closedAngleRetraints;

            //Setup Animation
            FR.motors = Animation.motors;
            FR.phases = Animation.phases;
            foreach (Phase P in FR.phases)
                foreach (RideAnimationEvent RAE in P.Events)
                    RAE.Check(FR);



            //Basic FlatRide Settings
            FR.fenceStyle = AssetManager.Instance.rideFenceStyles.rideFenceStyles[0].identifier;
            FR.entranceGO = Extra.FlatRideEntrance(FR.gameObject);
            FR.exitGO = AssetManager.Instance.attractionExitGO;
            FR.categoryTag = "Attractions/Flat Ride";
            FR.defaultEntranceFee = 1f;
            FR.entranceExitBuilderGO = AssetManager.Instance.flatRideEntranceExitBuilderGO;

            base.Decorate();
        }
    }
}

