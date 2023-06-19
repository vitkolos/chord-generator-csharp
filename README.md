# Generátor hmatů akordů

| Vít Kološ, 1. ročník, IPP | letní semestr 2022/2023 | NPRG031 Programování 2 |
| - | - | - |

## Anotace

Program pro zadaný akord určí hmaty, kterými jej lze na kytaru či jiný strunný nástroj zahrát, a zobrazí jejich diagramy.

## Použití programu

Za předpokladu, že je program správně konfigurován (viz sekce [Konfigurace](#konfigurace)), spočívá jeho použití v zadání [názvu akordu](#název-akordu) (např. D#maj7) a potvrzení vstupu tlačítkem Generovat nebo klávesou Enter. Následně se zobrazí několik způsobů, jak daný akord na zvolený nástroj zahrát. Tyto varianty jsou seřazeny podle jistých kritérií, obvykle je vhodné použít jeden z prvních tří zobrazených hmatů. Pokud konfigurační soubor obsahuje více hudebních nástrojů, je možné vybrat, pro který z nich se hmaty mají generovat.

### Název akordu

Název akordu se může skládat ze tří částí: základního tónu, typu akordu a basového tónu (za lomítkem). Všechny podporované typy akordů musí být uvedeny v dokumentaci. Není-li typ akordu zadán, je automaticky vyhodnocen jako `major` (tento typ akordu by tedy v konfiguraci neměl chybět). Basový tón se píše až za typ akordu a musí být zleva oddělen lomítkem.

Tóny (základní i basové) jsou tvořeny jedním nebo dvěma znaky, kde první je velké písmeno a druhý je znak `#` nebo `b`. Písmena odpovídají českému (respektive německému) systému pojmenování not, tedy po notě A následují (po půltónech) B, H, C. (V anglosaském systému by to bylo A, Bb, B, C.) Funguje enharmonická záměna, tedy platí, že G# = Ab nebo že B = Hb = A#. S tónem B je kvůli lepší kompatibilitě nakládáno specificky, pokud za ním následuje posuvka (křížek nebo béčko), tak se tato již nevyhodnocuje (tedy Bb = B). Dvojité posuvky (Gbb) ani alternativní označení (Dis, Es) nejsou podporovány.

Příklady možných názvů akordů: `C/E`, `Ebmaj7`, `Fsus4`, `F#m`

### Doplňkové funkce

Je-li přítomná složka `audio` se zvukovými soubory (požadovaná vnitřní struktura složky a názvy souborů jsou specifikovány v kódu programu, konkrétně v souboru `classes/ChordPlayer.cs`) a spadají-li tóny akordu do podporovaného rozsahu, je možné konkrétní variantu akordu odpovídající danému hmatu přehrát kliknutím na jeho diagram.

Konfigurační soubor lze otevřít přímo z okna programu stisknutím tlačítka „konfigurovat“.

## Konfigurace

Program očekává existenci konfiguračního souboru `config.txt`. Ten se načítá při startu programu, pokud tedy program běží a tento soubor je změněn, je nutné program spustit znova.

Konfigurační soubor se skládá ze dvou částí oddělených prázdným řádkem. První část odpovídá seznamu hudebních nástrojů, druhá seznamu typů akordů. Jeden řádek odpovídá jednomu nástroji nebo typu akordu. Každý řádek obsahuje dvě nebo tři hodnoty oddělené čárkou.

### Konfigurace hudebního nástroje

U hudebního nástroje určuje první hodnota jeho název (nesmí obsahovat čárku).

Druhá určuje ladění strun, jde o seznam tónů odpovídajících jednotlivým strunám, pořadí v seznamu odpovídá pořadí na nástroji (zleva doprava). Pokud jsou všechny tóny v jedné oktávě (jednočárkované) a lze je zapsat bez posuvek, je možné je napsat bez mezer (např. `GCEA`). Jinak je nutné je oddělit mezerami, přičemž za název tónu se uvádí číslo oktávy (podle *vědecké* notace, kde jednočárkovaná oktáva má číslo 4, *komorní a* by se tedy zapsalo jako A4). Pokud číslo oktávy chybí, je použita výchozí (jednočárkovaná) oktáva. Specifika zápisu tónu jsou uvedeny v sekci [Název akordu](#název-akordu).

Poslední hodnota odpovídá počtu dostupných pražců (nultý se nepočítá), tato hodnota je volitelná (výchozí počet je 20) a obvykle nehraje příliš velkou roli, protože většina základních (výhodných) hmatů se vejde na prvních několik pražců.

### Konfigurace typu akordu

První hodnota odpovídá seznamu možných označení akordu oddělených mezerou. Speciální roli hraje označení `major`, neboť pokud uživatel při používání programu nezadá typ akordu, automaticky se zvolí tento typ.

Druhá hodnota obsahuje seznam tónů, z nichž se akord skládá, tóny jsou zde reprezentovány svými vzdálenostmi od tóniky. Každý tón tedy odpovídá číslu z rozsahu 0–11. Číslo 10 lze alternativně zapsat písmenem `t` nebo `A`, číslo 11 písmenem `e` nebo `B`. Je-li některý z tónů zapsán více než jedním znakem, je nutné tóny oddělit mezerami.

### Vzorová konfigurace

Tento vzor záměrně obsahuje několik variant značení, při praktickém použití je vhodné zvolit jednu z nich.

```csv
kytara,E2 A2 D3 G3 H3 E4
ukulele,GCEA,19

major,0 4 7
m mi,0 3 7
7,0 4 7 10
m7,0 3 7 A
maj maj7,047e
```

## Slovníček pojmů

- akord – souzvuk tónů
- hmat – způsob, jak stisknout struny na strunném hudebním nástroji, aby zněly požadované tóny (respektive kýžený akord)
- diagram (hmatu) – schematické znázornění hmatu (umístění jednotlivých prstů na strunách)
- tónika – základní tón stupnice či tóniny (případně akordu)
- barré – způsob držení hmatu, kdy ukazováček drží několik strun najednou (pro účely mého programu všechny struny)
- hmatník – část nástroje, nad níž jsou nataženy struny
- pražec – kovový pásek vsazený do hmatníku, nad nímž je nutné stisknout strunu, aby zněl určitý tón (k-tý pražec tón struny obvykle zvyšuje o k půltónů; nultý pražec se pochopitelně nemačká)
- otevřená struna – struna, která v daném hmatu není stisklá a zní
- tlumená struna – struna, která v daném hmatu není stiklá a nezní (nebo ji hráč prsty tlumí, aby nezněla)

## Řešený problém a algoritmus

Za zajímavé části programu považuji generování všech možných hmatů pro určitý akord, seřazení těchto variant podle vhodně zvolených kritérií, očíslování jednotlivých prstů v daném hmatu a vykreslení diagramu. Z toho první dvě řeší ústřední problém, kterým se můj zápočtový program zabýval, totiž **jak nalézt vhodný hmat**, kterým lze akord zahrát (ideálně ten, který muzikanti obvykle používají), bez pomoci internetu či příruček.

### Vhodný hmat

Ale jak poznat vhodný hmat? Nu, to je ten, který se dobře hraje a dobře zní. Je jisté, že hmat, k jehož stisknutí je potřeba 6 prstů, se moc dobře hrát nebude. Hmat akordu D sice zní dobře, ale pokud by ho program vrátil jako hmat akordu C, nebyl bych spokojen. Určil jsem tyto nutné podmínky, aby se hmat dal považovat za platný:

1. musí obsahovat všechny tóny akordu a žádné jiné,
2. k jeho stisknutí jsou použity nejvýše čtyři prsty,
3. prsty se vejdou na hmatník (hmat nepoužívá neexistující pražce),
4. je-li v akordu použito barré, je aktivně využíváno,
5. tlumené struny nejsou z obou stran obklopeny netlumenými.

Splnění uvedených podmínek je realizováno rekurzivním voláním funkce, která postupně vygeneruje všechny možné akordy. O zadaném akordu je známo, z jakých tónů se musí skládat (kombinací základního tónu a typu akordu, případně i basového tónu), funkce tedy na každé struně zkouší postupně stisknout každý z tónů (nehledě na oktávu, tudíž na jedné struně je určitý tón obvykle vícekrát) nebo strunu tlumit. Zároveň si eviduje, kolik tónů je již ve vygenerovaném souzvuku zastoupeno. Pokud výsledný hmat splňuje nutná kritéria, je přidán do seznamu všech možných hmatů.

Pro ukulele (čtyřstrunný nástroj s 19 pražci) teoreticky existuje 160 000 hmatů. Po aplikaci uvedených kritérií pro určitý akord jich zbyde přibližně 700. Ty je potřeba nějak seřadit a určit, které jsou nejvýhodnější.

Klíč, podle nějž lze hmaty řadit, zajišťuje ohodnocovací funkce, která každému hmatu přiděluje skóre. To je ovlivněno těmito kritérii: barré, základní tón (zda je nejnižším tónem souzvuku), výška nejnižšího tónu, výškové rozpětí tónů v souzvuku, vzájemná vzdálenost prstů na hmatníku, počet použitých prstů, přítomnost tlumených strun. Je tam rovněž několik kritérií, které mají tak výraznou váhu, že akord obvykle odsunou z první desítky: téměř nevyužívané barré, basový tón není nejnižším tónem, příliš mnoho tlumených strun, tlumené struny na obou stranách hmatníku. Některá kritéria mají dokonce různou váhu v závislosti na počtu strun daného hudebního nástroje.

Ohodnocovací funkci jsem vytvářel ručně, snažil jsem se, aby tradiční hmaty všech základních akordů byly na prvním místě nebo alespoň v první trojici. Sice stále nevrací dokonalé výsledky, ale někdy se ukazuje, že „správný“ výsledek ani vrátit nemůže, jako v případě kytarového akordu C7, jehož tradiční hmat neobsahuje tón g.

#### Jinak a hůře

Jedním z možných jiných řešení je algoritmus, který by na každé struně zvolil první (nejnižší) tón, který je v daném akordu obsažen. Pro dostatečně jednoduchý nástroj, jakým je ukulele, a pár základních akordů tento postup skutečně vrací správné výsledky. U jiných akordů však tímto způsobem mineme velmi výhodné hmaty a snahy o odvrácení tohoto jevu vedou k řešení velmi podobnému tomu, které jsem použil ve svém programu.

### Prsty a diagramy

Problém počítání prstů jsem vyřešil poměrně jednoduše, číslují se zleva doprava, shora dolů. Navíc jsem přidal přeskakování, mechanismus jsem okoukal u hmatů na ukulele. Za každý prázdný pražec (pod barré) se počítadlo posouvá o jeden prst. Jediným omezením je, že se takové přičítání nesmí dopočítat malíčku.

Vykreslování diagramů naopak obsahuje nemálo výpočtů, vše se točí okolo rozměrů plátna a dělení volného místa mezi jednotlivé pražce či struny. Zajímavým pozorováním je, že obvykle stačí vykreslit jen malou část hmatníku, většina akordů se totiž vejde na pět pražců. To je samozřejmě dáno tím, že klást prsty daleko od sebe je velmi náročné, z čehož vychází i nastavení ohodnocovací funkce.

## Program

### Formulářová aplikace

Okenní formulářová aplikace je implementována pomocí frameworku Windows Forms. Možnost současného přehrávání několika hudebních stop zajišťuje knihovna Windows Presentation Foundation. Bezchybné vykreslení na zařízení se „zvětšeným“ zobrazením umožňuje externí metoda `SetProcessDPIAware`.

### Konzolová aplikace a testy

Program lze rovněž spustit v konzoli, stačí použít argument `console`. Po zadání jména akordu se vypíšou jeho diagramy (deset nejlepších) v sestupném pořadí podle skóre. Zadání názvu nástroje na daný nástroj přepne. Slovní spojení `run tests` spustí sadu testů a vypíše jejich výstup. Testy lze rovněž provést spuštěním programu s argumentem `tests`.

Testy prověřují zejména [zpracování vstupu](#zpracování-vstupu) a generování hmatů pro několik základních akordů.

### Zpracování vstupu

O zpracování vstupu všeho druhu se stará třída `Parser`. Ta rovněž při spuštění programu načítá konfiguraci. V případě chybného uživatelského vstupu (chybného jména akordu) je vyvolána `FormatException`. Ta je v okenní aplikaci zachycena a vypsána na obrazovku.

### Konstanty a statické třídy

Konstanty jsou rozděleny do tří statických tříd: `Program`, `Music` a `Strings`. Třída `Strings` obsahuje všechny texty, na které uživatel může narazit v grafickém prostředí okenní aplikace. V `Music` se nacházejí všechny hudební konstanty a také metoda `Modulo` zajišťující modulární aritmetiku půltónů. `Program` obsahuje ostatní konstanty, základní metody programu a pomocnou metodu pro sčítání prvků pole.

### Objekty

Pro každý akord zadaný uživatelem vzniká objekt `Chord`. Aktuálně vybraný nástroj je reprezentován ukazatelem na odpovídající objekt `Instrument`.

Pomocí těchto dvou objektů se následně vytváří instance třídy `Positions`, která obsahuje seznam objektů `Position`, přičemž každý z nich odpovídá jenomu hmatu a počítá jeho skóre.

Při prvním požadavku o vykreslení/vypsání diagramu daného hmatu je vytvořen objekt `Diagram`, který obsahuje polotovar diagramu hmatu a obsluhuje požadavky o jeho výpis či vykreslení, jež mu předává objekt `Position`.

Objekt `ChordPlayer` je vytvořen při spuštění okenní aplikace a zajišťuje přehrávání akordů.

### Průchod programem

Při spuštění programu se načte konfigurační soubor. Jako výchozí hudební nástroj se použije ten, který je v konfiguračním souboru uveden jako první. Uživatel může hudební nástroj přepnout na jiný z nabídky. Jakmile zadá jméno akordu, vytvoří se pro tento akord objekt. Pomocí něj a zvoleného nástroje se pak vytvoří seznam všech možných hmatů s ohodnoceními. Tento seznam se následně seřadí podle skóre a první deset položek se vypíše na obrazovku.

## Závěr

Ačkoliv program řeší zadání a funguje efektivně, bylo by možné provést drobné úpravy či rozšíření.

Zrychlení výpočtu (i když nepatrného) by bylo jistě dosaženo přidáním další nutné podmínky – aby hmat sahal nejvýše přes 7 pražců.

Jistou změnu ve výsledcích vyhodnocení by pravděpodobně přineslo umožnění částečných barré – tedy že by mohl libovolný prst držet libovolný počet strun najednou. Někdo by mohl jako omezující vnímat zákaz tlumených strun mezi dvěma netlumenými, ač tam bych přínos zrušení tohoto omezení vnímal jako diskutabilní.

Dalším možným rozšířením je úprava či dokonce jakési automatické generování parametrů vyhodnocovací funkce, aby se tradiční hmaty umisťovaly ve výsledných žebříčcích na prvních (nebo alespoň vyšších) příčkách.

Kromě generování akordů a jejich vyhodnocování by také bylo možné rozšířit funkce aplikace například o vypsání konkrétních tónů, které u daného hmatu na strunách zní, nebo hromadný režim, který by umožňoval zobrazit hmaty pro více různých akordů.
