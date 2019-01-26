using UnityEngine;

namespace Assets.Escene.Scripts.Blocks
{
    public class DelegateBlock : Block
    {

        public DelegateBlock() : base("Delegate Block", new Color32(0, 255, 0, 128),
        new Vector3(-Layer.BlockSize / 2, -Layer.BlockSize / 2),
        new Vector3(-Layer.BlockSize / 2, Layer.BlockSize / 2),
        new Vector3(Layer.BlockSize / 2, Layer.BlockSize / 2),
        new Vector3(Layer.BlockSize / 2, -Layer.BlockSize / 2))
        {

        }

        public override void AngleDetector(MobileObject target, Transform contactPoint, int x, int y, Layer l)
        {
            float ny = (l.Y - Layer.BlockSize * l.height);
            ny += y + Layer.BlockSize;

            int b = l.getBlock(contactPoint.position.x, ny);

            if (b >= 0)
            {
                float X = l.getXPos(contactPoint.position.x) * Layer.BlockSize;
                float Y = l.getYPos(ny) * Layer.BlockSize;

                Layer.AllBlocks[b].AngleDetector(target, contactPoint, x, y, l);
            }
        }

        public override void Down(MobileObject target, Transform contactPoint, int x, int y, Layer l)
        {
        }

        public override void Left(MobileObject target, Transform contactPoint, int x, int y, Layer l)
        {
        }

        public override void Right(MobileObject target, Transform contactPoint, int x, int y, Layer l)
        {
        }

        public override void Up(MobileObject target, Transform contactPoint, int x, int y, Layer l)
        {
            float ny = (l.Y - Layer.BlockSize * l.height);
            ny += y + Layer.BlockSize;

            int b = l.getBlock(contactPoint.position.x, ny);

            if (b >= 0)
            {
                float X = l.getXPos(contactPoint.position.x) * Layer.BlockSize;
                float Y = l.getYPos(ny) * Layer.BlockSize;

                Layer.AllBlocks[b].Up(target, contactPoint, x, y, l);
            }
        }
    }
}
