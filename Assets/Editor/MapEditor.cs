using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditor
{
#if UNITY_EDITOR
    // %(Ctrl), #(Shift), &(Alt)4
    [MenuItem("Tools/GenerateMap %#g")]
    private static void GenerateMap()
    {
        GameObject[] Maps = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach (GameObject map in Maps)
        {
            Tilemap tmbase = Util.FindChild<Tilemap>(map, "Tilemap_Base", true);
            Tilemap tm = Util.FindChild<Tilemap>(map, "Tilemap_Collision", true);            

            using (var writer = File.CreateText($"Assets/Resources/Map/{map.name}.txt"))
            {
                writer.WriteLine(tmbase.cellBounds.xMin);
                writer.WriteLine(tmbase.cellBounds.xMax);
                writer.WriteLine(tmbase.cellBounds.yMin);
                writer.WriteLine(tmbase.cellBounds.yMax);

                for (int y = tmbase.cellBounds.yMax-1; y >= tmbase.cellBounds.yMin; y--)
                {
                    for (int x = tmbase.cellBounds.xMin; x < tmbase.cellBounds.xMax; x++)
                    {
                        TileBase tile = tm.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null)
                        {
                            writer.Write("1");
                        }
                        else
                        {
                            writer.Write("0");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }
    }


#endif
 }
