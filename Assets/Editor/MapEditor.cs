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
        GenerateByPath("Assets/Resources/Map");
        GenerateByPath("Common/MapData");
    }

    private static void GenerateByPath(string pathPrefix)
    {
        GameObject[] Maps = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach (GameObject map in Maps)
        {
            //Tilemap tmbase = Util.FindChild<Tilemap>(map, "Tilemap_Base", true);
            Tilemap tm = Util.FindChild<Tilemap>(map, "Tilemap_Collision", true);
            Tilemap tmblank = Util.FindChild<Tilemap>(map, "Tilemap_Blank", true);

            using (var writer = File.CreateText($"{pathPrefix}/{map.name}.txt"))
            {
                writer.WriteLine(tm.cellBounds.xMin);
                writer.WriteLine(tm.cellBounds.xMax);
                writer.WriteLine(tm.cellBounds.yMin);
                writer.WriteLine(tm.cellBounds.yMax);

                for (int y = tm.cellBounds.yMax - 1; y >= tm.cellBounds.yMin; y--)
                {
                    for (int x = tm.cellBounds.xMin; x < tm.cellBounds.xMax; x++)
                    {
                        TileBase tile = tm.GetTile(new Vector3Int(x, y, 0));
                        TileBase tileblank = tmblank.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null || tileblank != null)
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
