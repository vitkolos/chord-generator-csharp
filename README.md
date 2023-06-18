# Generátor hmatů akordů

| Vít Kološ, 1. ročník, IPP | letní semestr 2022/2023 | NPRG031 Programování 2 |
| - | - | - |

## Anotace

Program pro zadaný akord určí hmaty, kterými jej lze na kytaru či jiný strunný nástroj zahrát, a zobrazí jejich diagramy.

## Použití programu

Za předpokladu, že je program správně konfigurován (viz sekce [Konfigurace](#konfigurace)), spočívá jeho použití v zadání [názvu akordu](#název-akordu) (např. D#maj7) a potvrzení vstupu tlačítkem Generovat nebo klávesou Enter. Následně se zobrazí několik způsobů, jak daný akord na zvolený nástroj zahrát. Tyto varianty jsou seřazeny podle několika kritérií, obvykle je vhodné použít jeden z prvních tří zobrazených hmatů. Pokud konfigurační soubor obsahuje více hudebních nástrojů, je možné vybrat, pro který z nich se hmaty mají generovat.

### Název akordu

Název akordu se může skládat ze tří částí: základního tónu, typu akordu a basového tónu za lomítkem. Všechny podporované typy akordů musí být uvedeny v dokumentaci. Není-li typ akordu zadán, je automaticky vyhodnocen jako `major` (tento typ akordu by tedy v konfiguraci neměl chybět). Basový tón se píše až za typ akordu a je zleva oddělen lomítkem.

Tóny (základní i basové) jsou tvořeny jedním nebo dvěma znaky, kde první je velké písmeno a druhý je znak `#` nebo `b`. Písmena odpovídají českému (respektive německému) systému pojmenování not, tedy po notě A následují (po půltónech) B, H, C. (V anglosaském systému by to bylo A, Bb, B, C.) Funguje enharmonická záměna, tedy platí, že G# = Ab nebo že B = Hb = A#. S tónem B je nakládáno specificky, pokud za ním následuje posuvka (křížek nebo béčko), tak se tato již nevyhodnocuje (tedy Bb = B). Dvojité posuvky (Gbb) ani alternativní označení (Dis, Es) nejsou podporovány.

Příklady možných názvů akordů: `C/E`, `Ebmaj7`, `Fsus4`, `F#m`

### Doplňkové funkce

Je-li přítomná složka `audio` se zvukovými soubory (požadovaná vnitřní struktura složky a názvy souborů jsou specifikovány v kódu programu, konkrétně v souboru `classes/ChordPlayer.cs`) a spadají-li tóny akordu do podporovaného rozsahu, je možné konkrétní variantu akordu odpovídající danému hmatu přehrát kliknutím na jeho diagram.

Konfigurační soubor lze otevřít přímo z okna programu stisknutím tlačítka „konfigurovat“.

## Konfigurace

Program očekává existenci konfiguračního souboru `config.txt`. Ten se načítá při startu programu, pokud tedy program běží a tento soubor je změněn, je nutné program spustit znova.

Konfigurační soubor se skládá ze dvou částí oddělených prázdným řádkem. První část odpovídá seznamu hudebních nástrojů, druhá seznamu typů akordů. Jeden řádek odpovídá jednomu nástroji nebo typu akordu. Každý řádek obsahuje dvě nebo tři hodnoty oddělené čárkou.

### Konfigurace hudebního nástroje

U hudebního nástroje určuje první hodnota jeho název (nesmí obsahovat čárku).

Druhá určuje ladění strun, jde o seznam tónů odpovídajících jednotlivým strunám, pořadí v seznamu odpovídá pořadí na nástroji (zleva doprava). Pokud jsou všechny tóny v jedné oktávě (jednočárkované) a lze je zapsat bez posuvek, je možné je napsat bez mezer (např. `GCEA`). Jinak je nutné je oddělit mezerami, přičemž za název tónu se uvádí číslo oktávy (podle *vědecké* notace, kde jednočárkovaná oktáva má číslo 4, *komorní a* by se tedy zapsalo jako A4). Pokud číslo oktávy chybí, je použita výchozí (jednočárkovaná) oktáva. Specifika zápisu tónu jsou uvedeny v sekci [Název akordu](#název-akordu).

Poslední hodnota odpovídá počtu dostupných pražců (nultý se nepočítá), tato hodnota je volitelná (výchozí počet je 20) a obvykle nehraje příliš velkou roli, protože většina základních akordů se vejde na prvních několik pražců.

### Konfigurace typu akordu

První hodnota odpovídá seznamu možných označení akordu oddělených mezerou. Speciální roli hraje označení `major`, neboť pokud uživatel při používání programu nezadá typ akordu, automaticky se zvolí tento typ.

Druhá hodnota obsahuje seznam tónů, z nichž se akord skládá, tóny jsou zde reprezentovány svými vzdálenostmi od tóniky. Každý tón tedy odpovídá číslu z rozsahu 0–11. Číslo 10 lze alternativně zapsat písmenem `t` nebo `A`, číslo 11 písmenem `e` nebo `B`. Je-li některý z tónů zapsán více než jedním znakem, je nutné tóny oddělit mezerami.

### Vzorová konfigurace

Tento vzor záměrně obsahuje několik variant značení, při praktickém použití je vhodné zvolit jednu z nich.

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
- pražec – kovový pásek vsazený do hmatníku, nad nímž je nutné stisknout strunu, aby zněl určitý tón (k-tý pražec tón struny obvykle zvyšuje o k půltónů; nultý pražec se pochopitelně nemačká)
- otevřená struna – struna, která v daném hmatu není stisklá a zní
- tlumená struna – struna, která v daném hmatu není stiklá a nezní (nebo ji hráč prsty tlumí, aby nezněla)

## Řešený problém a algoritmus

Za zajímavé části programu považuji generování všech možných hmatů pro určitý akord, seřazení těchto variant podle vhodně zvolených kritérií, očíslování jednotlivých prstů v daném hmatu a vykreslení diagramu. Z toho první dvě řeší ústřední problém, kterým se můj zápočtový program zabýval, totiž **jak nalézt vhodný hmat**, kterým lze akord zahrát (ideálně ten, který muzikanti obvykle používají), bez pomoci internetu či příruček.

### Vhodný hmat

Ale jak poznat vhodný hmat? Nu, to je ten, který se dobře hraje a dobře zní. Je jisté, že hmat, k jehož stisknutí je potřeba 6 prstů, se moc dobře hrát nebude. Hmat akordu D sice zní dobře, ale pokud by ho program vrátil jako hmat akordu C, nebyl bych spokojen. Určil jsem tyto nutné podmínky, aby se hmat dal považovat za platný:

1. musí obsahovat všechny tóny akordu a žádné jiné,
2. k jeho stisknutí jsou použity nejvýše čtyři prsty,
3. prsty se vejdou na hmatník (hmat nepoužívá neexistující pražce),
4. je-li v akordu použito barré, je aktivně využíváno,
5. tlumené struny nejsou z obou stran obklopeny netlumenými.

Splnění těchto podmínek je realizováno rekurzivním voláním funkce, která postupně vygeneruje všechny možné akordy. O zadaném akordu je známo, z jakých tónů se musí skládat (kombinací základního tónu a typu akordu, případně i basového tónu), funkce tedy na každé struně zkouší postupně stisknout každý z tónů nebo strunu tlumit. Zároveň si eviduje, kolik tónů je již ve vygenerovaném souzvuku zastoupeno. Pokud výsledný hmat splňuje nutná kritéria, je přidán do seznamu všech možných hmatů.

Pro ukulele (čtyřstrunný nástroj s 19 pražci) teoreticky existuje 160 000 hmatů. Po aplikaci uvedených kritérií pro určitý akord jich zbyde přibližně 700. Ty je potřeba nějak seřadit a určit, které jsou nejvýhodnější.

Řazení řeší ohodnocovací funkce, která každému hmatu přiděluje skóre. To ovlivňují následující kritéria: barré, základní tón (zda je nejnižším tónem souzvuku), výška nejnižšího tónu, výškové rozpětí tónů v souzvuku, vzájemná vzdálenost prstů na hmatníku, počet použitých prstů, přítomnost tlumených strun. Je tam rovněž přítomno několik kritérií, které mají tak výraznou váhu, že akord obvykle odsunou z první desítky: téměř nevyužívané barré, basový tón není nejnižším tónem, příliš mnoho tlumených strun, tlumené struny na obou stranách hmatníku.

Ohodnocovací funkci jsem vytvářel ručně, snažil jsem se, aby tradiční hmaty všech základních akordů byly na prvním místě nebo alespoň v první trojici. Sice stále nevrací dokonalé výsledky, ale někdy se ukazuje, že „správný“ výsledek ani vrátit nemůže, jako v případě kytarového akordu C7, jehož tradiční hmat neobsahuje tón g.

#### Jinak a hůře

Jedním z možných jiných řešení je algoritmus, který by na každé struně zvolil první (nejnižší) tón, který je v daném akordu obsažen. Pro dostatečně jednoduchý nástroj, jakým je ukulele, a pár základních akordů tento postup skutečně vrací správné výsledky. U jiných akordů však tímto způsobem mineme velmi výhodné hmaty a snahy o odvrácení tohoto jevu vedou k řešení velmi podobnému tomu mému.

### Prsty a diagramy



## Závěr

- eliminovat nechytnutelné hmaty
- hledat barréčka kde nejsou
- umožnit tlumené struny uprostřed
- generovat ohodnocovací funkci automaticky (?)
