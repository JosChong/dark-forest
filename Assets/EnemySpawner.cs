using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject[] enemies;
	public int spawnType = 0;
	public int health = 300;
    private float time = 0;
    public float spawnInterval = 0;
    public int spawnDistance = 0;
    public float freezeHeight = 0;
    public int spawnLimit = 0;
    private int spawnCount = 0;
    public float spawnHeight = 0;
	
	private Color origColor;
    public bool destructible = true;

    public bool spawnerActive = true;
	
	void Start() {
        enemies = new GameObject[2];
        enemies[0] = (GameObject) Resources.Load("Spider");
        enemies[1] = (GameObject) Resources.Load("Skeleton");
		Spawn(spawnType);
		
        if (destructible)
        {
            origColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        }
	}
 
	void Update() {
        if (!spawnerActive)
        {
            return;
        }

		time += Time.deltaTime;
 
		if (time >= spawnInterval) {
			time = 0.0f;
			Spawn(spawnType);
		}
	}

	void Spawn(int type) {
        if (spawnLimit != 0 && spawnCount >= spawnLimit) {
            return;
        }

		if (type == 0)
		{
			GameObject enemy = Instantiate(enemies[type], transform.position + new Vector3(spawnDistance, spawnHeight, -1), Quaternion.identity);
            enemy.GetComponent<SpiderEnemy>().freezeHeight = freezeHeight - 0.4f;
            spawnCount++;

		}
		else if (type == 1)
        {
            GameObject enemy = Instantiate(enemies[type], transform.position + new Vector3(spawnDistance, 0, -1), Quaternion.identity);
            enemy.GetComponent<SkeletonEnemy>().freezeHeight = freezeHeight;
            spawnCount++;
        }
        else
        {
            int randomType = Random.Range(0, enemies.Length);
            Spawn(randomType);
        }
    }

    public void DealDamage(int damage)
    {
	    health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
	        StartCoroutine(coTintEnemy());
        }
    }
	
	public IEnumerator coTintEnemy()
	{
		Color modifiedColor = new Color(origColor.r - .5f, origColor.g - 1.0f, origColor.b - 1.0f, 1f);
		gameObject.GetComponentInChildren<SpriteRenderer>().color = modifiedColor;
		yield return new WaitForSeconds(0.20f);
		gameObject.GetComponentInChildren<SpriteRenderer>().color = origColor;
		yield return null;
	}
}
