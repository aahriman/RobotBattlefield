# Úvod
Úkolem hry je naprogramovat autonomního virtuálního robota, který bojuje v aréně s ostatními roboty. 
Níže je popsáno, jak zprovoznit potřebné programy a jak se prostředí chová. 

Součástí staženého balíčku je soubor *vyuka.pdf* ve které je popsáno, jak se za pomoci prostředí učit.

## Jak získat aplikaci
Před vlastním programováním autonomního robota je nutné aplikaci získat. Postup získání aplikace je následující:
1. Kliknete na záložku releases. 
2. V okně, které se objeví, v sekci Assets kliknete na *RobotBattlefield-1.1.zip*. 
3. Rozbalíte stažený soubor 
   1. Otevřete složku, do které se soubor stáhl.
   2. Kliknete pravým tlačítkem na soubor.
   3. Vyberete možnost *Rozbalit soubor zde* případně *Extract file here*.
   4. Vytvoří se složka *RobotBattlefield-bin*.
 
Před dalším pokračováním se doporučuje přenést složku RobotBattlefield-bin do vlastní složky na ploše (např. do složky *moji roboti*).

## Spuštění hry

Nejprve musí být spuštěna aréna a až poté mohou být spuštěni roboti.

### Spuštění arény
Návod ke spuštění arény: 
1. Otevřete složku *RobotBattlefield-bin\battlefields*.
2. Dvojím klepnutím na *DeadmatchBattlefield.bat* spustíte arénu.
3. Pokud vám vyskočí okno firewallu, pak zadejte, že to chcete povolit (Pokud to zakážene, nebude možné u sebe spusti server, do kterého se připojí kamarádi. Tedy s nimi nebudete moci vést souboj.
).

Tato aréna je spuštěna pro dva roboty,každé ho v jednom týmu.

Pokud budete chtít připojit více robotů, musíte provést následující:

1. Upravte soubor *D-MyConfig.config*
    1. Otevřte složku *RobotBattlefield-bin\battlefields*.
    2. Otevřte soubor *D-MyConfig.config* pomocí Poznámkového bloku.
    3. Pokud soubor *D-MyConfig.config* nevidíte, ale vidíte pouze soubor *D-MyConfig*: 
        1. V horní liště Průzkumníku klikněte na *Zobrazení* 
        2. Zaškrtněte možnost *Přípony názvů a souborů*
    4. Po otevření změňte číslo za textem *"ROBOTS_IN_TEAM":* na vámi požadovaný počet robotů.
    5. Soubor uložte a zavřete.
2. Server s tímto nastavením spustíte dvojím kliknutím na *D-MyConfig.bat*.

### Spuštění robotů
V projektu je již naprogramován robot Spot. Ten lze přidat do zápasu tímto způsobem:
1. Spustíte arénu (pokud jste ji ještě nespustili). 
2. Ve složce *RobotBattlefield-bin\robots* poklepejte na *Spot.bat*, a tím je robot přidán do arény. Pokud jste spustili klasickou arénu, musíte přidat do arény 2 roboty (tedy dvakrát spota) teprve pak začne zápas.

### Zobrazení průběhu zápasu

Zápas je možné sledovat během utkání, kde políčko s popisem *Delay* je doba čekání mezi zobrazením jednotlivých kol. Doba mezi koly se tímto nemění, pouze jejich zobrazení.

Průběh zápasu je také možné sledovat po skončení tohoto zápasu. Provedete to následovně:

1. Ve složce *RobotBattlefield-bin* poklepáte na *Viewer.bat*.
2. V okně, které se objeví,  kliknete na tlačítko *Choose file* a projdete do složky odkud jste spouštěli arénu. V této složce vyberete soubor *arena_match2000.txt* (v případě spuštění arény na jiném portu než 2000 se v názvu souboru též změní číslo 2000 na vámi zvolené. Port lze změnit v souboru *D-MyConfig.bat*,pokud nevíte, co to znamená,taktutopoznámku ignorujte).Název souboru lze změnit změněním atributu *"MATCH_SAVE_FILE":* v konfiguračním souboru viz A.2.1 (toto jméno je vždy stejné nezávisle na zvoleném portu). 
3. Potvrdíte tlačítkem "Otevřít". 
4. Pak krokujete, nebo přehráváte animaci.
  1. Tlačítkem *Play* přehráváte animaci průběhu zápasu. 
  2. Tlačítkem *Pause* pozastavíte přehrávání animace. 
  3. Tlačítkem *Next turn* přehrajete jeden další tah. 
  4. Tlačítkem *Previous turn* přehrajete předešlý tah. 
  5. Tlačítkem *Reset* se vrátíte na začátek animace.

TODO: popisky zobrazování

## Jak začít programovat 

Obvykle se programuje ve vývojovém prostředí (tzv. IDE).
Pro jazyk C# je nejspíše nejpoužívanějším vývojovým prostředí Visual Studio (možné stáhnout z https://www.visualstudio.com/vs/community/ kliknutím na tlačítko *Download VS Comunity 2017*). 
Níže je tedy popsán postup, jak začít programovat za pomoci tohoto IDE. 

### Ve Visual studio 2017

Než začneme, je vhodné vložit šablonu pro vytváření robotích projektů:

1. Ze složky *RobotBattlefield-bin\import* zkopírujte soubour *RobotApplication.zip*.
2. Vložte soubour *RobotTemplate.zip* do složky *Documents\Visual Studio 2017\Templates\ProjectTemplates*.

Spustíte Visual studio a dále pokračujete podle postupu:
1. Vytvoříte projekt.
    1. V nabídce horní lišty vyberete *File → New → Project*.
    2. Z nabídky *Templates* vyberete *Visual C#*.
    3. Uprostřed nahoře vyberte v seznamu *.Net Framework 4.5* (nebo vyšší) vedle `Templates:`
    4. Do kolonky vedle `Name` napíšete jméno robota (např. Robot).
    5. [Volitelné-je možné přeskočit] Zvolíte cestu ke složce, v níž senachází složka *RobotBattlefield-bin* (např. složka moji roboti). V `Location` kliknete na tlačítko se třemi tečkami a pak se objeví okno průzkumníka, kde tuto složku vyberete (tam se bude ukládat váš projekt). 
    6. V okně vyberete *Robot Application* (musí být v modrém rámečku).
    7. Zmáčknete tlačítko *OK*.
    
2. Přidáte knihovnu *ClientLibrary.dll* a *BaseLibrary.dll* k vašemu řešení (tyto knihovny jsou v *RobotBattlefield-bin\import*)
