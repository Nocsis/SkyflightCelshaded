using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glowButton : MonoBehaviour
{
    //TODO: Auslagern in PushButton class, glowButton erben lassen

    enum Directions
    {
        Xpos,
        Xneg,
        Ypos,
        Yneg,
        Zpos,
        Zneg
    };

    [SerializeField] private Directions forwardDirection = Directions.Xpos;
    [SerializeField] private float pushDepth = 0.01f;

    [SerializeField] private bool pressed = false;

    private Vector3 originalPos;
    private Vector3 pushedPos;
    private Material buttonMaterial;

    private void Awake()
    {
        originalPos = transform.position;
        buttonMaterial = GetComponent<Renderer>().material;

        if (pressed)
            buttonMaterial.EnableKeyword("_EMISSION");
        else
            buttonMaterial.DisableKeyword("_EMISSION");
        DynamicGI.UpdateEnvironment();

        if (forwardDirection == Directions.Xpos)
            pushedPos = originalPos - new Vector3(pushDepth, 0, 0);
        else if(forwardDirection == Directions.Xneg)
            pushedPos = originalPos + new Vector3(pushDepth, 0, 0);
        else if (forwardDirection == Directions.Ypos)
            pushedPos = originalPos - new Vector3(0, pushDepth, 0);
        else if (forwardDirection == Directions.Yneg)
            pushedPos = originalPos + new Vector3(0, pushDepth, 0);
        else if (forwardDirection == Directions.Zpos)
            pushedPos = originalPos - new Vector3(0, 0, pushDepth);
        else if (forwardDirection == Directions.Zneg)
            pushedPos = originalPos + new Vector3(0, 0, pushDepth);
    }

    public void OnClick()
    {
        StartCoroutine(Push());
    }

    IEnumerator Push()
    {
        while(Vector3.Distance(transform.position, pushedPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pushedPos, pushDepth * 10 * Time.deltaTime);
            yield return null;
        }

        pressed = !pressed;

        if (pressed)
            buttonMaterial.EnableKeyword("_EMISSION");
        else
            buttonMaterial.DisableKeyword("_EMISSION");

        DynamicGI.UpdateEnvironment();

        while (Vector3.Distance(transform.position, originalPos) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPos, pushDepth * 10 * Time.deltaTime);
            yield return null;
        }
    }
}
