using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseIcon : MonoBehaviour
{
    public Sprite point, click;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
    }

    public void ToPoint()
    {
        image.sprite = point;
    }

    public void ToClick()
    {
        image.sprite = click;
    }

}
