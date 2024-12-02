using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(FadeOutFlow());
    }

    private IEnumerator FadeOutFlow()
    {
        var time = 0f;
        while (time < 1.5f)
        {
            time += Time.deltaTime;
            image.color = new Color(0, 0, 0, Mathf.Lerp(1f, 0f, time / 1.5f));
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
