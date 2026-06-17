using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDamageFeedback : MonoBehaviour
{
    public Image damageImage;

    public void ShowDamage()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        Color c = damageImage.color;

        c.a = 0.5f;
        damageImage.color = c;

        yield return new WaitForSeconds(0.1f);

        c.a = 0f;
        damageImage.color = c;
    }
}