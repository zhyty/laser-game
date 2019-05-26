﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    // FIXME dont default this
    private static string levelName = "dev";

    public static void OpenLevel(string name) {
        levelName = name;
        SceneManager.LoadScene("PuzzleBase");
    }

    public TileFactory factory;
    public Transform camera;

    float simTime_;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");
        LoadMap();
        simTime_ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        simTime_ += Time.deltaTime;
    }

    public float SimTime() {
        return simTime_;
    }

    private void LoadMap()
    {
        var map = MapParser.GetMap(@"Assets\Maps\" + levelName);

        CentreCamera(map.rows, map.cols);

        foreach (var entry in map.tiles) {
            TileHandler tile = ConstructTile(entry.Value);
            float orientation = GetOrientation(entry.Value);
            tile.Init(this, entry.Key, orientation);
        }
    }

    private void CentreCamera(int rows, int cols)
    {
        camera.position = new Vector3(cols / 2.0f, rows / 2.0f, -10);
    }

    private TileHandler ConstructTile(MapParser.TileType tile)
    {
        switch (tile)
        {
            case MapParser.TileType.MirrorForward:
            case MapParser.TileType.MirrorBackward:
                return factory.CreateFlatMirror();
            default:
                throw new Exception("Attempted to create unsupported tile type " + Enum.GetName(typeof(MapParser.TileType), tile));
        }
    }
    
    private float GetOrientation(MapParser.TileType tile)
    {
        switch (tile)
        {
            case MapParser.TileType.MirrorForward:
                return -45;
            case MapParser.TileType.MirrorBackward:
                return 45;
            default:
                return 0;
        }
    }
}
