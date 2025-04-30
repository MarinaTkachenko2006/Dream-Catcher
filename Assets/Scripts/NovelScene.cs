using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "NewNovelScene", menuName = "Visual Novel/Scene")]
public class NovelScene : ScriptableObject
{
    [TextArea(3, 10)] public string[] textPages;
    public Sprite background;
    public TMP_FontAsset font;
    public Color textColor = Color.white;
}
