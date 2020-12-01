using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingControl : MonoBehaviour
{
    public GameObject[] buildingPrefabs;

    private GameObject[,] _buildingMap;

    private GameObject ghostImage;

    [SerializeField]
    private Material[] ghostmaterials;
    


    private void Start()
    {
        Vector2 worldSize = TerrainControl.wordlSize;
        _buildingMap = new GameObject[(int)worldSize.x,(int) worldSize.y];

        EventManager.current.onShowGhostBuilding += showGhostBuilding;
        EventManager.current.onPlacingBuilding += placeBuilding;
        EventManager.current.onBuildingPlaced += hideGhostBuilding;
        EventManager.current.onRemoveBuilding += removeBuilding;
    }

    private GameObject getPrefab(string building)
    {
        foreach (GameObject g in buildingPrefabs)
            if (g.name.Equals(building))
                return g;

        return null;
    }

    public void placeBuilding()
    {
        if (!canPlaceBuilding())
            return;

        Vector3 contstructionPosition = ghostImage.transform.position;
        Node positionNode = Pathfinding.getNodeByPosition(contstructionPosition);

        positionNode.canWalkHere = false;

        GameObject building = Instantiate(ghostImage,contstructionPosition,ghostImage.transform.rotation, transform);
        building.name = ghostImage.name;

        _buildingMap[(int)contstructionPosition.x, (int)contstructionPosition.z] = building;

        EventManager.current.buildingPlaced();
    }

    public void showGhostBuilding(string building)
    {
        GameObject targetObject = getPrefab(building);
        GameObject cursorObject = CursorObject.cursorObject;

        ghostImage = Instantiate(targetObject, cursorObject.transform.position, Quaternion.identity, cursorObject.transform);
        ghostImage.name = targetObject.name;
    }

    public void hideGhostBuilding()
    {
        Destroy(ghostImage);
    }

    public void removeBuilding(Vector3 position)
    {
        GameObject buildingToRemove = getBuilding((int)position.x, (int)position.z);

        if (buildingToRemove == null)
            return;

        Node positionNode = Pathfinding.getNodeByPosition(buildingToRemove.transform.position);

        positionNode.canWalkHere = true;
    }

    public GameObject getBuilding(int x,int z)
    {
        return _buildingMap[x, z];
    }

    private void rotateGhost(float rotation)
    {
        ghostImage?.transform.Rotate(new Vector3(0,rotation,0));
    }

    private bool canPlaceBuilding()
    {
        if (ghostImage == null)
            return false;
        int x, z;

        x = (int)ghostImage.transform.position.x;
        z = (int)ghostImage.transform.position.z;

        return (_buildingMap[x, z] == null);

    }

   

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            rotateGhost(-90);

        if (Input.GetKeyDown(KeyCode.E))
            rotateGhost(90);

    }
}
