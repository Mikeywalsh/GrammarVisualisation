using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TreeVisualisation.Core
{
    interface ITreeNode
    {
        int Depth { get; }

        Vector2 Position { get; }

        int SiblingCount();
    }
}
