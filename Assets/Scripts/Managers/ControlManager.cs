using System;
using System.Collections;
using UnityEngine;

public class ControlManager : MonoSingleton<ControlManager>
{
    private Camera mainCamera = null;
    private CardModule select = null;

    public void Reset()
    {
        select = null;
        UpdateManager.Add(UpdateControl());
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        UpdateManager.Add(UpdateControl());
    }

    private IEnumerator UpdateControl()
    {
        while (true)
        {
            yield return null;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                if (hit.collider == null) continue;
                else if (select != null && hit.collider.gameObject.Equals(select.gameObject))
                {
                    select.RotateAnimation(false);
                    select = null;
                }
                else if (hit.collider.gameObject.GetComponent<CardModule>() is CardModule newCard)
                {
                    newCard.RotateAnimation(true);

                    float time = Time.time;
                    float waitTime = select == null ?
                        CardModule.CARD_ROTATE_TIME * .5f :
                        CardModule.CARD_ROTATE_TIME;

                    while (Time.time - waitTime <= time)
                        yield return null;

                    if (select == null)
                    {
                        select = newCard;
                    }
                    else if (newCard.CardColor.Equals(select.CardColor))
                    {
                        //  OK!
                        select.ReleaseAnimation();
                        newCard.ReleaseAnimation();
                    }
                    else
                    {
                        select.RotateAnimation(false);
                        newCard.RotateAnimation(false);
                        select = null;
                    }
                }

            }
        }

    }

}
