using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceComponent : ComponentItem, IItem {

    public float outgoingEnergy = 10;

    public override void Activate() {
        foreach (ComponentItem c in outgoingConnections)
            c.Power(outgoingEnergy);
    }
}
