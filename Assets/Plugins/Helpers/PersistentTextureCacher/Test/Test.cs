using System.Collections;
using System.IO;
using Assets.Plugins.Helpers;
using UnityEngine;

public class Test : MonoBehaviour
{
    public IEnumerator LoadImage()
    {
        var str = Path.Combine(Application.streamingAssetsPath, "penguin.png");
        var www = new WWW(@"file://" + str);

        yield return www;

        if (www.error == null)
        {
            var texture = www.texture;

            PersistentTextureCacher.SaveElement("Cached_tex", texture, "Penguins");

            Debug.Log("Loading complete successfull");
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Load image from Streaming Assets folder"))
        {
            StartCoroutine(LoadImage());
        }

        if (GUILayout.Button("Retrieve from cache"))
        {
            var texture = PersistentTextureCacher.GetTexture("Cached_tex", "Penguins");

            // You must clear data or it will be memory leak
            DestroyImmediate(GetComponent<SpriteRenderer>().sprite);
            GetComponent<SpriteRenderer>().sprite =
                Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    Vector2.zero);
        }

        if (GUILayout.Button("Wipe cache"))
        {
            PersistentTextureCacher.WipeCache();
            Destroy(GetComponent<SpriteRenderer>().sprite);
        }
    }
}