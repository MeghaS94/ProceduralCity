using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Roads : MonoBehaviour {
	public int width;
	public int height;
	public int initialSpacing = 4; 				//All points initially generated are spaced by this number
	public float probabilityThreshold = 0.8f;	//decides whether or not the form an edge(road)
	public int seed;

	/// <summary>
	//The variables below are made public so that other classes can access them.
	/// </summary>
	public static List<Vector3> vertices = new List<Vector3>();
	public Dictionary <Vector3, int> neighbourCount = new Dictionary<Vector3, int>(); //how many roads go out of every intersection point
	public static Dictionary <Vector3, List<Vector3> > neighbourGraph = new Dictionary<Vector3, List<Vector3> > (); //node and all the nodes its connected to
	public List<List<Vector3> > edges = new List< List<Vector3> >(); //how many roads actually exist
	public List<GameObject> roads = new List<GameObject>();

	
	//function to generate points on the grid spaced by x units left and right
	void generateGridPoints()
	{
		GameObject building;
		int count = 0;
		for (int i=0; i<width; i=i+initialSpacing) {
			for(int j=0;j<height;j=j+initialSpacing){
				vertices.Add(new Vector3(i,0,j));
				building = (GameObject)Instantiate (Resources.Load ("preFabs/cubePrefab1" ));
				building.transform.position = vertices[count];
				count +=1 ;
			}
		}
	}


	//scatter points on the grid at random
	public void generateRandomPoints(){
		//vertices stores the random points
		GameObject building;

		UnityEngine.Random.seed = seed;
		//885, 761, 481- best! 199, 931, 618 , 398
		int count = 0;

		for (int i=0; i<width; i=i+6) {
			for(int j=0;j<height;j=j+6){
				Vector3 temp = new Vector3(i,0,j) + new Vector3(UnityEngine.Random.Range(-2f, 2f), 0 , UnityEngine.Random.Range(-2f, 2f));
				vertices.Add(temp);//new Vector3(Random.Range(-1f, 1f), 0 , Random.Range(-1f, 1f)));
				neighbourCount.Add(vertices[count], 0);
				List<Vector3> temp1 = new List<Vector3>(); //temp variable way
				neighbourGraph.Add(vertices[count], temp1);
				building = (GameObject)Instantiate (Resources.Load ("preFabs/cubePrefab1" ));
				building.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				building.transform.position = vertices[count];

				count +=1 ;
			}
		}

	}
	
	//connect the points to form roads, and store the neighbours
	List<Vector3> returnNeighbours(Vector3 point){
		//returns neighours of the given point
		List<Vector3> neighbours = new List<Vector3> ();

		for (int i=0; i<vertices.Count; i++) {
			float distance = (point - vertices[i]).magnitude;
			float mean = 6f;
			float std_dev = 2f;
			//generate a number from the random distribution with mean and std deviation and use that as a metric to decide
			float radius = RandomFromDistribution.RandomNormalDistribution(mean, std_dev) ;

			Console.WriteLine(distance);
			if(distance < radius ){
				neighbours.Add(vertices[i]);
			}

		}
		return neighbours;
	}

	//populate the edges array
	public void returnEdges(){

		for(int i=0;i<vertices.Count;i++){
			List<Vector3> neighbours = returnNeighbours(vertices[i]);
			neighbourGraph [vertices [i]] = neighbours;
			for(int j=0;j<neighbours.Count;j++){
				float nodeProb = UnityEngine.Random.Range(0f, 1f);
				float neighbourProb = UnityEngine.Random.Range(0f, 1f);
				float neighbourC = neighbourCount[neighbours[j]] * 0.5f; //I dont know why
				float probability = neighbourC*neighbourProb + nodeProb;
				//add an edge, update dictionary 
				if(probability > probabilityThreshold ){
					List <Vector3> temp  = new List<Vector3>();
					temp.Add(vertices[i]);
					temp.Add(neighbours[j]);
					edges.Add(temp);
					neighbourCount[vertices[i]] += 1;
					neighbourCount[neighbours[j]] += 1;

				}
			}
		}
	}
	
	//just an extra method to place a prefab scaled and oriented in a certain way
	void placeRoad(Vector3 a, Vector3 b){
		GameObject road;
		Vector3 midpoint = a+b/2f;
		road = (GameObject)Instantiate (Resources.Load ("preFabs/cubePrefab" ));
		road.transform.position = midpoint;
		road.transform.LookAt (b);
		float r = UnityEngine.Random.Range (0.1f, 1.0f);
		road.transform.localScale = new Vector3(r, 0.1f, Vector3.Distance(a,b));

	}
	
	public void placeRoads(){
		for (int i=0; i<edges.Count; i++) {
			Vector3 midpoint = (edges[i][0] + edges[i][1]) / 2f;
			GameObject road;
			road = (GameObject)Instantiate (Resources.Load ("preFabs/cubePrefab" ));
			road.transform.position = midpoint;
			road.transform.LookAt (edges[i][1]);
			float r = UnityEngine.Random.Range (0.1f, 1.0f);
			road.transform.localScale = new Vector3(r, 0.1f, Vector3.Distance(edges[i][0], edges[i][1]));
			road.name = "road_" + i.ToString();
			roads.Add (road);
		}
		/*
		for (int i=0; i<subGraphPoints.Count-1; i++) {
			//List <Vector3> temp  = new List<Vector3>();
			Vector3 midpoint = (subGraphPoints[i] + subGraphPoints[i+1]) / 2f;
			//midpoint.y = midpoint.y + 0.5f;
			GameObject road;
			road = (GameObject)Instantiate (Resources.Load ("preFabs/cubePrefab" ));
			road.transform.position = midpoint;
			road.transform.LookAt (subGraphPoints[i+1]);
			float r = UnityEngine.Random.Range (0.1f, 1.0f);
			road.transform.localScale = new Vector3(r, 0.1f, Vector3.Distance(subGraphPoints[i], subGraphPoints[i+1]));
			//temp.Add(subGraphPoints[i]);
			//temp.Add(subGraphPoints[i+1]);
			//edges.Add(temp);
		}
		*/
	}
}
