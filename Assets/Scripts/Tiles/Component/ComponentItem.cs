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

    public void Reset() { 
        ClearConnections();
    }

    public virtual void Power(float incoming) { //call this when a component receives energy from another component
        currentEnergy += incoming;
        if (currentEnergy > stats.energyCost && active)
            Activate();
    }

    public virtual void Activate() { //call this when a component has enough energy to activate
        SplitPower();
        Drain(stats.energyCost);
    }

    public void Drain(float energy) {
        currentEnergy -= energy;
    }

    public void SplitPower() { //call this to split the energy equally between outgoing connections (eg. for a wire)
        if (currentEnergy > stats.energyCost)
            foreach (ComponentItem c in outgoingConnections)
                c.Power(currentEnergy / outgoingConnections.Count);
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

        tile = null;
        incomingConnections.Clear();
        outgoingConnections.Clear();
    }
}
