using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CardModule : MonoBehaviour
{
    public const float CARD_ROTATE_TIME = .25f;
    public const float CARD_OPACITY_TIME = .25f;

    [SerializeField] private SpriteRenderer frontCard = null;
    [SerializeField] private SpriteRenderer backRenderer = null;
    [SerializeField] private AudioSource audioSource = null;

    private bool isActive = true;

    public void ResetValue()
    {
        isActive = true;
        gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;
        frontCard.color = Color.white;
    }

    public Color CardColor
    {
        get => backRenderer.color;
        set => backRenderer.color = value;
    }

    public void RotateAnimation(bool isUp)
    {
        if (!isActive) return;
        else if (!gameObject.activeSelf) return;

        UpdateManager.Add(UpdateRotate(isUp, CARD_ROTATE_TIME));
        audioSource.Play();
    }

    public void ReleaseAnimation()
    {
        if (!isActive) return;

        UpdateManager.Add(UpdateOpacity(CARD_OPACITY_TIME));
    }

    private IEnumerator UpdateRotate(bool isUp, float time)
    {
        float currTime = Time.time;
        Vector3 vecAngle = isUp ? Vector3.up : Vector3.zero;

        Quaternion prev = isUp ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);
        Quaternion next = isUp ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;

        while (Time.time - currTime <= time)
        {
            transform.rotation = Quaternion.Lerp(
                prev,
                next,
                (Time.time - currTime) / time);

            yield return null;
        }

        transform.rotation = next;
        yield break;
    }

    private IEnumerator UpdateOpacity(float time)
    {
        float currTime = Time.time;
        Color color = CardColor;

        while (Time.time - currTime <= time)
        {
            color.a = Mathf.Lerp(
                1f, 0f,
                (Time.time - currTime) / time);

            CardColor = color;
            frontCard.color = new Color(1f, 1f, 1f, color.a);

            yield return null;
        }

        color.a = 0f;
        CardColor = color;
        frontCard.color = new Color(1f, 1f, 1f, color.a);

        gameObject.SetActive(false);
        yield break;
    }

}