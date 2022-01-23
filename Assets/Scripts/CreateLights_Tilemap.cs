using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 
using UnityEngine.Experimental.Rendering.Universal;

public class CreateLights_Tilemap : MonoBehaviour
{

    public Light2D light; 
    public List<Light2D> Lights = new List<Light2D>(); 

    void Awake() {
         Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(position) != null)
            {
                Vector3 cellPosition = tilemap.GetCellCenterWorld(position);
                Lights.Add(Instantiate(light, cellPosition, Quaternion.identity));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
