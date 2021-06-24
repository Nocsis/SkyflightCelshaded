using UnityEngine;

public class Painter : MonoBehaviour
{
    public Paintable paintable;

    [SerializeField] private Color drawingColor = Color.blue;

    private RaycastHit touch;
    private Quaternion lastAngle;
    private bool lastTouch;

    void Update()
    {
        float tipHeight = transform.Find("Tip").transform.localScale.y;
        Vector3 tip = transform.Find("Tip").transform.position;

        if (lastTouch)
        {
            tipHeight *= 1.1f;
        }

        // Check for a Raycast from the tip of the pen
        if (Physics.Raycast(tip, transform.forward, out touch, tipHeight)) //&& check for being held)
        {
            if (!touch.collider.TryGetComponent<Paintable>(out _))
                return;
            this.paintable = touch.collider.GetComponent<Paintable>();

            // Give haptic feedback when touching the whiteboard
            //controllerActions.TriggerHapticPulse(0.05f);

            // Set whiteboard parameters
            paintable.SetColor(drawingColor);
            paintable.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
            paintable.ToggleTouch(true);

            // If we started touching, get the current angle of the pen
            if (lastTouch == false)
            {
                lastTouch = true;
                lastAngle = transform.rotation;
            }
        }
        else
        {
            if(paintable != null)
                paintable.ToggleTouch(false);
            lastTouch = false;
        }

        // Lock the rotation of the pen if "touching"
        if (lastTouch)
        {
            transform.rotation = lastAngle;
        }
    }
}
