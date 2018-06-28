using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------------------------------------
// Helper class that performs depth first search on a graph
//------------------------------------------------------------------------------------------------
public class dfs : MonoBehaviour {

	public Dictionary <Vector3, List<Vector3> > roadWays = Roads.neighbourGraph;
	public List<Vector3> nodes = Roads.vertices;
	Dictionary<Vector3, int> visited = new Dictionary<Vector3, int>();	
	// Use this for initialization

	public void initialise(){
		//initialise the visited array
		for (int i=0; i<nodes.Count; i++) {
			visited.Add (nodes [i], 0);
		}
	}

	public List<Vector3> dfsUtil(){
		List<Vector3> subgraphNodes = new List<Vector3> ();
		for (int i=0; i<nodes.Count; i++) {
			if (visited [nodes [i]] != 1) {
				subgraphNodes.Add (nodes [i]);
				DFS (nodes[i]);
			}
		}
		return subgraphNodes;
	}

	void DFS(Vector3 vertex){
		visited [vertex] = 1;
		for (int i=0; i<roadWays[vertex].Count; i++) {
			if (visited [roadWays [vertex] [i]] != 1) {
				DFS (roadWays [vertex] [i]);
			}
		}
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
