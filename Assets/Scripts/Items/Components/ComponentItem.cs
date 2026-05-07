using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentItem : Item {

    //ItemStats stats (is inherrited)

    public float currentEnergy = 0;
    public bool active = true;
    public List<Vector2> inSpots;
    public List<Vector2> outSpots;
    public float rotation = 0; //0, 90, 180, 270
    public List<ComponentItem> incomingConnections = new List<ComponentItem>();
    public List<ComponentItem> outgoingConnections = new List<ComponentItem>();

    public virtual void OnPlace() {
        ClearConnections();
        Connect(); // Connect to nearby components when placed on the board
    }

    public void Reset() { 
        ClearConnections();
    }

    public virtual void Power(float incoming, List<ComponentItem> chain = null) { // When a component receives energy from another component
        if (chain == null)
            chain = new List<ComponentItem>();
        currentEnergy += incoming;
        if (currentEnergy > stats.energyCost && active)
            Activate(chain);
    }

    public virtual void Activate(List<ComponentItem> chain) { // When a component has enough energy to activate
        chain.Add(this);
        SplitPower(chain);
        Drain(stats.energyCost);
    }

    public void Drain(float energy) {
        currentEnergy -= energy;
    }

    public void SplitPower(List<ComponentItem> chain) { //call this to split the energy equally between outgoing connections (eg. for a wire)
        if (currentEnergy > stats.energyCost)
            foreach (ComponentItem c in outgoingConnections)
                c.Power(currentEnergy / outgoingConnections.Count, chain);
    }

    public void Connect() {
        ProcessSpots(outSpots, isOutgoing:true);
        ProcessSpots(inSpots, isOutgoing:false);
    }

    private void ProcessSpots(List<Vector2> checkSpots, bool isOutgoing) {
        foreach (Vector2 check in checkSpots) {
            Vector2 rotatedCheck = Quaternion.Euler(0, 0, this.rotation) * (Vector3)check;

            ComponentItem target = QuickReferences.qr.brd.GetComponentAtPosition(rotatedCheck, this.myTile.id);
            if (target == null || target == this)
                continue;
            if (target.incomingConnections.Contains(this) || this.outgoingConnections.Contains(target) || 
                target.outgoingConnections.Contains(this) || this.incomingConnections.Contains(target)) // Depending on place order, this may be relevant
                continue;


            List<Vector2> compareSpots = isOutgoing ? target.inSpots : target.outSpots;
            foreach (Vector2 comparison in compareSpots) {
                Vector2 rotatedCompare = Quaternion.Euler(0, 0, target.rotation) * (Vector3)comparison;
                if ((rotatedCheck - (-rotatedCompare)).sqrMagnitude > 0.0001f)
                    continue;

                if (isOutgoing) {
                    this.outgoingConnections.Add(target);
                    target.incomingConnections.Add(this);
                } else {
                    this.incomingConnections.Add(target);
                    target.outgoingConnections.Add(this);
                }

                Debug.Log("Connected " + name + " to " + target.name);
                target.Connect();
            }

        }
    }


    private void ClearConnections() {
        for (int i = 0; i < incomingConnections.Count; i++) {
            ComponentItem incomingC = incomingConnections[i];
            incomingC.outgoingConnections.Remove(this);
            Debug.Log("Removed " + name + " incoming connection from " + incomingC.name);
        }
        for (int i = 0; i < outgoingConnections.Count; i++) {
            ComponentItem outgoingC = outgoingConnections[i];
            outgoingC.incomingConnections.Remove(this);
            Debug.Log("Removed " + name + " outgoing connection from " + outgoingC.name);
        }

        incomingConnections.Clear();
        outgoingConnections.Clear();
    }
}
