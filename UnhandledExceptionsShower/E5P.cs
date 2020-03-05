using EFT;
using EFT.Interactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnhandledException
{
    class E5P
    {
        public class Players {
            public class Update {

                public static void PlayerList() {
                    try
                    {
                        Cons.AliveCount.Reset();
                        Cons.Main.tPlayer = new List<Player>();
                        var enum_Players = Cons.Main._GameWorld.RegisteredPlayers.GetEnumerator();
                        int groupNum = 0;
                        while (enum_Players.MoveNext())
                        {
                            var p = enum_Players.Current;
                            if (p.PointOfView == EPointOfView.FirstPerson)
                            {
                                // updating local player data
                                Cons.Main._localPlayer = p;
                                Cons.LocalPlayer.SetGroup(p);
                                Cons.LocalPlayer.Weapon.SetRecoil();
                                Cons.LocalPlayer.Weapon.UpdateAmmo();
                                Cons.LocalPlayer.Status.UpdateStatus();
                            }
                            else
                            {
                                if (p.HealthController.IsAlive)
                                {
                                    float distance = FastMath.FD(Camera.main.transform.position, p.Transform.position);
                                    Calculations.CalculateAlive(distance);
                                    // add to list if its not below 1m and not above max distance also it player is in screen (not counting width of screen - cause we are using snaplines as an radar
                                    if (!Cons.LocalPlayer.isInYourGroup(p) && p.GroupId != "0" && p.GroupId != "" && p.GroupId != null && p.GroupId != string.Empty)
                                    {
                                        try
                                        {
                                            Types.GroupTable.Add(p.GroupId, groupNum++);
                                        }
                                        catch (Exception)
                                        {
                                            groupNum--;
                                        }
                                    }
                                    if (distance > 1f && distance <= Cons.Distances.Players) // && FUNC.isInScreenYZ(FUNC.W2S(p.Transform.position))
                                    {
                                        Cons.Main.tPlayer.Add(p);
                                    }
                                }
                            }
                        }
                        Cons.Main._players = Cons.Main.tPlayer;
                    }
                    catch (Exception e)
                    {
                        ErrorHandler.Catch("Get_Players", e);
                    }
                }
                public static void DeadBodyList() {
                    try
                    {
                        Cons.Main.tCorpses = new List<LootItem>();
                        List<LootItem>.Enumerator temporalCorpsesEnum = Cons.Main._GameWorld.LootItems.GetValuesEnumerator().GetEnumerator();
                        while (temporalCorpsesEnum.MoveNext())
                        {
                            LootItem temp = temporalCorpsesEnum.Current;
                            if (temp.GetType() == Types.Corpse || temp.GetType() == Types.ObserverCorpse)
                                Cons.Main.tCorpses.Add(temp);
                        }
                        Cons.Main._corpses = Cons.Main.tCorpses;
                        temporalCorpsesEnum.Dispose();
                    }
                    catch (Exception e)
                    {
                        ErrorHandler.Catch("Get_Corpses", e);
                    }
                }
            }
            public class Draw {
                public static void Players()
                {
                    try
                    {
                        #region [INITIALS] - to skip data overflow (incase)
                        float deltaDistance = 25f;
                        string playerDisplayName = "";
                        float devLabel = 1f;
                        string Status = "";
                        var LabelSize = new GUIStyle { fontSize = 12 };
                        Color playerColor = Constants.Colors.ESP.npc;
                        float distancesAxisY_0 = 0;
                        float distancesAxisY_1 = 0;
                        float distancesAxisY_2 = 0;
                        Color Backup;
                        #endregion
                        var e = Cons.Main._players.GetEnumerator();
                        while (e.MoveNext())
                        {
                            var player = e.Current;
                            if (FUNC.isInScreenYZ(FUNC.W2S(player.Transform.position)))
                            {
                                //fix for colors not holds after leaving screen
                                Calculations.PlayerType playerType = Calculations.PlayerType.Scav;
                                playerDisplayName = Calculations.PlayerName(player, ref playerType);
                                playerColor = Calculations.PlayerColor(playerType);
                                #region [Snap.Lines]
                                if (Cons.Bools.SnapLines && player != Cons.Main._localPlayer)
                                {
                                    Calculations.SnapLines(player, playerColor);
                                }
                                #endregion
                                if (FUNC.isInScreenRestricted(FUNC.W2S(player.Transform.position)))
                                {
                                    float dTO = FastMath.FD(Camera.main.transform.position, player.Transform.position);
                                    // main head vector 3d (x,y,z)
                                    Vector3 pHeadVector = FUNC.W2S(player.PlayerBones.Head.position);
                                    // setting head size comparing head position and neck position and multiplying by 1.5 (actually its head size)
                                    float find_sizebox = Math.Abs(pHeadVector.y - FUNC.W2S(player.PlayerBones.Neck.position).y) * 1.5f; // size of the head - its not good but its scaling without much maths
                                                                                                                                        // making sure head will not be too big
                                    find_sizebox = (find_sizebox > 30f) ? 30f : find_sizebox;
                                    float half_sizebox = (find_sizebox > 30f) ? 15f : find_sizebox / 2f;
                                    // size of fonts depending on distance
                                    int FontSize = 12;
                                    FastMath.DistSizer(dTO, ref FontSize, ref deltaDistance, ref devLabel);
                                    LabelSize.fontSize = FontSize;
                                    //create 3 size table of distances for texts (name, status, weapon)
                                    distancesAxisY_0 = deltaDistance + 10f;
                                    distancesAxisY_1 = distancesAxisY_0 + FontSize + 1;
                                    distancesAxisY_2 = distancesAxisY_1 + FontSize + 1;
                                    Status = Calculations.GetPlayerTotalHealth(player); // Health here 
                                    #region BONE-DUMP
                                    /*if (dTO < 20f)
                                    {
                                        //string forDump = "";
                                        try
                                        {
                                            for (int i = 0; i < 134; i++)
                                            {
                                                if ((i > 13 && i <= 27) || i == 38 || i == 39 || i == 40 || i == 41 || (i >= 66 && i <= 70) || i == 132)
                                                {
                                                    string name = "";
                                                    try
                                                    {
                                                        name = FUNC.Bones.SkeletonBoneName(player.PlayerBones.AnimatedTransform.Original.gameObject.GetComponent<PlayerBody>().SkeletonRootJoint, i);
                                                        name = name.Split('/')[name.Split('/').Length - 1];
                                                        Vector3 pos = FUNC.W2S(FUNC.Bones.GetBonePosByID(player, i));
                                                        Drawing.Text(new Rect(pos.x, Screen.height - pos.y, 20f, 20f), i.ToString());
                                                    }
                                                    catch (Exception)
                                                    {
                                                        name = "bad";
                                                    }
                                                    //forDump = forDump + name + " - " + i + Environment.NewLine;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ErrorHandler.Catch("DUMPED", ex);
                                        }
                                        //ErrorHandler.Dump("DUMPED", forDump);
                                    }*/
                                    #endregion
                                    #region [BONE-ESP]
                                    if (Cons.Bools.ShowBones)
                                    {
                                        Calculations.PlayerBones(dTO, player);
                                    }
                                    #endregion
                                    if (playerType != Calculations.PlayerType.TeamMate)
                                    {
                                        Backup = GUI.color;
                                        GUI.color = Color.red;
                                        Drawing.Special.DrawPoint(pHeadVector.x - half_sizebox, (float)(Screen.height - pHeadVector.y) - half_sizebox, find_sizebox, Constants.Colors.Red);
                                        GUI.color = Backup;
                                    }
                                    #region [VISIBILITY-CHECK]
                                    string isVisible = "";
                                    if (Raycast.BodyRaycastCheck(player.gameObject, pHeadVector))
                                    {
                                        isVisible = "+";
                                    }
                                    #endregion
                                    #region [Group-Num]
                                    int gInt;
                                    string groupID = "";
                                    try
                                    {
                                        if (Types.GroupTable.TryGetValue(player.GroupId, out gInt))
                                        {
                                            groupID = "{" + gInt + "}";
                                        }
                                    }
                                    catch (Exception) { }
                                    #endregion
                                    #region [INIT-Texts]
                                    string nameNickname = $"{playerDisplayName}{groupID}";
                                    string playerStatus = $"{isVisible}[{(int)dTO}m] {Status}";
                                    string WeaponName = "";
                                    #endregion

                                    #region [TRY-DecodeWeaponName]
                                    try
                                    {
                                        WeaponName = player.Weapon.ShortName.Localized();
                                    }
                                    catch (Exception)
                                    {
                                        WeaponName = "No Weapon";
                                    }
                                    #endregion

                                    // set colors now
                                    LabelSize.normal.textColor = playerColor;
                                    #region Slot 0 - Player Name (vector, size, drawing)
                                    if (nameNickname != "")
                                    {
                                        Vector2 vector_playerName = GUI.skin.GetStyle(nameNickname).CalcSize(new GUIContent(nameNickname));
                                        float player_NameText = (devLabel == 1f) ? vector_playerName.x : (vector_playerName.x / devLabel);
                                        Drawing.Special.DrawText(
                                            nameNickname,
                                            pHeadVector.x - player_NameText / 2f,
                                            (float)Screen.height - FUNC.W2S(player.PlayerBones.Head.position).y - distancesAxisY_0,
                                            vector_playerName,
                                            LabelSize,
                                            playerColor
                                        );
                                    }
                                    #endregion
                                    #region Slot 1 - Status (distance, health)
                                    Vector2 vector_playerStatus = GUI.skin.GetStyle(playerStatus).CalcSize(new GUIContent(playerStatus));
                                    float player_TextWidth = (devLabel == 1f) ? vector_playerStatus.x : (vector_playerStatus.x / devLabel);
                                    Drawing.Special.DrawText(
                                        playerStatus,
                                        pHeadVector.x - player_TextWidth / 2f,
                                        (float)Screen.height - FUNC.W2S(player.PlayerBones.Head.position).y - distancesAxisY_1,
                                        vector_playerStatus,
                                        LabelSize,
                                        playerColor
                                    );
                                    #endregion
                                    #region Slot 2 - Weapon Name (vector, size, drawing) - if not empty
                                    if (WeaponName != "")
                                    {
                                        Vector2 vector_WeaponName = GUI.skin.GetStyle(WeaponName).CalcSize(new GUIContent(WeaponName));
                                        float player_WeaponName = (devLabel == 1f) ? vector_WeaponName.x : (vector_WeaponName.x / devLabel);
                                        Drawing.Special.DrawText(
                                            WeaponName,
                                            pHeadVector.x - player_TextWidth / 2f,
                                            (float)Screen.height - FUNC.W2S(player.PlayerBones.Head.position).y - distancesAxisY_2,
                                            vector_WeaponName,
                                            LabelSize,
                                            playerColor
                                        );
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorHandler.Catch("e5p_Players_Draw_Players", e);
                    }
                }
                public static void DeadBodies()
                {
                    if (Cons.Main._corpses == null)
                        return;
                    var e = Cons.Main._corpses.GetEnumerator();
                    var LabelSize = new GUIStyle { fontSize = 12 };
                    float deltaDistance = 25f;
                    float devLabel = 1f;
                    while (e.MoveNext())
                    {
                        try
                        {
                            var item = e.Current;
                            if (item != null)
                            {
                                if (FUNC.isInScreenRestricted(FUNC.W2S(item.transform.position)))
                                { // do not display out of bounds items
                                    float distance = FastMath.FD(Camera.main.transform.position, item.transform.position);
                                    if (distance < Cons.Distances.Corpses)
                                    {
                                        Vector3 itemPosition = FUNC.W2S(item.transform.position);
                                        float[] boxSize = new float[2] { 3f, 1.5f };
                                        int FontSize = 12;
                                        FastMath.DistSizer(distance, ref FontSize, ref deltaDistance, ref devLabel);
                                        LabelSize.fontSize = FontSize;
                                        LabelSize.normal.textColor = new Color(.7f, .7f, .7f, .8f);
                                        string distanceText = $"{(int)distance}m";
                                        Vector2 sizeOfText = GUI.skin.GetStyle(distanceText).CalcSize(new GUIContent(distanceText));
                                        Drawing.Special.DrawPoint(
                                            itemPosition.x - boxSize[1],
                                            (float)(Screen.height - itemPosition.y) - boxSize[1], 
                                            boxSize[0], 
                                            Constants.Colors.ESP.bodies
                                        );
                                        Drawing.Special.DrawText(
                                            distanceText,
                                            itemPosition.x - sizeOfText.x / 2f,
                                            (float)(Screen.height - itemPosition.y) - deltaDistance - 1,
                                            sizeOfText,
                                            LabelSize,
                                            Constants.Colors.ESP.bodies
                                        );
                                    }
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ErrorHandler.Catch("Corpses", ex);
                        }
                    }
                }
            }
            public class Calculations {
                #region CalculateAlivePlayers - Incrementor
                public static void CalculateAlive(float distance) {
                    Cons.AliveCount.All++; // all players count - on map
                    if (distance > 0f && distance <= 25f)
                    {
                        Cons.AliveCount.dist_0_25++; // players between 0m and 25m
                    }
                    if (distance > 25f && distance <= 50f)
                    {
                        Cons.AliveCount.dist_25_50++; // players between 25m and 50m
                    }
                    if (distance > 50f && distance <= 100f)
                    {
                        Cons.AliveCount.dist_50_100++; // players between 50m and 100m
                    }
                    if (distance > 100f && distance <= 250f)
                    {
                        Cons.AliveCount.dist_100_250++; // players between 100m and 250m
                    }
                    if (distance > 250f && distance <= 1000f)
                    {
                        Cons.AliveCount.dist_250_1000++; // players between 250m and 1000m
                    }
                }
                #endregion
                #region Draw - SnapLines
                public static void SnapLines(Player p, Color c) {
                    Vector3 w2s = FUNC.W2S(p.PlayerBones.RootJoint.position);
                    if (FUNC.isInScreenYZ(w2s))
                    {
                        Drawing.DrawLine(
                            new Vector2(
                                (Screen.width / 2),
                                Screen.height
                                ),
                            new Vector2(
                                w2s.x,
                                Screen.height - w2s.y
                                ),
                            c
                       );
                    }
                }
                #endregion
                #region Get - Player Total Health
                public static string GetPlayerTotalHealth(Player p){
                    return ((int)(p.HealthController.GetBodyPartHealth(EFT.HealthSystem.EBodyPart.Common).Current)).ToString() + " hp";
                }
                #endregion
                #region ENUM - PlayerType
                public enum PlayerType
                {
                    Scav,
                    PlayerScav,
                    Player,
                    TeamMate,
                    Boss
                }
                #endregion
                #region Draw - PlayerBones
                public static void PlayerBones(float distance, Player player)
                {
                    if (distance < 100f)
                    {
                        var pRPVect = FUNC.W2S(player.PlayerBones.RightPalm.position);
                        var PLPVect = FUNC.W2S(player.PlayerBones.LeftPalm.position);
                        var PLShVect = FUNC.W2S(player.PlayerBones.LeftShoulder.position);
                        var PLRShVect = FUNC.W2S(player.PlayerBones.RightShoulder.position);
                        var PLNeckVect = FUNC.W2S(player.PlayerBones.Neck.position);
                        var PLCentrVect = FUNC.W2S(player.PlayerBones.Pelvis.position);
                        //var PLRTighVect = FUNC.W2S(player.PlayerBones.RightThigh2.position);
                        //var PLLTighVect = FUNC.W2S(player.PlayerBones.LeftThigh2.position);
                        var PLRFootVect = FUNC.W2S(player.PlayerBones.KickingFoot.position);
                        var PLLFootVect = FUNC.W2S(FUNC.Bones.GetBonePosByID(player, 18));
                        var PLLBowVect = FUNC.W2S(FUNC.Bones.GetBonePosByID(player, 91));
                        var PLRBowVect = FUNC.W2S(FUNC.Bones.GetBonePosByID(player, 112));
                        var PLLKneeVect = FUNC.W2S(FUNC.Bones.GetBonePosByID(player, 17));
                        var PLRKneeVect = FUNC.W2S(FUNC.Bones.GetBonePosByID(player, 22));
                        Color Backup = GUI.color;
                        GUI.color = Color.white;
                        if (FUNC.isInScreenRestricted(PLNeckVect) && FUNC.isInScreenRestricted(PLCentrVect))
                            Drawing.DrawLine(new Vector2(PLNeckVect.x, (float)Screen.height - PLNeckVect.y), new Vector2(PLCentrVect.x, (float)Screen.height - PLCentrVect.y), Color.white, 1f);
                        if (FUNC.isInScreenRestricted(PLShVect) && FUNC.isInScreenRestricted(PLLBowVect))
                            Drawing.DrawLine(new Vector2(PLShVect.x, (float)Screen.height - PLShVect.y), new Vector2(PLLBowVect.x, (float)Screen.height - PLLBowVect.y), Color.white, 1f);
                        if (FUNC.isInScreenRestricted(PLRShVect) && FUNC.isInScreenRestricted(PLRBowVect))
                            Drawing.DrawLine(new Vector2(PLRShVect.x, (float)Screen.height - PLRShVect.y), new Vector2(PLRBowVect.x, (float)Screen.height - PLRBowVect.y), Color.white, 1f);
                        if (FUNC.isInScreenRestricted(PLLBowVect) && FUNC.isInScreenRestricted(PLPVect))
                            Drawing.DrawLine(new Vector2(PLLBowVect.x, (float)Screen.height - PLLBowVect.y), new Vector2(PLPVect.x, (float)Screen.height - PLPVect.y), Color.white, 1f);
                        if (FUNC.isInScreenRestricted(PLRBowVect) && FUNC.isInScreenRestricted(pRPVect))
                            Drawing.DrawLine(new Vector2(PLRBowVect.x, (float)Screen.height - PLRBowVect.y), new Vector2(pRPVect.x, (float)Screen.height - pRPVect.y), Color.white, 1f);
                        if (FUNC.isInScreenRestricted(PLRShVect) && FUNC.isInScreenRestricted(PLShVect))
                            Drawing.DrawLine(new Vector2(PLRShVect.x, (float)Screen.height - PLRShVect.y), new Vector2(PLShVect.x, (float)Screen.height - PLShVect.y), Color.white, 1f);
                        if (FUNC.isInScreenRestricted(PLLKneeVect) && FUNC.isInScreenRestricted(PLCentrVect))
                            Drawing.DrawLine(new Vector2(PLLKneeVect.x, (float)Screen.height - PLLKneeVect.y), new Vector2(PLCentrVect.x, (float)Screen.height - PLCentrVect.y), Color.white, 1f);
                        if (FUNC.isInScreenRestricted(PLRKneeVect) && FUNC.isInScreenRestricted(PLCentrVect))
                            Drawing.DrawLine(new Vector2(PLRKneeVect.x, (float)Screen.height - PLRKneeVect.y), new Vector2(PLCentrVect.x, (float)Screen.height - PLCentrVect.y), Color.white, 1f);
                        if (FUNC.isInScreenRestricted(PLLKneeVect) && FUNC.isInScreenRestricted(PLLFootVect))
                            Drawing.DrawLine(new Vector2(PLLKneeVect.x, (float)Screen.height - PLLKneeVect.y), new Vector2(PLLFootVect.x, (float)Screen.height - PLLFootVect.y), Color.white, 1f);
                        if (FUNC.isInScreenRestricted(PLRKneeVect) && FUNC.isInScreenRestricted(PLRFootVect))
                            Drawing.DrawLine(new Vector2(PLRKneeVect.x, (float)Screen.height - PLRKneeVect.y), new Vector2(PLRFootVect.x, (float)Screen.height - PLRFootVect.y), Color.white, 1f);
                        GUI.color = Backup;
                    }
                }
                #endregion
                #region Get - Player Color
                public static Color PlayerColor(PlayerType playerType)
                {
                    switch (playerType)
                    {
                        case PlayerType.TeamMate: // this one must be above Player
                            return Constants.Colors.ESP.group;
                        case PlayerType.Player:
                            return Constants.Colors.ESP.player[1];
                        case PlayerType.Scav:
                            return Constants.Colors.ESP.npc;
                        case PlayerType.PlayerScav:
                            return Constants.Colors.ESP.scav_player;
                        case PlayerType.Boss:
                            return Constants.Colors.ESP.npc;
                        default:
                            return Constants.Colors.ESP.npc;
                    }
                }
                #endregion
                #region Get - Player Name
                public static string PlayerName(Player player, ref PlayerType playerType)
                {
                    if (player.Profile.Info.RegistrationDate <= 0)
                    {
                        playerType = PlayerType.Scav;
                        return "";
                    }
                    else if (Cons.LocalPlayer.isInYourGroup(player))
                    {
                        playerType = PlayerType.TeamMate;
                        if (Cons.Bools.StreamerMode)
                        {
                            return "team";
                        }
                        else
                        {
                            return player.Profile.Info.Nickname;
                        }
                    }
                    else if (player.Profile.Info.Side == EPlayerSide.Savage)
                    {
                        playerType = PlayerType.PlayerScav;
                        return "";
                    }
                    else
                    {
                        playerType = PlayerType.Player;
                        if (Cons.Bools.StreamerMode)
                        {
                            return player.Profile.Info.Side.ToString() + " [" + player.Profile.Info.Level.ToString() + "]";
                        }
                        else
                        {
                            return player.Profile.Info.Nickname + " [" + player.Profile.Info.Level.ToString() + "]";
                        }
                    }
                }
                #endregion
            }
        }
        public class Exfils {
            public class Update
            {
                public static void ExfilsList() {
                    try
                    {
                        if (Cons.Main._exfils == null)
                        {
                            Cons.Main.tExfils = new List<ExfiltrationPoint>();
                            for (int i = 0; i < Cons.Main._GameWorld.ExfiltrationController.ExfiltrationPoints.Length; i++)
                            {
                                Cons.Main.tExfils.Add(Cons.Main._GameWorld.ExfiltrationController.ExfiltrationPoints[i]);
                            }
                            Cons.Main._exfils = Cons.Main.tExfils;
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorHandler.Catch("e5p_Exfils_Update_ExfilsList", e);
                    }
                }
            }
            public class Draw
            {
                public static void Exfils()
                {
                    if (Cons.Main._exfils == null)
                        return;
                    var e = Cons.Main._exfils.GetEnumerator();
                    var LabelSize = new GUIStyle { fontSize = 12 };
                    float deltaDistance = 25f;
                    float devLabel = 1f;
                    while (e.MoveNext())
                    {
                        try
                        {
                            var exfil = e.Current;
                            if (exfil != null)
                            {
                                if (FUNC.isInScreenRestricted(FUNC.W2S(exfil.transform.position)))
                                { // do not display out of bounds items
                                    float distance = FastMath.FD(Camera.main.transform.position, exfil.transform.position);
                                    if (distance < Cons.Distances.Exfils)
                                    {
                                        Vector3 itemPosition = FUNC.W2S(exfil.transform.position);
                                        float[] boxSize = new float[2] { 3f, 1.5f };
                                        int FontSize = 12;
                                        FastMath.DistSizer(distance, ref FontSize, ref deltaDistance, ref devLabel);
                                        LabelSize.fontSize = FontSize;
                                        LabelSize.normal.textColor = new Color(.7f, .7f, .7f, .8f);
                                        string requirements = (exfil.HasRequirements) ? "req" : "";
                                        string exfil_Status = Calculations.TypeOfExfiltration(exfil.Status);

                                        string distanceText = $"({(int)distance}m){requirements}";
                                        Vector2 sizeOfText2 = GUI.skin.GetStyle(exfil_Status).CalcSize(new GUIContent(exfil_Status));
                                        Vector2 sizeOfText = GUI.skin.GetStyle(distanceText).CalcSize(new GUIContent(distanceText));
                                        Drawing.Special.DrawPoint(itemPosition.x - boxSize[1], (float)(Screen.height - itemPosition.y) - boxSize[1], boxSize[0], Constants.Colors.Red);
                                        Drawing.Special.DrawText(
                                            exfil_Status,
                                            itemPosition.x - sizeOfText2.x / 2f,
                                            (float)Screen.height - itemPosition.y - deltaDistance - FontSize - 1,
                                            sizeOfText2,
                                            LabelSize,
                                            Constants.Colors.Red
                                        );
                                        Drawing.Special.DrawText(
                                            distanceText,
                                            itemPosition.x - sizeOfText.x / 2f,
                                            (float)Screen.height - itemPosition.y - deltaDistance - 1,
                                            sizeOfText,
                                            LabelSize,
                                            Constants.Colors.Red
                                        );
                                    }
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ErrorHandler.Catch("Exfils", ex);
                        }
                    }
                }
            }
            public class Calculations
            {
                public static string TypeOfExfiltration(EExfiltrationStatus status)
                {
                    switch (status)
                    {
                        case EExfiltrationStatus.AwaitsManualActivation:
                            return "ManualActivation";
                        case EExfiltrationStatus.Countdown:
                            return "Timer";
                        case EExfiltrationStatus.NotPresent:
                            return "n/a";
                        case EExfiltrationStatus.Pending:
                            return "Pending";
                        case EExfiltrationStatus.RegularMode:
                            return "Default";
                        case EExfiltrationStatus.UncompleteRequirements:
                            return "Requirements";
                        default:
                            return "";
                    }
                }
            }
        }
        public class Items {
            public class Update
            {
                public static void ItemsList() {
                    try
                    {
                        Cons.Main.tItems = new List<LootItem>();
                        List<LootItem>.Enumerator temporalItemsEnum = Cons.Main._GameWorld.LootItems.GetValuesEnumerator().GetEnumerator();
                        while (temporalItemsEnum.MoveNext())
                        {
                            LootItem temp = temporalItemsEnum.Current;
                            if (temp.GetType() == Types.LootItem || temp.GetType() == Types.ObservedLootItem)
                            {
                                if (Cons.LootSearcher == "")
                                    Cons.Main.tItems.Add(temp);
                                try
                                {
                                    if (temp.Item.ShortName.Localized().ToLower().IndexOf(Cons.LootSearcher) >= 0)
                                    {
                                        Cons.Main.tItems.Add(temp);
                                    }
                                }
                                catch (Exception e) 
                                {
                                    ErrorHandler.Catch("LootItem_Translate_SearcherIndexOf", e);
                                }
                            }
                        }
                        Cons.Main._lootItems = Cons.Main.tItems;
                        temporalItemsEnum.Dispose();

                    }
                    catch (Exception e)
                    {
                        ErrorHandler.Catch("Get_LootItems", e);
                    }
                }
                public static void ContainerList() {
                    Cons.Main._containers = Cons.Main._GameWorld.ItemOwners.GetEnumerator();
                }
            }
            public class Draw
            {
                public static void Items()
                {
                    if (Cons.Main._lootItems == null)
                        return;
                    var e = Cons.Main._lootItems.GetEnumerator();
                    var LabelSize = new GUIStyle { fontSize = 12 };
                    float deltaDistance = 25f;
                    float devLabel = 1f;
                    while (e.MoveNext())
                    {
                        try
                        {
                            var item = e.Current;
                            if (item != null)
                            {
                                if (FUNC.isInScreenRestricted(FUNC.W2S(item.transform.position)))
                                { // do not display out of bounds items
                                    float distance = FastMath.FD(Camera.main.transform.position, item.transform.position);
                                    if (distance < Cons.Distances.Loot)
                                    {
                                        Vector3 itemPosition = FUNC.W2S(item.transform.position);
                                        float[] boxSize = new float[2] { 3f, 1.5f };
                                        int FontSize = 12;
                                        FastMath.DistSizer(distance, ref FontSize, ref deltaDistance, ref devLabel);
                                        LabelSize.fontSize = FontSize;
                                        LabelSize.normal.textColor = Constants.Colors.ESP.items;
                                        string distanceText = $"{(int)distance}m";
                                        string nameText = "";
                                        try
                                        {
                                            nameText = item.Item.ShortName.Localized();
                                        }
                                        catch (Exception exp)
                                        {
                                            ErrorHandler.Catch("LootTranslation", exp, item.Item.ShortName);
                                            nameText = "";
                                        }
                                        Vector2 sizeOfText = GUI.skin.GetStyle(distanceText).CalcSize(new GUIContent(distanceText));
                                        Drawing.Special.DrawPoint(itemPosition.x - boxSize[1], (float)(Screen.height - itemPosition.y) - boxSize[1], boxSize[0], Constants.Colors.ESP.items);
                                        Drawing.Special.DrawText(
                                            distanceText,
                                            itemPosition.x - sizeOfText.x / 2f,
                                            (float)Screen.height - itemPosition.y - deltaDistance - 1,
                                            sizeOfText,
                                            LabelSize,
                                            Constants.Colors.ESP.items
                                            );
                                        Drawing.Special.DrawText(
                                            nameText,
                                            itemPosition.x - sizeOfText.x / 2f,
                                            (float)Screen.height - itemPosition.y - deltaDistance - FontSize - 1,
                                            sizeOfText,
                                            LabelSize,
                                            Constants.Colors.ESP.items
                                            );
                                    }
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ErrorHandler.Catch("LootItems", ex);
                        }
                    }
                }
                public static void Containers()
                {
                    var e = Cons.Main._containers;
                    var LabelSize = new GUIStyle { fontSize = 12 };
                    float deltaDistance = 25f;
                    float devLabel = 1f;
                    while (e.MoveNext())
                    {
                        try
                        {
                            var Container = e.Current.Key;
                            var Location = e.Current.Value;
                            if (Container.RootItem.IsContainer)
                            {
                                #region Find Item Inside if specified
                                if (!Calculations.ItemIsInside(Container)) { continue; }
                                #endregion
                                if (FUNC.W2S(Location.Transform.position).z > 0.01f)
                                { // do not display out of bounds items
                                    float distance = FastMath.FD(Camera.main.transform.position, Location.Transform.position);
                                    if (distance < Cons.Distances.Crates)
                                    {
                                        Vector3 itemPosition = FUNC.W2S(Location.Transform.position);
                                        float[] boxSize = new float[2] { 3f, 1.5f };
                                        int FontSize = 12;
                                        FastMath.DistSizer(distance, ref FontSize, ref deltaDistance, ref devLabel);
                                        LabelSize.fontSize = FontSize;
                                        LabelSize.normal.textColor = new Color(.7f, .7f, .7f, .8f);
                                        string distanceText = $"<{(int)distance}m>";
                                        Vector2 sizeOfText = GUI.skin.GetStyle(distanceText).CalcSize(new GUIContent(distanceText));
                                        Drawing.Special.DrawPoint(itemPosition.x - boxSize[1], (float)(Screen.height - itemPosition.y) - boxSize[1], boxSize[0], Constants.Colors.ESP.items);
                                        Drawing.Special.DrawText(
                                            distanceText,
                                            itemPosition.x - sizeOfText.x / 2f,
                                            (float)Screen.height - itemPosition.y - deltaDistance - 1,
                                            sizeOfText,
                                            LabelSize,
                                            Constants.Colors.ESP.items
                                            );
                                    }
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ErrorHandler.Catch("Containers", ex);
                        }
                    }
                }
            }
            public class Calculations
            {
                public static bool ItemIsInside(GInterface163 Container) {
                    if (Cons.LootSearcher == "")
                        return true;
                    var inside = Container.Items.GetEnumerator();
                    while (inside.MoveNext())
                    {
                        var item = inside.Current;
                        if (item.ShortName.Localized().ToLower().IndexOf(Cons.LootSearcher) >= 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }
        public class Throwables {
            public class Update
            {
                public static void ThrowableList() {
                    try
                    {
                        Cons.Main.tGrenades = new List<Throwable>();
                        List<Throwable>.Enumerator grenades = Cons.Main._GameWorld.Grenades.GetValuesEnumerator().GetEnumerator();
                        while (grenades.MoveNext())
                        {
                            Cons.Main.tGrenades.Add(grenades.Current);
                        }
                        Cons.Main._grenades = Cons.Main.tGrenades;
                        grenades.Dispose();
                    }
                    catch (Exception e)
                    {
                        ErrorHandler.Catch("Get_GrenadesThrown", e);
                    }
                }
            }
            public class Draw
            {
                public static void Grenades()
                {
                    // 100m for grenades is more then enough
                    if (Cons.Main._grenades == null || Cons.Main._localPlayer == null)
                        return;
                    var e = Cons.Main._grenades.GetEnumerator();
                    var LabelSize = new GUIStyle { fontSize = 12 };
                    float deltaDistance = 25f;
                    float devLabel = 1f;
                    while (e.MoveNext())
                    {
                        try
                        {
                            var throwable = e.Current;
                            if (throwable != null)
                            {
                                if (FUNC.isInScreenRestricted(FUNC.W2S(throwable.transform.position)))
                                {
                                    float dTO = FastMath.FD(Camera.main.transform.position, throwable.transform.position);
                                    if (dTO > Cons.Distances.Grenade)
                                        continue;

                                    Vector3 pGrenadePosition = FUNC.W2S(throwable.transform.position);
                                    int FontSize = 10;
                                    FastMath.DistSizer(dTO, ref FontSize, ref deltaDistance, ref devLabel);
                                    LabelSize.fontSize = FontSize;
                                    LabelSize.normal.textColor = Constants.Colors.ESP.grenades;
                                    string distanceText = $"{(int)dTO}m";
                                    Vector2 sizeOfText = GUI.skin.GetStyle(distanceText).CalcSize(new GUIContent(distanceText));
                                    Drawing.Special.DrawPoint(
                                        pGrenadePosition.x - 1.5f,
                                        (float)(Screen.height - pGrenadePosition.y) - 1.5f,
                                        3f,
                                        Constants.Colors.ESP.grenades
                                    );
                                    Drawing.Special.DrawText(
                                        distanceText,
                                        pGrenadePosition.x - sizeOfText.x / 2f,
                                        (float)Screen.height - pGrenadePosition.y - deltaDistance - 1,
                                        sizeOfText,
                                        LabelSize,
                                        Constants.Colors.ESP.grenades
                                    );
                                }
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            ErrorHandler.Catch("Grenade", ex);
                        }
                    }

                }
            }
            public class Calculations
            {

            }
        }
    }
}
