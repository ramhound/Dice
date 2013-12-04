using UnityEngine;
using System.Collections;

public class SortedIntegers {
    private static int[] data = new int[10];

    public static void Add(int d) {
        for(int i = 0; i < data.Length; i++) {
            if(data[i] <= d) {
                int t1 = data[i];
                data[i] = d;
                for(int j = i; j < data.Length; j++) {
                    int t2;
                    if(i + 1 < data.Length) {
                        if(t1 >= data[++i]) {
                            t2 = data[i];
                            data[i] = t1;
                            t1 = t2;
                        }
                    }
                } break;
            } 
        }
    }

    public static int GetHighest() {
        return data[0];
    }

    public static int GetLowest() {
        return data[data.Length - 1];
    }

    public static int[] GetData() {
        return data;
    }
}
