using Minigame.RGLight;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class YoungheeAnimationUI : MonoBehaviour
{
    private Image _image;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    public RGLightManager RGLightManager { get; private set; }

    public IEnumerator YoungheeAnimation(bool forward)
    {
        if (forward == false) UpsetYounghee(false);
        for (int i = 0; i < sprites.Count; i++)
        {
            if (forward) _image.sprite = sprites[i];
            else _image.sprite = sprites[sprites.Count - (i + 1)];
            yield return new WaitForSeconds(0.05f);
        }

        if (forward) UpsetYounghee(true);
    }

    private void UpsetYounghee(bool value)
    {
        _image.color = Color.white;

        if (value) _image.color = Color.red;
    }

    public void ShowImage(bool value)
    {
        _image.enabled = value;

        if (value)
        {
            _image.sprite = sprites[0];
        }
    }

    public void Init(RGLightManager manager)
    {
        RGLightManager = manager;
        _image = GetComponent<Image>();

        ShowImage(false);
    }
}
