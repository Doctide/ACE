DELETE FROM `weenie` WHERE `class_Id` = 490150;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490150, 'drudgeflyingmachineoutpost', 10, '2005-02-09 10:00:00') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490150,   1,         16) /* ItemType - Creature */
     , (490150,   2,         63) /* CreatureType - Statue */
     , (490150,   6,         -1) /* ItemsCapacity */
     , (490150,   7,         -1) /* ContainersCapacity */
     , (490150,  16,         32) /* ItemUseable - Remote */
     , (490150,  25,          5) /* Level */
     , (490150,  27,          0) /* ArmorType - None */
     , (490150,  93,    2098204) /* PhysicsState - Ethereal, ReportCollisions, IgnoreCollisions, Gravity, ReportCollisionsAsEnvironment */
     , (490150,  95,          3) /* RadarBlipColor - White */
     , (490150, 133,          0) /* ShowableOnRadar - Undefined */
     , (490150, 134,         16) /* PlayerKillerStatus - RubberGlue */
     , (490150, 146,         87) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490150,   1, True ) /* Stuck */
     , (490150,   8, True ) /* AllowGive */
     , (490150,  12, True ) /* ReportCollisions */
     , (490150,  13, True ) /* Ethereal */
     , (490150,  19, False) /* Attackable */
     , (490150,  41, True ) /* ReportCollisionsAsEnvironment */
     , (490150,  50, True ) /* NeverFailCasting */
     , (490150,  52, True ) /* AiImmobile */
     , (490150,  82, True ) /* DontTurnOrMoveWhenGiving */
     , (490150,  83, True ) /* NpcLooksLikeObject */
     , (490150,  90, True ) /* NpcInteractsSilently */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490150,   1,       5) /* HeartbeatInterval */
     , (490150,   2,       0) /* HeartbeatTimestamp */
     , (490150,   3,    0.16) /* HealthRate */
     , (490150,   4,       5) /* StaminaRate */
     , (490150,   5,       1) /* ManaRate */
     , (490150,  13,    0.79) /* ArmorModVsSlash */
     , (490150,  14,    0.79) /* ArmorModVsPierce */
     , (490150,  15,     0.8) /* ArmorModVsBludgeon */
     , (490150,  16,       1) /* ArmorModVsCold */
     , (490150,  17,       1) /* ArmorModVsFire */
     , (490150,  18,       1) /* ArmorModVsAcid */
     , (490150,  19,       1) /* ArmorModVsElectric */
     , (490150,  54,      55) /* UseRadius */
     , (490150,  64,       1) /* ResistSlash */
     , (490150,  65,       1) /* ResistPierce */
     , (490150,  66,       1) /* ResistBludgeon */
     , (490150,  67,       1) /* ResistFire */
     , (490150,  68,       1) /* ResistCold */
     , (490150,  69,       1) /* ResistAcid */
     , (490150,  70,       1) /* ResistElectric */
     , (490150,  71,       1) /* ResistHealthBoost */
     , (490150,  72,       1) /* ResistStaminaDrain */
     , (490150,  73,       1) /* ResistStaminaBoost */
     , (490150,  74,       1) /* ResistManaDrain */
     , (490150,  75,       1) /* ResistManaBoost */
     , (490150, 104,      10) /* ObviousRadarRange */
     , (490150, 125,       1) /* ResistHealthDrain */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490150,   1, 'Hot Air Balloon') /* Name */
     , (490150,  15, 'With proper instructions it might be possible to fly this thing.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490150,   1, 0x020012AC) /* Setup */
     , (490150,   2, 0x0900017F) /* MotionTable */
     , (490150,   3, 0x20000001) /* SoundTable */
     , (490150,   8, 0x06003711) /* Icon */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490150,   1, 200, 0, 0) /* Strength */
     , (490150,   2,  10, 0, 0) /* Endurance */
     , (490150,   3,  10, 0, 0) /* Quickness */
     , (490150,   4,  10, 0, 0) /* Coordination */
     , (490150,   5,  10, 0, 0) /* Focus */
     , (490150,   6,  10, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490150,   1,    10, 0, 0, 15) /* MaxHealth */
     , (490150,   3,    10, 0, 0, 20) /* MaxStamina */
     , (490150,   5,  2000, 0, 0, 2010) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490150, 32, 0, 3, 0, 400, 0, 2282.025914679296) /* ItemEnchantment     Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490150,  0,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (490150,  1,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (490150,  2,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (490150,  3,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (490150,  4,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (490150,  5,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (490150,  6,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (490150,  7,  4,  0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (490150,  8,  4,  2, 0.75,    0,    0,    0,    0,    0,    0,    0,    0,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`, `weenie_Class_Id`, `style`, `substyle`, `quest`, `vendor_Type`, `min_Health`, `max_Health`)
VALUES (490150,  7 /* Use */,      1, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`, `delay`, `extent`, `motion`, `message`, `test_String`, `min`, `max`, `min_64`, `max_64`, `min_Dbl`, `max_Dbl`, `stat`, `display`, `amount`, `amount_64`, `hero_X_P_64`, `percent`, `spell_Id`, `wealth_Rating`, `treasure_Class`, `treasure_Type`, `p_Script`, `sound`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (@parent_id,  0,  74 /* TakeItems */, 0, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0 /* Undef */, 490153 /* Energy Infused Rock */, 1, 0 /* Undef */, 0, False, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
, (@parent_id,  1,  99 /* TeleportTarget */, 2, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,   0xF76C0019 ,95.371689, 22.520020, 13.289044, 0.321486, 0.000000, 0.000000, -0.946914)
     , (@parent_id,  2,  18 /* DirectBroadcast */, 0.5, 1, NULL, 'The Balloon Heads out to sea with a strong gust of wind. You can sea the Island in the distance.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
     , (@parent_id,  3,  18 /* DirectBroadcast */, 3, 1, NULL, 'You jump to safety as the balloon begins to head out to sea.', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
