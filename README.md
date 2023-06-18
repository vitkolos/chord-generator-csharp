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

Druhá určuje ladění strun, jde o seznam tónů odpovídajících jednotlivým strunám, pořadí v seznamu odpovídá pořadí na nástroji (zleva doprava). Pokud jsou všechny tóny v jedné oktávě (jednočárkované) a lze je zapsat bez posuvek, je možné je napsat bez mezer (např. `GCEA`). Jinak je nutné je oddělit mezerami, přičemž za název tónu se uvádí číslo oktávy (podle *vědecké* notace, kde jednočárkovaná oktáva má číslo 4, *komorní a* by se tedy zapsalo jako A4). Pokud číslo oktávy chybí, je použita výchozí (jednočárkovaná) oktáva.

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
