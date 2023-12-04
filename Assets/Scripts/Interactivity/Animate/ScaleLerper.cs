using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLerper : MonoBehaviour
{
    private Vector3 minScale;
    public Vector3 maxScale;
    public bool repeatable = false;
    public float speed = 1;
    public float duration = 5f;
    public float delay = 1f;

    IEnumerator Start()
    {
        minScale = transform.localScale;
        //float maxScaler = minScale.x * 1.2f;
        //maxScale = new Vector3(maxScaler, maxScaler, maxScaler);

        yield return new WaitForSeconds(delay);

        while (repeatable)
        {
            yield return RepeatLerp(minScale, maxScale, duration);
            yield return RepeatLerp(maxScale, minScale, duration);
        }
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }
}
