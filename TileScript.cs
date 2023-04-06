using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] GameObject[] walls;
    [SerializeField] SpriteRenderer floor;

    public enum States {
        empty,
        current,
        completed,
    }

    public void setState(States state) {
        switch (state) {
            case States.empty:
                floor.color = new Color(0.84f, 0.84f, 0.84f);
                break;
            case States.current:
                floor.color = Color.yellow;
                break;
            case States.completed:
                floor.color = Color.blue;
                break;
        }
    }

    public void destroyWall(Vector2 direction) {
        if (direction == new Vector2(0, 1)) {
            walls[0].SetActive(false);
        }
        if (direction == new Vector2(-1, 0)) {
            walls[1].SetActive(false);
        }
    }

    public void destroyNextWall(Vector2 direction) {
        if (direction == new Vector2(0, -1)) {
            walls[0].SetActive(false);
        }
        if (direction == new Vector2(1, 0)) {
            walls[1].SetActive(false);
        }
    }

    public void addWall(Vector2 direction) {
        if (direction.y == -1) {
            walls[2].SetActive(true);
        }
        if (direction.x == 1) {
            walls[3].SetActive(true);
        }
    }

    public void destroyTile() {
        Destroy(gameObject);
    }

    public void resize(int scale) {
        transform.localScale = new Vector3(scale, scale, scale);
    }    
}
