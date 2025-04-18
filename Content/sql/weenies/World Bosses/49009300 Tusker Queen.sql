DELETE FROM `weenie` WHERE `class_Id` = 49009300;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (49009300, 'worldbosstuskerqueen', 10, '2005-02-09 10:00:00') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (49009300,   1,         16) /* ItemType - Creature */
     , (49009300,   2,         12) /* CreatureType - Cow */
     , (49009300,   6,         -1) /* ItemsCapacity */
     , (49009300,   7,         -1) /* ContainersCapacity */
	 , (49009300,   3,         82) /* PaletteTemplate - Pink Purple */
     , (49009300,  16,          1) /* ItemUseable - No */
     , (49009300,  25,        899) /* Level */
	 , (49009300,  72,         48) /* FriendType - Virindi */
     , (49009300,  27,          0) /* ArmorType - None */
	 , (49009300, 103,          3) /* GeneratorDestructionType - Destroy */
	 , (49009300,  81,          50) /* MaxGeneratedObjects */
     , (49009300,  82,          50) /* InitGeneratedObjects */
     , (49009300,  40,          1) /* CombatMode - NonCombat */
     , (49009300,  68,          1) /* TargetingTactic - Random */
     , (49009300,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (49009300, 101,        183) /* AiAllowedCombatStyle - Unarmed, OneHanded, OneHandedAndShield, Bow, Crossbow, ThrownWeapon */
     , (49009300, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (49009300, 146,      400000000) /* XpOverride */
	 , (49009300, 307,         50) /* DamageRating */
     , (49009300, 308,         200) /* DamageResistRating */
	 , (49009300, 386,         10) /* Overpower */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (49009300,   1, False ) /* Stuck */
     , (49009300,   6, False) /* AiUsesMana */
     , (49009300,  11, False) /* IgnoreCollisions */
     , (49009300,  12, True ) /* ReportCollisions */
	 , (49009300,  29, True ) /* NoCorpse */
     , (49009300,  13, False) /* Ethereal */
     , (49009300,  52, False ) /* AiImmobile */
	 , (49009300,  65, True ) /* IgnoreMagicResist */
     , (49009300,  66, True ) /* IgnoreMagicArmor */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (49009300,   1,     1.5) /* HeartbeatInterval */
     , (49009300,   2,       0) /* HeartbeatTimestamp */
     , (49009300,   3,       1) /* HealthRate */
     , (49009300,   4,  750000) /* StaminaRate */
     , (49009300,   5,      50) /* ManaRate */
     , (49009300,  13,       5) /* ArmorModVsSlash */
     , (49009300,  14,       5) /* ArmorModVsPierce */
     , (49009300,  15,       5) /* ArmorModVsBludgeon */
     , (49009300,  16,       5) /* ArmorModVsCold */
     , (49009300,  17,    2.25) /* ArmorModVsFire */
     , (49009300,  18,       5) /* ArmorModVsAcid */
     , (49009300,  19,       5) /* ArmorModVsElectric */
     , (49009300,  31,      50) /* VisualAwarenessRange */
     , (49009300,  34,       1) /* PowerupTime */
     , (49009300,  36,       1) /* ChargeSpeed */
     , (49009300,  39,      1.8) /* DefaultScale */
	 , (49009300,  55,       50) /* HomeRadius */
     , (49009300,  64,       0.1) /* ResistSlash */
     , (49009300,  65,       0.1) /* ResistPierce */
     , (49009300,  66,       0.1) /* ResistBludgeon */
     , (49009300,  67,       0.5) /* ResistFire */
     , (49009300,  68,       0.1) /* ResistCold */
     , (49009300,  69,       0.1) /* ResistAcid */
     , (49009300,  70,       0.1) /* ResistElectric */
     , (49009300,  71,       1) /* ResistHealthBoost */
     , (49009300,  72,       0) /* ResistStaminaDrain */
     , (49009300,  73,       1) /* ResistStaminaBoost */
     , (49009300,  74,       0) /* ResistManaDrain */
     , (49009300,  75,       1) /* ResistManaBoost */
     , (49009300,  80,       1) /* AiUseMagicDelay */
     , (49009300, 104,      50) /* ObviousRadarRange */
     , (49009300, 122,       0) /* AiAcquireHealth */
	 , (49009300, 166,      1) /* ResistNether */
     , (49009300, 125,      1) /* ResistHealthDrain */
     , (49009300, 127,       2) /* AiCounteractEnchantment */
	 , (49009300,  43,      20) /* GeneratorRadius */
	 , (49009300, 151,    0.80) /* IgnoreShield */
	 , (49009300,  41,     600) /* RegenerationInterval */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (49009300,   1, 'Tusker Queen') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (49009300,   1, 0x02000964) /* Setup */
     , (49009300,   2, 0x0900000C) /* MotionTable */
     , (49009300,   3, 0x20000011) /* SoundTable */
     , (49009300,   4, 0x3000000B) /* CombatTable */
     , (49009300,   6, 0x0400102F) /* PaletteBase */
     , (49009300,   7, 0x10000262) /* ClothingBase */
     , (49009300,   8, 0x06001033) /* Icon */
     , (49009300,  22, 0x34000027) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (49009300,   1, 900, 0, 0) /* Strength */
     , (49009300,   2, 900, 0, 0) /* Endurance */
     , (49009300,   3, 900, 0, 0) /* Quickness */
     , (49009300,   4, 900, 0, 0) /* Coordination */
     , (49009300,   5, 1200, 0, 0) /* Focus */
     , (49009300,   6, 1200, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (49009300,   1,  224252, 0, 0, 124252) /* MaxHealth */
     , (49009300,   3,  1000000, 0, 0,  1000000) /* MaxStamina */
     , (49009300,   5,  1000000, 0, 0, 1000000) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (49009300,  6, 0, 3, 0, 200, 0, 608.0722585280382) /* MeleeDefense        Specialized */
     , (49009300,  7, 0, 3, 0, 480, 0, 608.0722585280382) /* MissileDefense      Specialized */
     , (49009300, 13, 0, 3, 0, 400, 0, 608.0722585280382) /* Heavy Weapon      Specialized */
     , (49009300, 15, 0, 3, 0, 200, 0, 608.0722585280382) /* MagicDefense        Specialized */
     , (49009300, 20, 0, 3, 0, 100, 0, 608.0722585280382) /* Deception           Specialized */
     , (49009300, 24, 0, 3, 0, 200, 0, 608.0722585280382) /* Run                 Specialized */
     , (49009300, 31, 0, 3, 0, 900, 0, 608.0722585280382) /* CreatureEnchantment Specialized */
     , (49009300, 33, 0, 3, 0, 900, 0, 608.0722585280382) /* LifeMagic           Specialized */
     , (49009300, 34, 0, 3, 0, 1200, 0, 608.0722585280382) /* WarMagic            Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (49009300,  0,  4,  0,    0,  1800, 700, 700, 700, 700, 700, 700, 700,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (49009300,  1,  4,  0,    0,  1800, 700, 700, 700, 700, 700, 700, 700,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (49009300,  2,  4,  0,    0,  1800, 700, 700, 700, 700, 700, 700, 700,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (49009300,  3,  4,  0,    0,  1800, 700, 700, 700, 700, 700, 700, 700,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (49009300,  4,  4,  0,    0,  1800, 700, 700, 700, 700, 700, 700, 700,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (49009300,  5,  4, 120, 0.75, 1800, 700, 700, 700, 700, 700, 700, 700,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (49009300,  6,  4,  0,    0,  1800, 700, 700, 700, 700, 700, 700, 700,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (49009300,  7,  4,  0,    0,  1800, 700, 700, 700, 700, 700, 700, 700,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (49009300,  8,  4, 120, 0.75, 1800, 700, 700, 700, 700, 700, 700, 700,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (49009300,  4481,   2.2)  /* Incantation of Fire Vulnerability Other */
     , (49009300,  118,    2.25)  /* Flame Blast VI */
     , (49009300,  4423,   2.3)  /* Incantation of Flame Arc */     
	 , (49009300,  2773,   2.015)  /* Martyr's Tenacity VII */
;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (49009300, 9, 49009302,  0, 0, 1, False) /* Create Tusker Queen Loot Gen for ContainTreasure */;