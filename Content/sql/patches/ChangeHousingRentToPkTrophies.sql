DELETE FROM ace_shard.biota_properties_create_list WHERE destination_type IN (16,32);

/* Buy item list for mansions */
INSERT INTO ace_shard.biota_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
SELECT biota.id, 16, 1000002, 500, 0, 0, 0
FROM ace_shard.biota biota
JOIN ace_world.weenie weenie
ON biota.weenie_class_id = weenie.class_id
WHERE weenie.`type` = 55
AND class_Name LIKE '%mansion%';

/* Rent item list for mansions */
INSERT INTO ace_shard.biota_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
SELECT biota.id, 32, 1000002, 150, 0, 0, 0
FROM ace_shard.biota biota
JOIN ace_world.weenie weenie
ON biota.weenie_class_id = weenie.class_id
WHERE weenie.`type` = 55
AND class_Name LIKE '%mansion%';