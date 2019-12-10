using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int width = 10; // entero del eje X
    [SerializeField] private int height = 10; // entero del eje Y
    [SerializeField] private GameObject panel1; // GameObject del panel del primer jugador
    [SerializeField] private GameObject panel2; // GameObject del panel del segundo jugador
    [SerializeField] private Text timeP1; // El texto en el cual se mostrará el tiempo del jugador 1
    [SerializeField] private Text timeP2; // El texto en el cual se mostrará el tiempo del jugador 2
    [SerializeField] private float mainTimer1 = 30.00f; // El tiempo principal e inicial del jugador 1
    [SerializeField] private float mainTimer2 = 30.00f; // El tiempo principal e inicial del jugador 2
    private GameObject[,] grid; // Matríz de los GameObjects primitivos
    private bool isPlayerOneTurn = true; // Booleano que define el turno del jugador
    private bool gameRound = true; // Booleano que define si el juego ha finalizado o no
    private bool canCount = true; // Booleano auxiliar del temporizador
    private bool doOnce = false; // Otro booleano auxiliar del temporizador
    private float timeLeftP1 = 30.00f; // El tiempo del jugador 1
    private float timeLeftP2 = 30.00f; // el tiempo del jugador 2
    private int time1 = 0; // Maximizador de reducción del tiempo del jugador 1
    private int time2 = 0; // Maximizador de reducción del tiempo del jugador 2



    private void Setup() // Función de inicio
    {
        grid = new GameObject[width, height]; // Definición de la matríz
        for (int i = 0; i < width; i++) // Operación para crear primitivos en X
        {
            for (int j = 0; j < height; j++) // Operación para crear primitivos en Y
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube); // Creación de GameObjects primitivos
                go.transform.position = new Vector3(j * 1.5f, i * 1.5f, 0); // La repartición y separación de los GameObjects
                grid[j, i] = go;
            }
        }
    }
    private void PickAPiece(int x, int y)
    {
        if (grid[x,y].GetComponent<Renderer>().material.color == Color.red)
        {
            grid[x, y].GetComponent<Renderer>().material.color = Color.red;
        }
    }
    private void SelectColor(int x, int y)
    {
        if (grid[x,y].GetComponent<Renderer>().material.color == Color.red)
        {
            Color assignedColor = Color.clear;
            if(isPlayerOneTurn)
            {
                assignedColor = Color.green;
            }
            else
            {
                assignedColor = Color.blue;
            }

            grid[x, y].GetComponent<Renderer>().material.color = assignedColor;
            isPlayerOneTurn = !isPlayerOneTurn;
            CheckHorizontal(x,y,assignedColor,assignedColor);
            CheckVertical(x,y,assignedColor,assignedColor);
            CheckPlusDiagonal(x,y,assignedColor,assignedColor);
            CheckMinusDiagonal(x,y,assignedColor,assignedColor);
        }
    }
    private void RestoreColor(int x,int y)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[j,i].GetComponent<Renderer>().material.color == Color.red)
                {
                    grid[j, i].GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    }
    private void CheckColor(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (!(grid[x, y].GetComponent<Renderer>().material.color == Color.green) && !(grid[x, y].GetComponent<Renderer>().material.color == Color.blue) && gameRound)
            {
                GameObject go = grid[x, y];
                go.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
    private void Panels()
    {
        if (gameRound)
        {
            if (isPlayerOneTurn)
            {
                panel1.GetComponent<Image>().material.color = Color.green;
            }
            else
            {
                panel2.GetComponent<Image>().material.color = Color.blue;
            }
        }
    }
    void Start()
    {
        Setup();
        Time.timeScale = 1;
        if(time1 >= 1)
        {
            timeLeftP1 = mainTimer1 - 5 * time1;
            if (mainTimer1 <= 0) mainTimer1 = 1.0f;
        }
        else
        {
            timeLeftP1 = mainTimer1;
        }
        
        if (time2 >= 1)
        {
            timeLeftP2 = mainTimer2 - 5 * time2;
            if (mainTimer2 <= 0) mainTimer2 = 1.0f;
        }
        else
        {
            timeLeftP2 = mainTimer2;
        }
    }

    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var x = (int)(mousePosition.x / 1.5f + 0.5f);
        var y = (int)(mousePosition.y / 1.5f + 0.5f);
        
        if (!(mousePosition.x >= 0) && !(mousePosition.x < width) && !(mousePosition.y >= 0) && !(mousePosition.y < height)) return;
        RestoreColor(x,y);
        CheckColor(x,y);
        Panels();

        if(gameRound)
        {
            if (isPlayerOneTurn)
            {
                if(timeLeftP1 >= 0.0f && canCount)
                {
                    timeLeftP1 -= Time.deltaTime;
                    timeP1.text = timeLeftP1.ToString("F");
                }
                else if (timeLeftP1 <= 0.0f && !doOnce)
                {
                    canCount = false;
                    doOnce = true;
                    timeP1.text = "0.00";
                    timeLeftP1 = 0.0f;
                }
                if(timeLeftP1 == 0.0f) 
                {
                    if(time1 == 6)
                    {
                        mainTimer1 = 1.0f;
                    }
                    else
                    {
                        time1++;
                        mainTimer1 -= 5.0f;
                    }

                    isPlayerOneTurn = false; 
                    timeLeftP1 = mainTimer1;
                    canCount = true;
                    doOnce = false;                  
                }
            }
            else
            {
                if(timeLeftP2 >= 0.0f && canCount)
                {
                    timeLeftP2 -= Time.deltaTime;
                    timeP2.text = timeLeftP2.ToString("F");
                }
                else if (timeLeftP2 <= 0.0f && !doOnce)
                {
                    canCount = false;
                    doOnce = true;
                    timeP2.text = "0.00";
                    timeLeftP2 = 0.0f;
                }
                if(timeLeftP2 == 0.0f) 
                {
                    if(time2 == 6)
                    {
                        mainTimer2 = 1.0f;
                    }
                    else
                    {
                        time2++;
                        mainTimer2 -= 5.0f;
                    }

                    isPlayerOneTurn = true;
                    timeLeftP2 = mainTimer2;
                    canCount = true;
                    doOnce = false;
                }
            }
        } 

        if (Input.GetMouseButtonDown(0))
        {
            SelectColor(x,y);
        }
    }
    private bool CheckHorizontal(int x, int y, Color colorToCompare, Color assignedColor)
    {
        var counter = 0;
        for (int i = x-3; i <= x+3; i++)
        {
            if (i >= 0 && i < width)
            {
                if (grid[i, y].GetComponent<Renderer>().material.color == colorToCompare)
                {
                    counter++;
                    if (counter >= 4)
                    {
                        if (assignedColor == Color.blue)
                        {
                            print("El jugador 2 es el ganador");
                        }
                        else
                        {
                            print("El jugador 1 es el ganador");
                        }
                        gameRound = false;
                        return true;
                    }
                }
                else
                {
                    counter = 0;
                }
                
            }
        }
        return false;
    }
    private bool CheckVertical(int x, int y, Color colorToCompare, Color assignedColor)
    {
        var counter = 0;
        for (int j = y-3; j <= y+3; j++)
        {
            if (j >= 0 && j < height)
            {
                if (grid[x,j].GetComponent<Renderer>().material.color == colorToCompare)
                {
                    counter++;
                    if (counter >= 4)
                    {
                        if (assignedColor == Color.blue)
                        {
                            print("El jugador 2 es el ganador");
                        }
                        else
                        {
                            print("El jugador 1 es el ganador");
                        }
                        gameRound = false;
                        return true;
                    }
                }
                else
                {
                    counter = 0;
                }
            }
        }
        return false;
    }

    private bool CheckPlusDiagonal(int x, int y, Color comparedColor, Color assignedColor)
    {
        var counter = 0;
        var j = y - 4;

        for (int i = x - 3; i <= x + 3; i++)
        {
            j++;
            if (j >= 0 && j < height && i >= 0 && i < width)
            {
                if (grid[i,j].GetComponent<Renderer>().material.color == comparedColor)
                {
                    counter++;

                    if (counter >= 4)
                    {
                        if (assignedColor == Color.blue)
                        {
                            print("El jugador 2 es el ganador");
                        }
                        else
                        {
                            print("El jugador 1 es el ganador");
                        }
                        gameRound = false;
                        return true;                       
                    }
                }
                else
                {
                    counter = 0;
                }
            }
        }
        return false;
    }
    
    private bool CheckMinusDiagonal(int x, int y, Color comparedColor, Color assignedColor)
    {
        var counter = 0;
        var j = y + 4;

        for (int i = x - 3; i <= x + 3; i++)
        {
            j--;

            if (j < 0 || j >= height || i < 0 || i >= width)
            {
                continue;
            }
            if (grid[i, j].GetComponent<Renderer>().material.color == comparedColor)
            {
                counter++;
                if (counter >= 4)
                {
                    if (assignedColor == Color.blue) 
                    {
                        print("El jugador 2 es el ganador");
                    }
                    else
                    {
                        print("El jugador 1 es el ganador");
                    }

                    gameRound = false;
                    return true;
                }
            }
            else
            {
                counter = 0;
            }
        }
        return false;
    }
}
