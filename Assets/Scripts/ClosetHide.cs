using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetHide : MonoBehaviour
{

    [SerializeField] private GameObject interactionText;

    [SerializeField] private MoveMenuCamera cameraInside;
    [SerializeField] private MoveMenuCamera cameraOutside;

    private bool playerInRange = false;

    private bool playerInsideCloset = false;

    public bool playerOpeningCloset = false;   

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerObj")
        {
            interactionText.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "PlayerObj")
        {
            interactionText.SetActive(false);
            playerInRange = false;
        }

    }

    private IEnumerator EnterCloset()
    {

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().rb.velocity = Vector3.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>().enabled = false;
        GameObject.FindGameObjectWithTag("PlayerInterface").GetComponent<Canvas>().enabled = false;

        StartCoroutine(cameraInside.CameraMovement());

        interactionText.SetActive(false);

        cameraOutside.finalCameraPos = cameraInside.initialCameraPos;
        cameraOutside.finalCameraRotation = cameraInside.initialCameraRotation;

        anim.Play("Closet_Open");

        yield return new WaitForSeconds(cameraInside.duration);

        playerInsideCloset = true;

    }

    private IEnumerator ExitCloset()
    {

        StartCoroutine(cameraOutside.CameraMovement());

        anim.Play("Closet_Open");

        yield return new WaitForSeconds(cameraInside.duration);

        interactionText.SetActive(true);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>().enabled = true;
        GameObject.FindGameObjectWithTag("PlayerInterface").GetComponent<Canvas>().enabled = true;

        playerInsideCloset = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !playerOpeningCloset)
        {

            if (!playerInsideCloset) StartCoroutine(EnterCloset());
            
            else StartCoroutine(ExitCloset());

        }
    }

}
