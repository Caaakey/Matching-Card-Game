using System;
using System.Collections;
using System.Collections.Generic;

public class UpdateManager : MonoSingleton<UpdateManager>
{
    public readonly struct UpdateValue
    {
        public UpdateValue(IEnumerator routine, Action callback)
        {
            this.routine = routine;
            this.callback = callback;
        }

        public readonly IEnumerator routine;
        public readonly Action callback;
    }

    private readonly List<UpdateValue> updateValues = new List<UpdateValue>();

    public static void Add(IEnumerator routine, Action callback = null)
        => Get.updateValues.Add(new UpdateValue(routine, callback));

    public static void Remove(IEnumerator routine)
        => Get.updateValues.Remove(Get.updateValues.Find(x => x.routine.Equals(routine)));

    public static void Clear()
        => Get.updateValues.Clear();

    private void Update()
    {
        for (int i = 0; i < updateValues.Count; ++i)
        {
            if (!updateValues[i].routine.MoveNext())
            {
                updateValues[i].callback?.Invoke();
                updateValues.RemoveAt(i--);
            }
        }
    }

}
