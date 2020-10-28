using Abilities;
using GameplayEntities;
using LLHandlers;
using LLScreen;
using UnityEngine;

namespace Invis_a_Ball
{
    public class Invis_a_Ball : MonoBehaviour
    {
        const string modVersion = "1.2.0";
        const string repositoryOwner = "Daioutzu";
        const string repositoryName = "LLBMM-Invis_a_Ball";

        public static Invis_a_Ball Instance { get; private set; }
        public ModMenuIntegration MMI { get; private set; }
        bool modIntegrated;

        public static void Initialize()
        {
            GameObject gameObject = new GameObject("Invis_a_Ball");
            Instance = gameObject.AddComponent<Invis_a_Ball>();
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            if (MMI == null) { MMI = gameObject.AddComponent<ModMenuIntegration>(); Debug.Log("[LLBMM] Invis_a_Ball: Added GameObject \"ModMenuIntegration\""); }
            Debug.Log("[LLBMM] Invis_a_Ball Started");
        }

        void OnDestroy()
        {
            Debug.Log("[LLBMM] Invis_a_Ball Destroyed");
        }

        bool invisBall;
        bool invisPlayer;
        bool invisOpponents;
        bool ballEffects;
        bool superEffects;
        bool playerImpactEffects;
        bool playerHitEffects;
        JOFJHDJHJGI currentGameState;
        GameMode currentGameMode;

        //bool InGame => World.instance != null && (currentGameState == JOFJHDJHJGI.CDOFDJMLGLO || currentGameState == JOFJHDJHJGI.LGILIJKMKOD);

        void ModMenuInit()
        {
            if ((MMI != null && !modIntegrated) || LLModMenu.ModMenu.Instance.currentOpenMod == "Invis_a_Ball")
            {

                invisBall = MMI.GetTrueFalse(MMI.configBools["(bool)invisibileBall"]);
                invisPlayer = MMI.GetTrueFalse(MMI.configBools["(bool)invisibilePlayer"]);
                invisOpponents = MMI.GetTrueFalse(MMI.configBools["(bool)invisibileOpponents"]);
                ballEffects = MMI.GetTrueFalse(MMI.configBools["(bool)BallEffects"]);
                superEffects = MMI.GetTrueFalse(MMI.configBools["(bool)SuperEffects"]);
                playerImpactEffects = MMI.GetTrueFalse(MMI.configBools["(bool)PlayerImpactEffects"]);
                playerHitEffects = MMI.GetTrueFalse(MMI.configBools["(bool)PlayerHitEffects"]);

                if (!modIntegrated) { Debug.Log($"[LLBMM] Invis_a_Ball: ModMenuIntegration Done"); };
                modIntegrated = true;
            }
        }

#if DEBUG
        void PrintAllGameObjects()
        {
            string txt = "";
            foreach (var name in FindObjectsOfType<GameObject>())
            {
                string str = (name.transform.parent != null) ? name.transform.parent.gameObject.name : "NO_PARENT";
                txt += $"{str}/{name.name}\n";
            }
            Debug.Log(txt);
        }
#endif
        bool InGame()
        {
            currentGameState = DNPFJHMAIBP.HHMOGKIMBNM(); // GameStates.GetCurrent
            currentGameMode = JOMBNFKIHIC.GIGAKBJGFDI.PNJOKAICMNN; //GameSettings.current.gameMode
            return World.instance != null && (currentGameState == JOFJHDJHJGI.CDOFDJMLGLO || currentGameState == JOFJHDJHJGI.LGILIJKMKOD) && !UIScreen.loadingScreenActive;
        }

        void Update()
        {
            ModMenuInit();

#if DEBUG
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                PrintAllGameObjects();
            }
#endif
            if (InGame())
            {
                if (invisBall)
                {
                    SetBallInvisibile();
                }

                if (invisPlayer)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Controller con = ALDOKEMAOMB.BJDPHEHJJJK(i).GDEMBCKIDMA;
                        //IsLocal & inMatch
                        if (!con.IsNone() && ALDOKEMAOMB.BJDPHEHJJJK(i).GAFCIHKIGNM && ALDOKEMAOMB.BJDPHEHJJJK(i).NGLDMOLLPLK)
                        {
                            SetPlayerInvisibile(i);
                            i = 4;
                        }
                    };
                }

                if (invisOpponents)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Controller con = ALDOKEMAOMB.BJDPHEHJJJK(i).GDEMBCKIDMA;
                        if (con.IsNone() && ALDOKEMAOMB.BJDPHEHJJJK(i).NGLDMOLLPLK)
                        {
                            SetPlayerInvisibile(i);
                        }
                    };
                }

                ClearEffects();
                ClearPlayerEffects(!playerImpactEffects);
                ClearPlayerHitEffects(!playerHitEffects);
            }
        }

        void SetPlayerInvisibile(int playerNr = 0)
        {
            PlayerEntity playerEnt = ALDOKEMAOMB.BJDPHEHJJJK(playerNr).JCCIAMJEODH;
            if (playerEnt != null)
            {
                playerEnt.SetVisible(false);
                if (playerEnt.character == Character.CANDY)
                {
                    playerEnt.SetVisible(false, "runLegsVisual");
                }
            }
        }

        void SetBallInvisibile(int ballNr = 0)
        {
            BallEntity ballEnt = BallHandler.instance.GetBall(ballNr);

            if (ballEnt != null)
            {
                ballEnt.PlayAnim("off", "glowVisual");
                ballEnt.SetBallTrailActive(false);
                ballEnt.SetVisible(false);
                ALDOKEMAOMB.ICOCPAFKCCE(delegate (PlayerEntity player)
                {
                    switch (player.character)
                    {
                        case Character.BAG:
                            ((BagPlayer)player).SetVisible(false, "shadowVisual"); break;
                        case Character.GRAF:
                            ((GrafPlayer)player).SetVisible(false, "pieceVisual");
                            ((GrafPlayer)player).SetVisible(false, "paintBlobVisual"); break;
                        case Character.CANDY:
                            ballEnt.SetVisible(false, $"candyBall{(int)player.variant}Visual"); break;
                        case Character.COP:
                            if (FindObjectsOfType<Chain>() != null)
                            {
                                foreach (var chain in FindObjectsOfType<Chain>())
                                {
                                    chain.enabled = false;
                                }
                            }
                            if (ballEnt.HasVisual("cuffVisual"))
                            {
                                ballEnt.PlayAnim("off", "cuffVisual");
                            }
                            if (ballEnt.HasVisual("cuffDetectiveVisual"))
                            {
                                ballEnt.PlayAnim("off", "cuffDetectiveVisual");
                            }
                            break;

                    }
                });
            }
        }

        void ClearPlayerEffects(bool enable = false)
        {
            if (!enable) { return; }

            if (EffectHandler.instance != null)
            {
                var filteredEffects = EffectHandler.instance.effects.FindAll(x => x.effectData.active && PlayerEffect(x));
                foreach (var effect in filteredEffects)
                {
                    var playerEnt = PlayerHandler.instance.GetPlayerEntity(0);
                    //new Vector2f(this.moveBox.bounds.center.x, this.moveBox.bounds.min.y) is what the code below Reads
                    IBGCBLLKIHA position = new IBGCBLLKIHA(playerEnt.moveBox.bounds.LOLBPNFNKMI.GCPKPHMKLBN, playerEnt.moveBox.bounds.MOGDHBGHAOA.CGJJEHPPOAN);
                    IBGCBLLKIHA position2 = effect.GetPosition();
                    HHBCPNCDNDH gcpkphmklbn = IBGCBLLKIHA.FCKBPDNEAOG(position, position2).KEMFCABCHLO;

                    if (HHBCPNCDNDH.HPLPMEAOJPM(gcpkphmklbn, HHBCPNCDNDH.NKKIFJJEPOL(1m)))
                    {
                        EffectHandler.instance.Deactivate(effect);
                        effect.Reset();
                    }
                }
            }
        }

        void ClearPlayerHitEffects(bool enable = false)
        {
            if (!enable) { return; }
            BallEntity ballEnt = BallHandler.instance.GetBall(0);
            if (ballEnt != null)
            {
                ballEnt.PlayAnim("off", "grindVisual");
                ballEnt.PlayAnim("off", "buntVisual");
            }
        }

        void ClearEffects()
        {
            if (EffectHandler.instance != null)
            {
                var filteredEffects = EffectHandler.instance.effects.FindAll(x => x.effectData.active && (BallEffect(x) || SuperEffect(x) || PlayerHitEffect(x)));
                foreach (var effect in filteredEffects)
                {
                    EffectHandler.instance.Deactivate(effect);
                    effect.Reset();
                }
            }
        }

        bool PlayerEffect(EffectEntity effect)
        {
            if (playerImpactEffects) { return false; }
            string effectName = effect.effectData.graphicName;
            switch (effectName)
            {
                case "jumpDust":
                case "doubleJumpDust":
                case "landDust":
                case "landDustFront":
                case "moveDust":
                case "slowDust":
                case "walljumpDust":
                    return true;
                default:
                    return false;
            }
        }

        bool PlayerHitEffect(EffectEntity effect)
        {
            if (playerHitEffects) { return false; }
            string effectName = effect.effectData.graphicName;
            switch (effectName)
            {
                case "hitEffect2":
                case "hitEffect_1":
                case "hitEffect_2":
                case "hitEffect_3":
                case "hitEffect_4":
                case "deflectHitEffect_1":
                case "deflectHitEffect_2":
                case "deflectHitEffect_3":
                case "deflectHitEffect_4":
                case "smashBlast_1":
                case "smashBlast_2":
                case "smashBlast_3":
                case "smashBlast_4":
                case "smashBlastSmall_1":
                case "smashBlastSmall_2":
                case "smashBlastSmall_3":
                case "smashBlastSmall_4":
                case "highSpeedReleaseEffect":
                    return true;
                default:
                    return false;
            }
        }

        bool BallEffect(EffectEntity effect)
        {
            if (ballEffects) { return false; }

            string effectName = effect.effectData.graphicName;
            switch (effectName)
            {
                case "hitEffectDust":
                case "hitEffectDustSmall":
                    return true;
                default:
                    return false;
            }
        }

        bool SuperEffect(EffectEntity effect)
        {
            if (superEffects) { return false; }

            string effectName = effect.effectData.graphicName;
            switch (effectName)
            {
                case "diceSuper":
                case "bubblePop":
                case "candySplash":
                case "sonataSoundwave":
                case "sonataSoundwaveMini":
                case "ToxicSplash":
                    return true;
                default:
                    return false;
            }
        }

#if DEBUG
        void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 200, 30), "Hello World v2", GUI.skin.label);
        }
#endif
    }
}
