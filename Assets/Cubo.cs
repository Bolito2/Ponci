using UnityEngine;
using System.Collections;

[System.Serializable]
public class Cubo
{
    public enum CubeColor
    {
        rojo,
        amarillo,
        azul,
        blanco
    }

    public CubeColor color;

    public int x, y, z;

    public GameObject gameObject;

    public Cubo(int x, int y, int z, GameObject gameObject)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        this.gameObject = gameObject;

        color = CubeColor.blanco;
    }
}
