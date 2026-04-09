using UnityEngine;
using UnityEngine.UI;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int totalCubes = 10;
    public float cubeSpacing = 1.0f;
   
    void Start()
    {
        GenCube();
    }

    public void GenCube()
    {
        Vector3 myPosition = transform.position;

        for (int i = 0; i < totalCubes; i++)
        {
            Vector3 pdsition = new Vector3(myPosition.x, myPosition.y , myPosition.z + (i * cubeSpacing));
            Instantiate(cubePrefab, pdsition, Quaternion.identity);

        }
    }
}
