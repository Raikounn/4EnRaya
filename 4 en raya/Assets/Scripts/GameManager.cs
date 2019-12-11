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



    private void SetUp() // Función de montaje del tablero
    {
        grid = new GameObject[width, height]; // Definición de la matríz
        for (int i = 0; i < width; i++) // Operación para crear primitivos en X
        {
            for (int j = 0; j < height; j++) // Operación para crear primitivos en Y
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube); // Creación de GameObjects primitivos
                go.transform.position = new Vector3(j * 1.5f, i * 1.5f, 0); // La repartición y separación de los GameObjects
                grid[j, i] = go; // Se define 'go' como variable contenedora del array o matríz multidimencional 'grid'
            }
        }
    }
    private void SelectColor(int x, int y) // Función de selección de color según el turno
    {
        if (grid[x,y].GetComponent<Renderer>().material.color == Color.red) // Se pregunta si en el grid hay algun GameObject de color rojo
        {
            Color assignedColor = Color.clear; // Se crea una variable contenedora vacía y se establece limpia para cada turno
            if(isPlayerOneTurn) // Se pregunta si el turno del primer jugador es true 
            {
                assignedColor = Color.green; // Se usa la variable vacía para contener el color del primer jugador
            }
            else // Si el turno del jugador 1 es false entonces se lleva a cabo la linea de abajo
            {
                assignedColor = Color.blue; // La variable vacía contiene el color del segundo jugador
            }

            grid[x, y].GetComponent<Renderer>().material.color = assignedColor; // Se aplica el color asignado en el lugar que cumple los requisitos
            isPlayerOneTurn = !isPlayerOneTurn; // Se cambia el valor de 'isPlayerTurn' de false a true y viceversa

            CheckHorizontal(x,y,assignedColor,assignedColor);           // Se llaman los verificadores: horizontal, vertical, diagonal positivo y diagonal negativo
            CheckVertical(x,y,assignedColor,assignedColor);             // que en caso de cumplir los requisitos que poseen, se determinará un ganador basado
            CheckPlusDiagonal(x,y,assignedColor,assignedColor);         // en el color del assignedColor y en los datos que todos los verificadores
            CheckMinusDiagonal(x,y,assignedColor,assignedColor);        // contienen.
        }
    }
    private void RestoreColor(int x,int y) // Función para restaurar el grid o tablero al pasar el cursor
    {
        for (int i = 0; i < width; i++) // Operación que ejecuta la "limpieza" en X
        {
            for (int j = 0; j < height; j++) // Operacion que ejecuta la "limpieza" en Y
            {
                if (grid[j,i].GetComponent<Renderer>().material.color == Color.red) // Se pregunta si el GameObject es de color rojo
                {
                    grid[j, i].GetComponent<Renderer>().material.color = Color.white; // se pinta de color blanco todos los GameObjects que sean rojos
                }
            }
        }
    }
    private void CheckColor(int x, int y) // Función para evitar borrar las jugadas de los jugadores
    {
        if (x >= 0 && y >= 0 && x < width && y < height) // Se comprueba si ocurre dentro de los parámetros dados
        {
            // Pregunta si los GameObjects no son verdes ni a los azules
            if (!(grid[x, y].GetComponent<Renderer>().material.color == Color.green) && !(grid[x, y].GetComponent<Renderer>().material.color == Color.blue) && gameRound)
            {
                GameObject go = grid[x, y]; // Define el grid en la variable GameObject go
                go.GetComponent<Renderer>().material.color = Color.red; // Si lo anterior se cumple, el grid puede contener el color rojo
            }
        }
    }
    private void Panels() //Función específica para los paneles del canvas
    {
        if (gameRound) // Si el juego está activado (true) se continua con su contenido
        {
            if (isPlayerOneTurn) // Se comprueba si es el turno del jugador 1
            {
                panel1.GetComponent<Image>().material.color = Color.green; // El panel cambia a color verde
            }
            else // Sino
            {
                panel2.GetComponent<Image>().material.color = Color.blue; // El panel cambia a color azul
            }
        }
    }
    private void Timer() // Función del temporizador por jugador
    {
        if (gameRound) // Si el juego está activo se ejecuta el contenido
        {
            if (isPlayerOneTurn) // si el turno es del jugador 1 se ejecuta el contenido
            {
                if (timeLeftP1 >= 0.0f && canCount) // Si el tiempo es mayor o igual a 0.0f y canCount es true se ejecuta el contenido
                {
                    timeLeftP1 -= Time.deltaTime; // Se inicia el temporizador
                    timeP1.text = timeLeftP1.ToString("F"); // Se muestra el temporizador en el panel
                }
                else if (timeLeftP1 <= 0.0f && !doOnce) // Sino, si el tiempo es menor o igual a 0.0f y doOnce es false se ejecuta el contenido
                {
                    canCount = false; // canCount se vuelve false
                    doOnce = true; // doOnce se vuelve true
                    timeP1.text = "0.00"; // Se muestra el final del temporizador como 0.00
                    timeLeftP1 = 0.0f; // El tiempo queda en 0.0f
                }
                if (timeLeftP1 == 0.0f) // Si el tiempo es igual a 0.0f se ejecuta lo que sigue
                {
                    if (time1 == 6) // Si time1 es igual a 6 se ejecuta lo siguiente
                    {
                        mainTimer1 = 1.0f; // El tiempo inicial queda en 1.0f segundos
                    }
                    else // si no es igual a 6
                    {
                        time1++; // time1 suma uno cada iteración
                        mainTimer1 -= 5.0f; // Se reduce el tiempor inicial en -5.0f segundos
                    }

                    isPlayerOneTurn = false; // El turno del jugador 1 se vuelve false
                    timeLeftP1 = mainTimer1; // Se reinicia el temporizador
                    canCount = true; // canCount se vuelve true
                    doOnce = false; // doOnce se vuelve false
                }
            }
            else // Sino es turno del jugador 1, se realiza todo lo anterior descrito pero para el temporizador del jugador 2
            {
                if (timeLeftP2 >= 0.0f && canCount)
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
                if (timeLeftP2 == 0.0f)
                {
                    if (time2 == 6)
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
    }
    void Start() // Función de Inicio
    {
        SetUp(); // Llamada de la función Setup
        Time.timeScale = 1; // Se define y comprueba la escala de tiempo
        if(time1 >= 1) // Si el tiempo 1 es mayor o igual a 1
        {
            timeLeftP1 = mainTimer1 - 5 * time1; // Se establece el funcionamiento del temporizador de jugador 1
            if (mainTimer1 <= 0) mainTimer1 = 1.0f; // Si el tiempo es menor o igual a 0 se convierte en 1
        }
        else // Sino
        {
            timeLeftP1 = mainTimer1; // El tiempo es el establecido previamente
        }
        
        if (time2 >= 1) // Lo mismo de arriba pero para el jugador 2
        {
            timeLeftP2 = mainTimer2 - 5 * time2;
            if (mainTimer2 <= 0) mainTimer2 = 1.0f;
        }
        else
        {
            timeLeftP2 = mainTimer2;
        }
    }

    void Update() //Función de actualización
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Comprueba el lugar del cursor en la pantalla
        var x = (int)(mousePosition.x / 1.5f + 0.5f); // Variable X de la posición del mouse
        var y = (int)(mousePosition.y / 1.5f + 0.5f); // Variable Y de la posición del mouse
        
        // Si nada de lo descrito en esta linea ocurre, no pasa nada
        if (!(mousePosition.x >= 0) && !(mousePosition.x < width) && !(mousePosition.y >= 0) && !(mousePosition.y < height)) return;
        
        // Se llaman las funciones
        RestoreColor(x,y);
        CheckColor(x,y);
        Panels();
        Timer();



        if (Input.GetMouseButtonDown(0)) // Si se pulsa el boton izquierdo del mouse se ejecuta
        {
            SelectColor(x,y); // Se llama la función SelectColor
        }
    }
    private bool CheckHorizontal(int x, int y, Color colorToCompare, Color assignedColor) // Función de verificador horizontal
    {
        var counter = 0; // Se crea la variable counter y se establece su valor a 0
        for (int i = x-3; i <= x+3; i++) // Se hace una comprobación en horizontal de los colores de los GameObjects
        {
            if (i >= 0 && i < width) // Si esto se cumple se ejecuta lo que sigue
            {
                if (grid[i, y].GetComponent<Renderer>().material.color == colorToCompare) // Si el color es igual al color a comparar se ejecuta lo que sigue
                {
                    counter++; // El counter aumenta en 1 cada vez que ocurre la iteración
                    if (counter >= 4) // Si el counter es mayor o igual a 4 se ejecuta lo que sigue
                    {
                        // Si el color asignado es azul se imprime al jugador 2 como ganador, de lo contrario se imprime al jugador 1 como ganador
                        if (assignedColor == Color.blue) 
                        {
                            print("El jugador 2 es el ganador");
                        }
                        else
                        {
                            print("El jugador 1 es el ganador");
                        }
                        gameRound = false; // De tener 4 o más en el contador, se finaliza la partida
                        return true; // Si el counter llega a 4 o más, la función retorna como verdadero (true)
                    }
                }
                else
                {
                    counter = 0; // Si el colorToCompare es distinto al assignedColor, el counter regresa a 0
                }
                
            }
        }
        return false; // Si nada de lo establecido se cumple, la función retorna como falso (false)
    }

    // Tanto CheckVertical como CheckPlusDiagonal y CheckMinusDiagonal son descripciones idénticas a CheckHorizontal
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
