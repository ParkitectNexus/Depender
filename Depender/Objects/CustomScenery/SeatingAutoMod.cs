
using UnityEngine;
namespace Depender.Types
{
    public class SeatingAutoMod : ModdedObject
    {
        public bool hasBackRest;
        public int seatCount;
        public override void Decorate()
        {

            if (seatCount != 0)
            {
                if (seatCount == 1.0)
                {
                    GameObject seat1 = new GameObject("Seat");

                    seat1.transform.parent = Object.transform;

                    seat1.transform.localPosition = new Vector3(0, 0.1f, 0);
                    seat1.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }
                else if (seatCount == 2.0)
                {
                    GameObject seat1 = new GameObject("Seat");
                    GameObject seat2 = new GameObject("Seat");

                    seat1.transform.parent = Object.transform;
                    seat2.transform.parent = Object.transform;

                    seat1.transform.localPosition = new Vector3(0.1f, 0.1f, 0);
                    seat1.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    seat2.transform.localPosition = new Vector3(-0.1f, 0.1f, 0);
                    seat2.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }


                Object.AddComponent<Seating>();

                Object.GetComponent<Seating>().hasBackRest = hasBackRest;
            }
            base.Decorate();
        }
    }
}

