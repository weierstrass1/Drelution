using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SolidSlope45 : SolidSlopeBlock
{
    public SolidSlope45() : base(0, BlockLayer.BlockSize, 0, BlockLayer.BlockSize)
    {
        blockName = "45° Degrees";
    }
}
