using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacitorComponent : ComponentItem, IItem {

    public override void Use(){}

    public override void Activate() {
        if (currentEnergy > stats.energyCost) {
            SplitPower();
            Drain(currentEnergy);
        }
    }

}
