
## Genernal Information

   ###  # comment
   
   ### @@include=incName  
      include script file, load currentFileName+incName
      not support @@include in incName file

      lot-enemy-b.txt 
     @@include=arrow.txt  =>  
      .\arrow.txt 
      .\lot-enemy-b-arrow.txt 
       templates\commons\arrow.txt
       
      


   ### @@var@@varName=varValue

     @@var@@aa=12221,13331
       
     
     {{aa}}...

   

   ### EquipType


     None=0, Good=1, Weapon=2, Protector=3, Accessory=4, Gem=5


   ### ShopLineupParam EquipType

      WEAPON=0, ARMOR=1, RING=2, GOOD=3, ASH=4
   


## shop-spec.txt(ShopLineupParam)

    
##### format
      rowId;newEquipId;newName;equipType;price[;sellQuantity]
      
##### examples

       #100515,[Merchant Kale] Chain Coif
       100515;201000;Banished Knight Helm;1;200;1

       #100516,[Merchant Kale] Chain Armor
       100516;201100;Banished Knight Armor;1;200;1

       #100517,[Merchant Kale] Chain Gauntlets
       100517;200200;Banished Knight Gauntlets;1;200;1

       #100518,[Merchant Kale] Chain Leggings
       100518;200300;Banished Knight Greaves;1;200;1  



## recipe-spec.txt

#### format
equipType use shop equipType

     equipId;equipType

#### examples

     
       

## lot-enemy-b.txt(ItemLotParam_enemy) lot-map-b.txt(ItemLotParam_map)


#### format


       @@index=index(1-8)
       @@point=point(1000=100%)
       rowId1,...|equipId1,...|equipType1,...|lotCount

##### example

       #[Godrick Soldier] Bolt -> 100% Arrow
       @@index=1
       @@point=1000

       #50000000 Arrow
       431100000,431100010,431100020|50000000|2|20
       equal
       431100000,431100010,431100020|50000000,50000000,50000000|2,2,2|20  

       



## lot-enemy-spec.txt(ItemLotParam_enemy) lot-map-spec.txt(ItemLotParam_map)

#### format

      @@et=equipType 
      rowId;equipId;lotIndex;lotCount

#### example

      @@et=2
      #942370070,[Troll Carriage - Gatefront Ruins] Lordsworn's Greatsword
      #3100000;Sacred Relic Sword
      942370070;3100000



## update-row.txt

#### @@id=rowId1[,rowId2[,...]]
   set rowId

#### format

        @@param=paramName;
        @@id=rowId1,...
        key;value     
         
### sp-effect.txt 
     same update-row.txt always param=SpEffectParam

##### example
       
       @@id=310000,310010,310020
       #310000,Crimson Amber Medallion 
       #310010,Crimson Amber Medallion +1
       #310020,Crimson Amber Medallion +2
       motionInterval;1
       changeHpPoint;-5
       changeMpPoint;-2

     
### chara-init.txt 
    same update-row.txt   
    always param=CharaInitParam  



## update-item.txt 

#### format



@@param=name
rowId;key;value

paramName;rowId;key;value    

paramName;rowId;key;value;rowName





