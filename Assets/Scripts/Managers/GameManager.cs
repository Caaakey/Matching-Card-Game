using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public CardModule[] cards = null;
    private bool isStart = false;

    private void Awake()
    {
        if (ControlManager.Get == null) return;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(
            new Rect(Screen.width - (24f + 96f), Screen.height - (24f + 68f), 96f, 68f),
            isStart ? "ReStart" : "Start"))
        {
            if (isStart)
            {
                UpdateManager.Clear();
                ControlManager.Get.Reset();

                for (int i = 0; i < cards.Length; ++i)
                    cards[i].ResetValue();
            }

            GameStart();
        }
    }
#endif

    private void GameStart()
    {
        List<Color> colors = new List<Color>(cards.Length / 2);
        RandomColor(colors, cards.Length / 2);

        colors.AddRange(colors);
        Shuffle(colors, colors.Count);

        for (int i = 0; i < cards.Length; ++i)
        {
            cards[i].CardColor = colors[i];
            cards[i].gameObject.SetActive(true);
        }

        isStart = true;
    }

    private void RandomColor(List<Color> list, int count)
    {
        if (count == 0) return;

        Color color = new Color
            (
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );

        if (list.Exists(x => x.Equals(color))) RandomColor(list, count);
        else
        {
            list.Add(color);
            RandomColor(list, count - 1);
        }
    }

    //  Fisher-Yates shuffle
    //  https://en.wikipedia.org/wiki/Fisher–Yates_shuffle
    private void Shuffle(List<Color> list, int count)
    {
        while (count-- > 1)
        {
            int d = Random.Range(0, count);

            Color color = list[d];
            list[d] = list[count];
            list[count] = color;
        }
    }

}
