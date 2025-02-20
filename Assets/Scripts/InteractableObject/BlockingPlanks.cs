using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockingPlanks : InteractableObject
{

    [SerializeField] private GameObject[] redScrews;
    [SerializeField] private GameObject redPlank;

    [SerializeField] private GameObject[] greenScrews;
    [SerializeField] private GameObject greenPlank;

    [SerializeField] private GameObject[] purpleScrews;
    [SerializeField] private GameObject purplePlank;

    private int currentPlanks = 3;

    public override void Interaction(string itemName)
    {

        currentPlanks--;

        if (itemName == "RedScrewDriver")
        {
            StartCoroutine(Unscrew(redScrews, redPlank));
        }

        else if (itemName == "GreenScrewDriver")
        {
            StartCoroutine(Unscrew(greenScrews, greenPlank));
        }

        else if (itemName == "PurpleScrewDriver")
        {
            StartCoroutine(Unscrew(purpleScrews, purplePlank));
        }

        // Object not interactable anymore
        if (currentPlanks == 0)
        {
            PlayerInterface.Instance.TextToPlayer.text = "";
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<CheckItem>().enabled = false;
        }

    }

    public IEnumerator Unscrew(GameObject[] screws, GameObject plank)
    {

        foreach (GameObject screw in screws)
        {
            screw.GetComponent<Animator>().Play("ScrewAnim");
            yield return new WaitForSeconds(screw.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            Destroy(screw);
        }

        //Lerps the plank to the side

        float time = 0;

        Vector3 initialPos = plank.transform.position;

        while (time < 0.6)
        {
            time += Time.deltaTime;
            plank.transform.position = Vector3.Lerp(initialPos, initialPos + new Vector3(-1, 0, 0), time);
            yield return null;
        }

        Destroy(plank);

    }

    public override void InteractionError()
    {
        StartCoroutine(ShowError());
    }

    public IEnumerator ShowError()
    {
        PlayerInterface.Instance.ErrorText.text = "You need to find the screwdrivers...";
        yield return new WaitForSeconds(2);
        PlayerInterface.Instance.ErrorText.text = "";
    }

}
