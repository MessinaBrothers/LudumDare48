using Godot;
using System;
using System.Collections.Generic;

public class Utility : Node {

    public static void Shuffle<T>(T[] array, Random rng) {
        int n = array.Length;
        while (n > 1) {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    public static void Shuffle<T>(List<T> array, Random rng) {
        int n = array.Count;
        while (n > 1) {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

}