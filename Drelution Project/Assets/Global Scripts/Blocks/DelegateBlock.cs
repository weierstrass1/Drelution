using UnityEngine;

public class DelegateBlock : Block
{

    public DelegateBlock() : base("Delegate Block", new Color32(0, 255, 0, 128),
    new Vector3(-BlockLayer.BlockSize / 2, -BlockLayer.BlockSize / 2),
    new Vector3(-BlockLayer.BlockSize / 2, BlockLayer.BlockSize / 2),
    new Vector3(BlockLayer.BlockSize / 2, BlockLayer.BlockSize / 2),
    new Vector3(BlockLayer.BlockSize / 2, -BlockLayer.BlockSize / 2))
    {

    }

    public override void AngleDetector(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        float ny = GetY(y, l) + BlockLayer.BlockSize;

        int b = l.getBlock(contactPoint.position.x, ny);

        if (b >= 0)
        {

            BlockLayer.AllBlocks[b].AngleDetector(target, contactPoint, x, l.getYPos(ny), l);
        }
    }

    public override void Down(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        float ny = GetY(y, l) - BlockLayer.BlockSize;

        int b = l.getBlock(contactPoint.position.x, ny);

        if (b >= 0)
        {

            BlockLayer.AllBlocks[b].AngleDetector(target, contactPoint, x, l.getYPos(ny), l);
        }
    }

    public override void Left(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        float nx = GetY(x, l) - BlockLayer.BlockSize;

        int b = l.getBlock(nx, contactPoint.position.y);

        if (b >= 0)
        {

            BlockLayer.AllBlocks[b].AngleDetector(target, contactPoint, l.getXPos(nx), y, l);
        }
    }

    public override void Right(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        float nx = GetY(x, l) + BlockLayer.BlockSize;

        int b = l.getBlock(nx, contactPoint.position.y);

        if (b >= 0)
        {

            BlockLayer.AllBlocks[b].AngleDetector(target, contactPoint, l.getXPos(nx), y, l);
        }
    }

    public override void Up(MobileObject target, Transform contactPoint, int x, int y, BlockLayer l)
    {
        float ny = GetY(y, l) + BlockLayer.BlockSize;

        int b = l.getBlock(contactPoint.position.x, ny);

        if (b >= 0)
        {

            BlockLayer.AllBlocks[b].Up(target, contactPoint, x, l.getYPos(ny), l);
        }
    }
}
