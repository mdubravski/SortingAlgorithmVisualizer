using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SortingAlgorithmVisualizer
{
    class SortEngineBubble : ISortEngine
    {

        private int[] array;
        private Graphics g;
        private int maxVal;
        Brush WhiteBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        Brush BlackBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

        public SortEngineBubble(int[] array, Graphics g, int maxVal) 
        {
            this.array = array;
            this.g = g;
            this.maxVal = maxVal;
        }


        public void NextStep()
        {
            // bubble sort O(n^2)
                for (int i = 0; i < array.Count()-1; i++)
                {
                    if (array[i] > array[i + 1]) 
                    {
                        Swap(i, i + 1);
                    }
                }
        }

        private void Swap(int i, int v)
        {
            int temp = array[i];
            array[i] = array[i + 1];
            array[i + 1] = temp;

            DrawBar(i, array[i]);
            DrawBar(v, array[v]);

        }

        public bool IsSorted()
        {
            for (int i = 0; i < array.Count() - 1; i++)
            {
                if (array[i] > array[i + 1]) return false;
            }
            return true;
            
        }

        private void DrawBar(int position, int height) 
        {
            // remove old values display black background
            g.FillRectangle(BlackBrush, position, 0, 1, maxVal);
            // show new values
            g.FillRectangle(WhiteBrush, position, maxVal - array[position], 1, maxVal);
        }

        public void ReDraw()
        {
            throw new NotImplementedException();
        }


    }
}
