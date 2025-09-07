using System;
using System.Collections.Generic;

namespace Game.Bag.Model
{
    /// <summary>
    /// 背包整体数据快照（从 bag.json 反序列化）。
    /// </summary>
    [Serializable]
    public class BagDatabase
    {
        public BagDatabase()
        {
            grid = new GridSize { rows = 1, columns = 1 };
            items = new List<BagItem>();
        }

        [Serializable]
        public class GridSize
        {
            public int rows = 4;
            public int columns = 5;
        }

        public GridSize grid = new GridSize();
        public List<BagItem> items = new List<BagItem>();

        #region Properties
        public int Capacity => grid.rows * grid.columns;
        #endregion
    }
}
