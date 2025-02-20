using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusePanel : InteractableObject
{

    private Animator anim;

    private int currentFuses = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Interaction(string itemName)
    {

        if (currentFuses == 0)
        {
            anim.Play("AddFuseOne");
        }
        else if (currentFuses == 1){
            anim.Play("AddFuseTwo");
        }
        else if (currentFuses == 2)
        {
            anim.Play("AddFuseThree");
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponent<CheckItem>().enabled = false;
        }

        currentFuses++;

    }

    public override void InteractionError()
    {
        StartCoroutine(ShowError());
    }

    public IEnumerator ShowError()
    {
        PlayerInterface.Instance.ErrorText.text = "You need to find the fuses...";
        yield return new WaitForSeconds(2);
        PlayerInterface.Instance.ErrorText.text = "";
    }

}
