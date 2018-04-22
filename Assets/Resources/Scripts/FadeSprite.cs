using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSprite : Singleton<FadeSprite> {

    public float secondsToFade;

    public void FadeMe()
    {
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade()
    {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        while (color.a < 1)
        {
            color.a += Time.deltaTime / secondsToFade;
            spriteRenderer.color = color;
            yield return null;
        }
        yield return null;
    }
}
