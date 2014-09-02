using System;
using System.Collections;
using System.IO;
using Assets.Plugins.Helpers;
using UnityEngine;

public class Test : MonoBehaviour
{
    public IEnumerator Start()
    {
        var str = Path.Combine(Application.streamingAssetsPath, "penguin.png");
        var www = new WWW(@"file://" + str);

        yield return www;

        if (www.error == null)
        {
            var texture = www.texture;

            PersistentTextureCacher.SaveElement("Cached_tex", texture);
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Retrieve from cache"))
        {
            var texture = PersistentTextureCacher.GetTexture("Cached_tex");
            GetComponent<SpriteRenderer>().sprite =
                Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    Vector2.zero);
        }
    }
}