using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootDucks : MonoBehaviour
{
    public GameObject duckPrefab;
    public Transform[] spawnPoints;
    public int ducksPerRound = 2;
    public float roundDelay = 2f;

    public int shotsRemaining = 3; // Fixed: Declared this variable
    public int score = 0; // Fixed: Declared this variable

    private int ducksShot = 0;
    private List<GameObject> activeDucks = new List<GameObject>();

    public DogController dogController;
    public AudioSource shootSound;
    public AudioSource duckHitSound;
    public AudioSource missSound;

    public LayerMask duckLayer; // Fixed: Declared this variable

    void Start()
    {
        StartCoroutine(SpawnDucks());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click to shoot
        {
            Shoot();
        }
    }

    IEnumerator SpawnDucks()
    {
        while (true)
        {
            shotsRemaining = 3; // Reset shots at the start of each round
            ducksShot = 0;
            activeDucks.Clear();

            for (int i = 0; i < ducksPerRound; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject duck = Instantiate(duckPrefab, spawnPoint.position, Quaternion.identity);
                activeDucks.Add(duck);
            }

            yield return new WaitUntil(() => AllDucksGone() || shotsRemaining <= 0);

            yield return new WaitForSeconds(1f);

            if (ducksShot == 0)
            {
                dogController.OnMiss();
            }
            else
            {
                dogController.OnDuckCaught(ducksShot);
                score += ducksShot * 10; // Fixed: Score now correctly increments
            }

            yield return new WaitForSeconds(roundDelay);
        }
    }

    bool AllDucksGone()
    {
        activeDucks.RemoveAll(duck => duck == null);
        return activeDucks.Count == 0;
    }

    void Shoot()
    {
        if (shotsRemaining <= 0) return;

        shootSound.Play();
        shotsRemaining--;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, duckLayer);

        if (hit.collider != null)
        {
            GreenDuck duck = hit.collider.GetComponent<GreenDuck>();
            if (duck != null)
            {
                duckHitSound.Play();
                duck.Die(); // Fixed: Ensure GreenDuck.Die() is public
                ducksShot++;
            }
        }
        else
        {
            missSound.Play();
        }
    }
}
