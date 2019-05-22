using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
// proxy pattern

public class AssetProxy
{
    public AssetProxy(System.Type type)
    {
        loader = new AssetLoader(type);
    }

    protected AssetLoader loader;

    public dynamic LoadAsset(string path)
    {
        if(File.Exists(Application.dataPath + "/Resources/" + path))
        {
            return loader.LoadAsset(Path.ChangeExtension(path,null));
        }

        throw new System.Exception(string.Format("No asset at path " + path));
    }
    
}

public class AssetLoader
{
    System.Type type;

    public AssetLoader(System.Type type)
    {
        this.type = type;
    }

    internal dynamic LoadAsset(string path)
    {
        Object obj = Resources.Load(path, type);
        return obj;
    }
}
