using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jam
{
    public struct GridInfo
    {
        public GridInfo(bool isOccupied, bool hasSlot)
        {
            this.isOccupied = isOccupied;
            this.hasSlot = hasSlot;
        }
        public bool isOccupied;
        public bool hasSlot;
    }

    public class Grid
    {
        public int Width { protected set; get; }
        public int Height { protected set; get; }

        private GridInfo[] m_grid;
        // Start is called before the first frame update

        public GridInfo this[int x, int y]
        {
            get
            {
                if (!CheckBounds(x, y)) return new GridInfo(true, true);
                return m_grid[x + y * Width];
            }
        }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            m_grid = new GridInfo[width * height];
        }

        bool CheckBounds(int x, int y)
        {
            if (x < 0 || x >= Width) return false;
            if (y < 0 || y >= Height) return false;
            return true;
        }

        public void SetOccupied(int x, int y, bool value)
        {
            if (!CheckBounds(x, y)) return;
            int index = x + y * Width;
            GridInfo info = m_grid[index];
            info.isOccupied = value;
            m_grid[index] = info;
        }

        public void SetSlot(int x, int y, bool value)
        {
            if (!CheckBounds(x, y)) return;
            int index = x + y * Width;
            GridInfo info = m_grid[index];
            info.hasSlot = value;
            m_grid[index] = info;
        }
    }
}
