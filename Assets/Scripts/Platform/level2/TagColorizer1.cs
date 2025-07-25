using UnityEngine;
using System.Collections;

public class Tagcolorizer1 : MonoBehaviour
{
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public float duration = 3f;

    private Coroutine effectCoroutine;

 
    public void ColorPanels()
    {
        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);
        effectCoroutine = StartCoroutine(HighlightRoutine());
    }

    private IEnumerator HighlightRoutine()
    {
        
        var originals = new System.Collections.Generic.List<(Renderer, Color[])>();
        foreach (Transform child in transform)
        {
            var renderer = child.GetComponent<Renderer>();
            if (!renderer) continue;

            
            Color[] raw = new Color[renderer.materials.Length];
            for (int i = 0; i < raw.Length; i++)
                raw[i] = renderer.materials[i].color;

            originals.Add((renderer, raw));

            // 태그별로 지정색 입힘
            if (child.CompareTag("Correct"))
            {
                foreach (var mat in renderer.materials)
                    mat.color = correctColor;
            }
            else if (child.CompareTag("Wrong"))
            {
                foreach (var mat in renderer.materials)
                    mat.color = wrongColor;
            }
        }

        yield return new WaitForSeconds(duration);

        // 원래 색상 복구
        foreach (var pair in originals)
        {
            var renderer = pair.Item1;
            var prev = pair.Item2;
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].color = prev[i];
            }
        }

        effectCoroutine = null;
    }
}
