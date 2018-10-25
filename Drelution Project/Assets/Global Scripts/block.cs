using UnityEngine;

public abstract class Block
{

    protected Color32 color;
    protected string blockName;
    protected Vector3[] vertices;

    public Block(string name, Color32 Color, params Vector3[] verts)
    {
        blockName = name;
        color = Color;
        vertices = verts;
    }

    public Color32 Color
    {
        get { return color; }
    }

    public string BlockName
    {
        get { return blockName; }
    }

    public Vector3[] Vertices
    {
        get { return vertices; }
    }

    public float GetX(int x, Layer l)
    {
        return l.X + x * Layer.BlockSize;
    }

    public float GetY(int y, Layer l)
    {
        return (l.Y - (l.height * Layer.BlockSize)) + y * Layer.BlockSize;
    }

    /// <summary>
    /// It is called when a Right Contact Point is into the block.
    /// </summary>
    /// <param name="target">Object that owns the Contact Point </param>
    /// <param name="contactPoint">Contact Point</param>
    /// <param name="x">X position of the block</param>
    /// <param name="y">Y position of the block</param>
    /// <param name="blockSize">Size of the block</param>
    public abstract void Left(MobileObject target, Transform contactPoint, int x, int y, Layer l);

    /// <summary>
    /// It is called when a Left Contact Point is into the block.
    /// </summary>
    /// <param name="target">Object that owns the Contact Point </param>
    /// <param name="contactPoint">Contact Point</param>
    /// <param name="x">X position of the block</param>
    /// <param name="y">Y position of the block</param>
    /// <param name="blockSize">Size of the block</param>
    public abstract void Right(MobileObject target, Transform contactPoint, int x, int y, Layer l);

    /// <summary>
    /// It is called when a Down Contact Point is into the block.
    /// </summary>
    /// <param name="target">Object that owns the Contact Point </param>
    /// <param name="contactPoint">Contact Point</param>
    /// <param name="x">X position of the block</param>
    /// <param name="y">Y position of the block</param>
    /// <param name="blockSize">Size of the block</param>
    public abstract void Up(MobileObject target, Transform contactPoint, int x, int y, Layer l);

    public abstract void AngleDetector(MobileObject target, Transform contactPoint, int x, int y, Layer l);

    /// <summary>
    /// It is called when a Up Contact Point is into the block.
    /// </summary>
    /// <param name="target">Object that owns the Contact Point </param>
    /// <param name="contactPoint">Contact Point</param>
    /// <param name="x">X position of the block</param>
    /// <param name="y">Y position of the block</param>
    /// <param name="blockSize">Size of the block</param>
    public abstract void Down(MobileObject target, Transform contactPoint, int x, int y, Layer l);
}