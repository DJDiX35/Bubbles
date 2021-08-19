using UnityEngine;

// Универсальный мультипроектный класс
[System.Serializable]
public class MinMaxTypes
{
    [System.Serializable]
    public struct Double
    {
        public double min;
        public double max;
    }
    [System.Serializable]
    public struct Float
    {
        public float min;
        public float max;
    }
    [System.Serializable]
    public struct Int
    {
        public int min;
        public int max;
    }
    [System.Serializable]
    public struct Long
    {
        public long min;
        public long max;
    }
}
