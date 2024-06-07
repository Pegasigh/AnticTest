using System;
using System.Collections.Generic;

public class Grid<TData>
{
    private Dictionary<(int, int), TData> dictionary;




    public Grid()
    {
        dictionary = new Dictionary<(int, int), TData> ();
    }

    public void AddData (int i, int j, TData data)
    {
        var key = (i, j);
        if (dictionary.ContainsKey(key))
        {
            throw new ArgumentException($"Data already found at position ({i},{j})");
        }
        dictionary[key] = data;
    }

    public TData GetData (int i, int j)
    {
        var key = (i, j);
        if (dictionary.TryGetValue(key, out var data)) return data;

        throw new KeyNotFoundException($"No data found at position ({i},{j})");
    }

    public void DeleteData (int i, int j)
    {
        var key = (i, j);
        if (dictionary.Remove(key))
        {
            //successfully deleted
        }
        else
        {
            throw new KeyNotFoundException($"No data found at position ({i},{j}) to delete");
        }
    }

    public void DeleteData (TData data)
    {
        foreach (var pair in dictionary)
        {
            if (EqualityComparer<TData>.Default.Equals(pair.Value, data))
            {
                //pair found
                dictionary.Remove(pair.Key);
                return;
            }
        }

        //pair not found
        throw new KeyNotFoundException("No data found to delete");
    }
}
