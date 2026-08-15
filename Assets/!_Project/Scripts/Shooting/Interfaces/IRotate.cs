using System;
using System.Collections.Generic;
using UnityEngine;

public interface IRotate
{
    public Quaternion ParentRotation 
    { 
        get => Quaternion.identity;
    }
    public Quaternion LocalRotation { get;}
    public Quaternion GlobalRotation 
    {
        get => ParentRotation * LocalRotation;
    }
    public Vector3 CurrentPosition { get; }

    public void RotateTowards(Vector3 direction)
    {
        Rotate(Quaternion.LookRotation(direction, Vector3.up));
    }

    public void Rotate(Quaternion newRotation)
    {
        Quaternion newLocalRot = Quaternion.Inverse(ParentRotation) * newRotation;
        RotateLocally(newLocalRot);
    }

    public void RotateLocally(Quaternion newLocalRotation);
}
