using UnityEngine;
using System.Collections.Generic;

public class TileSpawnerScript : MonoBehaviour
{
    [SerializeField] int size;
    [SerializeField] int scale;
    [SerializeField] int randomness;
    [SerializeField] GameObject tile;
    List<GameObject> tiles = new List<GameObject>(); 

    void Start() {
        tiles = generateMaze(); 
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            destroyTiles();
            tiles = generateMaze();
        }
    }

    public void destroyTiles () {
        foreach (GameObject i in tiles) {
            i.GetComponent<TileScript>().destroyTile();
        }
    }

    public List<GameObject> generateMaze () {       
        int x, y;
        int repeatCounter = 0;
        GameObject currentTile;
        Vector2    currentPos;
        Vector2   repeatedPos = new Vector2(0, 0);
        List<GameObject> tilesList = new List<GameObject>();
        List<Vector2>        tilePos = new List<Vector2>();
        List<Vector2>  availablePath = new List<Vector2>();
        List<Vector2> completedTiles = new List<Vector2>();
        List<Vector2>    currentPath = new List<Vector2>();

        for (x = 0; x < size; x++) {
            for (y = 0; y < size; y++) {
                var tileSpawned = Instantiate(tile, new Vector2(x*scale, y*scale), Quaternion.identity);
                tilesList.Add(tileSpawned);
                tilePos.Add(new Vector2(x, y));
                tileSpawned.GetComponent<TileScript>().resize(scale);
                if (y == 0)      tileSpawned.GetComponent<TileScript>().addWall(new Vector2(0, -1));
                if (x == size-1) tileSpawned.GetComponent<TileScript>().addWall(new Vector2(1,  0));
            }
        }

        currentTile = tilesList[Random.Range(0, tilesList.Count - 1)];
        currentTile.GetComponent<TileScript>().setState(TileScript.States.current);
        currentPos = tilePos[tilesList.IndexOf(currentTile)];

        while (completedTiles.Count < tilesList.Count) {
            availablePath.Clear();

            if (currentPos.x - 1 != -1   && !currentPath.Contains(new Vector2(currentPos.x - 1, currentPos.y)) && !completedTiles.Contains(new Vector2(currentPos.x - 1, currentPos.y))) availablePath.Add(new Vector2(-1, 0));
            if (currentPos.x + 1 != size && !currentPath.Contains(new Vector2(currentPos.x + 1, currentPos.y)) && !completedTiles.Contains(new Vector2(currentPos.x + 1, currentPos.y))) availablePath.Add(new Vector2(1,  0));
            if (currentPos.y - 1 != -1   && !currentPath.Contains(new Vector2(currentPos.x, currentPos.y - 1)) && !completedTiles.Contains(new Vector2(currentPos.x, currentPos.y - 1))) availablePath.Add(new Vector2(0, -1));
            if (currentPos.y + 1 != size && !currentPath.Contains(new Vector2(currentPos.x, currentPos.y + 1)) && !completedTiles.Contains(new Vector2(currentPos.x, currentPos.y + 1))) availablePath.Add(new Vector2(0,  1));
            if (currentPath.Count == 0) availablePath.Remove(new Vector2(0,  1));

            if (availablePath.Count > 0) {
                var direction = Random.Range(0, availablePath.Count - 1);
                
                if (availablePath[direction] == repeatedPos) {
                    repeatCounter++;
                    if (repeatCounter > randomness && availablePath.Count > 1) {
                        availablePath.Remove(repeatedPos);
                        direction = Random.Range(0, availablePath.Count - 1);
                        repeatCounter = 0;
                    }
                } else {
                    repeatedPos = availablePath[direction];
                }

                tilesList[tilePos.IndexOf(currentPos)].GetComponent<TileScript>().destroyWall(availablePath[direction]);
                currentPos += availablePath[direction];

                tilesList[tilePos.IndexOf(currentPos)].GetComponent<TileScript>().destroyNextWall(availablePath[direction]);
                currentPath.Add(currentPos);
                tilesList[tilePos.IndexOf(currentPos)].GetComponent<TileScript>().setState(TileScript.States.current);
            } else {
                currentPos = currentPath[currentPath.Count - 1];
                completedTiles.Add(currentPath[currentPath.Count - 1]);
                tilesList[tilePos.IndexOf(currentPos)].GetComponent<TileScript>().setState(TileScript.States.completed);
                currentPath.Remove(currentPath[currentPath.Count - 1]);
            }
        }

        foreach (GameObject i in tilesList) {
            i.GetComponent<TileScript>().setState(TileScript.States.empty);
        }

        return tilesList;
    }
}
