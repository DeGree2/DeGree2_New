using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageScript : MonoBehaviour {
    public float fadeTime = 4f; //how long will it take to fade away

    //makes text slowly fade away
    public void FadeAway()
    {
        StartCoroutine(FadeText(fadeTime, GetComponent<Text>()));
    }

    private IEnumerator FadeText(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
