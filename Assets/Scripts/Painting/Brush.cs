using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brush : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private Transform tipForHeight;

    [SerializeField] private int brushSize = 5;

    private Renderer renderer;

    private Color[] colors;

    private float tipHeight;

    private PaintingCanvas paintCanvas;

    private bool touchedLastFrame;
    private RaycastHit touch;

    private Vector2 lastTouchPos;
    private Quaternion lastTouchRot;
    // Start is called before the first frame update
    void Start()
    {
        renderer = tip.GetComponentInChildren<Renderer>();
        colors = Enumerable.Repeat(renderer.material.color, brushSize * brushSize).ToArray();
        //probably wrong
        tipHeight = tipForHeight.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    //Maybe make a grab interactable and put this in process interactable
    private void Draw()
    {
        
        if (Physics.Raycast(tipForHeight.position, transform.up, out touch, tipHeight))
        {
            if (touch.transform.CompareTag("PaintCanvas"))
            {
                if (paintCanvas == null)
                {
                    paintCanvas = touch.transform.GetComponent<PaintingCanvas>();
                }

                Vector2 touchPos = new Vector2(touch.textureCoord.x, touch.textureCoord.y);

                int x = (int) (touchPos.x * paintCanvas.textureSize.x - (brushSize / 2));
                int y = (int) (touchPos.y * paintCanvas.textureSize.x - (brushSize / 2));

                if (y < 0 || y > paintCanvas.textureSize.y || x < 0 || x > paintCanvas.textureSize.x)
                    return;
                
                if (touchedLastFrame)
                {
                    paintCanvas.texture.SetPixels(x,y,brushSize,brushSize,colors);

                    for (float i = 0.01f; i < 1.0f; i += 0.03f)
                    {
                        var lerpX = (int) Mathf.Lerp(lastTouchPos.x, x, i);
                        var lerpY = (int) Mathf.Lerp(lastTouchPos.y, y, i);
                        paintCanvas.texture.SetPixels(lerpX,lerpY,brushSize,brushSize,colors);
                    }

                    transform.rotation = lastTouchRot;
                    
                    paintCanvas.texture.Apply();
                }

                lastTouchPos = new Vector2(x, y);
                lastTouchRot = transform.rotation;
                touchedLastFrame = true;
                return;
            }
        }

        paintCanvas = null;
        touchedLastFrame = false;
    }
}
