using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Component : Item {

    //need to copy stats over

    public float energy;
    public float activationCost;
    public bool active = true;
    public List<Vector2> inSpots;
    public List<Vector2> outSpots;
    public List<Component> incomingConnections;
    public List<Component> outgoingConnections;
    public InventorySlot tile;

    public virtual void Power(float incoming) {
        energy += incoming;
        if (energy > 0 && active)
            Activate();
    }

    public virtual void Activate() {
        energy -= activationCost;
        if (energy > 0)
            foreach (Component c in outgoingConnections)
                c.Power(energy/outgoingConnections.Count);
        energy = 0;
    }

    public void ClearConnections() {
        for (int i = 0; i < incomingConnections.Count; i++) {
            Component incomingC = incomingConnections[i];
            incomingC.outgoingConnections.Remove(this);
            Debug.Log("Removed " + name + " incoming connection from " + incomingC.name);
        }
        for (int i = 0; i < outgoingConnections.Count; i++) {
            Component outgoingC = outgoingConnections[i];
            outgoingC.incomingConnections.Remove(this);
            Debug.Log("Removed " + name + " outgoing connection from " + outgoingC.name);
        }

        tile = null;
        incomingConnections.Clear();
        outgoingConnections.Clear();
    }
}
