using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class IniciarJuego : MonoBehaviour
{

    public GameObject cubo;
    public int N;

    public Material rojo, blanco;

    public Cubo[] cubos;

    //pos0 : dim x = 0; y = 1; z = 2;
    //pos1 : capa(dim)
    //pos2 : fila = 0; columna = 1;
    //pos3 : num de fila/columna(0 -> N-1)
    //pos4 : cubos de cada fila/columna
    public Cubo[,,,,] cubosFyL;
    public Cubo[,,,] cubosD;
    public Cubo[,] cubosMD;

    void Start()
    {
        //Google Play
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            // enables saving game progress.
            .EnableSavedGames()
            // require access to a player's Google+ social graph to sign in
            .RequireGooglePlus()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate((bool success) => {
            Debug.Log("LOLXD");
        });

        //Lo que viene siendo el juego xdlol
        cubos = new Cubo[N * N * N];
        cubosFyL = new Cubo[3, N, 2, N, N];
        cubosD = new Cubo[3, N, 2, N];
        cubosMD = new Cubo[4, N]; 

        transform.position = Vector3.one * (N - 1) / 2;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                for (int k = 0; k < N; k++)
                {
                    GameObject cuboInstanciado = Instantiate(cubo, new Vector3(i, j, k), Quaternion.identity) as GameObject;
                    cubos[i * N * N + j * N + k] = new Cubo(i, j, k, cuboInstanciado);

                    ////Filas y columnas
                    //sobre X
                    cubosFyL[0, i, 0, k, j] = cubos[i * N * N + j * N + k];
                    cubosFyL[0, i, 1, j, k] = cubos[i * N * N + j * N + k];
                    //sobre Y
                    cubosFyL[1, j, 0, i, k] = cubos[i * N * N + j * N + k];
                    cubosFyL[1, j, 1, k, i] = cubos[i * N * N + j * N + k];
                    //sobre Z
                    cubosFyL[2, k, 0, i, j] = cubos[i * N * N + j * N + k];
                    cubosFyL[2, k, 1, j, i] = cubos[i * N * N + j * N + k];

                    ////Diagonales Bidimensionales
                    //sobre X
                    if(cubos[i * N * N + j * N + k].z == cubos[i * N * N + j * N + k].y)
                    {
                        cubosD[0, i, 0, k] = cubos[i * N * N + j * N + k];
                    }
                    if (cubos[i * N * N + j * N + k].z == N - cubos[i * N * N + j * N + k].y - 1)
                    {
                        cubosD[0, i, 1, k] = cubos[i * N * N + j * N + k];
                    }
                    //sobre Y
                    if (cubos[i * N * N + j * N + k].x == cubos[i * N * N + j * N + k].z)
                    {
                        cubosD[1, j, 0, i] = cubos[i * N * N + j * N + k];
                    }
                    if (cubos[i * N * N + j * N + k].x == N - cubos[i * N * N + j * N + k].z - 1)
                    {
                        cubosD[1, j, 1, i] = cubos[i * N * N + j * N + k];
                    }
                    //sobre Z
                    if (cubos[i * N * N + j * N + k].x == cubos[i * N * N + j * N + k].y)
                    {
                        cubosD[2, k, 0, i] = cubos[i * N * N + j * N + k];
                    }
                    if (cubos[i * N * N + j * N + k].x == N - cubos[i * N * N + j * N + k].y - 1)
                    {
                        cubosD[2, k, 1, i] = cubos[i * N * N + j * N + k];
                    }

                    ////Diagonales Tridimensionales
                    if(cubos[i * N * N + j * N + k].z == cubos[i * N * N + j * N + k].x && cubos[i * N * N + j * N + k].z == cubos[i * N * N + j * N + k].y)
                    {
                        cubosMD[0, cubos[i * N * N + j * N + k].z] = cubos[i * N * N + j * N + k];
                    }
                    if (cubos[i * N * N + j * N + k].z == N -cubos[i * N * N + j * N + k].x - 1 && cubos[i * N * N + j * N + k].z == cubos[i * N * N + j * N + k].y)
                    {
                        cubosMD[1, cubos[i * N * N + j * N + k].z] = cubos[i * N * N + j * N + k];
                    }
                    if (cubos[i * N * N + j * N + k].z == cubos[i * N * N + j * N + k].x && cubos[i * N * N + j * N + k].z == N - cubos[i * N * N + j * N + k].y - 1)
                    {
                        cubosMD[2, cubos[i * N * N + j * N + k].z] = cubos[i * N * N + j * N + k];
                    }
                    if (cubos[i * N * N + j * N + k].z == N - cubos[i * N * N + j * N + k].x - 1 && cubos[i * N * N + j * N + k].z == N - cubos[i * N * N + j * N + k].y - 1)
                    {
                        cubosMD[3, cubos[i * N * N + j * N + k].z] = cubos[i * N * N + j * N + k];
                    }
                }
            }
        }

        StartCoroutine(prueba());
    }

    IEnumerator prueba()
    {
        //Ver filas y columnas
        /*
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
                            cubosFyL[d, l, rc, numrc, c].gameObject.GetComponent<MeshRenderer>().material = rojo;
                        }
                        yield return new WaitForSeconds(0.25f);

                        for (int c = 0; c < N; c++)
                        {
                            cubosFyL[d, l, rc, numrc, c].gameObject.GetComponent<MeshRenderer>().material = blanco;
                        }
                    }
                }
            }
        }
        */

        //Ver diagonales bidimensionales
        /*
        for (int d = 0; d < 3; d++)
        {
            for (int l = 0; l < N; l++)
            {
                for(int o = 0; o <= 1; o++)
                {
                    for(int c = 0; c < N; c++)
                    {
                        cubosD[d, l, o, c].gameObject.GetComponent<MeshRenderer>().material = rojo;
                    }
                    yield return new WaitForSeconds(0.75f);
                    for (int c = 0; c < N; c++)
                    {
                        cubosD[d, l, o, c].gameObject.GetComponent<MeshRenderer>().material = blanco;
                    }
                }
            }
        }
        */

        //Ver diagonales tridimensionales
        /*
        for(int i = 0; i < 4; i++)
        {
            for (int c = 0; c < N; c++)
            {
                cubosMD[i, c].gameObject.GetComponent<MeshRenderer>().material = rojo;
            }
            yield return new WaitForSeconds(2f);
            for (int c = 0; c < N; c++)
            {
                cubosMD[i, c].gameObject.GetComponent<MeshRenderer>().material = blanco;
            }
        }
        */
    yield return null;
    }
}
