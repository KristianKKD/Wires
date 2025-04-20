using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceComponent : Component, IItem {

    public float outgoingEnergy = 10;

    public void Use() {

    }

    public override void Activate() {
        foreach (Component c in outgoingConnections)
            c.Power(outgoingEnergy);
    }
}
