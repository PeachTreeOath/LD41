using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : Singleton<ResourceLoader>
{
    [HideInInspector]
    public GameObject laneHighlight;
    [HideInInspector]
    public Sprite healIcon;
    [HideInInspector]
    public Sprite drawIcon;
    [HideInInspector]
    public Sprite attackIcon;
    [HideInInspector]
    public Sprite timeIcon;

    protected override void Awake()
    {
        base.Awake();
        LoadResources();
    }

    private void LoadResources()
    {
        laneHighlight = Resources.Load<GameObject>("Prefabs/LaneHighlight");
        healIcon = Resources.Load<Sprite>("Textures/icons_v2/white_health");
        drawIcon = Resources.Load<Sprite>("Textures/icons_v2/white_draw");
        attackIcon = Resources.Load<Sprite>("Textures/icons_v2/white_sword");
        timeIcon = Resources.Load<Sprite>("Textures/icons_v2/white_time");
    }

}
