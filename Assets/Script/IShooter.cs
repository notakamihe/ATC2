using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IShooter
{
    void Shoot();
    void FireShot(Vector3 direction);
}
