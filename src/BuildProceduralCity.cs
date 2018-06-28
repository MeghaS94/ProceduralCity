using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//------------------------------------------------------------------------------------------------
// The main function to build a procedural city, defining some variables.
// Main functionality :
// Scatter buildings, place roads, make sure every road is reachable from every other
// Ensure buildings lie along the edges of the road.
// Try to minimize building road intersections.
//------------------------------------------------------------------------------------------------
public class BuildProceduralCity : MonoBehaviour {
	public int	width;
	public int 	height;
	public int 	initialSpacing = 4; 		//All points initially generated are spaced by this number
	public float 	mean = 6f;
	public float 	std_dev = 2f;
	public float 	probabilityThreshold = 0.8f;
	public int 	seed;
	public float 	offsetFromRoad;


	/// <summary>
	//The variables below are made public so that other classes can access them.
	/// </summary>
	public List<List<Vector3> > edges = new List< List<Vector3> >(); //number of roads

	//All the roads and all the valid building positions
	List<GameObject> roads = new List<GameObject>();
	List<GameObject> houses = new List<GameObject>();

	//a list that stores a point from every subgraph
	List<Vector3> subGraphPoints; 

//--------------------------------------------------------------------------------------------
	void Start () {

		seed = UnityEngine.Random.Range(0, 1000);
		Roads road = new Roads ();
		road.width = width;
		road.height = height;
		road.mean = mean;
		road.std_dev = std_dev;
		// generate random grid positions
		road.generateRandomPoints ();
		road.returnEdges ();
		edges = road.edges;

		// place roads
		road.placeRoads ();
		roads = road.roads;

		// place buildings
		Buildings buildings = new Buildings ();
		buildings.width = width;
		buildings.height = height;
		buildings.edges = edges;
		buildings.c = offsetFromRoad;	//intercept offset
		buildings.placeBuildings ();
		buildings.putBuildingsWithPerlinNoise ();
		houses = buildings.houses;

		scatterHouses ();
		removeIntersections ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//--------------------------------------------------------------------------------------------------

	//Check that roads and houses do not overlap one another
	void removeIntersections(){
		//for every road, for every building, check bounds of road intersects bounds of building. if yes - destroy
		for (int i=0; i<roads.Count; i++) {
			Bounds b1 = GameObject.Find ("road_" + i.ToString ()).GetComponent<Renderer> ().bounds;
			for (int j=0; j<houses.Count; j++) {
				Bounds b2 = GameObject.Find ("house_" + j.ToString ()).GetComponent<Renderer> ().bounds;
				if (b1.Intersects (b2)) {
					Destroy (GameObject.Find ("house_" + j.ToString()));
				}
			}
		}
	}


	void scatterHouses(){
		//100 random houses scattered around the city (area without any roads laid out)
		//scatter some random trees
		for (int i=0; i<100; i++) {
			float X = UnityEngine.Random.Range(0f, width + 50f);
			float Z = UnityEngine.Random.Range(height, height + 50f);
			/*if (X < width && Z > height) {
				X = -X;
			}
			else if (Z < height && X > width) {
				Z = -Z;
			}*/
			GameObject building;
			String name = "preFabs/" + "toyHouse" + UnityEngine.Random.Range (1, 7).ToString ();
			building = (GameObject)Instantiate (Resources.Load (name));
			building.transform.localScale = new Vector3 (0.5f, 2f, 0.5f);
			building.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(-180f, 180f)*Vector3.up);
			building.transform.position = new Vector3(X, 0f, Z);

			float X1 = UnityEngine.Random.Range(width, width + 50f);
			float Z1 = UnityEngine.Random.Range(0f, height + 50f);
			/*if (X < width && Z > height) {
				X = -X;
			}
			else if (Z < height && X > width) {
				Z = -Z;
			}*/
			GameObject building1;
			String name1 = "preFabs/" + "toyHouse" + UnityEngine.Random.Range (1, 7).ToString ();
			building1 = (GameObject)Instantiate (Resources.Load (name1));
			building1.transform.localScale = new Vector3 (0.5f, 2f, 0.5f);
			building1.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(-180f, 180f)*Vector3.up);
			building1.transform.position = new Vector3(X1, 0f, Z1);
		}

		for (int i=0; i<50; i++) {
			float X = UnityEngine.Random.Range (0f, width + 50f);
			float Z = UnityEngine.Random.Range (height, height + 50f);

			GameObject tree;
			String name1 = "preFabs/" + "tree" + UnityEngine.Random.Range (1, 4).ToString ();
			tree = (GameObject)Instantiate (Resources.Load (name1));
			//tree.transform.localScale = new Vector3 (0.5f, 2f, 0.5f);
			tree.transform.rotation = Quaternion.Euler (UnityEngine.Random.Range (-180f, 180f) * Vector3.up);
			tree.transform.position = new Vector3 (X, 0f, Z);

			float X1 = UnityEngine.Random.Range (width, width + 50f);
			float Z1 = UnityEngine.Random.Range (0f, height + 50f);

			GameObject tree1;
			String name2 = "preFabs/" + "tree" + UnityEngine.Random.Range (1, 4).ToString ();
			tree1 = (GameObject)Instantiate (Resources.Load (name2));
			//tree.transform.localScale = new Vector3 (0.5f, 2f, 0.5f);
			tree1.transform.rotation = Quaternion.Euler (UnityEngine.Random.Range (-180f, 180f) * Vector3.up);
			tree1.transform.position = new Vector3 (X1, 0f, Z1);
		}
		for(int i=0;i<50;i++){
			float X2 = UnityEngine.Random.Range (0f, width);
			float Z2 = UnityEngine.Random.Range (0f, height);
			GameObject tree3;
			String name3 = "preFabs/" + "tree" + UnityEngine.Random.Range (1, 4).ToString ();
			tree3 = (GameObject)Instantiate (Resources.Load (name3));
			float scale = UnityEngine.Random.Range (0.1f, 0.3f);
			tree3.transform.localScale = new Vector3 (scale, scale, scale);
			tree3.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(-180f, 180f)*Vector3.up);
			tree3.transform.position = new Vector3(X2, 0f, Z2);
		}
		//scatter some houses and tress outside the width and height area to get the seamless blend
	}
	

}
