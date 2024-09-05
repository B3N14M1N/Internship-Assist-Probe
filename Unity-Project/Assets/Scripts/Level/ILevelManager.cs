using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelManager
{
    void LoadLevel();
    void RemoveContainer(ContainerMono container);
}
