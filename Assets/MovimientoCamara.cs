using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovimientoCamara : MonoBehaviour {

	void Start ()
    {
        winText.enabled = false;
        panel.enabled = false;
        replay.gameObject.SetActive(false);
        menu.gameObject.SetActive(false);

        Input.simulateMouseWithTouches = true;
        canClick = true;
        StartCoroutine(checkMouse());
	}

    public int N;
    public Vector3 rotation;
    public float sens;

    public bool android;

    public IniciarJuego iniciarJuego;

    public Material rojo, amarillo, azul, blanco;

    public bool canClick;

    RaycastHit hit;

    public bool seleccionado;

    //0 : rojo
    //1 : azul
    bool turno = false;

    public Cubo[] sel1, sel2;

    public Sprite fondoRojo, fondoAzul;
    public UnityEngine.UI.Image panel, replay, menu;

    Cubo interseccion;

    Vector3 lastCPos, lastCNormal;

    void SeleccionarPos()
    {
        for (int i = 0; i < N; i++)
        {
            if (sel1[i].color == Cubo.CubeColor.amarillo)
            {
                sel1[i].gameObject.GetComponent<MeshRenderer>().material = blanco;
                sel1[i].color = Cubo.CubeColor.blanco;
            }
            if (sel1[i].color == Cubo.CubeColor.rojo)
            {
                sel1[i].gameObject.GetComponent<MeshRenderer>().material = rojo;
            }
            if (sel1[i].color == Cubo.CubeColor.azul)
            {
                sel1[i].gameObject.GetComponent<MeshRenderer>().material = azul;
            }
        }
        for (int j = 0; j < N; j++)
        {
            if (sel2[j].color == Cubo.CubeColor.amarillo)
            {
                sel2[j].gameObject.GetComponent<MeshRenderer>().material = blanco;
                sel2[j].color = Cubo.CubeColor.blanco;
            }
            if (sel2[j].color == Cubo.CubeColor.rojo)
            {
                sel2[j].gameObject.GetComponent<MeshRenderer>().material = rojo;
            }
            if (sel2[j].color == Cubo.CubeColor.azul)
            {
                sel2[j].gameObject.GetComponent<MeshRenderer>().material = azul;
            }
        }
        if (turno)
        {
            interseccion.color = Cubo.CubeColor.azul;
            interseccion.gameObject.GetComponent<MeshRenderer>().material = azul;
        }
        else
        {
            interseccion.color = Cubo.CubeColor.rojo;
            interseccion.gameObject.GetComponent<MeshRenderer>().material = rojo;
        }

        seleccionado = false;

        canClick = true;

        turno = !turno;

        ComprobarPartida();
    }

    Cubo.CubeColor colorAComprobar;
    bool iguales;
    void ComprobarPartida()
    {
        if (!turno)
            colorAComprobar = Cubo.CubeColor.azul;
        else
            colorAComprobar = Cubo.CubeColor.rojo;

        //Comprobar filas y columnas
        for (int d = 0; d < 3; d++)
        {
            for (int l = 0; l < N; l++)
            {
                for (int rc = 0; rc <= 1; rc++)
                {
                    for (int numrc = 0; numrc < N; numrc++)
                    {
                        for (int c = 0; c < N; c++)
                        {
                            if (iniciarJuego.cubosFyL[d, l, rc, numrc, c].color == colorAComprobar)
                            {
                                iguales = true;
                            }
                            else
                            {
                                iguales = false;
                                break;
                            }
                        }
                        if (iguales)
                        {
                            FinDelJuego();
                            return;
                        }
                    }
                }
            }
        }
        

        //Comprobar diagonales bidimensionales
        for (int d = 0; d < 3; d++)
        {
            for (int l = 0; l < N; l++)
            {
                for(int o = 0; o <= 1; o++)
                {
                    for (int c = 0; c < N; c++)
                    {
                        if (iniciarJuego.cubosD[d, l, o, c].color == colorAComprobar)
                        {
                            iguales = true;
                        }
                        else
                        {
                            iguales = false;
                            break;
                        }
                    }
                    if (iguales)
                    {
                        FinDelJuego();
                        return;
                    }
                }
            }
        }
        

        //Comprobar diagonales tridimensionales
        for (int i = 0; i < 4; i++)
        {
            for (int c = 0; c < N; c++)
            {
                if (iniciarJuego.cubosMD[i, c].color == colorAComprobar)
                {
                    iguales = true;
                }
                else
                {
                    iguales = false;
                    break;
                }
            }
            if (iguales)
            {
                FinDelJuego();
                return;
            }
        }
    }
    public UnityEngine.UI.Text winText;
    bool fgame;
    void FinDelJuego()
    {
        panel.enabled = true;
        replay.gameObject.SetActive(true);
        menu.gameObject.SetActive(true);
        fgame = true;
        winText.enabled = true;

        if (!turno)
        {
            winText.text = "BLUE WINS";
            winText.color = Color.blue;
            replay.color = Color.blue;
            menu.color = Color.blue;
            panel.sprite = fondoAzul;

            foreach(Cubo cubo in iniciarJuego.cubos)
            {
                cubo.color = Cubo.CubeColor.azul;
                cubo.gameObject.GetComponent<MeshRenderer>().material = azul;
            }
        }
        else
        {
            winText.text = "RED WINS";
            winText.color = Color.red;
            replay.color = Color.red;
            menu.color = Color.red;
            panel.sprite = fondoRojo;

            foreach (Cubo cubo in iniciarJuego.cubos)
            {
                cubo.color = Cubo.CubeColor.rojo;
                cubo.gameObject.GetComponent<MeshRenderer>().material = rojo;
            }
        }

        StartCoroutine(DesaparecerBloques());
    }

    IEnumerator DesaparecerBloques()
    {
        float scale = 1;
        float t = 0;

        while(scale > 0)
        {
            scale = Mathf.Lerp(1, 0, t);

            t += Time.deltaTime;

            panel.color = new Vector4(panel.color.r, panel.color.g, panel.color.b, 1 - scale);
            winText.color = new Vector4(winText.color.r, winText.color.g, winText.color.b, 1 - scale);
            replay.color = new Vector4(winText.color.r, winText.color.g, winText.color.b, 1 - scale);
            menu.color = new Vector4(winText.color.r, winText.color.g, winText.color.b, 1 - scale);

            foreach (Cubo cubo in iniciarJuego.cubos)
            {
                cubo.gameObject.transform.localScale = Vector3.one * scale;
            }

            yield return null;
        }
    }

    void ComprobarSeleccion()
    {
        if (seleccionado)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (sel1[i].x == sel2[j].x && sel1[i].y == sel2[j].y && sel1[i].z == sel2[j].z)
                    {
                        interseccion = sel1[i];
                    }
                }
            }

            if (interseccion != null && interseccion.color == Cubo.CubeColor.amarillo)
            {
                Invoke("SeleccionarPos", 1f);
            }
            else
            {
                if (interseccion.color == Cubo.CubeColor.rojo)
                {
                    interseccion.gameObject.GetComponent<MeshRenderer>().material = rojo;
                }
                if (interseccion.color == Cubo.CubeColor.azul)
                {
                    interseccion.gameObject.GetComponent<MeshRenderer>().material = azul;
                }

                for (int i = 0; i < N; i++)
                {
                    if (sel1[i].color == Cubo.CubeColor.amarillo)
                    {
                        sel1[i].gameObject.GetComponent<MeshRenderer>().material = blanco;
                        sel1[i].color = Cubo.CubeColor.blanco;
                    }
                    if (sel1[i].color == Cubo.CubeColor.rojo)
                    {
                        sel1[i].gameObject.GetComponent<MeshRenderer>().material = rojo;
                    }
                    if (sel1[i].color == Cubo.CubeColor.azul)
                    {
                        sel1[i].gameObject.GetComponent<MeshRenderer>().material = azul;
                    }
                }
                for (int j = 0; j < N; j++)
                {
                    if (sel2[j].color == Cubo.CubeColor.amarillo)
                    {
                        sel2[j].gameObject.GetComponent<MeshRenderer>().material = blanco;
                        sel2[j].color = Cubo.CubeColor.blanco;
                    }
                    if (sel2[j].color == Cubo.CubeColor.rojo)
                    {
                        sel2[j].gameObject.GetComponent<MeshRenderer>().material = rojo;
                    }
                    if (sel2[j].color == Cubo.CubeColor.azul)
                    {
                        sel2[j].gameObject.GetComponent<MeshRenderer>().material = azul;
                    }
                }
                seleccionado = false;
                canClick = true;
            }
        }
        else { seleccionado = true; canClick = true; }
    }

    void CheckPos()
    {
        if (hit.normal.x != 0)
                            {
            int juisio = 0;
            foreach (Cubo cubo in iniciarJuego.cubos)
            {
                if (cubo.y == hit.transform.position.y && cubo.z == hit.transform.position.z)
                {
                    cubo.gameObject.GetComponent<MeshRenderer>().material = amarillo;
                    if (cubo.color == Cubo.CubeColor.blanco)
                    {
                        //cubo.gameObject.GetComponent<MeshRenderer>().material = amarillo;
                        cubo.color = Cubo.CubeColor.amarillo;
                    }

                    if (seleccionado)
                        sel2[juisio] = cubo;
                    else
                        sel1[juisio] = cubo;

                    juisio++;
                }
            }
            canClick = false;
            ComprobarSeleccion();
        }
        if (hit.normal.y != 0)
        {
            int juisio = 0;
            foreach (Cubo cubo in iniciarJuego.cubos)
            {
                if (cubo.x == hit.transform.position.x && cubo.z == hit.transform.position.z)
                {
                    cubo.gameObject.GetComponent<MeshRenderer>().material = amarillo;
                    if (cubo.color == Cubo.CubeColor.blanco)
                    {
                        //cubo.gameObject.GetComponent<MeshRenderer>().material = amarillo;
                        cubo.color = Cubo.CubeColor.amarillo;
                    }

                    if (seleccionado)
                        sel2[juisio] = cubo;
                    else
                        sel1[juisio] = cubo;

                    juisio++;
                }
            }
            canClick = false;
            ComprobarSeleccion();
        }
        if (hit.normal.z != 0)
        {
            int juisio = 0;
            foreach (Cubo cubo in iniciarJuego.cubos)
            {
                if (cubo.x == hit.transform.position.x && cubo.y == hit.transform.position.y)
                {
                    cubo.gameObject.GetComponent<MeshRenderer>().material = amarillo;
                    if (cubo.color == Cubo.CubeColor.blanco)
                    {
                        //cubo.gameObject.GetComponent<MeshRenderer>().material = amarillo;
                        cubo.color = Cubo.CubeColor.amarillo;
                    }

                    if (seleccionado)
                        sel2[juisio] = cubo;
                    else
                        sel1[juisio] = cubo;

                    juisio++;
                }
            }
            canClick = false;
            ComprobarSeleccion();
        }
    }

    IEnumerator checkMouse()
    {
        N = iniciarJuego.N;
        sel1 = new Cubo[N];
        sel2 = new Cubo[N];

        for (int i = 0; i < N; i++)
        {
            sel1[i] = new Cubo(0, 0, 0, null);
        }
        for (int j = 0; j < N; j++)
        {
            sel2[j] = new Cubo(0, 0, 0, null);
        }

        Vector3 lastPos = Vector3.zero;
        Vector2 lastPosA = Vector2.zero;

        while (!fgame)
        {
            if (android)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.Equals(Input.touches[0]))
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            lastPosA = touch.position;

                            if (canClick)
                            {
                                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
                                {
                                    if (!seleccionado)
                                    {
                                        lastCNormal = hit.normal;
                                        lastCPos = hit.transform.position;


                                        CheckPos();
                                    }
                                    else
                                    {
                                        if (lastCNormal != hit.normal || lastCPos != hit.transform.position)
                                        {
                                            CheckPos();
                                        }
                                    }
                                }
                            }
                        }
                        if (touch.phase == TouchPhase.Moved)
                        {
                            if (touch.position - lastPosA != Vector2.zero)
                            {
                                rotation = new Vector3(-(touch.position - lastPosA).y, (touch.position - lastPosA).x, 0f);
                                transform.Rotate(rotation * sens);
                            }
                            lastPosA = touch.position;
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && canClick)
                {
                    lastPos = Input.mousePosition;
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                    {
                        if (!seleccionado)
                        {
                            lastCNormal = hit.normal;
                            lastCPos = hit.transform.position;


                            CheckPos();
                        }
                        else
                        {
                            if (lastCNormal != hit.normal || lastCPos != hit.transform.position)
                            {
                                CheckPos();
                            }
                        }
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    if (Input.mousePosition - lastPos != Vector3.zero)
                    {
                        rotation = new Vector3(-(Input.mousePosition - lastPos).y, (Input.mousePosition - lastPos).x, 0f);
                        transform.Rotate(rotation * sens);
                    }
                }
                lastPos = Input.mousePosition;
            }
            yield return null;
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(0);
    }
}
