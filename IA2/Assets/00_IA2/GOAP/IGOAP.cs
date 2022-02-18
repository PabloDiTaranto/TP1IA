using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGOAP
{
    void SetPlan(IEnumerable<GOAPAction> plan);
}
