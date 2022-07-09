using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGeneratorWithMouse : MonoBehaviour
{
    public GameObject LevelPrefab;
    public HexaNode HexaNodePrefab;
    public Image BottomPipe;
    public List<Material> Materials;
    private int materialIndicator = 0;
    private List<HexaType> types = new List<HexaType>()
    { HexaType.red, HexaType.green, HexaType.blue, HexaType.yellow, HexaType.purple, HexaType.black, HexaType.white, HexaType.empty};

    private HexaState hexaState;
    void Start()
    {
        GameObject go = Instantiate(LevelPrefab);
        go.transform.parent = transform;
        hexaState = go.GetComponent<HexaState>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            materialIndicator = (materialIndicator + 1) % Materials.Count;
            BottomPipe.material = Materials[materialIndicator];
        }
    }

    public void AddHexa(Vector3 pos)
    {
        HexaNode h = Instantiate(HexaNodePrefab, pos, Quaternion.identity);
        h.transform.parent = hexaState.transform;
        h.type = types[materialIndicator];
        h.GetComponent<SpriteRenderer>().material = Materials[materialIndicator];
    }
}
