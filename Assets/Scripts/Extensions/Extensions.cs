
using System;
using System.Collections.Generic;

public static class Extensions
{
    public static void Shuffle<T>(this List<T> list) 
    {
        Random random = new Random();
        int count = list.Count;
        
        for(int i = 0; i < count; i++)
        {
            int randomIndex = random.Next(0, count);
            T current = list[randomIndex];
            list[randomIndex] = list[i];
            list[i] = current;
        }
    }

}

