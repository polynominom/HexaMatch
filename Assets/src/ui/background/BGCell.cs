using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Background
{
    public class BGCell: MonoBehaviour
    {
        // row and column index in the imaginary cell background
        private uint _row;
        private uint _col;

        public void Awake()
        {
            
        }

        public void Start()
        {

        }

        public void SetCell(uint row, uint col)
        {
            _row = row;
            _col = col;
            
        }
    }
}

