using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace 
{
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;

    public TerrainFace(Mesh mesh,int resolution,Vector3 localUp){
        this.mesh = mesh;
        this.localUp = localUp;
        this.resolution = resolution;

        this.axisA = new Vector3(localUp.y,localUp.z,localUp.x);
        this.axisB = Vector3.Cross(localUp,this.axisA);
    }

    public void ConstructMesh()
    {
         Vector3[] vertices = new Vector3[resolution*resolution];
         int[] triangles = new int[(resolution-1)*(resolution-1)*2*3];
         int triangleIdx = 0;

         for(int y=0; y<resolution;y++){
            for (int x=0; x<resolution;x++){
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x,y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x-0.5f)*2*axisA + (percent.y-0.5f)*2*axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere;

                if(x != resolution-1 && y != resolution-1){
                    triangles[triangleIdx]   = i ;
                    triangles[triangleIdx+1] = i + resolution + 1;
                    triangles[triangleIdx+2] = i + resolution;

                    triangles[triangleIdx+3] = i ;
                    triangles[triangleIdx+4] = i + 1;
                    triangles[triangleIdx+5] = i + resolution + 1;
                    triangleIdx += 6;
                }
            }
         }

         mesh.Clear();
         mesh.vertices = vertices;
         mesh.triangles = triangles;
         mesh.RecalculateNormals();
    }
}