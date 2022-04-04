using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{
    public class Refinery : Structure
    {

        private Vent myVent;
        
        protected override void Start()
        {
            base.Start();
            myVent = FindVentAtPos(transform.position);
            myVent.IsClaimed = true;
            InvokeRepeating(nameof(ExtractGas), 1, 1);
        }

        private void OnDestroy()
        {
            if (myVent != null)
            {
                myVent.IsClaimed = false;
            }
        }

        private void ExtractGas()
        {
            if (myVent == null || myVent.Gas <= 0)
            {
                CancelInvoke();
                GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
                if (myVent != null)
                {
                    CancelInvoke();
                    GetComponentInChildren<ParticleSystem>().Stop();
                }
            }
            else
            {
                myVent.Gas--;
                Game.Instance.Gas++;
            }
        }

        public override bool CanBuildHere(Vector3 pos, out Vector3 destinationPos)
        {
            var vent = FindVentAtPos(pos);
            if (vent != null && !vent.IsClaimed)
            {
                destinationPos = vent.transform.position;
                return true;
            }
            else
            {
                destinationPos = Vector3.zero;
                return false;
            }
        }
        
        private Vent FindVentAtPos(Vector3 pos)
        {
            foreach (var vent in Game.Instance.Map.Vents)
            {
                if (Vector3.Distance(pos, vent.transform.position) < 1.1)
                {
                    return vent;
                }
            }
            return null;
        }
    }
}