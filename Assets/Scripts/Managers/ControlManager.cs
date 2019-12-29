using System;
using System.Collections;
using UnityEngine;

public class ControlManager : MonoSingleton<ControlManager>
{
    private Camera mainCamera = null;
    private CardModule Select { get; set; } = null;

    public void Reset()
    {
        Select = null;
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
                else if (Select != null && hit.collider.gameObject.Equals(Select.gameObject))
                {
                    Select.RotateAnimation(false);
                    Select = null;
                }
                else if (hit.collider.gameObject.GetComponent<CardModule>() is CardModule newCard)
                {
                    newCard.RotateAnimation(true);

                    float time = Time.time;
                    float waitTime = Select == null ?
                        CardModule.CARD_ROTATE_TIME * .5f :
                        CardModule.CARD_ROTATE_TIME;

                    while (Time.time - waitTime <= time)
                        yield return null;

                        //  new Card
                    if (Select == null) Select = newCard;
                    else if (newCard.CardColor.Equals(Select.CardColor))
                    {
                        //  Matching!
                        Select.ReleaseAnimation();
                        newCard.ReleaseAnimation();
                        Select = null;
                    }
                    else
                    {
                        //  Miss!
                        Select.RotateAnimation(false);
                        newCard.RotateAnimation(false);
                        Select = null;
                    }
                }

            }
        }

    }

}
