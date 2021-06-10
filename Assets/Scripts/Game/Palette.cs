using System;
using UnityEngine;


public enum ColorId
{
    id_1,
    id_2, 
    id_3,
    id_4,
    id_5
}

[Serializable]
public struct ColorInfo
{
    public ColorId Id;
    public Color Color;
}

[CreateAssetMenu(fileName = "Palette", order = 1)]
public class Palette : ScriptableObject
{
    [SerializeField] private ColorInfo[] _colors;

    public ColorInfo[] Colors => _colors;
}