using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public class SortingList<T>
{
    public SortingList(List<ItemSortData<T>> sortDatas)
    {
        this.sorters = sortDatas;
    }

    public class ItemSortData<T>
    {
        public string name;
        public Comparison<T> sort;
    }

    private List<ItemSortData<T>> sorters;

    private ItemSortData<T> currentSorter { get { return sorters[currentSortIndex]; } }
    private int currentSortIndex = 0;

    public string CurrentSortName { get => currentSorter.name; }
    public Comparison<T> CurrentSort { get => currentSorter.sort; }

    public void NextSorter()
    {
        currentSortIndex++;
        if (currentSortIndex >= sorters.Count)
            currentSortIndex = 0;
    }

    public void PrevSorter()
    {
        currentSortIndex--;
        if(currentSortIndex < 0)
            currentSortIndex = sorters.Count - 1;
    }
}
