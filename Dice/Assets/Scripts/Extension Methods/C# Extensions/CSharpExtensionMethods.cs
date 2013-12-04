using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This extension will take a multidimensional array of type T and flatten it into a single array
/// </summary>
public static class CSharpExtensionMethods {
    public static T[] Flatten<T>(this T[,] array) {
        if(array == null) return null;
        T[] flat = new T[array.GetLength(0) * array.GetLength(1)];
        int index = 0;
        for(int i = 0; i < array.GetLength(0); i++) {
            for(int j = 0; j < array.GetLength(1); j++) {
                flat[index++] = array[i, j];
            }
        } return flat;
    }

    /// <summary>
    /// Converts a single dimensional array into a List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static List<T> ToList<T>(this T[] array){
        return new List<T>(array);
    }
}
