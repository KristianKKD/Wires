using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacitorComponent : ComponentItem, IItem {

    public override void Use(){}

    public override void Activate(List<ComponentItem> chain) {
        chain.Add(this);
        if (currentEnergy > stats.energyCost) {
            SplitPower(chain);
            Drain(currentEnergy);
        }
    }

}
