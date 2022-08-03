using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField]
    GameObject TitleText;
    Text CompText;

    GameObject PlayerScore;
    GameObject AIScore;
    Text AIText;
    Text PlayerText;

    GameObject RestartButton;

    public int Difficulty;

    int AIEat = 0;
    public float StartAI = 0.0f;
    public int PlayerEat = 0;
    public int PlayNumber = 1;
    // Start is called before the first frame update
    void Start()
    {
        CompText = TitleText.GetComponent<Text>();
        PlayerScore = GameObject.Find("Death Red/Text");
        AIScore = GameObject.Find("Death Green/Text");
        AIText = AIScore.GetComponent<Text>();
        PlayerText = PlayerScore.GetComponent<Text>();
        RestartButton = GameObject.Find("RestartButton");
        RestartButton.SetActive(false);

        Difficulty = GameObject.Find("Dropdown").GetComponent<Dropdown>().value + 1;
        Image Dropdowns = GameObject.Find("Dropdown").GetComponent<Image>();
        if (Difficulty == 1) Dropdowns.color = Color.green;
        if (Difficulty == 2) Dropdowns.color = Color.yellow;
        if (Difficulty == 3) Dropdowns.color = Color.red;
    }

    public void ChangeDifficulty(int Value)
    {
        Difficulty = Value + 1;
        Image Dropdowns = GameObject.Find("Dropdown").GetComponent<Image>();
        if(Difficulty == 1) Dropdowns.color = Color.green;
        if (Difficulty == 2) Dropdowns.color = Color.yellow;
        if (Difficulty == 3) Dropdowns.color = Color.red;

    }

    int[] EvaluationFunction(int[] StatusPlayGround)
    {
        int Red = 0;
        int Green = 0;
        for (int i = 0; i < StatusPlayGround.Length; i++) //Перебираем все фишки
        {
            if (StatusPlayGround[i] == 2) Red++;
            else if (StatusPlayGround[i] == 1) Green++;
        }
        if (Difficulty == 1 || Difficulty == 3)
        {
            Green += PlayerEat;
            Red += AIEat;
        }
        int Score = Red - Green;
        int[] Out = new int[3]{ 0, 0, Score };
        return Out;
    }

    public void StartPlay()
    {
        if (PlayNumber == 2 && PlayerEat < 16 && AIEat < 16)
        {
            GameObject[] AllPin = GameObject.FindGameObjectsWithTag("Pin"); //Заносим все фишки в массив
            int[] CopyStatus = new int[AllPin.Length]; //Массив для копий статусов фишек

            for (int i = 0; i < AllPin.Length; i++) //Перебираем все фишки
            {
                AllPin[i].GetComponent<EventPins>().EventPinsID = i; //Устанавливаем соответствие скрипта определенному игровому объекту
                CopyStatus[i] = AllPin[i].GetComponent<EventPins>().Status; //Копируем статусы
            }
            int[] Output = new int[3];
            if(Difficulty == 1) Output = MiniMax(AllPin, CopyStatus, 1, 2, true, int.MinValue, int.MaxValue);
            else if (Difficulty == 2) Output = MiniMax(AllPin, CopyStatus, 2, 2, true, int.MinValue, int.MaxValue);
            else if (Difficulty == 3) Output = MiniMax(AllPin, CopyStatus, 2, 2, true, int.MinValue, int.MaxValue);
            print(Output[0]);
            print(Output[1]);
            print(Output[2]);
            if(Output[0] == 0)
            {
            RandomSelect:
                Output[1] = Random.Range(0, AllPin.Length);
                bool ReSelect = true;

                //Вперед
                if (AllPin[Output[1]].GetComponent<EventPins>().Forward != null && AllPin[Output[1]].GetComponent<EventPins>().Forward.GetComponent<EventPins>().Status == 0) ReSelect = false;
                //Назад
                if (AllPin[Output[1]].GetComponent<EventPins>().Back != null && AllPin[Output[1]].GetComponent<EventPins>().Back.GetComponent<EventPins>().Status == 0) ReSelect = false;
                //Лево
                if (AllPin[Output[1]].GetComponent<EventPins>().Left != null && AllPin[Output[1]].GetComponent<EventPins>().Left.GetComponent<EventPins>().Status == 0) ReSelect = false;
                //Право
                if (AllPin[Output[1]].GetComponent<EventPins>().Right != null && AllPin[Output[1]].GetComponent<EventPins>().Right.GetComponent<EventPins>().Status == 0) ReSelect = false;
                //Верхняя левая диагональ
                if (AllPin[Output[1]].GetComponent<EventPins>().UpLeftDiagonal != null && AllPin[Output[1]].GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>().Status == 0) ReSelect = false;
                //Верхняя правая диагональ
                if (AllPin[Output[1]].GetComponent<EventPins>().UpRightDiagonal != null && AllPin[Output[1]].GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>().Status == 0) ReSelect = false;
                //Нижняя левая диагональ
                if (AllPin[Output[1]].GetComponent<EventPins>().DownLeftDiagonal != null && AllPin[Output[1]].GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>().Status == 0) ReSelect = false;
                //Нижняя правая диагональ
                if (AllPin[Output[1]].GetComponent<EventPins>().DownRightDiagonal != null && AllPin[Output[1]].GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>().Status == 0) ReSelect = false;




                if(AllPin[Output[1]].GetComponent<EventPins>().Status != 2 || ReSelect == true) goto RandomSelect;
                GoPin(AllPin[Output[1]].GetComponent<EventPins>(), Output[0]);
            }
            else GoPin(AllPin[Output[1]].GetComponent<EventPins>(), Output[0]);
        }
    }

    int[] MiniMax(GameObject[] Pins, int[] RootStatusTree, int Depth, int Player = 2, bool IsFirstCall = false, int alpha = int.MinValue, int beta = int.MaxValue)
    {
        int[] RootStatusTreeCopy = (int[])RootStatusTree.Clone();
        int Eat = 0;
        if (Player == 2) Eat = 1;
        else Eat = 2;
        int[] ForwardOF = new int[3]{ 0, 0, 0 }; // [0] - Направление хода [1] - Точка которой ходят [2] - Значение ОФ
        int[] BackOF = new int[3] { 0, 0, 0 };
        int[] LeftOF = new int[3] { 0, 0, 0 };
        int[] RightOF = new int[3] { 0, 0, 0 };
        int[] UpRightDiagonalOF = new int[3] { 0, 0, 0 };
        int[] UpLeftDiagonalOF = new int[3] { 0, 0, 0 };
        int[] DownRightDiagonalOF = new int[3] { 0, 0, 0 };
        int[] DownLeftDiagonalOF = new int[3] { 0, 0, 0 };
        int[] SelectedOF = EvaluationFunction(RootStatusTree);

        for (int i = 0; i < Pins.Length; i++) //Перебираем все фишки
        {
            bool[] DirectionAccess = new bool[8] { false, false, false, false, false, false, false, false };
            EventPins PinScript = Pins[i].GetComponent<EventPins>();
            if (RootStatusTree[PinScript.EventPinsID] == Player)
            {
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                RootStatusTree = (int[])RootStatusTreeCopy.Clone();
                PinScript = Pins[i].GetComponent<EventPins>();
                if (PinScript.Forward != null)
                {
                    if (RootStatusTree[PinScript.Forward.GetComponent<EventPins>().EventPinsID] == Eat)
                    {
                        while (PinScript.Forward != null)
                        {
                            if (RootStatusTree[PinScript.Forward.GetComponent<EventPins>().EventPinsID] == Eat && PinScript.Forward.GetComponent<EventPins>().Forward != null && RootStatusTree[PinScript.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>().EventPinsID] == 0)
                            {
                                RootStatusTree[PinScript.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>().EventPinsID] = Player;
                                RootStatusTree[PinScript.Forward.GetComponent<EventPins>().EventPinsID] = 0;
                                RootStatusTree[PinScript.EventPinsID] = 0;

                                PinScript = PinScript.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>();
                                DirectionAccess[0] = true;
                            }
                            else break;
                        }
                    }
                    else if (RootStatusTree[PinScript.Forward.GetComponent<EventPins>().EventPinsID] == 0)
                    {
                        RootStatusTree[PinScript.Forward.GetComponent<EventPins>().EventPinsID] = Player;
                        RootStatusTree[PinScript.EventPinsID] = 0;
                        DirectionAccess[0] = true;
                    }
                    if (Depth > 1)
                    {
                        if (Player == 2)
                        {
                            ForwardOF = MiniMax(Pins, RootStatusTree, Depth - 1, 1, false, alpha, beta);
                            if (ForwardOF[2] < SelectedOF[2] && IsFirstCall == false)
                            {
                                SelectedOF[0] = 1;
                                SelectedOF[1] = i;
                                SelectedOF[2] = ForwardOF[2];
                            }
                            if (ForwardOF[2] >= SelectedOF[2] && IsFirstCall == true && DirectionAccess[0] == true)
                            {
                                SelectedOF[0] = 1;
                                SelectedOF[1] = i;
                                SelectedOF[2] = ForwardOF[2];
                            }
                            if (beta > ForwardOF[2]) beta = ForwardOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            ForwardOF = MiniMax(Pins, RootStatusTree, Depth - 1, 2, false, alpha, beta);
                            if (ForwardOF[2] > SelectedOF[2])
                            {
                                SelectedOF[2] = ForwardOF[2];
                            }
                            if (alpha < ForwardOF[2]) alpha = ForwardOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                    else
                    {
                        ForwardOF = EvaluationFunction(RootStatusTree);
                        if (Player == 2)
                        {
                            if (ForwardOF[2] > SelectedOF[2])
                            {
                                SelectedOF[0] = 1;
                                SelectedOF[1] = i;
                                SelectedOF[2] = ForwardOF[2];
                            }
                            if (alpha < ForwardOF[2]) alpha = ForwardOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            if (ForwardOF[2] < SelectedOF[2])
                            {
                                SelectedOF[2] = ForwardOF[2];
                            }
                            if (beta > ForwardOF[2]) beta = ForwardOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                RootStatusTree = (int[])RootStatusTreeCopy.Clone();
                PinScript = Pins[i].GetComponent<EventPins>();
                if (PinScript.Back != null)
                {
                    if (RootStatusTree[PinScript.Back.GetComponent<EventPins>().EventPinsID] == Eat)
                    {
                        while (PinScript.Back != null)
                        {
                            if (RootStatusTree[PinScript.Back.GetComponent<EventPins>().EventPinsID] == Eat && PinScript.Back.GetComponent<EventPins>().Back != null && RootStatusTree[PinScript.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>().EventPinsID] == 0)
                            {
                                RootStatusTree[PinScript.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>().EventPinsID] = Player;
                                RootStatusTree[PinScript.Back.GetComponent<EventPins>().EventPinsID] = 0;
                                RootStatusTree[PinScript.EventPinsID] = 0;
                                PinScript = PinScript.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>();
                                DirectionAccess[1] = true;
                            }
                            else break;
                        }
                    }
                    else if (RootStatusTree[PinScript.Back.GetComponent<EventPins>().EventPinsID] == 0)
                    {
                        RootStatusTree[PinScript.Back.GetComponent<EventPins>().EventPinsID] = Player;
                        RootStatusTree[PinScript.EventPinsID] = 0;
                        DirectionAccess[1] = true;
                    }
                    if (Depth > 1)
                    {
                        if (Player == 2)
                        {
                            BackOF = MiniMax(Pins, RootStatusTree, Depth - 1, 1, false, alpha, beta);
                            if (BackOF[2] < SelectedOF[2] && IsFirstCall == false)
                            {
                                SelectedOF[0] = 2;
                                SelectedOF[1] = i;
                                SelectedOF[2] = BackOF[2];
                            }
                            if (BackOF[2] >= SelectedOF[2] && IsFirstCall == true && DirectionAccess[1] == true)
                            {
                                SelectedOF[0] = 2;
                                SelectedOF[1] = i;
                                SelectedOF[2] = BackOF[2];
                            }
                            if (beta > BackOF[2]) beta = BackOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            BackOF = MiniMax(Pins, RootStatusTree, Depth - 1, 2, false, alpha, beta);
                            if (BackOF[2] > SelectedOF[2])
                            {
                                SelectedOF[2] = BackOF[2];
                            }
                            if (alpha < BackOF[2]) alpha = BackOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                    else
                    {
                        BackOF = EvaluationFunction(RootStatusTree);
                        if (Player == 2)
                        {
                            if (BackOF[2] > SelectedOF[2])
                            {
                                SelectedOF[0] = 2;
                                SelectedOF[1] = i;
                                SelectedOF[2] = BackOF[2];
                            }
                            if (alpha < BackOF[2]) alpha = BackOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            if (BackOF[2] < SelectedOF[2])
                            {
                                SelectedOF[2] = BackOF[2];
                            }
                            if (beta > BackOF[2]) beta = BackOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                RootStatusTree = (int[])RootStatusTreeCopy.Clone();
                PinScript = Pins[i].GetComponent<EventPins>();
                if (PinScript.Left != null)
                {
                    if (RootStatusTree[PinScript.Left.GetComponent<EventPins>().EventPinsID] == Eat)
                    {
                        while (PinScript.Left != null)
                        {
                            if (RootStatusTree[PinScript.Left.GetComponent<EventPins>().EventPinsID] == Eat && PinScript.Left.GetComponent<EventPins>().Left != null && RootStatusTree[PinScript.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>().EventPinsID] == 0)
                            {
                                RootStatusTree[PinScript.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>().EventPinsID] = Player;
                                RootStatusTree[PinScript.Left.GetComponent<EventPins>().EventPinsID] = 0;
                                RootStatusTree[PinScript.EventPinsID] = 0;

                                PinScript = PinScript.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>();
                                DirectionAccess[2] = true;
                            }
                            else break;
                        }
                    }
                    else if (RootStatusTree[PinScript.Left.GetComponent<EventPins>().EventPinsID] == 0)
                    {
                        RootStatusTree[PinScript.Left.GetComponent<EventPins>().EventPinsID] = Player;
                        RootStatusTree[PinScript.EventPinsID] = 0;
                        DirectionAccess[2] = true;
                    }
                    if (Depth > 1)
                    {
                        if (Player == 2)
                        {
                            LeftOF = MiniMax(Pins, RootStatusTree, Depth - 1, 1, false, alpha, beta);
                            if (LeftOF[2] < SelectedOF[2] && IsFirstCall == false)
                            {
                                SelectedOF[0] = 3;
                                SelectedOF[1] = i;
                                SelectedOF[2] = LeftOF[2];
                            }
                            if (LeftOF[2] >= SelectedOF[2] && IsFirstCall == true && DirectionAccess[2] == true)
                            {
                                SelectedOF[0] = 3;
                                SelectedOF[1] = i;
                                SelectedOF[2] = LeftOF[2];
                            }
                            if (beta > LeftOF[2]) beta = LeftOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            LeftOF = MiniMax(Pins, RootStatusTree, Depth - 1, 2, false, alpha, beta);
                            if (LeftOF[2] > SelectedOF[2])
                            {
                                SelectedOF[2] = LeftOF[2];
                            }
                            if (alpha < LeftOF[2]) alpha = LeftOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                    else
                    {
                        LeftOF = EvaluationFunction(RootStatusTree);
                        if (Player == 2)
                        {
                            if (LeftOF[2] > SelectedOF[2])
                            {
                                SelectedOF[0] = 3;
                                SelectedOF[1] = i;
                                SelectedOF[2] = LeftOF[2];
                            }
                            if (alpha < LeftOF[2]) alpha = LeftOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            if (LeftOF[2] < SelectedOF[2])
                            {
                                SelectedOF[2] = LeftOF[2];
                            }
                            if (beta > LeftOF[2]) beta = LeftOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                RootStatusTree = (int[])RootStatusTreeCopy.Clone();
                PinScript = Pins[i].GetComponent<EventPins>();
                if (PinScript.Right != null)
                {
                    if (RootStatusTree[PinScript.Right.GetComponent<EventPins>().EventPinsID] == Eat)
                    {
                        while (PinScript.Right != null)
                        {
                            if (RootStatusTree[PinScript.Right.GetComponent<EventPins>().EventPinsID] == Eat && PinScript.Right.GetComponent<EventPins>().Right != null && RootStatusTree[PinScript.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>().EventPinsID] == 0)
                            {
                                RootStatusTree[PinScript.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>().EventPinsID] = Player;
                                RootStatusTree[PinScript.Right.GetComponent<EventPins>().EventPinsID] = 0;
                                RootStatusTree[PinScript.EventPinsID] = 0;

                                PinScript = PinScript.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>();
                                DirectionAccess[3] = true;
                            }
                            else break;
                        }
                    }
                    else if (RootStatusTree[PinScript.Right.GetComponent<EventPins>().EventPinsID] == 0)
                    {
                        RootStatusTree[PinScript.Right.GetComponent<EventPins>().EventPinsID] = Player;
                        RootStatusTree[PinScript.EventPinsID] = 0;
                        DirectionAccess[3] = true;
                    }
                    if (Depth > 1)
                    {
                        if (Player == 2)
                        {
                            RightOF = MiniMax(Pins, RootStatusTree, Depth - 1, 1, false, alpha, beta);
                            if (RightOF[2] < SelectedOF[2] && IsFirstCall == false)
                            {
                                SelectedOF[0] = 4;
                                SelectedOF[1] = i;
                                SelectedOF[2] = RightOF[2];
                            }
                            if (RightOF[2] >= SelectedOF[2] && IsFirstCall == true && DirectionAccess[3] == true)
                            {
                                SelectedOF[0] = 4;
                                SelectedOF[1] = i;
                                SelectedOF[2] = RightOF[2];
                            }
                            if (beta > RightOF[2]) beta = RightOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            RightOF = MiniMax(Pins, RootStatusTree, Depth - 1, 2, false, alpha, beta);
                            if (RightOF[2] > SelectedOF[2])
                            {
                                SelectedOF[2] = RightOF[2];
                            }
                            if (alpha < RightOF[2]) alpha = RightOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                    else
                    {
                        RightOF = EvaluationFunction(RootStatusTree);
                        if (Player == 2)
                        {
                            if (RightOF[2] > SelectedOF[2])
                            {
                                SelectedOF[0] = 4;
                                SelectedOF[1] = i;
                                SelectedOF[2] = RightOF[2];
                            }
                            if (alpha < RightOF[2]) alpha = RightOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            if (RightOF[2] < SelectedOF[2])
                            {
                                SelectedOF[2] = RightOF[2];
                            }
                            if (beta > RightOF[2]) beta = RightOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                RootStatusTree = (int[])RootStatusTreeCopy.Clone();
                PinScript = Pins[i].GetComponent<EventPins>();
                if (PinScript.UpRightDiagonal != null)
                {
                    if (RootStatusTree[PinScript.UpRightDiagonal.GetComponent<EventPins>().EventPinsID] == Eat)
                    {
                        while (PinScript.UpRightDiagonal != null)
                        {
                            if (RootStatusTree[PinScript.UpRightDiagonal.GetComponent<EventPins>().EventPinsID] == Eat && PinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal != null && RootStatusTree[PinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>().EventPinsID] == 0)
                            {
                                RootStatusTree[PinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>().EventPinsID] = Player;
                                RootStatusTree[PinScript.UpRightDiagonal.GetComponent<EventPins>().EventPinsID] = 0;
                                RootStatusTree[PinScript.EventPinsID] = 0;

                                PinScript = PinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>();
                                DirectionAccess[4] = true;
                            }
                            else break;
                        }
                    }
                    else if (RootStatusTree[PinScript.UpRightDiagonal.GetComponent<EventPins>().EventPinsID] == 0)
                    {
                        RootStatusTree[PinScript.UpRightDiagonal.GetComponent<EventPins>().EventPinsID] = Player;
                        RootStatusTree[PinScript.EventPinsID] = 0;
                        DirectionAccess[4] = true;
                    }
                    if (Depth > 1)
                    {
                        if (Player == 2)
                        {
                            UpRightDiagonalOF = MiniMax(Pins, RootStatusTree, Depth - 1, 1, false, alpha, beta);
                            if (UpRightDiagonalOF[2] < SelectedOF[2] && IsFirstCall == false)
                            {
                                SelectedOF[0] = 5;
                                SelectedOF[1] = i;
                                SelectedOF[2] = UpRightDiagonalOF[2];
                            }
                            if (UpRightDiagonalOF[2] >= SelectedOF[2] && IsFirstCall == true && DirectionAccess[4] == true)
                            {
                                SelectedOF[0] = 5;
                                SelectedOF[1] = i;
                                SelectedOF[2] = UpRightDiagonalOF[2];
                            }
                            if (beta > UpRightDiagonalOF[2]) beta = UpRightDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            UpRightDiagonalOF = MiniMax(Pins, RootStatusTree, Depth - 1, 2, false, alpha, beta);
                            if (UpRightDiagonalOF[2] > SelectedOF[2])
                            {
                                SelectedOF[2] = UpRightDiagonalOF[2];
                            }
                            if (alpha < UpRightDiagonalOF[2]) alpha = UpRightDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                    else
                    {
                        UpRightDiagonalOF = EvaluationFunction(RootStatusTree);
                        if (Player == 2)
                        {
                            if (UpRightDiagonalOF[2] > SelectedOF[2])
                            {
                                SelectedOF[0] = 5;
                                SelectedOF[1] = i;
                                SelectedOF[2] = UpRightDiagonalOF[2];
                            }
                            if (alpha < UpRightDiagonalOF[2]) alpha = UpRightDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            if (UpRightDiagonalOF[2] < SelectedOF[2])
                            {
                                SelectedOF[2] = UpRightDiagonalOF[2];
                            }
                            if (beta > UpRightDiagonalOF[2]) beta = UpRightDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                RootStatusTree = (int[])RootStatusTreeCopy.Clone();
                PinScript = Pins[i].GetComponent<EventPins>();
                if (PinScript.UpLeftDiagonal != null)
                {
                    if (RootStatusTree[PinScript.UpLeftDiagonal.GetComponent<EventPins>().EventPinsID] == Eat)
                    {
                        while (PinScript.UpLeftDiagonal != null)
                        {
                            if (RootStatusTree[PinScript.UpLeftDiagonal.GetComponent<EventPins>().EventPinsID] == Eat && PinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal != null && RootStatusTree[PinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>().EventPinsID] == 0)
                            {
                                RootStatusTree[PinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>().EventPinsID] = Player;
                                RootStatusTree[PinScript.UpLeftDiagonal.GetComponent<EventPins>().EventPinsID] = 0;
                                RootStatusTree[PinScript.EventPinsID] = 0;

                                PinScript = PinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>();
                                DirectionAccess[5] = true;
                            }
                            else break;
                        }
                    }
                    else if (RootStatusTree[PinScript.UpLeftDiagonal.GetComponent<EventPins>().EventPinsID] == 0)
                    {
                        RootStatusTree[PinScript.UpLeftDiagonal.GetComponent<EventPins>().EventPinsID] = Player;
                        RootStatusTree[PinScript.EventPinsID] = 0;
                        DirectionAccess[5] = true;
                    }
                    if (Depth > 1)
                    {
                        if (Player == 2)
                        {
                            UpLeftDiagonalOF = MiniMax(Pins, RootStatusTree, Depth - 1, 1, false, alpha, beta);
                            if (UpLeftDiagonalOF[2] < SelectedOF[2] && IsFirstCall == false)
                            {
                                SelectedOF[0] = 6;
                                SelectedOF[1] = i;
                                SelectedOF[2] = UpLeftDiagonalOF[2];
                            }
                            if (UpLeftDiagonalOF[2] >= SelectedOF[2] && IsFirstCall == true && DirectionAccess[5] == true)
                            {
                                SelectedOF[0] = 6;
                                SelectedOF[1] = i;
                                SelectedOF[2] = UpLeftDiagonalOF[2];
                            }
                            if (beta > UpLeftDiagonalOF[2]) beta = UpLeftDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            UpLeftDiagonalOF = MiniMax(Pins, RootStatusTree, Depth - 1, 2, false, alpha, beta);
                            if (UpLeftDiagonalOF[2] > SelectedOF[2])
                            {
                                SelectedOF[2] = UpLeftDiagonalOF[2];
                            }
                            if (alpha < UpLeftDiagonalOF[2]) alpha = UpLeftDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                    else
                    {
                        UpLeftDiagonalOF = EvaluationFunction(RootStatusTree);
                        if (Player == 2)
                        {
                            if (UpLeftDiagonalOF[2] > SelectedOF[2])
                            {
                                SelectedOF[0] = 6;
                                SelectedOF[1] = i;
                                SelectedOF[2] = UpLeftDiagonalOF[2];
                            }
                            if (alpha < UpLeftDiagonalOF[2]) alpha = UpLeftDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            if (UpLeftDiagonalOF[2] < SelectedOF[2])
                            {
                                SelectedOF[2] = UpLeftDiagonalOF[2];
                            }
                            if (beta > UpLeftDiagonalOF[2]) beta = UpLeftDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                RootStatusTree = (int[])RootStatusTreeCopy.Clone();
                PinScript = Pins[i].GetComponent<EventPins>();
                if (PinScript.DownRightDiagonal != null)
                {
                    if (RootStatusTree[PinScript.DownRightDiagonal.GetComponent<EventPins>().EventPinsID] == Eat)
                    {
                        while (PinScript.DownRightDiagonal != null)
                        {
                            if (RootStatusTree[PinScript.DownRightDiagonal.GetComponent<EventPins>().EventPinsID] == Eat && PinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal != null && RootStatusTree[PinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>().EventPinsID] == 0)
                            {
                                RootStatusTree[PinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>().EventPinsID] = Player;
                                RootStatusTree[PinScript.DownRightDiagonal.GetComponent<EventPins>().EventPinsID] = 0;
                                RootStatusTree[PinScript.EventPinsID] = 0;

                                PinScript = PinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>();
                                DirectionAccess[6] = true;
                            }
                            else break;
                        }
                    }
                    else if (RootStatusTree[PinScript.DownRightDiagonal.GetComponent<EventPins>().EventPinsID] == 0)
                    {
                        RootStatusTree[PinScript.DownRightDiagonal.GetComponent<EventPins>().EventPinsID] = Player;
                        RootStatusTree[PinScript.EventPinsID] = 0;
                        DirectionAccess[6] = true;
                    }
                    if (Depth > 1)
                    {
                        if (Player == 2)
                        {
                            DownRightDiagonalOF = MiniMax(Pins, RootStatusTree, Depth - 1, 1, false, alpha, beta);
                            if (DownRightDiagonalOF[2] < SelectedOF[2] && IsFirstCall == false)
                            {
                                SelectedOF[0] = 7;
                                SelectedOF[1] = i;
                                SelectedOF[2] = DownRightDiagonalOF[2];
                            }
                            if (DownRightDiagonalOF[2] >= SelectedOF[2] && IsFirstCall == true && DirectionAccess[6] == true)
                            {
                                SelectedOF[0] = 7;
                                SelectedOF[1] = i;
                                SelectedOF[2] = DownRightDiagonalOF[2];
                            }
                            if (beta > DownRightDiagonalOF[2]) beta = DownRightDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            DownRightDiagonalOF = MiniMax(Pins, RootStatusTree, Depth - 1, 2, false, alpha, beta);
                            if (DownRightDiagonalOF[2] > SelectedOF[2])
                            {
                                SelectedOF[2] = DownRightDiagonalOF[2];
                            }
                            if (alpha < DownRightDiagonalOF[2]) alpha = DownRightDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                    else
                    {
                        DownRightDiagonalOF = EvaluationFunction(RootStatusTree);
                        if (Player == 2)
                        {
                            if (DownRightDiagonalOF[2] > SelectedOF[2])
                            {
                                SelectedOF[0] = 7;
                                SelectedOF[1] = i;
                                SelectedOF[2] = DownRightDiagonalOF[2];
                            }
                            if (alpha < DownRightDiagonalOF[2]) alpha = DownRightDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            if (DownRightDiagonalOF[2] < SelectedOF[2])
                            {
                                SelectedOF[2] = DownRightDiagonalOF[2];
                            }
                            if (beta > DownRightDiagonalOF[2]) beta = DownRightDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                RootStatusTree = (int[])RootStatusTreeCopy.Clone();
                PinScript = Pins[i].GetComponent<EventPins>();
                if (PinScript.DownLeftDiagonal != null)
                {
                    if (RootStatusTree[PinScript.DownLeftDiagonal.GetComponent<EventPins>().EventPinsID] == Eat)
                    {
                        while (PinScript.DownLeftDiagonal != null)
                        {
                            if (RootStatusTree[PinScript.DownLeftDiagonal.GetComponent<EventPins>().EventPinsID] == Eat && PinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal != null && RootStatusTree[PinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>().EventPinsID] == 0)
                            {
                                RootStatusTree[PinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>().EventPinsID] = Player;
                                RootStatusTree[PinScript.DownLeftDiagonal.GetComponent<EventPins>().EventPinsID] = 0;
                                RootStatusTree[PinScript.EventPinsID] = 0;

                                PinScript = PinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>();
                                DirectionAccess[7] = true;
                            }
                            else break;
                        }
                    }
                    else if (RootStatusTree[PinScript.DownLeftDiagonal.GetComponent<EventPins>().EventPinsID] == 0)
                    {
                        RootStatusTree[PinScript.DownLeftDiagonal.GetComponent<EventPins>().EventPinsID] = Player;
                        RootStatusTree[PinScript.EventPinsID] = 0;
                        DirectionAccess[7] = true;
                    }
                    if (Depth > 1)
                    {
                        if (Player == 2)
                        {
                            DownLeftDiagonalOF = MiniMax(Pins, RootStatusTree, Depth - 1, 1, false, alpha, beta);
                            if (DownLeftDiagonalOF[2] < SelectedOF[2] && IsFirstCall == false)
                            {
                                SelectedOF[0] = 8;
                                SelectedOF[1] = i;
                                SelectedOF[2] = DownLeftDiagonalOF[2];
                            }
                            if (DownLeftDiagonalOF[2] >= SelectedOF[2] && IsFirstCall == true && DirectionAccess[7] == true)
                            {
                                SelectedOF[0] = 8;
                                SelectedOF[1] = i;
                                SelectedOF[2] = DownLeftDiagonalOF[2];
                            }
                            if (beta > DownLeftDiagonalOF[2]) beta = DownLeftDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            DownLeftDiagonalOF = MiniMax(Pins, RootStatusTree, Depth - 1, 2, false, alpha, beta);
                            if (DownLeftDiagonalOF[2] > SelectedOF[2])
                            {
                                SelectedOF[2] = DownLeftDiagonalOF[2];
                            }
                            if (alpha < DownLeftDiagonalOF[2]) alpha = DownLeftDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                    else
                    {
                        DownLeftDiagonalOF = EvaluationFunction(RootStatusTree);
                        if (Player == 2)
                        {
                            if (DownLeftDiagonalOF[2] > SelectedOF[2])
                            {
                                SelectedOF[0] = 8;
                                SelectedOF[1] = i;
                                SelectedOF[2] = DownLeftDiagonalOF[2];
                            }
                            if (alpha < DownLeftDiagonalOF[2]) alpha = DownLeftDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                        else
                        {
                            if (DownLeftDiagonalOF[2] < SelectedOF[2])
                            {
                                SelectedOF[2] = DownLeftDiagonalOF[2];
                            }
                            if (beta > DownLeftDiagonalOF[2]) beta = DownLeftDiagonalOF[2];
                            if (beta <= alpha) return SelectedOF;
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////
                RootStatusTree = (int[])RootStatusTreeCopy.Clone();
                PinScript = Pins[i].GetComponent<EventPins>();
            }
        }
        return SelectedOF;
    }



    void GoPin(EventPins PinScript, int DirectionGo)
    {
        if(DirectionGo == 0)
        {
            MovePins(PinScript, -1);
            return;
        }
        bool Eat = false;
        switch(DirectionGo)
        {
            case 1://Вперед
            {
                EventPins NewPinScript = PinScript;

                while (NewPinScript.Forward != null)
                {
                    if (NewPinScript.Forward.GetComponent<EventPins>().Status == 1 && NewPinScript.Forward.GetComponent<EventPins>().Forward != null && NewPinScript.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>().Status == 0)
                    {
                            NewPinScript.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>().Status = PinScript.Status;
                            NewPinScript.Forward.GetComponent<EventPins>().Status = 0;
                            PinScript.Status = 0;

                            NewPinScript.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>().StartColor = PinScript.StartColor;
                            NewPinScript.Forward.GetComponent<EventPins>().StartColor = Color.black;
                            PinScript.StartColor = Color.black;

                            NewPinScript.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>().Render.color = NewPinScript.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>().StartColor;
                            NewPinScript.Forward.GetComponent<EventPins>().Render.color = NewPinScript.Forward.GetComponent<EventPins>().StartColor;
                            PinScript.Render.color = PinScript.StartColor;

                            PinScript = NewPinScript.Forward.GetComponent<EventPins>().Forward.GetComponent<EventPins>();
                            NewPinScript = PinScript;
                            AIEat++;
                            Eat = true;

                    }
                    else
                    {
                            if(Eat == false) MovePins(PinScript, DirectionGo);
                            break;
                    }
                }
                break;
            }
            case 2://Назад
            {
                EventPins NewPinScript = PinScript;

                while (NewPinScript.Back != null)
                {
                    if (NewPinScript.Back.GetComponent<EventPins>().Status == 1 && NewPinScript.Back.GetComponent<EventPins>().Back != null && NewPinScript.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>().Status == 0)
                    {
                        NewPinScript.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>().Status = PinScript.Status;
                        NewPinScript.Back.GetComponent<EventPins>().Status = 0;
                        PinScript.Status = 0;

                        NewPinScript.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>().StartColor = PinScript.StartColor;
                        NewPinScript.Back.GetComponent<EventPins>().StartColor = Color.black;
                        PinScript.StartColor = Color.black;

                        NewPinScript.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>().Render.color = NewPinScript.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>().StartColor;
                        NewPinScript.Back.GetComponent<EventPins>().Render.color = NewPinScript.Back.GetComponent<EventPins>().StartColor;
                        PinScript.Render.color = PinScript.StartColor;

                        PinScript = NewPinScript.Back.GetComponent<EventPins>().Back.GetComponent<EventPins>();
                        NewPinScript = PinScript;
                        AIEat++;
                        Eat = true;

                    }
                    else
                    {
                        if (Eat == false) MovePins(PinScript, DirectionGo);
                        break;
                    }
                }
                break;
            }
            case 3://Лево
            {
                EventPins NewPinScript = PinScript;

                while (NewPinScript.Left != null)
                {
                    if (NewPinScript.Left.GetComponent<EventPins>().Status == 1 && NewPinScript.Left.GetComponent<EventPins>().Left != null && NewPinScript.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>().Status == 0)
                    {
                        NewPinScript.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>().Status = PinScript.Status;
                        NewPinScript.Left.GetComponent<EventPins>().Status = 0;
                        PinScript.Status = 0;

                        NewPinScript.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>().StartColor = PinScript.StartColor;
                        NewPinScript.Left.GetComponent<EventPins>().StartColor = Color.black;
                        PinScript.StartColor = Color.black;

                        NewPinScript.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>().Render.color = NewPinScript.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>().StartColor;
                        NewPinScript.Left.GetComponent<EventPins>().Render.color = NewPinScript.Left.GetComponent<EventPins>().StartColor;
                        PinScript.Render.color = PinScript.StartColor;

                        PinScript = NewPinScript.Left.GetComponent<EventPins>().Left.GetComponent<EventPins>();
                        NewPinScript = PinScript;
                        AIEat++;
                        Eat = true;

                    }
                    else
                    {
                        if (Eat == false) MovePins(PinScript, DirectionGo);
                        break;
                    }
                }
                break;
            }
            case 4://Вправо
            {
                EventPins NewPinScript = PinScript;

                while (NewPinScript.Right != null)
                {
                    if (NewPinScript.Right.GetComponent<EventPins>().Status == 1 && NewPinScript.Right.GetComponent<EventPins>().Right != null && NewPinScript.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>().Status == 0)
                    {
                        NewPinScript.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>().Status = PinScript.Status;
                        NewPinScript.Right.GetComponent<EventPins>().Status = 0;
                        PinScript.Status = 0;

                        NewPinScript.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>().StartColor = PinScript.StartColor;
                        NewPinScript.Right.GetComponent<EventPins>().StartColor = Color.black;
                        PinScript.StartColor = Color.black;

                        NewPinScript.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>().Render.color = NewPinScript.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>().StartColor;
                        NewPinScript.Right.GetComponent<EventPins>().Render.color = NewPinScript.Right.GetComponent<EventPins>().StartColor;
                        PinScript.Render.color = PinScript.StartColor;

                        PinScript = NewPinScript.Right.GetComponent<EventPins>().Right.GetComponent<EventPins>();
                        NewPinScript = PinScript;
                        AIEat++;
                        Eat = true;

                    }
                    else
                    {
                        if (Eat == false) MovePins(PinScript, DirectionGo);
                        break;
                    }
                }
                break;
            }
            case 5://Верхняя правая диагональ
            {
                EventPins NewPinScript = PinScript;

                while (NewPinScript.UpRightDiagonal != null)
                {
                    if (NewPinScript.UpRightDiagonal.GetComponent<EventPins>().Status == 1 && NewPinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal != null && NewPinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>().Status == 0)
                    {
                        NewPinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>().Status = PinScript.Status;
                        NewPinScript.UpRightDiagonal.GetComponent<EventPins>().Status = 0;
                        PinScript.Status = 0;

                        NewPinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>().StartColor = PinScript.StartColor;
                        NewPinScript.UpRightDiagonal.GetComponent<EventPins>().StartColor = Color.black;
                        PinScript.StartColor = Color.black;

                        NewPinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>().Render.color = NewPinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>().StartColor;
                        NewPinScript.UpRightDiagonal.GetComponent<EventPins>().Render.color = NewPinScript.UpRightDiagonal.GetComponent<EventPins>().StartColor;
                        PinScript.Render.color = PinScript.StartColor;

                        PinScript = NewPinScript.UpRightDiagonal.GetComponent<EventPins>().UpRightDiagonal.GetComponent<EventPins>();
                        NewPinScript = PinScript;
                        AIEat++;
                        Eat = true;

                    }
                    else
                    {
                        if (Eat == false) MovePins(PinScript, DirectionGo);
                        break;
                    }
                }
                break;
            }
            case 6://Верхняя левая диагональ
            {
                EventPins NewPinScript = PinScript;

                while (NewPinScript.UpLeftDiagonal != null)
                {
                    if (NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().Status == 1 && NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal != null && NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>().Status == 0)
                    {
                        NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>().Status = PinScript.Status;
                        NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().Status = 0;
                        PinScript.Status = 0;

                        NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>().StartColor = PinScript.StartColor;
                        NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().StartColor = Color.black;
                        PinScript.StartColor = Color.black;

                        NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>().Render.color = NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>().StartColor;
                        NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().Render.color = NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().StartColor;
                        PinScript.Render.color = PinScript.StartColor;

                        PinScript = NewPinScript.UpLeftDiagonal.GetComponent<EventPins>().UpLeftDiagonal.GetComponent<EventPins>();
                        NewPinScript = PinScript;
                        AIEat++;
                        Eat = true;

                    }
                    else
                    {
                        if (Eat == false) MovePins(PinScript, DirectionGo);
                        break;
                    }
                }
                break;
            }
            case 7://Нижняя правая диагональ
            {
                EventPins NewPinScript = PinScript;

                while (NewPinScript.DownRightDiagonal != null)
                {
                    if (NewPinScript.DownRightDiagonal.GetComponent<EventPins>().Status == 1 && NewPinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal != null && NewPinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>().Status == 0)
                    {
                        NewPinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>().Status = PinScript.Status;
                        NewPinScript.DownRightDiagonal.GetComponent<EventPins>().Status = 0;
                        PinScript.Status = 0;

                        NewPinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>().StartColor = PinScript.StartColor;
                        NewPinScript.DownRightDiagonal.GetComponent<EventPins>().StartColor = Color.black;
                        PinScript.StartColor = Color.black;

                        NewPinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>().Render.color = NewPinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>().StartColor;
                        NewPinScript.DownRightDiagonal.GetComponent<EventPins>().Render.color = NewPinScript.DownRightDiagonal.GetComponent<EventPins>().StartColor;
                        PinScript.Render.color = PinScript.StartColor;

                        PinScript = NewPinScript.DownRightDiagonal.GetComponent<EventPins>().DownRightDiagonal.GetComponent<EventPins>();
                        NewPinScript = PinScript;
                        AIEat++;
                        Eat = true;
                    }
                    else
                    {
                        if (Eat == false) MovePins(PinScript, DirectionGo);
                        break;
                    }
                }
                break;
            }
            case 8://Нижняя левая диагональ
            {
                EventPins NewPinScript = PinScript;

                while (NewPinScript.DownLeftDiagonal != null)
                {
                    if (NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().Status == 1 && NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal != null && NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>().Status == 0)
                    {
                        NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>().Status = PinScript.Status;
                        NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().Status = 0;
                        PinScript.Status = 0;

                        NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>().StartColor = PinScript.StartColor;
                        NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().StartColor = Color.black;
                        PinScript.StartColor = Color.black;

                        NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>().Render.color = NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>().StartColor;
                        NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().Render.color = NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().StartColor;
                        PinScript.Render.color = PinScript.StartColor;

                        PinScript = NewPinScript.DownLeftDiagonal.GetComponent<EventPins>().DownLeftDiagonal.GetComponent<EventPins>();
                        NewPinScript = PinScript;
                        AIEat++;
                        Eat = true;
                    }
                    else
                    {
                        if (Eat == false) MovePins(PinScript, DirectionGo);
                        break;
                    }
                }
                break;
            }
        }
        PlayNumber = 1; //Переключаем ход на игрока
    }

    void MovePins(EventPins PinScript, int DirectionGo)
    {
        RepeatSelect:
        if (DirectionGo == -1) DirectionGo = Random.Range(1, 9);
        switch (DirectionGo)
        {
            case 1:
                {
                    if (PinScript.Forward == null || PinScript.Forward.GetComponent<EventPins>().Status != 0)
                    {
                        DirectionGo = -1;
                        goto RepeatSelect;
                    }
                    EventPins NewPinScript = PinScript.Forward.GetComponent<EventPins>();

                    NewPinScript.Status = PinScript.Status;
                    PinScript.Status = 0;

                    NewPinScript.StartColor = PinScript.StartColor;
                    PinScript.StartColor = Color.black;

                    NewPinScript.Render.color = NewPinScript.StartColor;
                    PinScript.Render.color = PinScript.StartColor;
                    break;
                }
            case 2:
                {
                    if (PinScript.Back == null || PinScript.Back.GetComponent<EventPins>().Status != 0)
                    {
                        DirectionGo = -1;
                        goto RepeatSelect;
                    }
                    EventPins NewPinScript = PinScript.Back.GetComponent<EventPins>();

                    NewPinScript.Status = PinScript.Status;
                    PinScript.Status = 0;

                    NewPinScript.StartColor = PinScript.StartColor;
                    PinScript.StartColor = Color.black;

                    NewPinScript.Render.color = NewPinScript.StartColor;
                    PinScript.Render.color = PinScript.StartColor;
                    break;
                }
            case 3:
                {
                    if (PinScript.Left == null || PinScript.Left.GetComponent<EventPins>().Status != 0)
                    {
                        DirectionGo = -1;
                        goto RepeatSelect;
                    }
                    EventPins NewPinScript = PinScript.Left.GetComponent<EventPins>();

                    NewPinScript.Status = PinScript.Status;
                    PinScript.Status = 0;

                    NewPinScript.StartColor = PinScript.StartColor;
                    PinScript.StartColor = Color.black;

                    NewPinScript.Render.color = NewPinScript.StartColor;
                    PinScript.Render.color = PinScript.StartColor;
                    break;
                }
            case 4:
                {
                    if (PinScript.Right == null || PinScript.Right.GetComponent<EventPins>().Status != 0)
                    {
                        DirectionGo = -1;
                        goto RepeatSelect;
                    }
                    EventPins NewPinScript = PinScript.Right.GetComponent<EventPins>();

                    NewPinScript.Status = PinScript.Status;
                    PinScript.Status = 0;

                    NewPinScript.StartColor = PinScript.StartColor;
                    PinScript.StartColor = Color.black;

                    NewPinScript.Render.color = NewPinScript.StartColor;
                    PinScript.Render.color = PinScript.StartColor;
                    break;
                }
            case 5:
                {
                    if (PinScript.UpRightDiagonal == null || PinScript.UpRightDiagonal.GetComponent<EventPins>().Status != 0)
                    {
                        DirectionGo = -1;
                        goto RepeatSelect;
                    }
                    EventPins NewPinScript = PinScript.UpRightDiagonal.GetComponent<EventPins>();

                    NewPinScript.Status = PinScript.Status;
                    PinScript.Status = 0;

                    NewPinScript.StartColor = PinScript.StartColor;
                    PinScript.StartColor = Color.black;

                    NewPinScript.Render.color = NewPinScript.StartColor;
                    PinScript.Render.color = PinScript.StartColor;
                    break;
                }
            case 6:
                {
                    if (PinScript.UpLeftDiagonal == null || PinScript.UpLeftDiagonal.GetComponent<EventPins>().Status != 0)
                    {
                        DirectionGo = -1;
                        goto RepeatSelect;
                    }
                    EventPins NewPinScript = PinScript.UpLeftDiagonal.GetComponent<EventPins>();

                    NewPinScript.Status = PinScript.Status;
                    PinScript.Status = 0;

                    NewPinScript.StartColor = PinScript.StartColor;
                    PinScript.StartColor = Color.black;

                    NewPinScript.Render.color = NewPinScript.StartColor;
                    PinScript.Render.color = PinScript.StartColor;
                    break;
                }
            case 7:
                {
                    if (PinScript.DownRightDiagonal == null || PinScript.DownRightDiagonal.GetComponent<EventPins>().Status != 0)
                    {
                        DirectionGo = -1;
                        goto RepeatSelect;
                    }
                    EventPins NewPinScript = PinScript.DownRightDiagonal.GetComponent<EventPins>();

                    NewPinScript.Status = PinScript.Status;
                    PinScript.Status = 0;

                    NewPinScript.StartColor = PinScript.StartColor;
                    PinScript.StartColor = Color.black;

                    NewPinScript.Render.color = NewPinScript.StartColor;
                    PinScript.Render.color = PinScript.StartColor;
                    break;
                }
            case 8:
                {
                    if (PinScript.DownLeftDiagonal == null || PinScript.DownLeftDiagonal.GetComponent<EventPins>().Status != 0)
                    {
                        DirectionGo = -1;
                        goto RepeatSelect;
                    }
                    EventPins NewPinScript = PinScript.DownLeftDiagonal.GetComponent<EventPins>();

                    NewPinScript.Status = PinScript.Status;
                    PinScript.Status = 0;

                    NewPinScript.StartColor = PinScript.StartColor;
                    PinScript.StartColor = Color.black;

                    NewPinScript.Render.color = NewPinScript.StartColor;
                    PinScript.Render.color = PinScript.StartColor;
                    break;
                }
        }
        PlayNumber = 1; //Переключаем ход на игрока
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (StartAI > 0)
        {
            StartAI -= Time.deltaTime;
            if(StartAI <= 0)
            {
                StartAI = 0.0f;
                StartPlay();
            }
        }
        AIText.text = AIEat + " Взятых противником";
        PlayerText.text = PlayerEat + " Взятых вами";

        if (PlayNumber == 1)
        {
            CompText.color = new Color32(52, 138, 48, 255);
            CompText.text = "Ваш ход";
        }
        else
        {
            CompText.color = new Color32(150, 37, 37, 255);
            CompText.text = "Ход противника";
        }

        if (AIEat >= 16)
        {
            GameObject MIMSG = GameObject.Find("MainInfoMessage");
            MIMSG.GetComponent<Text>().text = "Противник выиграл!";
            MIMSG.GetComponent<Text>().color = new Color32(150, 37, 37, 255);

            CompText.color = new Color32(150, 37, 37, 255);
            CompText.text = "END";

            RestartButton.SetActive(true);
        }
        else if (PlayerEat >= 16)
        {
            GameObject MIMSG = GameObject.Find("MainInfoMessage");
            MIMSG.GetComponent<Text>().text = "Вы выиграли!";
            MIMSG.GetComponent<Text>().color = new Color32(52, 138, 48, 255);

            CompText.color = new Color32(52, 138, 48, 255);
            CompText.text = "END";

            RestartButton.SetActive(true);
        }
    }
}
