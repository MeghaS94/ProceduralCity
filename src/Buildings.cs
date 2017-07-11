using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Buildings : MonoBehaviour {

	public int width;
	public int height;
	public float c;	
	public List<List<Vector3> > edges = new List< List<Vector3> >(); //how many roads actually exist
	//edges and the buildings corresponding to the edge
	//put index of edge and the list of buildings incident on the edge
	Dictionary <int, List<Vector3> > buildingsOnEdge = new Dictionary< int, List<Vector3>>();
	public List<Vector3> buildingPositions = new List<Vector3> ();	//The positions of each building
	public List<GameObject> houses = new List<GameObject>();


	public void placeBuildings(){
		for (int i=0; i<edges.Count; i++) {
			placeBuildingsOnEdge(edges[i][0] , edges[i][1] , i );
		}
	}
	
	//runs for one edge
	void placeBuildingsOnEdge(Vector3 a, Vector3 b, int indexOfEdge){
		List<Vector3> buildings = new List<Vector3> ();
		float slope = (b.z - a.z) / (b.x - a.x);
		float intercept = a.z - slope * (a.x);
		//parallel lines - y = mx + (c+1)/ (c-1)
		float minx = b.x;
		float maxx = a.x;
		if (a.x < b.x) {
			minx = a.x;
			maxx = b.x;
		}

		//Slope = slope;
		float miny = b.z;
		float maxy = a.z;
		if (a.z < b.z) {
			miny = a.z;
			maxy = b.z;
		}

		//two points p1, p2
		//assume x = minx

		//whats the offset of the buildings from the roads -> c
		c = -2f;
		float p1x = minx;
		float p1y = slope * p1x + (intercept + c);

		float p2x = maxx;
		float p2y = slope * p2x + (intercept + c);

		//////////////////////////////////
		/*float p3y = miny;
		float p3x = (p3y - (intercept-c))/slope;
		
		float p4y = maxy;
		float p4x = (p4y - (intercept-c))/slope;
		*/
		float p3x = minx;
		float p3y = slope * p3x + (intercept - c);
		
		float p4x = maxx;
		float p4y = slope * p4x + (intercept - c);

		//add an offset to the actual point position
		float offsett = 0f;
		Vector3 point1 = new Vector3 (offsett+p1x, 0f, offsett+p1y);
		Vector3 point2 = new Vector3 (offsett+p2x, 0f, offsett+p2y);

		Vector3 point3 = new Vector3 (offsett+p3x, 0f, offsett+p3y);
		Vector3 point4 = new Vector3 (offsett+p4x, 0f, offsett+p4y);

		//number of r values = len of edge / size occupied by building
		//to generate r value - r values spaced len of edge/ building size apart.
		Vector3 temp = a - b;
		float lenEdge = temp.magnitude;
		//edgeLen = lenEdge;
		float sizeOfBuilding = 1f;
		int numProgress = (int)(lenEdge / sizeOfBuilding);
		//edgeLen = numProgress;
		int factor = 20;
		for (int i=0; i<numProgress*factor; i++) {
			float r = (i - 0f) / (float)( numProgress * factor);
			Vector3 buildingPos1 = returnPoint(point1, point2, r); 
			Vector3 buildingPos2 = returnPoint(point3, point4, r); 

			//check if buildingPos is okay
			if(checkBuildingValidPos(buildingPos1)){
				putBuilding(buildingPos1);
				buildings.Add (buildingPos1);
			}
			if(checkBuildingValidPos(buildingPos2)){
				putBuilding(buildingPos2);
				buildings.Add (buildingPos2);
			}
		}

		//put index of edge and the list of buildings incident on the edge
		buildingsOnEdge.Add (indexOfEdge, buildings);
	}

	Vector3 returnPoint(Vector3 start, Vector3 final, float progress){
		//return a point on the straight line between start and final -> linear interpolation
		Vector3 point = new Vector3(start.x + (final.x - start.x) * progress , 0f, start.z + (final.z - start.z) * progress);
		return point;
	}

	void putBuilding(Vector3 pos){
		//Add a building to the array of building positions
		buildingPositions.Add (pos);
	}

	public void putBuildingsWithPerlinNoise(){

		for (int i=0; i<buildingPositions.Count; i++) {
			GameObject building;
			int result = (int)(Mathf.PerlinNoise (buildingPositions[i].x, buildingPositions[i].z) *10 );

			//downtown area -> Taller buildings
			if (buildingPositions[i].x >= width/2.0f - 10.0f && buildingPositions[i].x <= width/2.0f - 10.0f + 30f &&
			    buildingPositions[i].z >= height/2.0f - 10.0f && buildingPositions[i].z <= height/2.0f - 10.0f + 30f)
			{
				String name = "preFabs/" + "toySkyscraper" + UnityEngine.Random.Range (1, 9).ToString ();
				building = (GameObject)Instantiate (Resources.Load (name));
				float scale = UnityEngine.Random.Range (3.5f, 7f);
				building.transform.localScale = new Vector3 (0.5f, scale, 0.5f);
			}

			else if (result <= 3) {
				String name = "preFabs/" + "tree" + UnityEngine.Random.Range (1, 4).ToString ();
				//Debug.Log (name);
				building = (GameObject)Instantiate (Resources.Load (name));
				float scale = UnityEngine.Random.Range (0.1f, 0.3f);
				building.transform.localScale = new Vector3 (scale, scale, scale);

			} else if (result > 3 && result <= 5) {
				String name = "preFabs/" + "toyHouse" + UnityEngine.Random.Range (1, 7).ToString ();
				building = (GameObject)Instantiate (Resources.Load (name));
				building.transform.localScale = new Vector3 (0.5f, 1, 0.5f);
			} 
			else {
				String name = "preFabs/" + "toyHouse" + UnityEngine.Random.Range (1, 7).ToString ();
				building = (GameObject)Instantiate (Resources.Load (name));
				building.transform.localScale = new Vector3 (0.5f, 2, 0.5f);
			}
			building.transform.position = buildingPositions[i];
			building.name = "house_" + i.ToString();
			houses.Add (building);
			}


	}
	
	//checking if one buildingPosition overlaps another
	bool checkBuildingValidPos(Vector3 currPos){
		//the threshold can be randomised!
		for (int i=0; i<buildingPositions.Count; i++) {
			if(buildingPositions[i] != currPos){
				float distace = (buildingPositions[i] - currPos).magnitude;

				if(distace < 1f)
				{
					return false;
				}
			}
		}
		return true;
	}

}
