using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private PaintMode paintMode;

    [SerializeField]
    private Transform painterTip;

    [SerializeField]
    private float raycastLength = 0.01f;

    [SerializeField]
    private float spacing = 1f;

    [SerializeField]
    private Color color;

    [SerializeField]
    private Texture2D brush;

    [SerializeField]
    private Paintable paintable;

    private Collider paintableCollider;

    private Stamp stamp;

    private Vector2? lastDrawPosition = null;

    private void Awake()
    {
        if (paintable != null)
            Initialize(paintable);
        else
            Debug.LogError("[Painter]" + gameObject.name + " needs to have an assigned Paintable");
    }

    public void Initialize(Paintable newPaintable)
    {
        stamp = new Stamp(brush);
        stamp.mode = paintMode;

        paintable = newPaintable;
        paintableCollider = newPaintable.GetComponent<Collider>();
    }

    private void Update()
    {
        Ray ray = new Ray(painterTip.position, painterTip.forward);
        RaycastHit hit;

        if (paintableCollider.Raycast(ray, out hit, raycastLength))
        {
            //Debug.Log("Hit!");
            if (lastDrawPosition.HasValue && lastDrawPosition.Value != hit.textureCoord)
            {
                Debug.Log("Drawline");
                paintable.DrawLine(stamp, lastDrawPosition.Value, hit.textureCoord, color, spacing);
            }
            else
            {
                //Debug.Log("Splash");
                paintable.CreateSplash(hit.textureCoord, stamp, color);
            }

            lastDrawPosition = hit.textureCoord;
        }
        else
        {
            lastDrawPosition = null;
        }
    }

    public void ChangeColor(Color newColor)
    {
        color = newColor;
    }

    public void ChangePaintMode(PaintMode newPaintMode)
    {
        paintMode = newPaintMode;
        stamp.mode = paintMode;
    }

    public void ChangeStamp(Texture2D newBrush)
    {
        stamp = new Stamp(newBrush);
        stamp.mode = paintMode;
    }
}
