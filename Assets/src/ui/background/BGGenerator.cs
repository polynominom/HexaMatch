using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Background
{
    public class BGGenerator : MonoBehaviour
    {
        public bool shouldGenerate;
        public GameObject BGCell;
        public uint BGSize;
        private Canvas _canvas;

        // Start is called before the first frame update
        void Start()
        {
            this._canvas = GetComponentInParent<Canvas>();
            // ADDs cells in the center
            if(shouldGenerate)
                AddBGCells(18, 30);

            // Move away to fit cells into viewport
            FitToWindow();
        }

        private void FitToWindow()
        {
            var refRes = this._canvas.GetComponent<CanvasScaler>().referenceResolution;
            var rt = GetComponent<RectTransform>();
            //set left
            rt.offsetMin = new Vector2(-refRes.x, rt.offsetMin.y);
            //set top
            rt.offsetMax = new Vector2(rt.offsetMax.x, refRes.y);
        }

        private List<BGCell> AddBGCells(uint totalElementInARow, uint totalElementInACol)
        {
            var list = new List<BGCell>();
            for (uint row = 0; row < totalElementInARow; row++)
            {

                for (uint col = 0; col < totalElementInACol; col++)
                {
                    Vector2 pos = BackgroundHelper.GetPos(row, col, size: BGSize);
                    GameObject o = Instantiate(BGCell, pos, Quaternion.identity);
                    o.transform.localScale = new Vector3(BGSize, BGSize, 1);
                    o.transform.SetParent(transform, false);
                    var cell = o.GetComponent<BGCell>();
                    cell.SetCell(row, col);
                    list.Add(cell);
                }
            }
            return list;
        }
    }
}

