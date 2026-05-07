using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairComponent : ComponentItem {

    PairComponent pairedComponent;

    public override void Power(float incoming, List<ComponentItem> chain) {
        if (chain[chain.Count - 1] == pairedComponent) { // we are receiving power from our pair, so we should not pass it back
            outgoingConnections.Remove(pairedComponent);
            base.Power(incoming, chain);
            outgoingConnections.Add(pairedComponent);
        } else
            base.Power(incoming, chain);
    }

    public override void OnPlace() {
        base.OnPlace();

        if (pairedComponent != null) {
            outgoingConnections.Add(pairedComponent);
            pairedComponent.incomingConnections.Add(this);
            pairedComponent.outgoingConnections.Add(this);
            this.incomingConnections.Add(pairedComponent);
        }
    }

}
