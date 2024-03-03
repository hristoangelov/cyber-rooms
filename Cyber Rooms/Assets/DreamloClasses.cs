using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamloClasses : MonoBehaviour
{
    public Dreamlo dreamlo { get; set; }
}

public class Dreamlo
{
    public Leaderboard leaderboard { get; set; }
}

public class Entry
{
    public string name { get; set; }
    public string score { get; set; }
    public string seconds { get; set; }
    public string text { get; set; }
    public string date { get; set; }
}

public class Leaderboard
{
    public List<Entry> entry { get; set; }
}

public class Root
{
    public Dreamlo dreamlo { get; set; }
}
