DELETE FROM `recipe` WHERE `id` = 8923;

INSERT INTO `recipe` (`id`, `unknown_1`, `skill`, `difficulty`, `salvage_Type`, `success_W_C_I_D`, `success_Amount`, `success_Message`, `fail_W_C_I_D`, `fail_Amount`, `fail_Message`, `success_Destroy_Source_Chance`, `success_Destroy_Source_Amount`, `success_Destroy_Source_Message`, `success_Destroy_Target_Chance`, `success_Destroy_Target_Amount`, `success_Destroy_Target_Message`, `fail_Destroy_Source_Chance`, `fail_Destroy_Source_Amount`, `fail_Destroy_Source_Message`, `fail_Destroy_Target_Chance`, `fail_Destroy_Target_Amount`, `fail_Destroy_Target_Message`, `data_Id`, `last_Modified`)
VALUES (8923, 0, 29 /* ArmorTinkering */, 400, 0, 0, 0, 'You successfully imbue the item with the magics of the empowered amber.', 0, 0, 'You fail to imbue the item, destroying it in the process!', 1, 1, NULL, 0, 0, NULL, 1, 1, NULL, 1, 1, NULL, 0, '2021-12-14 05:15:31');

INSERT INTO `recipe_requirements_int` (`recipe_Id`, `index`, `stat`, `value`, `enum`, `message`)
VALUES (8923, 0,   9, 2097152, 4, 'You can only apply this augmentaion to a shield!') /* Target.ValidLocations - Shield NotEqual 2097152 */
     , (8923, 0, 371, 3, 3, 'This item has already been imbued!') /* Target.GearDamageResist GreaterThanEqual 3 */;

INSERT INTO `recipe_mod` (`recipe_Id`, `executes_On_Success`, `health`, `stamina`, `mana`, `unknown_7`, `data_Id`, `unknown_9`, `instance_Id`)
VALUES (8923, True, 0, 0, 0, False, 0, 1, 0);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `recipe_mods_int` (`recipe_Mod_Id`, `index`, `stat`, `value`, `enum`, `source`)
VALUES (@parent_id, 0, 371, 5, 2, 1) /* On Source.SuccessTarget Add GearDamageResist 5 to Target */;

DELETE FROM `cook_book` WHERE `recipe_Id` = 8923;

INSERT INTO `cook_book` (`recipe_Id`, `source_W_C_I_D`, `target_W_C_I_D`, `last_Modified`)
VALUES (8923, 53440 /* Empowered Amber: Shield Reinforcement */,    44 /* Buckler */, '2021-12-14 05:15:31')
     , (8923, 53440 /* Empowered Amber: Shield Reinforcement */,    91 /* Kite Shield */, '2021-12-14 05:15:31')
     , (8923, 53440 /* Empowered Amber: Shield Reinforcement */,    92 /* Large Kite Shield */, '2021-12-14 05:15:31')
     , (8923, 53440 /* Empowered Amber: Shield Reinforcement */,    93 /* Round Shield */, '2021-12-14 05:15:31')
     , (8923, 53440 /* Empowered Amber: Shield Reinforcement */,    94 /* Large Round Shield */, '2021-12-14 05:15:31')
     , (8923, 53440 /* Empowered Amber: Shield Reinforcement */,    95 /* Tower Shield */, '2021-12-14 05:15:31')
     , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 21158 /* Covenant Shield */, '2021-12-14 05:15:31')
     , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 28163 /* Round Shield */, '2021-12-14 05:15:31')
     , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 37291 /* Olthoi Shield */, '2021-12-14 05:15:31')
     , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 23356 /* Sanguinary Aegis*/,  '2021-12-14 05:15:31')
	 , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 23357 /* Sanguinary Aegis */, '2021-12-14 05:15:31')
	 , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 23358 /* Sanguinary Aegis */, '2021-12-14 05:15:31')
	 , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 23359 /* Sanguinary Aegis */, '2021-12-14 05:15:31')
	 , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 23360 /* Sanguinary Aegis */, '2021-12-14 05:15:31')
	 , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 35982 /* Aegis of the Golden Flame */, '2021-12-14 05:15:31')
	 , (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 43141 /* Aegis of the Gold Gear */, '2021-12-14 05:15:31')
	, (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 31395 /* Raven Aegis */, '2021-12-14 05:15:31')
	, (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 35295 /* Shield of Perfect Light */, '2021-12-14 05:15:31')
	, (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 33105 /* Shield of Isin Dule */, '2021-12-14 05:15:31')
	, (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 33106 /* Shield of Isin Dule */, '2021-12-14 05:15:31')
	, (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 30372 /* Shield of Engorgement */, '2021-12-14 05:15:31')
	, (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 30370 /* Twin Ward */, '2021-12-14 05:15:31')
	, (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 30371 /* Dread Marauder Shield */, '2021-12-14 05:15:31')
	, (8923, 53440 /* Empowered Amber: Shield Reinforcement */, 30373 /* Mirrored Justice */, '2021-12-14 05:15:31');