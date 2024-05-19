using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.UI.Image;

public class GameField : MonoBehaviour
{
    //public GameObject board;

    public GameObject tileHighlightPrefab;

    public GameObject whiteStonePrefab;
    public GameObject blackStonePrefab;
    public GameObject grayStonePrefab;

    public GameObject whiteStoneTrPrefab;
    public GameObject blackStoneTrPrefab;

    public Texture2D whiteStoneCursorTexture;
    public Texture2D blackStoneCursorTexture;
    public Texture2D defaultCursorTexture;

    public GameObject VictoryCenter;
    public GameObject VictoryImageDraw;
    public GameObject VictoryImageWhite;
    public GameObject VictoryImageBlack;

    public GameObject VictoryTextDraw;
    public GameObject VictoryTextBlack;
    public GameObject VictoryTextWhite;

    public GameObject ExitWithoutSaveWindow;

    public GameObject DrawWindow;

    public GameObject whiteMoveTable, blackMoveTable;

    private bool onField = false;

    private GameObject stoneTrPrefab;

    private Texture2D cursor;

    private GameObject stoneTemp;

    private List<GameObject> stones;

    private List<GameObject> locationHighlights;

    private bool gameVictory = false;

    public (int x, int y) PointToGrid(float posX, float posY)
    {
        var position = this.transform.position;
        float xx = position.x;
        float yy = position.y;

        //Debug.Log("������� ����� " + xx + " " + yy);

        float xSize = 855f * (Screen.currentResolution.width / 1920f);
        float ySize = 855f * (Screen.currentResolution.width / 1920f);

        Debug.Log("size " + xSize + " " + ySize);
        Debug.Log("resolution " + Screen.currentResolution.width + " " + Screen.currentResolution.height);

        xx -= xSize / 2;
        yy -= ySize / 2;

        //Debug.Log("������ " + xSize + " " + ySize);

        float stepX = xSize / 15;
        float stepY = ySize / 15;

        int xxx = (int)Math.Abs((posX - xx) / stepX);
        int yyy = (int)Math.Abs((posY - yy) / stepY);

        yyy = 14 - yyy;

        if (xxx < 0 || xxx > 14 || yyy < 0 || yyy > 14)
            throw new Exception();

        return (xxx,yyy);
    }

    public (float x, float y) GridToPoint(int x, int y)
    {
        if (x < 0 || x > 14 || y < 0 || y > 14)
            throw new Exception();

        var position = this.transform.position;
        float xx = position.x;
        float yy = position.y;

        float xSize = 855f * (Screen.currentResolution.width / 1920f);
        float ySize = 855f * (Screen.currentResolution.width / 1920f);

        Debug.Log("size " + xSize + " " + ySize);
        Debug.Log("resolution " + Screen.currentResolution.width + " " + Screen.currentResolution.height);

        xx -= xSize / 2;
        yy -= ySize / 2;

        float stepX = xSize / 15;
        float stepY = ySize / 15;

        float xxx = xx + (x) * stepX + stepX / 2;
        float yyy = yy + (14-y) * stepY + stepY / 2;

        return (xxx, yyy);
    }

    public void Awake()
    {
        //GameData.Init(-1,-1);
        //Debug.Log("awake");

        locationHighlights = new List<GameObject>();
        stones = new List<GameObject>();
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                if (GameData.field[i,j] == 1)
                    TrulyAddStone(i, j,1);
                else if (GameData.field[i, j] == 2)
                    TrulyAddStone(i, j,2);
                else if (GameData.field[i, j] == -1)
                    TrulyAddStone(i, j,-1);
            }
        }

        GameData.isSavedRecently = true;
        gameVictory = false;
    }

    private bool drawWarning = false;

    private bool timeToBreak = false;

    public void ItsTimeToBreak()
    {
        if (GameData.IsVictory(GameData.currentMove + 1)) return;
        //Debug.Log("timeToBreak pressed");
        timeToBreak = true;
    }


    public void Update()
    {
        if (GameData.currentMove % 2 == 0 && blackMoveTable.activeSelf)
        {
            blackMoveTable.SetActive(false);
            whiteMoveTable.SetActive(true);
        }
        else if (GameData.currentMove % 2 == 1 && whiteMoveTable.activeSelf)
        {
            blackMoveTable.SetActive(true);
            whiteMoveTable.SetActive(false);
        }

        if (GameData.currentMove % 2 != GameData.playerMove && GameData.gameMode == 0 && !gameVictory)
        {
            var pos = GameAI.BotsMove();

            AddStone(pos.x, pos.y);

            TrulyAddStone();
            GameData.isSavedRecently = false;
            drawWarning = false;

            if (GameData.IsVictory())
            {
                Debug.Log("botVictory");
                Victory(GameData.currentMove % 2);
                gameVictory = true;
            }

            if (GameData.IsDraw())
            {
                Victory(2);
                gameVictory = true;
            }

            GameData.currentMove++;
            timeToBreak = false;
        }

        if (wasClick)
        {
            var pos = Input.mousePosition;
            var fPos = PointToGrid(pos.x,pos.y);
            SetHighLights();
            AddStone(fPos.x,fPos.y);
            wasClick= false;
        }

        if (timeToBreak && !gameVictory)
        {
            if (stoneTemp != null)
            {
                TrulyAddStone();
                GameData.isSavedRecently = false;
                drawWarning = false;
                Debug.Log("StoneTemp check");
            }
            else if (!drawWarning)
            {
                drawWarning= true;
                GameData.currentMove++;
                Debug.Log("DrawWarning check");
            }
            else
            {
                DrawWindow.SetActive(true);
            }

            if (GameData.IsVictory())
            {
                Debug.Log("playerVictory");
                Victory(GameData.currentMove % 2);
                gameVictory= true;
            }

            if (GameData.IsDraw())
            {
                Victory(2);
                gameVictory = true;
            }

            GameData.currentMove++;
            timeToBreak = false;
        }

    }

    public void SetHighLights()
    {
        DeleteHighLights();
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                GameObject highlight;
                if (GameData.IsCorrect(i, j))
                {
                    var pos = GridToPoint(i, j);
                    highlight = Instantiate(tileHighlightPrefab, new Vector3(pos.x, pos.y, -1), Quaternion.identity, gameObject.transform);
                    locationHighlights.Add(highlight);
                }
            }
        }
    }

    public void DeleteHighLights()
    {
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }

    public void OnMouseEnter()
    {
        if (GameData.IsVictory(GameData.currentMove + 1)) return;
        onField = true;
        if (GameData.currentMove % 2 == 0)
        {
            Cursor.SetCursor(whiteStoneCursorTexture, new Vector3(0,0,0), CursorMode.Auto);
        }
        else
        {
            
            Cursor.SetCursor(blackStoneCursorTexture, new Vector3(0, 0, 0), CursorMode.Auto);
        }
        SetHighLights();
    }

    public void OnMouseExit()
    {
        DeleteHighLights();
        onField = false;
        Cursor.SetCursor(defaultCursorTexture, new Vector3(0, 0, 0), CursorMode.Auto);
    }

    private bool wasClick = false;

    public void OnClick()
    {
        //Debug.Log("click");
        if (GameData.IsVictory(GameData.currentMove + 1)) return;
        wasClick = true;
    }

    public void AddStone(int x, int y)
    {
        if (GameData.IsCorrect(x, y))
        {
            //GameData.field[x, y] = GameData.currentMove % 2 == 0 ? -1 : 1;

            Destroy(stoneTemp);

            DeleteHighLights();

            if (GameData.currentMove % 2 == 0)
            {
                stoneTrPrefab = whiteStoneTrPrefab;
            }
            else
            {
                stoneTrPrefab = blackStoneTrPrefab;
            }

            var pos = GridToPoint(x, y);
            stoneTemp = Instantiate(stoneTrPrefab, new Vector3(pos.x,pos.y,-1), Quaternion.identity, gameObject.transform);

        }
        else
        {
            // ���� ������ ���� �����
        }
    }

    public void TrulyAddStone(int x = -1, int y = -1, int color = -2)
    {
        DeleteHighLights();

        GameObject stone;

       // Debug.Log("TrulyAddStone "+ GameData.currentMove);

        int stoneInt;

        if ( (color==-2 &&GameData.currentMove % 2 == 0) || color == -1)
        {
            stone = whiteStonePrefab;
            stoneInt = -1;
        }
        else if ( (color == -2 && GameData.currentMove % 2 == 1) || color == 1)
        {
            stone = blackStonePrefab;
            stoneInt = 1;
        }
        else
        {
            stone = grayStonePrefab;
            stoneInt = 2;
        }

        int xx, yy;

        if (x==-1 && y == -1)
        {
            var pos = stoneTemp.transform.position;
            var pos2 = PointToGrid(pos.x, pos.y);
            xx = pos2.x;
            yy = pos2.y;
            Destroy(stoneTemp);
            stone = Instantiate(stone, pos, Quaternion.identity, gameObject.transform);
        }
        else
        {
            var pos = GridToPoint(x, y);
            xx = x;
            yy = y;
            stone = Instantiate(stone, new Vector3(pos.x,pos.y,-1), Quaternion.identity, gameObject.transform);
        }

        GameData.field[xx,yy] = stoneInt;

        stones.Add(stone);

        //Debug.Log("lastMove " + xx + " " + yy);

        
    }

    public void UndoMove()
    {
        if (stoneTemp!=null)
            Destroy(stoneTemp);
    }

    public void Victory(int side)
    {

        VictoryImageBlack.SetActive(false);
        VictoryImageWhite.SetActive(false);
        VictoryImageDraw.SetActive(false);

        VictoryTextDraw.SetActive(false);
        VictoryTextBlack.SetActive(false);
        VictoryTextWhite.SetActive(false);

        if (side == 0) // white
        {
            VictoryImageWhite.SetActive(true);
            VictoryTextWhite.SetActive(true);
        }
        else if (side == 1) // black
        {
            VictoryImageBlack.SetActive(true);
            VictoryTextBlack.SetActive(true);
        }
        else
        {
            VictoryImageDraw.SetActive(true);
            VictoryTextDraw.SetActive(true);
            //Debug.Log("draw");
        }
        
        VictoryCenter.SetActive(true);
    }


    public void ShowIfGmeNotRecentlyChecked()
    {
        Debug.Log("is saved recently "+ GameData.isSavedRecently);
        if (!GameData.isSavedRecently && !gameVictory)
        {
            ExitWithoutSaveWindow.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }

}
 
