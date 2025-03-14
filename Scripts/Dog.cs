/*using UnityEngine;
using System.Collections;

public class DogSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip laughClip;
    public AudioClip barkClip;

    public void PlayLaughSound()
    {
        audioSource.PlayOneShot(laughClip);
    }

    public void PlayBarkSound()
    {
        audioSource.PlayOneShot(barkClip);
    }
}

public class DogController : MonoBehaviour
{
    private Animator animator;
    private Vector3 startPosition;

    [Header("Movement Settings")]
    public float popUpHeight = 1f;
    public float popUpSpeed = 2f;

    [Header("Animation States")]
    private const string GRAB_ONE = "GrabOne";
    private const string GRAB_TWO = "GrabTwo";
    private const string LAUGH = "Laugh";

    private void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    public void PopUp(System.Action onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(MoveVertically(startPosition.y + popUpHeight, onComplete));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(MoveVertically(startPosition.y));
    }

    public void Laugh()
    {
        animator.Play(LAUGH);
    }

    public void GrabOne()
    {
        animator.Play(GRAB_ONE);
    }

    public void GrabTwo()
    {
        animator.Play(GRAB_TWO);
    }

    private IEnumerator MoveVertically(float targetY, System.Action onComplete = null)
    {
        while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), popUpSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        onComplete?.Invoke();
    }
}

public class DogBehavior : MonoBehaviour
{
    private DogController dogController;

    private void Start()
    {
        dogController = GetComponent<DogController>();
    }

    public void OnDuckShot(int ducks)
    {
        dogController.PopUp(() =>
        {
            if (ducks == 1)
                dogController.GrabOne();
            else if (ducks == 2)
                dogController.GrabTwo();
            StartCoroutine(HideAfterDelay());
        });
    }

    public void OnMiss()
    {
        dogController.PopUp(() =>
        {
            dogController.Laugh();
            StartCoroutine(HideAfterDelay());
        });
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        dogController.Hide();
    }
}*/
using UnityEngine;
using System.Collections;

public class DogController : MonoBehaviour
{
    private Animator animator;
    private Vector3 startPosition;
    private bool isMoving = false;

    [Header("Movement Settings")]
    public float popUpHeight = 1f;
    public float popUpSpeed = 2f;
    public float hideSpeed = 2f;

    [Header("Animation States")]
    private const string GRAB_ONE = "GrabOne";
    private const string GRAB_TWO = "GrabTwo";
    private const string LAUGH = "Laugh";

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    // Call this function when user misses
    public void OnMiss()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveUpAndLaugh());
        }
    }

    // Call this function when user catches 1 or 2 ducks
    public void OnDuckCaught(int ducksCaught)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveUpAndGrab(ducksCaught));
        }
    }

    // Coroutine to move the dog up and make it laugh
    private IEnumerator MoveUpAndLaugh()
    {
        isMoving = true;
        float targetY = startPosition.y + popUpHeight;
        
        // Move the dog up
        while (transform.position.y < targetY)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), popUpSpeed * Time.deltaTime);
            yield return null;
        }

        // Once the dog reaches the top, play the laugh animation
        animator.Play(LAUGH);

        // Wait for the laugh animation to finish, then hide the dog
        yield return new WaitForSeconds(1.5f);
        HideDog();
    }

    // Coroutine to move the dog up and grab one or two ducks
    private IEnumerator MoveUpAndGrab(int ducksCaught)
    {
        isMoving = true;
        float targetY = startPosition.y + popUpHeight;

        // Move the dog up
        while (transform.position.y < targetY)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), popUpSpeed * Time.deltaTime);
            yield return null;
        }

        // Once the dog reaches the top, play the appropriate grab animation
        if (ducksCaught == 1)
        {
            animator.Play(GRAB_ONE);
        }
        else if (ducksCaught == 2)
        {
            animator.Play(GRAB_TWO);
        }

        // Wait for the grab animation to finish, then hide the dog
        yield return new WaitForSeconds(1.5f);
        HideDog();
    }

    // Coroutine to hide the dog back to the bottom of the screen
    private void HideDog()
    {
        StartCoroutine(MoveDown());
    }

    // Coroutine to move the dog down after showing it
    private IEnumerator MoveDown()
    {
        float targetY = startPosition.y;

        while (transform.position.y > targetY)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, targetY, transform.position.z), hideSpeed * Time.deltaTime);
            yield return null;
        }

        // Once the dog reaches the bottom, reset isMoving flag
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        isMoving = false;
    }
}

