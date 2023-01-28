using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LevelSelect
{
    public class LevelSelectorGenerator : MonoBehaviour
    {
        private List<Transform> levelNodes;
        private Transform content;
        private readonly Vector3 firstPos = new Vector3(-144.8f, -77.9f, -18.9f);
        private readonly int DIAGONAL_LIMIT = 8;   

        public int GeneratableNodeCount;
        public Transform LevelSelectorUINode;
        public bool ShouldGenerate;
        

        void _FillLevelNodes()
        {
            
            for (int i = 0; i < content.childCount; ++i)
            {
                levelNodes.Add(content.GetChild(i));
            }
        }

        private Vector2Int _GetHexCoordinates(int startedNodeRow, int order)
        {
            int c = order % DIAGONAL_LIMIT;
            int r = startedNodeRow;
            if (c == 2 || c == 3)
                r = startedNodeRow+1;
            else if (c == 4 || c == 5)
                r = startedNodeRow+2;
            else if (c == 6 || c == 7)
                r = startedNodeRow+3;

            // Debug.Log("ORDER COORDINATE FOUND COL: " + c + " ROW: " + r + " FOR ORDER: " + order);
            return new Vector2Int(r, c);
        }

        private void _InitiNode(int startedNodeRow, int at)
        {
            Vector2Int coord = _GetHexCoordinates(startedNodeRow, at);
            Vector3 pos = firstPos + Background.BackgroundHelper.GetPos((uint)coord.x, (uint)coord.y, size: 60);
            GameObject o = Instantiate(LevelSelectorUINode.gameObject, pos, Quaternion.identity);
            o.transform.SetParent(content, false);
            o.GetComponent<LevelNodeUI>().ReferencedLevelId = at+1;
        }

        private void _GenerateNodes()
        {
            int startedNodeRow = 0;
            for (int i = 0; i < GeneratableNodeCount; ++i)
            {
                _InitiNode(startedNodeRow, i);

                if ((i + 1) % DIAGONAL_LIMIT == 0)
                    ++startedNodeRow;
            }
        }

        private void Awake()
        {
            content = transform.GetChild(0);
        }

        void Start()
        {
            if (ShouldGenerate)
            {
                _GenerateNodes();
            }
        }
    }
}
