using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.Video;

public class GeometryDashScript : MonoBehaviour
{

    public KMAudio audio;
    public KMBombInfo bomb;
    public AudioSource aud;
    public AudioClip[] audios;

    public KMSelectable[] buttons;

    public VideoPlayer player;
    public VideoClip[] internalClips;

    //start menu things
    public GameObject button;
    public GameObject background;
    public GameObject icon1;
    public GameObject icon2;
    public GameObject[] startTexts;
    public Material[] startBacks;

    //end menu things
    public GameObject[] endTexts;
    public Material endBack;
    public GameObject[] icons;
    public GameObject[] endButtons;
    public Sprite[] cubes;

    //lvl stats
    private string lvlname;
    private List<string> creators = new List<string>();
    private string verifier;
    private string difficulty;
    private string songname;
    private string songmaker;
    private bool coin;
    private bool mirror;
    private bool speed;
    private bool teleport;
    private bool transformC;
    private bool orbs;
    private bool pads;

    private bool started = false;
    private bool animating = false;
    private bool reset = false;
    private bool useInternal = false;
    private bool loading = true;

    private int correctBut;
    private int correctCubeIndex;
    private int startingNum;
    private int newNum;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        moduleSolved = false;
        foreach (KMSelectable obj in buttons)
        {
            KMSelectable pressed = obj;
            pressed.OnInteract += delegate () { PressButton(pressed); return false; };
        }
        if (Application.isEditor)
        {
            useInternal = true;
            loading = false;
        }
    }

    // Please not that this module cannot be tested in Unity! Please build it locally and run it in game to test it!
    private IEnumerator WaitForVideoClips()
    {
        yield return new WaitUntil(() => VideoLoader.clips != null);

        pickVideoAndStats();
        getNumber();
        randomizeButtons();
        loading = false;
    }

    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            endButtons[i].SetActive(false);
            icons[i].SetActive(false);
        }
        if (!useInternal)
            StartCoroutine(WaitForVideoClips());
        else
        {
            pickVideoAndStats();
            getNumber();
            randomizeButtons();
        }
    }

    void PressButton(KMSelectable pressed)
    {
        if (moduleSolved != true && loading != true)
        {
            pressed.AddInteractionPunch(0.25f);
            if (pressed == buttons[0])
            {
                button.SetActive(false);
                icon1.SetActive(false);
                icon2.SetActive(false);
                for (int i = 0; i < 3; i++)
                {
                    startTexts[i].SetActive(false);
                }
                StartCoroutine(startDelayPlay());
            }
            else if (pressed == buttons[1])
            {
                for (int i = 0; i < 2; i++)
                {
                    endTexts[i].SetActive(false);
                }
                for (int i = 0; i < 6; i++)
                {
                    endButtons[i].SetActive(false);
                    icons[i].SetActive(false);
                }
                StartCoroutine(startDelayPlay());
            }
            else if (pressed == buttons[2])
            {
                reset = true;
                audio.PlaySoundAtTransform("explode_11", transform);
                Debug.LogFormat("[Geometry Dash #{0}] The module has been reset!", moduleId);
                Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
                for (int i = 0; i < 2; i++)
                {
                    endTexts[i].SetActive(false);
                }
                started = false;
                creators.Clear();
                Start();
                button.SetActive(true);
                icon1.SetActive(true);
                icon2.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    startTexts[i].SetActive(true);
                }
            }
            else if (pressed == buttons[3])
            {
                if(correctBut == 0)
                {
                    Debug.LogFormat("[Geometry Dash #{0}] That cube was correct! Module Disarmed!", moduleId);
                    StartCoroutine(end());
                }
                else
                {
                    Debug.LogFormat("[Geometry Dash #{0}] That cube was incorrect! Strike!", moduleId);
                    GetComponent<KMBombModule>().HandleStrike();
                }
            }
            else if (pressed == buttons[4])
            {
                if (correctBut == 1)
                {
                    Debug.LogFormat("[Geometry Dash #{0}] That cube was correct! Module Disarmed!", moduleId);
                    StartCoroutine(end());
                }
                else
                {
                    Debug.LogFormat("[Geometry Dash #{0}] That cube was incorrect! Strike!", moduleId);
                    GetComponent<KMBombModule>().HandleStrike();
                }
            }
            else if (pressed == buttons[5])
            {
                if (correctBut == 2)
                {
                    Debug.LogFormat("[Geometry Dash #{0}] That cube was correct! Module Disarmed!", moduleId);
                    StartCoroutine(end());
                }
                else
                {
                    Debug.LogFormat("[Geometry Dash #{0}] That cube was incorrect! Strike!", moduleId);
                    GetComponent<KMBombModule>().HandleStrike();
                }
            }
            else if (pressed == buttons[6])
            {
                if (correctBut == 3)
                {
                    Debug.LogFormat("[Geometry Dash #{0}] That cube was correct! Module Disarmed!", moduleId);
                    StartCoroutine(end());
                }
                else
                {
                    Debug.LogFormat("[Geometry Dash #{0}] That cube was incorrect! Strike!", moduleId);
                    GetComponent<KMBombModule>().HandleStrike();
                }
            }
        }
    }

    private void pickVideoAndStats()
    {
        int rando = Random.Range(0, useInternal ? internalClips.Length : VideoLoader.clips.Length);
        if (rando > -1 && rando < 4)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Valor", moduleId);
            lvlname = "Valor";
            creators.Add("Inversion");
            creators.Add("MrSleepy");
            creators.Add("TheDevon");
            creators.Add("Mosertron");
            creators.Add("Megaman9");
            creators.Add("Pizzafire");
            creators.Add("Dysfunctional Popo");
            creators.Add("Benjamaster7");
            creators.Add("KrmaL");
            creators.Add("Uzendayo");
            creators.Add("Viceroy");
            creators.Add("Alter");
            creators.Add("Hexhammer");
            creators.Add("jdfr03");
            verifier = "KrmaL";
            difficulty = "Insane Demon";
            songname = "Vigor";
            songmaker = "AeronMusic";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 0)
            {
                player.clip = useInternal ? internalClips[0] : VideoLoader.clips[0];
                aud.clip = audios[0];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = true;
            }
            else if (rando == 1)
            {
                player.clip = useInternal ? internalClips[1] : VideoLoader.clips[1];
                aud.clip = audios[1];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 2)
            {
                player.clip = useInternal ? internalClips[2] : VideoLoader.clips[2];
                aud.clip = audios[2];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 3)
            {
                player.clip = useInternal ? internalClips[3] : VideoLoader.clips[3];
                aud.clip = audios[3];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
        }
        else if (rando > 3 && rando < 8)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Speed Racer", moduleId);
            lvlname = "Speed Racer";
            creators.Add("ZenthicAlpha");
            verifier = "ZenthicAlpha";
            difficulty = "Easy Demon";
            songname = "Chaoz Impact";
            songmaker = "ParagonX9";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 4)
            {
                player.clip = useInternal ? internalClips[4] : VideoLoader.clips[4];
                aud.clip = audios[4];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
            else if (rando == 5)
            {
                player.clip = useInternal ? internalClips[5] : VideoLoader.clips[5];
                aud.clip = audios[5];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
            else if (rando == 6)
            {
                player.clip = useInternal ? internalClips[6] : VideoLoader.clips[6];
                aud.clip = audios[6];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = false;
            }
            else if (rando == 7)
            {
                player.clip = useInternal ? internalClips[7] : VideoLoader.clips[7];
                aud.clip = audios[7];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
        }
        else if (rando > 7 && rando < 11)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Dry Out", moduleId);
            lvlname = "Dry Out";
            creators.Add("RobTop");
            verifier = "RobTop";
            difficulty = "Normal";
            songname = "Dry Out";
            songmaker = "DJVI";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 8)
            {
                player.clip = useInternal ? internalClips[8] : VideoLoader.clips[8];
                aud.clip = audios[8];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = false;
            }
            else if (rando == 9)
            {
                player.clip = useInternal ? internalClips[9] : VideoLoader.clips[9];
                aud.clip = audios[9];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
            else if (rando == 10)
            {
                player.clip = useInternal ? internalClips[10] : VideoLoader.clips[10];
                aud.clip = audios[10];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
        }
        else if (rando > 10 && rando < 15)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: A Bizarre Phantasm", moduleId);
            lvlname = "A Bizarre Phantasm";
            creators.Add("Hermes");
            creators.Add("Motu");
            creators.Add("COSINE");
            creators.Add("Reple");
            creators.Add("Ryan LC");
            creators.Add("Luneth");
            creators.Add("Koreaqwer");
            creators.Add("RedDragon");
            creators.Add("WOOGI1411");
            creators.Add("ZenthicAlpha");
            creators.Add("Miner");
            creators.Add("Zelda");
            creators.Add("Yuri");
            verifier = "GoodSmile";
            difficulty = "Extreme Demon";
            songname = "Betrayal of Fate";
            songmaker = "Goukisan";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 11)
            {
                player.clip = useInternal ? internalClips[11] : VideoLoader.clips[11];
                aud.clip = audios[11];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 12)
            {
                player.clip = useInternal ? internalClips[12] : VideoLoader.clips[12];
                aud.clip = audios[12];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
            else if (rando == 13)
            {
                player.clip = useInternal ? internalClips[13] : VideoLoader.clips[13];
                aud.clip = audios[13];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = false;
            }
            else if (rando == 14)
            {
                player.clip = useInternal ? internalClips[14] : VideoLoader.clips[14];
                aud.clip = audios[14];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = false;
            }
        }
        else if (rando > 14 && rando < 18)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Dynamic On Track", moduleId);
            lvlname = "Dynamic On Track";
            creators.Add("Crepuscule");
            verifier = "Crepuscule";
            difficulty = "Hard";
            songname = "Back On Track";
            songmaker = "DJVI";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 15)
            {
                player.clip = useInternal ? internalClips[15] : VideoLoader.clips[15];
                aud.clip = audios[15];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
            else if (rando == 16)
            {
                player.clip = useInternal ? internalClips[16] : VideoLoader.clips[16];
                aud.clip = audios[16];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
            else if (rando == 17)
            {
                player.clip = useInternal ? internalClips[17] : VideoLoader.clips[17];
                aud.clip = audios[17];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
        }
        else if (rando > 17 && rando < 22)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Drowning", moduleId);
            lvlname = "Drowning";
            creators.Add("Lugunium");
            creators.Add("Tronzeki");
            creators.Add("ConstaNEO");
            creators.Add("Parallon");
            creators.Add("Rlol");
            creators.Add("Nekho");
            creators.Add("Pineapple");
            creators.Add("Azartt");
            creators.Add("KowZ");
            creators.Add("NoctaFly");
            creators.Add("Axxorz");
            creators.Add("Gibbon");
            creators.Add("Atlas");
            creators.Add("TiTi26");
            creators.Add("FreeZor");
            creators.Add("DarwinGD");
            verifier = "HexagonDashers";
            difficulty = "Insane";
            songname = "Drowning";
            songmaker = "Omnivore";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 18)
            {
                player.clip = useInternal ? internalClips[18] : VideoLoader.clips[18];
                aud.clip = audios[18];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = true;
            }
            else if (rando == 19)
            {
                player.clip = useInternal ? internalClips[19] : VideoLoader.clips[19];
                aud.clip = audios[19];
                coin = true;
                mirror = false;
                speed = true;
                teleport = true;
                transformC = true;
                orbs = false;
                pads = true;
            }
            else if (rando == 20)
            {
                player.clip = useInternal ? internalClips[20] : VideoLoader.clips[20];
                aud.clip = audios[20];
                coin = false;
                mirror = false;
                speed = false;
                teleport = true;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 21)
            {
                player.clip = useInternal ? internalClips[21] : VideoLoader.clips[21];
                aud.clip = audios[21];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = false;
            }
        }
        else if (rando > 21 && rando < 25)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Electroman Adventures", moduleId);
            lvlname = "Electroman Adventures";
            creators.Add("RobTop");
            verifier = "RobTop";
            difficulty = "Insane";
            songname = "Electroman Adventures";
            songmaker = "Waterflame";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 22)
            {
                player.clip = useInternal ? internalClips[22] : VideoLoader.clips[22];
                aud.clip = audios[22];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
            else if (rando == 23)
            {
                player.clip = useInternal ? internalClips[23] : VideoLoader.clips[23];
                aud.clip = audios[23];
                coin = false;
                mirror = true;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 24)
            {
                player.clip = useInternal ? internalClips[24] : VideoLoader.clips[24];
                aud.clip = audios[24];
                coin = false;
                mirror = true;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = false;
            }
        }
        else if (rando > 24 && rando < 29)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Galatic Fragility", moduleId);
            lvlname = "Galatic Fragility";
            creators.Add("Koreaqwer");
            creators.Add("Tutti");
            creators.Add("Ple5");
            creators.Add("Vortiz");
            creators.Add("Hades");
            creators.Add("Gigas");
            creators.Add("ATHG");
            creators.Add("Theta");
            creators.Add("Dark Boshy");
            creators.Add("Dorami");
            verifier = "Tricks33";
            difficulty = "Insane Demon";
            songname = "Eurodancer";
            songmaker = "TMM43";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 25)
            {
                player.clip = useInternal ? internalClips[25] : VideoLoader.clips[25];
                aud.clip = audios[25];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = false;
            }
            else if (rando == 26)
            {
                player.clip = useInternal ? internalClips[26] : VideoLoader.clips[26];
                aud.clip = audios[26];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 27)
            {
                player.clip = useInternal ? internalClips[27] : VideoLoader.clips[27];
                aud.clip = audios[27];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
            else if (rando == 28)
            {
                player.clip = useInternal ? internalClips[28] : VideoLoader.clips[28];
                aud.clip = audios[28];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
        }
        else if (rando > 28 && rando < 32)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Time Machine", moduleId);
            lvlname = "Time Machine";
            creators.Add("RobTop");
            verifier = "RobTop";
            difficulty = "Harder";
            songname = "Time Machine";
            songmaker = "Waterflame";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 29)
            {
                player.clip = useInternal ? internalClips[29] : VideoLoader.clips[29];
                aud.clip = audios[29];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = false;
            }
            else if (rando == 30)
            {
                player.clip = useInternal ? internalClips[30] : VideoLoader.clips[30];
                aud.clip = audios[30];
                coin = true;
                mirror = true;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
            else if (rando == 31)
            {
                player.clip = useInternal ? internalClips[31] : VideoLoader.clips[31];
                aud.clip = audios[31];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
        }
        else if (rando > 31 && rando < 35)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: AntiPixel", moduleId);
            lvlname = "AntiPixel";
            creators.Add("Echonox");
            verifier = "Echonox";
            difficulty = "Harder";
            songname = "Grizzly(WIP)";
            songmaker = "Envy";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 32)
            {
                player.clip = useInternal ? internalClips[32] : VideoLoader.clips[32];
                aud.clip = audios[32];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = false;
            }
            else if (rando == 33)
            {
                player.clip = useInternal ? internalClips[33] : VideoLoader.clips[33];
                aud.clip = audios[33];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = false;
            }
            else if (rando == 34)
            {
                player.clip = useInternal ? internalClips[34] : VideoLoader.clips[34];
                aud.clip = audios[34];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = false;
            }
        }
        else if (rando > 34 && rando < 38)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: ColorZ", moduleId);
            lvlname = "ColorZ";
            creators.Add("Xtobe5");
            verifier = "Xtobe5";
            difficulty = "Normal";
            songname = "Streetlights";
            songmaker = "Envy";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 35)
            {
                player.clip = useInternal ? internalClips[35] : VideoLoader.clips[35];
                aud.clip = audios[35];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
            else if (rando == 36)
            {
                player.clip = useInternal ? internalClips[36] : VideoLoader.clips[36];
                aud.clip = audios[36];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
            else if (rando == 37)
            {
                player.clip = useInternal ? internalClips[37] : VideoLoader.clips[37];
                aud.clip = audios[37];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
        }
        else if (rando > 37 && rando < 40)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Old Auto Zone", moduleId);
            lvlname = "Old Auto Zone";
            creators.Add("Gelt");
            verifier = "Gelt";
            difficulty = "Auto";
            songname = "Clubstep";
            songmaker = "Dj-Nate";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 38)
            {
                player.clip = useInternal ? internalClips[38] : VideoLoader.clips[38];
                aud.clip = audios[38];
                coin = false;
                mirror = true;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
            else if (rando == 39)
            {
                player.clip = useInternal ? internalClips[39] : VideoLoader.clips[39];
                aud.clip = audios[39];
                coin = false;
                mirror = true;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
        }
        else if (rando > 39 && rando < 43)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: ReTraY", moduleId);
            lvlname = "ReTraY";
            creators.Add("DiMaViKuLov26");
            verifier = "DiMaViKuLov26";
            difficulty = "Easy";
            songname = "Golden Haze (Prev)";
            songmaker = "Detious";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 40)
            {
                player.clip = useInternal ? internalClips[40] : VideoLoader.clips[40];
                aud.clip = audios[40];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = false;
            }
            else if (rando == 41)
            {
                player.clip = useInternal ? internalClips[41] : VideoLoader.clips[41];
                aud.clip = audios[41];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = false;
            }
            else if (rando == 42)
            {
                player.clip = useInternal ? internalClips[42] : VideoLoader.clips[42];
                aud.clip = audios[42];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = false;
            }
        }
        else if (rando > 42 && rando < 46)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Grind District", moduleId);
            lvlname = "Grind District";
            creators.Add("TriAxis");
            creators.Add("Etzer");
            verifier = "TriAxis";
            difficulty = "Harder";
            songname = "Grind District";
            songmaker = "Waterflame";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 43)
            {
                player.clip = useInternal ? internalClips[43] : VideoLoader.clips[43];
                aud.clip = audios[43];
                coin = true;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
            else if (rando == 44)
            {
                player.clip = useInternal ? internalClips[44] : VideoLoader.clips[44];
                aud.clip = audios[44];
                coin = true;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = false;
            }
            else if (rando == 45)
            {
                player.clip = useInternal ? internalClips[45] : VideoLoader.clips[45];
                aud.clip = audios[45];
                coin = true;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
        }
        else if (rando > 45 && rando < 49)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Lunar Ocean", moduleId);
            lvlname = "Lunar Ocean";
            creators.Add("Gelt");
            verifier = "Gelt";
            difficulty = "Hard";
            songname = "Solar Rain";
            songmaker = "Geoplex";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 46)
            {
                player.clip = useInternal ? internalClips[46] : VideoLoader.clips[46];
                aud.clip = audios[46];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = true;
            }
            else if (rando == 47)
            {
                player.clip = useInternal ? internalClips[47] : VideoLoader.clips[47];
                aud.clip = audios[47];
                coin = false;
                mirror = true;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = false;
            }
            else if (rando == 48)
            {
                player.clip = useInternal ? internalClips[48] : VideoLoader.clips[48];
                aud.clip = audios[48];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = true;
            }
        }
        else if (rando > 48 && rando < 52)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Problematic", moduleId);
            lvlname = "Problematic";
            creators.Add("Dhafin");
            verifier = "Dhafin";
            difficulty = "Easy Demon";
            songname = "Problematic";
            songmaker = "Rukkus";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 49)
            {
                player.clip = useInternal ? internalClips[49] : VideoLoader.clips[49];
                aud.clip = audios[49];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = true;
            }
            else if (rando == 50)
            {
                player.clip = useInternal ? internalClips[50] : VideoLoader.clips[50];
                aud.clip = audios[50];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = false;
            }
            else if (rando == 51)
            {
                player.clip = useInternal ? internalClips[51] : VideoLoader.clips[51];
                aud.clip = audios[51];
                coin = true;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = false;
                pads = true;
            }
        }
        else if (rando > 51 && rando < 56)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Takeoff", moduleId);
            lvlname = "Takeoff";
            creators.Add("Nasgubb");
            verifier = "Nasgubb";
            difficulty = "Medium Demon";
            songname = "Jet Set";
            songmaker = "MadHouseDude";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 52)
            {
                player.clip = useInternal ? internalClips[52] : VideoLoader.clips[52];
                aud.clip = audios[52];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = true;
            }
            else if (rando == 53)
            {
                player.clip = useInternal ? internalClips[53] : VideoLoader.clips[53];
                aud.clip = audios[53];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 54)
            {
                player.clip = useInternal ? internalClips[54] : VideoLoader.clips[54];
                aud.clip = audios[54];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = false;
            }
            else if (rando == 55)
            {
                player.clip = useInternal ? internalClips[55] : VideoLoader.clips[55];
                aud.clip = audios[55];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
        }
        else if (rando > 55 && rando < 59)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Sakupen Hell", moduleId);
            lvlname = "Sakupen Hell";
            creators.Add("Noobas");
            verifier = "TrusTa";
            difficulty = "Extreme Demon";
            songname = "Iron God: Sakupen Hell Yes RMX";
            songmaker = "mr-jazzman";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 56)
            {
                player.clip = useInternal ? internalClips[56] : VideoLoader.clips[56];
                aud.clip = audios[56];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 57)
            {
                player.clip = useInternal ? internalClips[57] : VideoLoader.clips[57];
                aud.clip = audios[57];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 58)
            {
                player.clip = useInternal ? internalClips[58] : VideoLoader.clips[58];
                aud.clip = audios[58];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = true;
            }
        }
        else if (rando > 58 && rando < 62)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: love baba", moduleId);
            lvlname = "love baba";
            creators.Add("Zobros");
            creators.Add("Demonico17");
            verifier = "Zobros";
            difficulty = "Insane Demon";
            songname = "The Empire Of Toads";
            songmaker = "Bossfight";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 59)
            {
                player.clip = useInternal ? internalClips[59] : VideoLoader.clips[59];
                aud.clip = audios[59];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = false;
            }
            else if (rando == 60)
            {
                player.clip = useInternal ? internalClips[60] : VideoLoader.clips[60];
                aud.clip = audios[60];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = false;
                pads = false;
            }
            else if (rando == 61)
            {
                player.clip = useInternal ? internalClips[61] : VideoLoader.clips[61];
                aud.clip = audios[61];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = false;
            }
        }
        else if (rando > 61 && rando < 66)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: PP", moduleId);
            lvlname = "PP";
            creators.Add("AmorAltra");
            creators.Add("BoomKitty");
            verifier = "AmorAltra";
            difficulty = "Easy Demon";
            songname = "Peepee Song";
            songmaker = "BoomKitty";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 62)
            {
                player.clip = useInternal ? internalClips[62] : VideoLoader.clips[62];
                aud.clip = audios[62];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = true;
            }
            else if (rando == 63)
            {
                player.clip = useInternal ? internalClips[63] : VideoLoader.clips[63];
                aud.clip = audios[63];
                coin = true;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 64)
            {
                player.clip = useInternal ? internalClips[64] : VideoLoader.clips[64];
                aud.clip = audios[64];
                coin = true;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = true;
            }
            else if (rando == 65)
            {
                player.clip = useInternal ? internalClips[65] : VideoLoader.clips[65];
                aud.clip = audios[65];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = false;
            }
        }
        else if (rando > 65 && rando < 70)
        {
            Debug.LogFormat("[Geometry Dash #{0}] The chosen level is: Uprise", moduleId);
            lvlname = "Uprise";
            creators.Add("Blad3m");
            creators.Add("Menkatjezzz");
            verifier = "Ninetails";
            difficulty = "Extreme Demon";
            songname = "Uprise";
            songmaker = "Envy";
            Debug.LogFormat("[Geometry Dash #{0}] --------------------------------------------", moduleId);
            if (rando == 66)
            {
                player.clip = useInternal ? internalClips[66] : VideoLoader.clips[66];
                aud.clip = audios[66];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = false;
                orbs = true;
                pads = true;
            }
            else if (rando == 67)
            {
                player.clip = useInternal ? internalClips[67] : VideoLoader.clips[67];
                aud.clip = audios[67];
                coin = false;
                mirror = false;
                speed = true;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 68)
            {
                player.clip = useInternal ? internalClips[68] : VideoLoader.clips[68];
                aud.clip = audios[68];
                coin = false;
                mirror = false;
                speed = false;
                teleport = false;
                transformC = true;
                orbs = true;
                pads = true;
            }
            else if (rando == 69)
            {
                player.clip = useInternal ? internalClips[69] : VideoLoader.clips[69];
                aud.clip = audios[69];
                coin = false;
                mirror = false;
                speed = false;
                teleport = true;
                transformC = true;
                orbs = true;
                pads = true;
            }
        }
    }

    private void getNumber()
    {
        startingNum = 0;
        Debug.LogFormat("[Geometry Dash #{0}] Calculation of number...", moduleId);
        for (int i = 0; i < bomb.GetSerialNumberNumbers().Count(); i++)
        {
            startingNum += bomb.GetSerialNumberNumbers().ElementAt(i);
        }
        Debug.LogFormat("[Geometry Dash #{0}] Starting Num: {1}", moduleId, startingNum);
        newNum = startingNum;
        Debug.LogFormat("[Geometry Dash #{0}] Is there any coins present? '{1}'", moduleId, coin);
        if (coin)
        {
            int count = 0;
            string[] valids = { "A", "E", "I", "O", "U", "Y" };
            string temp = songmaker.ToUpper();
            for(int i = 0; i < temp.Length; i++)
            {
                if (valids.Contains((temp.ElementAt(i) + "")))
                {
                    count++;
                }
            }
            newNum += count;
            Debug.LogFormat("[Geometry Dash #{0}] Number of vowels in song creator is: {1}, which is then added to the starting number: {2}", moduleId, count, newNum);
        }
        else
        {
            newNum -= 9;
            Debug.LogFormat("[Geometry Dash #{0}] Subtracting 9 from starting number: {1}", moduleId, newNum);
        }
        Debug.LogFormat("[Geometry Dash #{0}] Is there any orange mirror portal present? '{1}'", moduleId, mirror);
        if (mirror)
        {
            if (lvlname.EndsWith("r") || lvlname.EndsWith("R"))
            {
                newNum -= 5;
                Debug.LogFormat("[Geometry Dash #{0}] Letter R was found at the end of the level's name, subtracting 5 from the number: {1}", moduleId, newNum);
            }
            else
            {
                int count = 0;
                for(int i = 0; i < bomb.GetPorts().Count(); i++)
                {
                    if (bomb.GetPorts().ElementAt(i).Equals("RJ45"))
                    {
                        count++;
                    }
                }
                newNum += count;
                Debug.LogFormat("[Geometry Dash #{0}] There is {1} RJ-45 ports, adding {1} to the number: {2}", moduleId, count, newNum);
            }
        }
        else
        {
            string[] valids = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string temp = verifier.ToUpper();
            int deter = 0;
            for(int i = 0; i < verifier.Length; i++)
            {
                if (valids.Contains((temp.ElementAt(i) + "")))
                {
                    deter = i;
                    break;
                }
            }
            char c = verifier.ElementAt(deter);
            int index = char.ToUpper(c) - 65;
            newNum += index;
            Debug.LogFormat("[Geometry Dash #{0}] First letter of the verifier's name: {1}, which in alphanumeric A=0...Z=25 is: {2}, which is then added to the number: {3}", moduleId, verifier.ElementAt(deter), index, newNum);
        }
        Debug.LogFormat("[Geometry Dash #{0}] Is there any 1x or 3x speed portals present? '{1}'", moduleId, speed);
        if (speed)
        {
            if(creators.Count > 1)
            {
                newNum *= 3;
                Debug.LogFormat("[Geometry Dash #{0}] The level is a collab, so the number is multiplied by 3: {1}", moduleId, newNum);
            }
            else
            {
                newNum *= 2;
                Debug.LogFormat("[Geometry Dash #{0}] The level is not a collab, so the number is multiplied by 2: {1}", moduleId, newNum);
            }
        }
        else
        {
            newNum += creators.Count;
            Debug.LogFormat("[Geometry Dash #{0}] The level has {1} creators, which added to the number is: {2}", moduleId, creators.Count, newNum);
        }
        Debug.LogFormat("[Geometry Dash #{0}] Is there any teleportation portals present? '{1}'", moduleId, teleport);
        if (teleport)
        {
            int sum = 0;
            string[] valids = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string temp = lvlname.ToUpper();
            for(int i = 0; i < temp.Length; i++)
            {
                string thing = "" + temp.ElementAt(i);
                if (valids.Contains(thing))
                {
                    int index = char.ToUpper(temp.ElementAt(i)) - 64;
                    sum += index;
                }
            }
            newNum += sum;
            Debug.LogFormat("[Geometry Dash #{0}] The sum of each letter's position in the level's name is: {1}, which added to the number is: {2}", moduleId, sum, newNum);
        }
        else
        {
            newNum += (7 + bomb.GetOnIndicators().Count());
            Debug.LogFormat("[Geometry Dash #{0}] Adding (the number of lit indicators + 7) to the number: {1}", moduleId, newNum);
        }
        Debug.LogFormat("[Geometry Dash #{0}] Is there any red, blue, or white transformation portals present? '{1}'", moduleId, transformC);
        if (transformC)
        {
            int sum = 0;
            string[] valids = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string temp = difficulty.ToUpper();
            for (int i = 0; i < temp.Length; i++)
            {
                string thing = "" + temp.ElementAt(i);
                if (valids.Contains(thing))
                {
                    sum++;
                }
            }
            newNum -= (sum + bomb.GetBatteryHolderCount());
            Debug.LogFormat("[Geometry Dash #{0}] The sum of the number of letters in the level's difficulty & the number of battery holders is: {1}, which subtracted from the number is: {2}", moduleId, sum + bomb.GetBatteryHolderCount(), newNum);
        }
        else
        {
            int sum = 0;
            string[] valids = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string temp = songname.ToUpper();
            for (int i = 0; i < temp.Length; i++)
            {
                string thing = "" + temp.ElementAt(i);
                if (valids.Contains(thing))
                {
                    sum++;
                }
            }
            newNum *= sum;
            Debug.LogFormat("[Geometry Dash #{0}] The number of letters in the level's song title is: {1}, which multiplied by the number is: {2}", moduleId, sum, newNum);
        }
        Debug.LogFormat("[Geometry Dash #{0}] Is there any blue, green, or black jump orbs present? '{1}'", moduleId, orbs);
        if (orbs)
        {
            newNum += (8 + bomb.GetOffIndicators().Count());
            Debug.LogFormat("[Geometry Dash #{0}] Adding (8 + the number of unlit indicators) to the number: {1}", moduleId, newNum);
        }
        else
        {
            List<string> tempcreate = creators;
            tempcreate.Sort();
            int index = 0;
            int ct = 0;
            string[] valids = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string temp = tempcreate[tempcreate.Count-1].ToUpper();
            for (int i = temp.Length-1; i >= 0; i--)
            {
                string thing = "" + temp.ElementAt(i);
                if (valids.Contains(thing))
                {
                    ct++;
                }
                if(ct == 2)
                {
                    index = char.ToUpper(temp.ElementAt(i)) - 64;
                    break;
                }
            }
            newNum += index;
            Debug.LogFormat("[Geometry Dash #{0}] The position of the second to last letter in the last creator alphabetically is: {1}, which added to the number is: {2}", moduleId, index, newNum);
        }
        Debug.LogFormat("[Geometry Dash #{0}] Is there any yellow or blue jump pads present? '{1}'", moduleId, pads);
        if (pads)
        {
            int ct = 0;
            for(int i = 0; i < bomb.GetSerialNumber().Length; i++)
            {
                if(bomb.GetSerialNumber()[i].Equals('U') || bomb.GetSerialNumber()[i].Equals('L') || bomb.GetSerialNumber()[i].Equals('E'))
                {
                    ct++;
                }
            }
            newNum *= (ct + bomb.GetPorts().Count());
            Debug.LogFormat("[Geometry Dash #{0}] The sum of the number of U's, L's, and E's in the serial number and the number of ports is: {1}, which multiplied by the number is: {2}", moduleId, ct + bomb.GetPorts().Count(), newNum);
        }
        else
        {
            int ct = 0;
            string[] valids = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string temp = songmaker.ToUpper();
            for (int i = 0; i < temp.Length; i++)
            {
                string thing = "" + temp.ElementAt(i);
                if (valids.Contains(thing))
                {
                    ct++;
                }
            }
            newNum -= (startingNum - ct);
            Debug.LogFormat("[Geometry Dash #{0}] The starting number minus the number of letters in the level's song creator is: {1}, which subtracted from the number is: {2}", moduleId, startingNum - ct, newNum);
        }
        correctCubeIndex = mod(newNum, 20) + 1;
        Debug.LogFormat("[Geometry Dash #{0}] Final Calculation: {1} % 20 + 1 = {2}", moduleId, newNum, correctCubeIndex);
        Debug.LogFormat("[Geometry Dash #{0}] The correct cube to press according to the manual's table is: {1}", moduleId, correctCubeIndex);
    }

    private void randomizeButtons()
    {
        int rando = Random.Range(0, 4);
        correctBut = rando;
        if (correctBut == 0)
        {
            icons[2].GetComponent<Image>().sprite = cubes[correctCubeIndex-1];
            int rand2 = correctCubeIndex-1;
            while (rand2+1 == correctCubeIndex)
            {
                rand2 = Random.Range(0, 20);
                icons[3].GetComponent<Image>().sprite = cubes[rand2];
            }
            int rand3 = correctCubeIndex - 1;
            while (rand3 + 1 == correctCubeIndex || rand3 == rand2)
            {
                rand3 = Random.Range(0, 20);
                icons[4].GetComponent<Image>().sprite = cubes[rand3];
            }
            int rand4 = correctCubeIndex - 1;
            while (rand4 + 1 == correctCubeIndex || rand4 == rand2 || rand4 == rand3)
            {
                rand4 = Random.Range(0, 20);
                icons[5].GetComponent<Image>().sprite = cubes[rand4];
            }
        }
        else if (correctBut == 1)
        {
            icons[3].GetComponent<Image>().sprite = cubes[correctCubeIndex-1];
            int rand2 = correctCubeIndex - 1;
            while (rand2 + 1 == correctCubeIndex)
            {
                rand2 = Random.Range(0, 20);
                icons[2].GetComponent<Image>().sprite = cubes[rand2];
            }
            int rand3 = correctCubeIndex - 1;
            while (rand3 + 1 == correctCubeIndex || rand3 == rand2)
            {
                rand3 = Random.Range(0, 20);
                icons[4].GetComponent<Image>().sprite = cubes[rand3];
            }
            int rand4 = correctCubeIndex - 1;
            while (rand4 + 1 == correctCubeIndex || rand4 == rand2 || rand4 == rand3)
            {
                rand4 = Random.Range(0, 20);
                icons[5].GetComponent<Image>().sprite = cubes[rand4];
            }
        }
        else if (correctBut == 2)
        {
            icons[4].GetComponent<Image>().sprite = cubes[correctCubeIndex-1];
            int rand2 = correctCubeIndex - 1;
            while (rand2 + 1 == correctCubeIndex)
            {
                rand2 = Random.Range(0, 20);
                icons[2].GetComponent<Image>().sprite = cubes[rand2];
            }
            int rand3 = correctCubeIndex - 1;
            while (rand3 + 1 == correctCubeIndex || rand3 == rand2)
            {
                rand3 = Random.Range(0, 20);
                icons[3].GetComponent<Image>().sprite = cubes[rand3];
            }
            int rand4 = correctCubeIndex - 1;
            while (rand4 + 1 == correctCubeIndex || rand4 == rand2 || rand4 == rand3)
            {
                rand4 = Random.Range(0, 20);
                icons[5].GetComponent<Image>().sprite = cubes[rand4];
            }
        }
        else if (correctBut == 3)
        {
            icons[5].GetComponent<Image>().sprite = cubes[correctCubeIndex-1];
            int rand2 = correctCubeIndex - 1;
            while (rand2 + 1 == correctCubeIndex)
            {
                rand2 = Random.Range(0, 20);
                icons[2].GetComponent<Image>().sprite = cubes[rand2];
            }
            int rand3 = correctCubeIndex - 1;
            while (rand3 + 1 == correctCubeIndex || rand3 == rand2)
            {
                rand3 = Random.Range(0, 20);
                icons[3].GetComponent<Image>().sprite = cubes[rand3];
            }
            int rand4 = correctCubeIndex - 1;
            while (rand4 + 1 == correctCubeIndex || rand4 == rand2 || rand4 == rand3)
            {
                rand4 = Random.Range(0, 20);
                icons[4].GetComponent<Image>().sprite = cubes[rand4];
            }
        }
    }

    private int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    private IEnumerator startDelayPlay()
    {
        animating = true;
        audio.PlaySoundAtTransform("playSound_01", transform);
        yield return new WaitForSeconds(1f);
        float fadeOutTime = 2.0f;
        Material originalMat = background.GetComponent<Renderer>().material;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            background.GetComponent<Renderer>().material.Lerp(originalMat, startBacks[0], Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
        player.Play();
        yield return new WaitForSeconds(.1f);
        aud.Play();
        Material originalMat2 = background.GetComponent<Renderer>().material;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
            background.GetComponent<Renderer>().material.Lerp(originalMat2, startBacks[1], Mathf.Min(1, t / fadeOutTime));
            yield return null;
        }
        yield return new WaitForSeconds(9f);
        player.Stop();
        for (int i = 0; i < 2; i++)
        {
            if (i == 1 && reset)
                continue;
            else
            {
                endTexts[i].SetActive(true);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (i == 1 && reset)
                continue;
            else
            {
                icons[i].SetActive(true);
                endButtons[i].SetActive(true);
            }
        }
        started = true;
        animating = false;
    }

    private IEnumerator end()
    {
        animating = true;
        player.clip = useInternal ? internalClips[internalClips.Length-1] : VideoLoader.clips[VideoLoader.clips.Length-1];
        audio.PlaySoundAtTransform("endStart_02", transform);
        for (int i = 0; i < 2; i++)
        {
            endTexts[i].SetActive(false);
        }
        for (int i = 0; i < 6; i++)
        {
            icons[i].SetActive(false);
            endButtons[i].SetActive(false);
        }
        player.Play();
        yield return new WaitForSeconds(3.5f);
        player.Stop();
        background.GetComponent<Renderer>().material = endBack;
        animating = false;
        moduleSolved = true;
        GetComponent<KMBombModule>().HandlePass();
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} play [Plays the level clip] | !{0} playfocus [Plays the level clip AND focuses on the module (use if zooming)] | !{0} cube <#> [Presses the specified cube 1-4 where 1 is topmost and 4 is bottommost] | !{0} reset [Resets the module]";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*playfocus\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (animating != true)
            {
                if (started == true)
                {
                    buttons[1].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                    while(animating)
                    {
                        yield return new WaitForSeconds(0.1f);
                        yield return "trycancel Focus on level clip cancelled due to cancel request.";
                    }
                }
                else
                {
                    buttons[0].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                    while (animating)
                    {
                        yield return new WaitForSeconds(0.1f);
                        yield return "trycancel Focus on level clip cancelled due to cancel request.";
                    }
                }
            }
            else
            {
                yield return "sendtochaterror I cannot play a level clip while I'm playing one already!";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*play\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (animating != true)
            {
                if (started)
                {
                    buttons[1].OnInteract();
                }
                else
                {
                    buttons[0].OnInteract();
                }
            }
            else
            {
                yield return "sendtochaterror I cannot play a level clip while I'm playing one already!";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*reset\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (!reset)
            {
                if (animating != true)
                {
                    if (started)
                    {
                        buttons[2].OnInteract();
                    }
                    else
                    {
                        yield return "sendtochaterror I cannot reset myself if I haven't been started!";
                    }
                }
                else
                {
                    yield return "sendtochaterror I cannot reset myself while I'm playing a level clip!";
                }
            }
            else
            {
                yield return "sendtochaterror I have already been reset once! I cannot be reset again!";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*cube 1\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (animating != true)
            {
                if (started)
                {
                    if (correctBut == 0)
                    {
                        yield return "solve";
                    }
                    buttons[3].OnInteract();
                }
                else
                {
                    yield return "sendtochaterror I cannot press a cube if I haven't been started!";
                }
            }
            else
            {
                yield return "sendtochaterror I press a cube while I'm playing a level clip!";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*cube 2\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (animating != true)
            {
                if (started)
                {
                    if (correctBut == 1)
                    {
                        yield return "solve";
                    }
                    buttons[4].OnInteract();
                }
                else
                {
                    yield return "sendtochaterror I cannot press a cube if I haven't been started!";
                }
            }
            else
            {
                yield return "sendtochaterror I press a cube while I'm playing a level clip!";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*cube 3\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (animating != true)
            {
                if (started)
                {
                    if (correctBut == 2)
                    {
                        yield return "solve";
                    }
                    buttons[5].OnInteract();
                }
                else
                {
                    yield return "sendtochaterror I cannot press a cube if I haven't been started!";
                }
            }
            else
            {
                yield return "sendtochaterror I press a cube while I'm playing a level clip!";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*cube 4\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (animating != true)
            {
                if (started)
                {
                    if (correctBut == 3)
                    {
                        yield return "solve";
                    }
                    buttons[6].OnInteract();
                }
                else
                {
                    yield return "sendtochaterror I cannot press a cube if I haven't been started!";
                }
            }
            else
            {
                yield return "sendtochaterror I press a cube while I'm playing a level clip!";
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        while (loading) { yield return true; yield return new WaitForSeconds(0.1f); }
        if (!started && !animating)
        {
            buttons[0].OnInteract();
        }
        while (animating) { yield return true; yield return new WaitForSeconds(0.1f); }
        buttons[correctBut+3].OnInteract();
        while (animating) { yield return true; yield return new WaitForSeconds(0.1f); }
    }
}