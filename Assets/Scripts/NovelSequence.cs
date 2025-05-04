using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "NovelSequence", menuName = "Visual Novel/Sequence")]
public class NovelSequence : ScriptableObject
{
    [System.Serializable]
    public class SceneData
    {
        public Sprite background;
        [TextArea(3, 10)] public string[] pages;
    }

    public SceneData[] scenes;
    public TMP_FontAsset font;
    public Color textColor = Color.white;
}
