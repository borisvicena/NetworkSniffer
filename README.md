# IMPLEMENTAČNÁ DOKUMENTÁCIA KU 2. PROJEKTU DO PREDMETU IPK (ZETA: Network sniffer) 2023

### Meno a Priezvisko: Boris Vícena

### Login: xvicen10

## Úvod

Tento dokument poskytuje stručný prehľad implementácie druhého projektu do predmetu IPK, zameraného na zachytávanie a analýzu sieťových paketov. Umožňuje používateľom vybrať si sieťové rozhrania a porty a nastaviť filtre pre špecifické protokoly, aby sa optimalizovalo získavanie a analýza relevantných dát.

## Obsah dokumentácie

1. Úvod
2. Implementácia
3. Popis funkcionality
4. Testovanie
5. Dodatočná funkcionalita
6. Zdroje

## Implementácia

Program _ipk-sniffer_ bol implementovaný v jazyku _C#_ s využitím _.NET Frameworku_, ktorý poskytuje _API_ pre sieťové operácie a prácu so sieťovými packetmi. Výber tohto jazyka bol motivovaný vysokou efektívnosťou a širokou podporou pre sieťové programovanie.

### Použité knižnice

- **SharpPcap:** Táto knižnica umožňuje nízkoúrovňový prístup k sieťovým rozhraniam, čo je nevyhnutné pre efektívne zachytávanie paketov. Poskytuje nástroje pre jednoduchú manipuláciu a analýzu paketov v reálnom čase.
- **PacketDotNet:** Knižnica je integrovaná s _SharpPcap_ a poskytuje _API_ pre analýzu a tvorbu paketových dát, čo zjednodušuje prácu s komplexnými sieťovými protokolmi ako _TCP_, _UDP_, _ICMP_ a ďalšie.

## Popis funkcionality

Cieľom bolo vytvoriť program, ktorý bude schopný úspešne zachytávať sieťové packety. Program je navrhnutý pre flexibilné filtrovanie a analýzu paketov v reálnom čase. Skladá sa z nasledujúcich hlavných komponentov:

### Trieda Options (Options.cs)

Táto trieda uchováva konfiguračné nastavenia programu. Umožňuje špecifikáciu sieťových rozhraní, portov a typov protokolov. Podporuje protokoly ako _TCP_, _UDP_, _ARP_, _NDP_, _ICMP_ (verzie 4 a 6), _IGMP_ a _MLD_. Umožňuje tiež určenie maximálneho počtu paketov na zachytenie.

### Trieda Sniffer (Program.cs)

Predstavuje hlavný vstupný bod aplikácie. Zodpovedá za inicializáciu a riadenie procesu zachytávania paketov na základe nastavení od používateľa. Obsahuje metódy na analýzu príkazových riadkov a spustenie zachytávania na zvolenom rozhraní.

### Trieda PacketContext (PacketContext.cs)

Reprezentuje kontext jedného sieťového paketu a obsahuje všetky relevantné údaje pre jeho analýzu, ako sú základné ethernetové dáta, _IP_, _TCP_, _UDP_, _ARP_, _NDP_, _ICMPv4_, _ICMPv6_, _IGMP_ a _MLD_ packety ak sú dostupné. Zaznamenáva tiež dĺžku, čas a samotné dáta zachytenia paketu.

### Trieda PacketHandler (PacketHandler.cs)

Zodpovedá za spracovanie prichádzajúcich paketov podľa nastavených filtrov a ich klasifikáciu do príslušných kontextov pre ďalšiu analýzu. Metódy zahŕňajú spracovanie konkrétnych paketov a extrahovanie dôležitých dát.

### Trieda PrintPacket (PrintPacket.cs)

Zodpovedná za výpis informácií o paketoch. Ponúka metódy na formátovanie a výpis detailov pre rôzne typy paketov, vrátane ethernetových, _IP_, _TCP_, _UDP a iných_.

## Testovanie

Testovanie programu bolo zamerané na overenie jeho schopnosti správne zachytiť a analyzovať jednotlivé sieťové pakety podľa užívateľských špecifikácií. Boli vykonané nasledujúce kroky:

### Testované Aspekty

- Správnosť filtrovania paketov podľa portu a protokolu.
- Výkonnosť aplikácie pri spracovaní veľkého množstva paketov.
- Presnosť výpisu dát z paketov.

### Dôvody Testovania

- Zabezpečiť, že aplikácia presne zachytáva a spracováva dáta podľa zadaných uživateľských argumentov.
- Overiť stabilitu a efektívnosť aplikácie pri používaní.

### Metódy Testovania

- Manuálne testovanie s využitím špecifických scenárov zachytávania paketov.

### Testovacie Prostredie

- Testy boli vykonané na viacerých sieťových rozhraniach s rôznymi operačnými systémami, vrátane _Linuxu_, _MacOS_ a _Windows_.
- Výsledky výstupu programu boli porovnávané s výstupmi z aplikácie _Wireshark_ pre určenie správnosti funkcionality.

## Dodatočná funkcionalita

Ak užívateľ špecifikuje, ktoré protokoly chce monitorovať, program poskytne na výpise paketov rozšírené informácie o týchto protokoloch. Toto zahŕňa podrobnejšie dáta o relevantných atribútoch packetu, čo umožňuje užívateľovi lepšie pochopenie obsahu a kontextu sieťovej komunikácie a packetu.

## Zdroje

- Dokumentácia jazyka C#, .NET Framework (Microsoft) a použitých knižníc (SharpPcap, PacketDotNet)
- [Geeks for Geeks: What is Packet Sniffing?](https://www.geeksforgeeks.org/what-is-packet-sniffing/)
