using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    private ShootDucks shootDucks;

    void Start()
    {
        shootDucks = Object.FindFirstObjectByType<ShootDucks>(); // Updated to avoid deprecated method
    }

    void Update()
    {
        scoreText.text = "Score: " + shootDucks.score;
    }
}
