using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingPlanks : InteractableObject
{

    [SerializeField] private GameObject redScrewOne;
    [SerializeField] private GameObject redScrewTwo;

    public override void Interaction()
    {

        if (PlayerInventory.Instance.currentItem.GetComponent<PickableItem>().scriptableItem.name == "RedScrewDriver")
        {
            redScrewOne.GetComponent<Animator>().Play("Screw");
        }

    }

    public override void InteractionError()
    {
        StartCoroutine(ShowError());
    }

    public IEnumerator ShowError()
    {
        PlayerInterface.Instance.ErrorText.text = "You need to find the screwdrivers";
        yield return new WaitForSeconds(2);
        PlayerInterface.Instance.ErrorText.text = "";
    }

}
