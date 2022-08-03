using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPins : MonoBehaviour
{
    public SpriteRenderer Render;
    public Color StartColor;
    [SerializeField]
    public int Status = 0; // 0 - Пусто | 1 - Фишка игрока 1 | 2 - Фишка игрока 2
    [SerializeField]
    public GameObject Forward;
    [SerializeField]
    public GameObject Back;
    [SerializeField]
    public GameObject Left;
    [SerializeField]
    public GameObject Right;
    [SerializeField]
    public GameObject UpRightDiagonal;
    [SerializeField]
    public GameObject UpLeftDiagonal;
    [SerializeField]
    public GameObject DownRightDiagonal;
    [SerializeField]
    public GameObject DownLeftDiagonal;

    public int EventPinsID = -1;

    bool SelectedPins = false;
    bool PaintPinst = false;
    bool IsEat = false;
    // Start is called before the first frame update
    void Start()
    {
        Render = GetComponent<SpriteRenderer>();
        StartColor = Render.color;
    }
    void OnMouseDown()
    {
        if (GetGameStatus() == 2) return;
        if (Status == 1)
        {
            if (SelectedPins == true) return;
            GameObject[] AllPin = GameObject.FindGameObjectsWithTag("Pin");
            for (int i = 0; i < AllPin.Length; i++)
            {
                EventPins ScriptPin;
                ScriptPin = AllPin[i].GetComponent<EventPins>();
                if (ScriptPin.SelectedPins == true)
                {
                    ScriptPin.Render.color = ScriptPin.StartColor;
                    ScriptPin.SelectedPins = false;
                }
            }
            SelectedPins = true;
            ClearAccesPin();
            ShowAccessPin();
        }
        else if (Status == 0 && PaintPinst == true)
        {
            MovePin();
        }
    }

    void MovePin()
    {
        GameObject[] AllPin = GameObject.FindGameObjectsWithTag("Pin");
        for (int i = 0; i < AllPin.Length; i++)
        {
            EventPins ScriptPin;
            ScriptPin = AllPin[i].GetComponent<EventPins>();
            if (ScriptPin.SelectedPins == true)
            {
                /*GameObject Clone = Instantiate(AllPin[i]);
                Clone.transform.position = AllPin[i].transform.position;
                Clone.transform.parent = AllPin[i].transform.parent;
                Clone.transform.localScale = AllPin[i].transform.localScale;

                Clone.transform.position = Vector3.Lerp(Clone.transform.position, transform.position, 0.5f);*/


                ScriptPin.Render.color = Color.black;
                ScriptPin.StartColor = Color.black;
                ScriptPin.Status = 0;
                ScriptPin.SelectedPins = false;


                KillPins(ScriptPin, this);

                ClearAccesPin();
                Render.color = new Color32(52, 138, 48, 255);
                StartColor = new Color32(52, 138, 48, 255);
                Status = 1;
                break;
            }
        }

        GameObject Parent = GameObject.Find("PlayGround");
        Main ScriptMain = Parent.GetComponent<Main>();
        ScriptMain.PlayNumber = 2;
        ScriptMain.StartAI = Random.Range(0, 3) + 1;
    }

    void KillPins(EventPins StartPins, EventPins EndPins)
    {
        switch(FindDirection(StartPins, EndPins))
        {
            case 1:
            {
                EventPins CheckPins = StartPins;
                while (CheckPins.Forward != null)
                {
                    if (CheckPins.Forward.GetComponent<EventPins>().IsEat == true)
                    {
                        CheckPins.Forward.GetComponent<EventPins>().Status = 0;

                        CheckPins.Forward.GetComponent<EventPins>().StartColor = Color.black;

                        CheckPins.Forward.GetComponent<EventPins>().Render.color = CheckPins.Forward.GetComponent<EventPins>().StartColor;

                        CheckPins.Forward.GetComponent<EventPins>().IsEat = false;

                        GameObject Parent = GameObject.Find("PlayGround");
                        Main ScriptMain = Parent.GetComponent<Main>();
                        ScriptMain.PlayerEat++;
                    }
                    if (CheckPins.Forward.GetComponent<EventPins>() == EndPins) break;
                    CheckPins = CheckPins.Forward.GetComponent<EventPins>();
                }
                break;
            }
            case 2:
            {
                EventPins CheckPins = StartPins;
                while (CheckPins.Back != null)
                {
                    if (CheckPins.Back.GetComponent<EventPins>().IsEat == true)
                    {
                        CheckPins.Back.GetComponent<EventPins>().Status = 0;

                        CheckPins.Back.GetComponent<EventPins>().StartColor = Color.black;

                        CheckPins.Back.GetComponent<EventPins>().Render.color = CheckPins.Back.GetComponent<EventPins>().StartColor;

                        CheckPins.Back.GetComponent<EventPins>().IsEat = false;

                        GameObject Parent = GameObject.Find("PlayGround");
                        Main ScriptMain = Parent.GetComponent<Main>();
                        ScriptMain.PlayerEat++;
                    }
                    if (CheckPins.Back.GetComponent<EventPins>() == EndPins) break;
                    CheckPins = CheckPins.Back.GetComponent<EventPins>();
                }
                break;
            }
            case 3:
            {
                EventPins CheckPins = StartPins;
                while (CheckPins.Left != null)
                {
                    if (CheckPins.Left.GetComponent<EventPins>().IsEat == true)
                    {
                        CheckPins.Left.GetComponent<EventPins>().Status = 0;

                        CheckPins.Left.GetComponent<EventPins>().StartColor = Color.black;

                        CheckPins.Left.GetComponent<EventPins>().Render.color = CheckPins.Left.GetComponent<EventPins>().StartColor;

                        CheckPins.Left.GetComponent<EventPins>().IsEat = false;

                        GameObject Parent = GameObject.Find("PlayGround");
                        Main ScriptMain = Parent.GetComponent<Main>();
                        ScriptMain.PlayerEat++;
                    }
                    if (CheckPins.Left.GetComponent<EventPins>() == EndPins) break;
                    CheckPins = CheckPins.Left.GetComponent<EventPins>();
                }
                break;
            }
            case 4:
            {
                EventPins CheckPins = StartPins;
                while (CheckPins.Right != null)
                {
                    if (CheckPins.Right.GetComponent<EventPins>().IsEat == true)
                    {
                        CheckPins.Right.GetComponent<EventPins>().Status = 0;

                        CheckPins.Right.GetComponent<EventPins>().StartColor = Color.black;

                        CheckPins.Right.GetComponent<EventPins>().Render.color = CheckPins.Right.GetComponent<EventPins>().StartColor;

                        CheckPins.Right.GetComponent<EventPins>().IsEat = false;

                        GameObject Parent = GameObject.Find("PlayGround");
                        Main ScriptMain = Parent.GetComponent<Main>();
                        ScriptMain.PlayerEat++;
                    }
                    if (CheckPins.Right.GetComponent<EventPins>() == EndPins) break;
                    CheckPins = CheckPins.Right.GetComponent<EventPins>();
                }
                break;
            }
            case 5:
            {
                EventPins CheckPins = StartPins;
                while (CheckPins.UpLeftDiagonal != null)
                {
                    if (CheckPins.UpLeftDiagonal.GetComponent<EventPins>().IsEat == true)
                    {
                        CheckPins.UpLeftDiagonal.GetComponent<EventPins>().Status = 0;

                        CheckPins.UpLeftDiagonal.GetComponent<EventPins>().StartColor = Color.black;

                        CheckPins.UpLeftDiagonal.GetComponent<EventPins>().Render.color = CheckPins.UpLeftDiagonal.GetComponent<EventPins>().StartColor;

                        CheckPins.UpLeftDiagonal.GetComponent<EventPins>().IsEat = false;

                        GameObject Parent = GameObject.Find("PlayGround");
                        Main ScriptMain = Parent.GetComponent<Main>();
                        ScriptMain.PlayerEat++;
                    }
                    if (CheckPins.UpLeftDiagonal.GetComponent<EventPins>() == EndPins) break;
                    CheckPins = CheckPins.UpLeftDiagonal.GetComponent<EventPins>();
                }
                break;
            }
            case 6:
            {
                EventPins CheckPins = StartPins;
                while (CheckPins.UpRightDiagonal != null)
                {
                    if (CheckPins.UpRightDiagonal.GetComponent<EventPins>().IsEat == true)
                    {
                        CheckPins.UpRightDiagonal.GetComponent<EventPins>().Status = 0;

                        CheckPins.UpRightDiagonal.GetComponent<EventPins>().StartColor = Color.black;

                        CheckPins.UpRightDiagonal.GetComponent<EventPins>().Render.color = CheckPins.UpRightDiagonal.GetComponent<EventPins>().StartColor;

                        CheckPins.UpRightDiagonal.GetComponent<EventPins>().IsEat = false;

                        GameObject Parent = GameObject.Find("PlayGround");
                        Main ScriptMain = Parent.GetComponent<Main>();
                        ScriptMain.PlayerEat++;
                    }
                    if (CheckPins.UpRightDiagonal.GetComponent<EventPins>() == EndPins) break;
                    CheckPins = CheckPins.UpRightDiagonal.GetComponent<EventPins>();
                }
                break;
            }
            case 7:
            {
                EventPins CheckPins = StartPins;
                while (CheckPins.DownLeftDiagonal != null)
                {
                    if (CheckPins.DownLeftDiagonal.GetComponent<EventPins>().IsEat == true)
                    {
                        CheckPins.DownLeftDiagonal.GetComponent<EventPins>().Status = 0;

                        CheckPins.DownLeftDiagonal.GetComponent<EventPins>().StartColor = Color.black;

                        CheckPins.DownLeftDiagonal.GetComponent<EventPins>().Render.color = CheckPins.DownLeftDiagonal.GetComponent<EventPins>().StartColor;

                        CheckPins.DownLeftDiagonal.GetComponent<EventPins>().IsEat = false;

                        GameObject Parent = GameObject.Find("PlayGround");
                        Main ScriptMain = Parent.GetComponent<Main>();
                        ScriptMain.PlayerEat++;
                    }
                    if (CheckPins.DownLeftDiagonal.GetComponent<EventPins>() == EndPins) break;
                    CheckPins = CheckPins.DownLeftDiagonal.GetComponent<EventPins>();
                }
                break;
            }
            case 8:
            {
                EventPins CheckPins = StartPins;
                while (CheckPins.DownRightDiagonal != null)
                {
                    if (CheckPins.DownRightDiagonal.GetComponent<EventPins>().IsEat == true)
                    {
                        CheckPins.DownRightDiagonal.GetComponent<EventPins>().Status = 0;

                        CheckPins.DownRightDiagonal.GetComponent<EventPins>().StartColor = Color.black;

                        CheckPins.DownRightDiagonal.GetComponent<EventPins>().Render.color = CheckPins.DownRightDiagonal.GetComponent<EventPins>().StartColor;

                        CheckPins.DownRightDiagonal.GetComponent<EventPins>().IsEat = false;

                        GameObject Parent = GameObject.Find("PlayGround");
                        Main ScriptMain = Parent.GetComponent<Main>();
                        ScriptMain.PlayerEat++;
                    }
                    if (CheckPins.DownRightDiagonal.GetComponent<EventPins>() == EndPins) break;
                    CheckPins = CheckPins.DownRightDiagonal.GetComponent<EventPins>();
                }
                break;
            }
        }
    }

    int FindDirection(EventPins StartPins, EventPins EndPins)
    {
        EventPins CheckPins = StartPins;
        while (CheckPins.Forward != null)
        {
            if (CheckPins.Forward.GetComponent<EventPins>() == EndPins) return 1;
            else CheckPins = CheckPins.Forward.GetComponent<EventPins>();
        }

        CheckPins = StartPins;
        while (CheckPins.Back != null)
        {
            if (CheckPins.Back.GetComponent<EventPins>() == EndPins) return 2;
            else CheckPins = CheckPins.Back.GetComponent<EventPins>();
        }

        CheckPins = StartPins;
        while (CheckPins.Left != null)
        {
            if (CheckPins.Left.GetComponent<EventPins>() == EndPins) return 3;
            else CheckPins = CheckPins.Left.GetComponent<EventPins>();
        }

        CheckPins = StartPins;
        while (CheckPins.Right != null)
        {
            if (CheckPins.Right.GetComponent<EventPins>() == EndPins) return 4;
            else CheckPins = CheckPins.Right.GetComponent<EventPins>();
        }

        CheckPins = StartPins;
        while (CheckPins.UpLeftDiagonal != null)
        {
            if (CheckPins.UpLeftDiagonal.GetComponent<EventPins>() == EndPins) return 5;
            else CheckPins = CheckPins.UpLeftDiagonal.GetComponent<EventPins>();
        }

        CheckPins = StartPins;
        while (CheckPins.UpRightDiagonal != null)
        {
            if (CheckPins.UpRightDiagonal.GetComponent<EventPins>() == EndPins) return 6;
            else CheckPins = CheckPins.UpRightDiagonal.GetComponent<EventPins>();
        }

        CheckPins = StartPins;
        while (CheckPins.DownLeftDiagonal != null)
        {
            if (CheckPins.DownLeftDiagonal.GetComponent<EventPins>() == EndPins) return 7;
            else CheckPins = CheckPins.DownLeftDiagonal.GetComponent<EventPins>();
        }

        CheckPins = StartPins;
        while (CheckPins.DownRightDiagonal != null)
        {
            if (CheckPins.DownRightDiagonal.GetComponent<EventPins>() == EndPins) return 8;
            else CheckPins = CheckPins.DownRightDiagonal.GetComponent<EventPins>();
        }
        return 0;
    }

    int GetGameStatus()
    {
        GameObject Parent = GameObject.Find("PlayGround");
        Main ScriptMain = Parent.GetComponent<Main>();
        return ScriptMain.PlayNumber;
    }

    void ClearAccesPin()
    {
        GameObject[] AllPin = GameObject.FindGameObjectsWithTag("Pin");
        for (int i = 0; i < AllPin.Length; i++)
        {
            EventPins ScriptPin;
            ScriptPin = AllPin[i].GetComponent<EventPins>();
            if (ScriptPin.Status != 0) continue;
            ScriptPin.Render.color = ScriptPin.StartColor;
            ScriptPin.SelectedPins = false;
            ScriptPin.PaintPinst = false;
            ScriptPin.IsEat = false;
        }
    }
    void ShowAccessPin()
    {
        if (Forward != null)
        {
            EventPins ScriptPin = Forward.GetComponent<EventPins>();
            if (ScriptPin.Status == 0)
            {
                ScriptPin.Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.PaintPinst = true;
            }
            else if (ScriptPin.Status == 1 && ScriptPin.Forward != null && ScriptPin.Forward.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.Forward.GetComponent<EventPins>().Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.Forward.GetComponent<EventPins>().PaintPinst = true;
            }
            else if (ScriptPin.Status == 2 && ScriptPin.Forward != null && ScriptPin.Forward.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.IsEat = true;
                EventPins CheckPins = ScriptPin.Forward.GetComponent<EventPins>();
                EventPins SelectedPin = CheckPins;

                while (CheckPins.Forward != null)
                {
                    if (CheckPins.Forward.GetComponent<EventPins>().Status == 2 && CheckPins.Forward.GetComponent<EventPins>().Forward != null && CheckPins.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>().Status == 0)
                    {
                        CheckPins.Forward.GetComponent<EventPins>().IsEat = true;
                        CheckPins = CheckPins.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>();
                        SelectedPin = CheckPins;
                    }
                    else break;
                }
                SelectedPin.Render.color = new Color32(203, 133, 0, 255);
                SelectedPin.PaintPinst = true;
            }
        }
        if (Back != null)
        {
            EventPins ScriptPin = Back.GetComponent<EventPins>();
            if (ScriptPin.Status == 0)
            {
                ScriptPin.Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.PaintPinst = true;
            }
            else if (ScriptPin.Status == 1 && ScriptPin.Back != null && ScriptPin.Back.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.Back.GetComponent<EventPins>().Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.Back.GetComponent<EventPins>().PaintPinst = true;
            }
            else if (ScriptPin.Status == 2 && ScriptPin.Back != null && ScriptPin.Back.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.IsEat = true;
                EventPins CheckPins = ScriptPin.Back.GetComponent<EventPins>();
                EventPins SelectedPin = CheckPins;

                while (CheckPins.Back != null)
                {
                    if (CheckPins.Back.GetComponent<EventPins>().Status == 2 && CheckPins.Back.GetComponent<EventPins>().Back != null && CheckPins.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>().Status == 0)
                    {
                        CheckPins.Back.GetComponent<EventPins>().IsEat = true;
                        CheckPins = CheckPins.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>();
                        SelectedPin = CheckPins;
                    }
                    else break;
                }
                SelectedPin.Render.color = new Color32(203, 133, 0, 255);
                SelectedPin.PaintPinst = true;
            }
        }
        if (Left != null)
        {
            EventPins ScriptPin = Left.GetComponent<EventPins>();
            if (ScriptPin.Status == 0)
            {
                ScriptPin.Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.PaintPinst = true;
            }
            else if (ScriptPin.Status == 1 && ScriptPin.Left != null && ScriptPin.Left.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.Left.GetComponent<EventPins>().Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.Left.GetComponent<EventPins>().PaintPinst = true;
            }
            else if (ScriptPin.Status == 2 && ScriptPin.Left != null && ScriptPin.Left.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.IsEat = true;
                EventPins CheckPins = ScriptPin.Left.GetComponent<EventPins>();
                EventPins SelectedPin = CheckPins;

                while (CheckPins.Left != null)
                {
                    if (CheckPins.Left.GetComponent<EventPins>().Status == 2 && CheckPins.Left.GetComponent<EventPins>().Left != null && CheckPins.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>().Status == 0)
                    {
                        CheckPins.Left.GetComponent<EventPins>().IsEat = true;
                        CheckPins = CheckPins.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>();
                        SelectedPin = CheckPins;
                    }
                    else break;
                }
                SelectedPin.Render.color = new Color32(203, 133, 0, 255);
                SelectedPin.PaintPinst = true;
            }
        }
        if (Right != null)
        {
            EventPins ScriptPin = Right.GetComponent<EventPins>();
            if (ScriptPin.Status == 0)
            {
                ScriptPin.Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.PaintPinst = true;
            }
            else if (ScriptPin.Status == 1 && ScriptPin.Right != null && ScriptPin.Right.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.Right.GetComponent<EventPins>().Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.Right.GetComponent<EventPins>().PaintPinst = true;
            }
            else if (ScriptPin.Status == 2 && ScriptPin.Right != null && ScriptPin.Right.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.IsEat = true;
                EventPins CheckPins = ScriptPin.Right.GetComponent<EventPins>();
                EventPins SelectedPin = CheckPins;

                while (CheckPins.Right != null)
                {
                    if (CheckPins.Right.GetComponent<EventPins>().Status == 2 && CheckPins.Right.GetComponent<EventPins>().Right != null && CheckPins.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>().Status == 0)
                    {
                        CheckPins.Right.GetComponent<EventPins>().IsEat = true;
                        CheckPins = CheckPins.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>();
                        SelectedPin = CheckPins;
                    }
                    else break;
                }
                SelectedPin.Render.color = new Color32(203, 133, 0, 255);
                SelectedPin.PaintPinst = true;
            }
        }
        if (UpLeftDiagonal != null)
        {
            EventPins ScriptPin = UpLeftDiagonal.GetComponent<EventPins>();
            if (ScriptPin.Status == 0)
            {
                ScriptPin.Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.PaintPinst = true;
            }
            else if (ScriptPin.Status == 1 && ScriptPin.UpLeftDiagonal != null && ScriptPin.UpLeftDiagonal.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.UpLeftDiagonal.GetComponent<EventPins>().Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.UpLeftDiagonal.GetComponent<EventPins>().PaintPinst = true;
            }
            else if (ScriptPin.Status == 2 && ScriptPin.UpLeftDiagonal != null && ScriptPin.UpLeftDiagonal.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.IsEat = true;
                EventPins CheckPins = ScriptPin.UpLeftDiagonal.GetComponent<EventPins>();
                EventPins SelectedPin = CheckPins;

                while (CheckPins.UpLeftDiagonal != null)
                {
                    if (CheckPins.UpLeftDiagonal.GetComponent<EventPins>().Status == 2 && CheckPins.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal != null && CheckPins.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>().Status == 0)
                    {
                        CheckPins.UpLeftDiagonal.GetComponent<EventPins>().IsEat = true;
                        CheckPins = CheckPins.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>();
                        SelectedPin = CheckPins;
                    }
                    else break;
                }
                SelectedPin.Render.color = new Color32(203, 133, 0, 255);
                SelectedPin.PaintPinst = true;
            }
        }
        if (UpRightDiagonal != null)
        {
            EventPins ScriptPin = UpRightDiagonal.GetComponent<EventPins>();
            if (ScriptPin.Status == 0)
            {
                ScriptPin.Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.PaintPinst = true;
            }
            else if (ScriptPin.Status == 1 && ScriptPin.UpRightDiagonal != null && ScriptPin.UpRightDiagonal.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.UpRightDiagonal.GetComponent<EventPins>().Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.UpRightDiagonal.GetComponent<EventPins>().PaintPinst = true;
            }
            else if (ScriptPin.Status == 2 && ScriptPin.UpRightDiagonal != null && ScriptPin.UpRightDiagonal.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.IsEat = true;
                EventPins CheckPins = ScriptPin.UpRightDiagonal.GetComponent<EventPins>();
                EventPins SelectedPin = CheckPins;

                while (CheckPins.UpRightDiagonal != null)
                {
                    if (CheckPins.UpRightDiagonal.GetComponent<EventPins>().Status == 2 && CheckPins.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal != null && CheckPins.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>().Status == 0)
                    {
                        CheckPins.UpRightDiagonal.GetComponent<EventPins>().IsEat = true;
                        CheckPins = CheckPins.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>();
                        SelectedPin = CheckPins;
                    }
                    else break;
                }
                SelectedPin.Render.color = new Color32(203, 133, 0, 255);
                SelectedPin.PaintPinst = true;
            }
        }
        if (DownLeftDiagonal != null)
        {
            EventPins ScriptPin = DownLeftDiagonal.GetComponent<EventPins>();
            if (ScriptPin.Status == 0)
            {
                ScriptPin.Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.PaintPinst = true;
            }
            else if (ScriptPin.Status == 1 && ScriptPin.DownLeftDiagonal != null && ScriptPin.DownLeftDiagonal.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.DownLeftDiagonal.GetComponent<EventPins>().Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.DownLeftDiagonal.GetComponent<EventPins>().PaintPinst = true;
            }
            else if (ScriptPin.Status == 2 && ScriptPin.DownLeftDiagonal != null && ScriptPin.DownLeftDiagonal.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.IsEat = true;
                EventPins CheckPins = ScriptPin.DownLeftDiagonal.GetComponent<EventPins>();
                EventPins SelectedPin = CheckPins;

                while (CheckPins.DownLeftDiagonal != null)
                {
                    if (CheckPins.DownLeftDiagonal.GetComponent<EventPins>().Status == 2 && CheckPins.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal != null && CheckPins.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>().Status == 0)
                    {
                        CheckPins.DownLeftDiagonal.GetComponent<EventPins>().IsEat = true;
                        CheckPins = CheckPins.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>();
                        SelectedPin = CheckPins;
                    }
                    else break;
                }
                SelectedPin.Render.color = new Color32(203, 133, 0, 255);
                SelectedPin.PaintPinst = true;
            }
        }
        if (DownRightDiagonal != null)
        {
            EventPins ScriptPin = DownRightDiagonal.GetComponent<EventPins>();
            if (ScriptPin.Status == 0)
            {
                ScriptPin.Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.PaintPinst = true;
            }
            else if (ScriptPin.Status == 1 && ScriptPin.DownRightDiagonal != null && ScriptPin.DownRightDiagonal.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.DownRightDiagonal.GetComponent<EventPins>().Render.color = new Color32(203, 133, 0, 255);
                ScriptPin.DownRightDiagonal.GetComponent<EventPins>().PaintPinst = true;
            }
            else if (ScriptPin.Status == 2 && ScriptPin.DownRightDiagonal != null && ScriptPin.DownRightDiagonal.GetComponent<EventPins>().Status == 0)
            {
                ScriptPin.IsEat = true;
                EventPins CheckPins = ScriptPin.DownRightDiagonal.GetComponent<EventPins>();
                EventPins SelectedPin = CheckPins;

                while (CheckPins.DownRightDiagonal != null)
                {
                    if (CheckPins.DownRightDiagonal.GetComponent<EventPins>().Status == 2 && CheckPins.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal != null && CheckPins.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>().Status == 0)
                    {
                        CheckPins.DownRightDiagonal.GetComponent<EventPins>().IsEat = true;
                        CheckPins = CheckPins.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>();
                        SelectedPin = CheckPins;
                    }
                    else break;
                }
                SelectedPin.Render.color = new Color32(203, 133, 0, 255);
                SelectedPin.PaintPinst = true;
            }
        }
    }
    void OnMouseEnter()
    {
        if(Status != 2) Render.color = new Color32(25, 39, 108, 255);
        //меняем на синий(цвет выбора) если это либо пустая точка, либо фишка игрока.
    }
    void OnMouseExit()
    {
        if(!SelectedPins) Render.color = StartColor;
        if (PaintPinst) Render.color = new Color32(203, 133, 0, 255);
        //Меняем цвеи обратно
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
