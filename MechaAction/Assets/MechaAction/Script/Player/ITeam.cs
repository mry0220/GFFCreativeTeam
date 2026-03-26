using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamType
{
    Player,
    Enemy,
    Neutral
}

public interface ITeam
{
    TeamType Team {  get; }
}
