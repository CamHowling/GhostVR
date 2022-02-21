using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketTagCheck : XRSocketInteractor
{

    public string targetTag = string.Empty;

    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && matchTag(interactable);
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && matchTag(interactable);
    }

    private bool matchTag(XRBaseInteractable interactable)
    {
        return interactable.CompareTag(targetTag);
    }
}
