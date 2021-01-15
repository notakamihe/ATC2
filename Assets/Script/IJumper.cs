using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IJumper
{
    void Jump(float force);
    void DetectForJump();
}
