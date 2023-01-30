using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;

public class Coord
{
    public int col;
    public int row;
    public Coord() { row = 0; col = 0; }
    public Coord(int col, int row) { this.col = col; this.row = row; }
    public override bool Equals(object obj)
    {
        return this.Equals(obj as Coord);
    }

    public override int GetHashCode()
    {
        return (col,row).GetHashCode();
    }

    public bool Equals(Coord other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return col == other.col && row == other.row;

    }
    public static Coord GetUpNeighborCoord(Coord coord)
    {
        return new Coord(coord.col, coord.row - 1);
    }

    public static Coord GetBottomNeighborCoord(Coord coord)
    {
        return new Coord(coord.col, coord.row + 1);
    }

    public static Coord GetUpRightCoord(Coord coord)
    {
        if (coord.col % 2 == 0)
            return new Coord(coord.col + 1, coord.row - 1);
        else
            return new Coord(coord.col + 1, coord.row);
    }

    public static Coord GetUpLeftCoord(Coord coord)
    {
        if (coord.col % 2 == 0)
            return new Coord(coord.col - 1, coord.row - 1);
        else
            return new Coord(coord.col - 1, coord.row);
    }

    public static Coord GetBottomRightCoord(Coord coord)
    {
        if (coord.col % 2 == 0)
            return new Coord(coord.col + 1, coord.row);
        else
            return new Coord(coord.col + 1, coord.row + 1);
    }

    public static Coord GetBottomLeftCoord(Coord coord)
    {
        if (coord.col % 2 == 0)
            return new Coord(coord.col - 1, coord.row);
        else
            return new Coord(coord.col - 1, coord.row + 1);
    }
}
public class Node
{
    public int id = -1;
    public Coord coord;
    public List<int> neighbors = new List<int>()
    { -1, -1, -1, -1, -1, -1};

    public Node()
    {
        
    }

    public Node(int id, Coord coord)
    {
        this.id = id;
        this.coord = coord;
    }
}

[Serializable]
public struct Probability
{
    public Probability(HexaType type, double prob)
    {
        this.type = type;
        this.prob = prob;
    }
    public HexaType type;
    public double prob; 
}

public class ProceduralHexagonalLevelGenerator : MonoBehaviour
{
    private static readonly System.Random random = new System.Random();

    public List<Material> materials;
    public List<Probability> typeProbabilities;
    public int maxRowCount = 7;
    public int maxColCount = 5;
    public Vector2Int nodeRange;
    public Vector3 startPos; //(-2.1, 2.6, 0.0)
    public HexaNode nodePrefab;


    private Vector3 DiagonalFactors = HexaStateHelper.UpperRightOffset;

    private List<Node> _debugNodes;
    private List<int> _debugVisitedNodes;
    private List<Coord> _debugCoords;

    private void Awake()
    {
        MakeScene();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public static double GetSomeRandomNumber()
    {
        lock(random)
        {
            return random.NextDouble();
        }
    }

    private List<Coord> GetAllCoord()
    {
        List<Coord> coords = new List<Coord>();
        for(int col = 0; col < maxColCount; col++)
            for(int row = 0; row < maxRowCount; row++)
                coords.Add(new Coord(col, row));

        return coords;
    }

    /* Possibilities
     hexaType = {
    "red"    : ["red", 0.2],
    "green"  : ["green", 0.15],
    "blue"   : ["blue", 0.3],
    "yellow" : ["yellow", 0.15],
    "purple" : ["purple", 0.1],
    "black"  : ["black", 0.05],
    "white"  : ["white", 0.05]
}
     */

    HexaType GetRandomType()
    {
        // from probabilities get CDF
        List<Probability> cdfList = new List<Probability>();
        double totalCDF = 0;
        typeProbabilities.ForEach((Probability p) =>
        {
            totalCDF += p.prob;
            cdfList.Add(new Probability(p.type, totalCDF));
            
        });

        double realRoll = GetSomeRandomNumber();

        foreach (var p in cdfList)
        {
            if (p.prob > realRoll)
                return p.type;
        }

        return HexaType.red;
    }

    private void GenerateState(List<Coord> coords)
    {
        List<HexaNode> nodes = new List<HexaNode>();
        List<HexaNode> targetNodes = new List<HexaNode>();

        coords.ForEach((Coord c) =>
        {
            float x = DiagonalFactors.x * c.col;
            float y = -c.row;
            if (c.col % 2 == 0)
                y += DiagonalFactors[1];
            var pos = startPos + new Vector3(x - 0.05F, y - 0.5F, -0.2F);
            HexaType type = GetRandomType();
            HexaNode h = Instantiate(nodePrefab, pos, Quaternion.identity);
            h.transform.parent = transform.GetChild(0);
            h.SetColor(type, materials);
            nodes.Add(h);

            var targetNode = Instantiate(nodePrefab, pos, Quaternion.identity);
            targetNode.transform.parent = transform.GetChild(1);
            targetNode.GetComponent<HexaMovement>().enabled = false;
            targetNode.GetComponent<PolygonCollider2D>().enabled = false;
            targetNode.SetColor(type, materials);
            targetNodes.Add(targetNode);
        });

        //initialize playable level
        var state = transform.GetChild(0).GetComponent<HexaState>();
        state.SetAllNodes(nodes);
        state.IsRandom = true;

        // initialize target state
        transform.GetChild(1).GetComponent<GoalStateRandomitazion>().SetAllNodes(nodes);
        transform.GetChild(1).position += new Vector3(200, 200);
    }

    private void AssignNeighbor(Node node1, Node node2)
    {
        if (Coord.GetUpNeighborCoord(node1.coord).Equals(node2.coord))
            node1.neighbors[0] = node2.id;
        else if (Coord.GetUpRightCoord(node1.coord).Equals(node2.coord))
            node1.neighbors[1] = node2.id;
        else if (Coord.GetBottomRightCoord(node1.coord).Equals(node2.coord))
            node1.neighbors[2] = node2.id;
        else if (Coord.GetBottomNeighborCoord(node1.coord).Equals(node2.coord))
            node1.neighbors[3] = node2.id;
        else if (Coord.GetBottomLeftCoord(node1.coord).Equals(node2.coord))
            node1.neighbors[4] = node2.id;
        else if (Coord.GetUpLeftCoord(node1.coord).Equals(node2.coord))
            node1.neighbors[5] = node2.id;
    }

    //checks if the removable id splitting the graph
    private bool IsSplitting(List<Coord> coords, int removeId)
    {
        List<Node> nodes = new List<Node>();
        for(int i = 0; i < coords.Count; i++)
        {
            if (i == removeId)
                continue;

            nodes.Add(new Node(i, coords[i]));
        }
        // ASSIGN THE NEIGHBORS AFTER EXCLUDING THE REMOVED ID
        // O(n_square) =??=???????
        for(int i = 0; i < nodes.Count-1;i++)
        {
            for(int j = i; j < nodes.Count; j++)
            {
                AssignNeighbor(nodes[i], nodes[j]);
                AssignNeighbor(nodes[j], nodes[i]);
            }
        }

        // TRAVERSE THE GRAPH WITH BREADTH-FIRST SEARCH.
        Queue<Node> nodeQueue = new Queue<Node>();
        List<int> visitedNodes = new List<int>();
        nodeQueue.Enqueue(nodes[0]);

        while (nodeQueue.Count > 0)
        {
            Node n = nodeQueue.Dequeue();
            if (visitedNodes.Contains(n.id))
                continue;

            visitedNodes.Add(n.id);
            n.neighbors.ForEach((int neighborId) =>
            {
                if(neighborId != -1)
                {
                    Node neighbor = nodes.Find((Node tmp)=> { return tmp.id == neighborId; });
                    nodeQueue.Enqueue(neighbor);
                }
            });
        }

        _debugNodes = nodes;
        _debugVisitedNodes = visitedNodes;
        _debugCoords = coords;
        return visitedNodes.Count != (coords.Count - 1);
    }

    private void RemoveRandomNodes(List<Coord> coords)
    {
        System.Random rnd = new System.Random();
        // get the total node count in the final graph
        int nodeCount = rnd.Next(nodeRange.x, nodeRange.y + 1);
        //calculate to be removed content count
        int iShouldRemove = maxColCount * maxRowCount - nodeCount;
        if (iShouldRemove < 0)
            throw new Exception();

        for (; iShouldRemove > 0; --iShouldRemove)
        {
            // OBTAIN RANDOM ID THAT DOESNT SPLIT THE GRAPH
            int randomId = rnd.Next(0, coords.Count);

            while (IsSplitting(coords, randomId))
                randomId = rnd.Next(0, coords.Count);

            coords.RemoveAt(randomId);
        }
    }

    private void MakeScene()
    {
        List<Coord> coords = GetAllCoord();
        RemoveRandomNodes(coords);
        GenerateState(coords);
    }


}
//    private HexaState state;
//    public int nodeCount;
//    public float addProb;
//    public float iterateProb;
//    private int globalCount = 1;
//    private string fname = "";

//    private List<HexaNode> allNodes;
//    

//    private ProcessStartInfo psi;
//    private string scriptPath = @"/Users/Apple/Desktop/PreceduralHexagonLogic/random_level_generator.py";
//    public ProceduralHexagonalLevelGenerator()
//    {
 
//    }

//    private void Awake()
//    {
//        state = transform.GetChild(0).GetComponent<HexaState>();
//    }

//    private void Update()
//    {
//        if(Input.GetKeyDown(KeyCode.A))
//        {
//            run();
//        }

//    }

//    public void run()
//    {
//        Destroy(transform.GetChild(0).gameObject);

//        GameObject go = Instantiate(LevelPreObj);
//        go.transform.parent = transform;
//        state = go.GetComponent<HexaState>();

//        allNodes = new List<HexaNode>();
//        psi = new ProcessStartInfo();
//        // point to python virtual env
//        psi.FileName = @"/usr/local/bin/python";

//        fname = "Temp" + globalCount.ToString();
//        // Provide arguments
//        psi.Arguments = string.Format("\"{0}\" -n \"{1}\" -pa \"{2}\" -pi \"{3}\" -fn \"{4}\"", scriptPath, nodeCount, addProb, iterateProb, fname);

//        // Process configuration
//        psi.UseShellExecute = false;
//        psi.CreateNoWindow = false;
//        psi.RedirectStandardOutput = true;
//        psi.RedirectStandardError = true;

//        // Execute process and get output
//        var errors = "nothing";
//        var results = "nothing";
//        using (var process = Process.Start(psi))
//        {
//            errors = process.StandardError.ReadToEnd();
//            results = process.StandardOutput.ReadToEnd();
//        }

//        // grab errors and display them in UI
//        StringBuilder buffy = new StringBuilder();
//        buffy.Append("ERRORS:\n");
//        buffy.Append(errors);
//        buffy.Append("\n\n");
//        buffy.Append("Results:\n");
//        buffy.Append(results);
//        UnityEngine.Debug.Log(buffy.ToString());
//        globalCount++;
//        state.GenerateNodes(fname);

        
//    }

//    IEnumerator AbortTask(Thread thread, int limitInSeconds)
//    {
//        var t = 0.0F;
//        while (t < limitInSeconds)
//        {
//            t += Time.deltaTime;
//            yield return new WaitForEndOfFrame();
//        }

//        if(thread.IsAlive)
//        {
//            UnityEngine.Debug.Log("no response from a thread, ABORTING IT!");
//            thread.Abort();
//        }
//        else
//        {
//            UnityEngine.Debug.Log("LMAO");
//        }
        
//    }
//}
