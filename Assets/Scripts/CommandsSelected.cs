using System;
using UnityEngine;
using UnityEngine.UI;

public class CommandsSelected : MonoBehaviour
{
    public Button Btn;
    public Image Img;
    public Text Txt;

    private Guid guid;

    public void SetInfo(string text, Color color, Guid guid)
    {
        Txt.text = text.ToUpper();
        Img.color = color;
        this.guid = guid;
        Btn.onClick.AddListener(() => removeCommand());
    }

    private void removeCommand()
    {
        BoardManager.Instance.Commands.RemoveCommand(guid);
        Destroy(this.gameObject);
    }

}
