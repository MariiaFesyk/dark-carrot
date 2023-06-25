using System.Collections.Generic;
using UnityEngine;

public static class ListExtension {
    public static void SwapRemoveAt<T>(this List<T> list, int index){
        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }
    public static void Shuffle<T>(this T[] array){
        for(int i = array.Length - 1; i > 0; i--){
            int j = Random.Range(0, i);
            T temp = array[j];
            array[j] = array[i - 1];
            array[i - 1] = temp;
        }
    }
}