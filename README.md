# Helper Asta

Helper Asta è un'applicazione desktop sviluppata in **C# e WPF** per supportare la gestione di un'asta Fantacalcio.

Il progetto nasce per un formato d'asta in cui i calciatori vengono scorsi progressivamente da una lista, anziché essere chiamati liberamente dai partecipanti. In questo contesto diventa importante sapere rapidamente quali alternative siano ancora disponibili per ruolo e fascia, senza dover tenere a mente l'intero elenco dei giocatori.

L'applicazione utilizza un dataset CSV preparato prima dell'asta e consente di cercare, filtrare e confrontare i calciatori ancora disponibili.

## Funzionalità

Helper Asta permette di:

- importare un dataset CSV;
- cercare un calciatore tramite nome completo o parziale;
- filtrare per ruolo, squadra, titolarità e obiettivo;
- combinare più filtri contemporaneamente;
- visualizzare le statistiche e le informazioni utili per l'asta;
- aprire il dettaglio di un giocatore senza bloccare la schermata principale;
- visualizzare automaticamente le alternative appartenenti allo stesso ruolo e alla stessa fascia;
- conoscere quante alternative della stessa categoria siano ancora disponibili;
- segnare un giocatore come acquistato e rimuoverlo dai giocatori disponibili;
- mantenere lo stato dell'asta anche dopo la chiusura o un eventuale arresto anomalo dell'applicazione.

## Utilizzo durante l'asta

Supponiamo che venga estratto un difensore classificato come:

```text
Ruolo: D
Obiettivo: Top
```

Aprendo il giocatore, Helper Asta mostra le sue statistiche e contemporaneamente filtra la schermata principale mostrando gli altri difensori `Top` ancora disponibili.

Questo permette di capire immediatamente quanto sia rischioso rinunciare a quel giocatore e quali alternative rimangano nella stessa fascia.

La finestra del dettaglio rimane indipendente dalla schermata principale, permettendo di confrontare il giocatore selezionato con gli altri risultati.

## Dataset

L'applicazione utilizza un file CSV con separatore `;` e codifica UTF-8.

Ogni riga rappresenta un calciatore.

Il dataset attualmente previsto contiene:

```text
nome
squadra
ruolo
titolarità
partite
media
fantamedia
obiettivo
gol
assist
rigori
punizioni
gialli
rossi
infortunio
estero
allenatore
situazione_allenatore
```

### Ruoli

I ruoli sono rappresentati tramite:

| Codice | Ruolo |
|---|---|
| P | Portiere |
| D | Difensore |
| C | Centrocampista |
| A | Attaccante |

### Titolarità

La colonna `titolarità` rappresenta una valutazione preventiva dell'utilizzo del giocatore nella stagione dell'asta:

| Codice | Significato |
|---|---|
| T | Titolare |
| R | Riserva |
| P | Panchinaro |

Non rappresenta le presenze della stagione precedente.

### Obiettivo

La colonna `obiettivo` identifica la fascia assegnata al giocatore in preparazione dell'asta.

Per i portieri:

1. Top
2. Semitop
3. Portiere affidabile
4. Riserva

Per difensori, centrocampisti e attaccanti:

1. Top
2. Semitop
3. 3° fascia
4. Titolare low cost
5. Titolare
6. 1 credito
7. Bug
8. Scommessa
9. Riserva
10. Panchinaro

### Rigori e punizioni

Le colonne `rigori` e `punizioni` indicano la posizione del giocatore nella gerarchia dei battitori:

| Valore | Significato |
|---|---|
| 0 | Non batte |
| 1 | Prima scelta |
| 2 | Seconda scelta |
| 3 | Terza scelta |

### Allenatore

La colonna `allenatore` indica la valutazione dell'impatto dell'allenatore sul ruolo del giocatore:

```text
meglio
neutrale
peggio
```

La colonna `situazione_allenatore` indica invece se l'allenatore è:

```text
Nuovo
Confermato
```

## Gestione della sessione d'asta

Helper Asta non modifica direttamente il dataset originale.

Se viene importato:

```text
giocatori_completi.csv
```

viene creata una copia di lavoro:

```text
giocatori_completi_asta.csv
```

Le operazioni dell'asta vengono eseguite esclusivamente sulla copia.

Quando un giocatore viene segnato come acquistato, il file di lavoro viene aggiornato immediatamente. In questo modo lo stato dell'asta non dipende dalla corretta chiusura dell'applicazione.

Se l'applicazione viene chiusa o si verifica un crash, alla successiva apertura è possibile riprendere la sessione utilizzando la copia già esistente.

Il CSV originale rimane quindi sempre disponibile come sorgente completa dei dati.

## Ricerca delle alternative

Uno degli elementi principali dell'applicazione è il confronto automatico tra giocatori appartenenti alla stessa fascia.

Quando viene aperto un calciatore, vengono utilizzati:

```text
Ruolo + Obiettivo
```

per individuare le alternative ancora disponibili.

Per esempio:

```text
Dimarco
Ruolo: D
Obiettivo: Top
```

porta automaticamente alla ricerca degli altri:

```text
Difensori
Top
```

ancora presenti nell'asta.

Il giocatore selezionato non viene considerato nel conteggio delle alternative.

## Struttura del progetto

```text
Helper-Asta/
│
├── README.md
├── Helper-Asta.sln
├── .gitignore
├── .gitattributes
│
└── src/
    └── HelperAsta/
        ├── HelperAsta.csproj
        ├── App.xaml
        ├── App.xaml.cs
        ├── AssemblyInfo.cs
        ├── MainWindow.xaml
        ├── MainWindow.xaml.cs
        ├── icona.ico
        │
        ├── Models/
        │   └── Giocatore.cs
        │
        ├── Services/
        │   └── CsvService.cs
        │
        └── Windows/
            ├── GiocatoreWindow.xaml
            └── GiocatoreWindow.xaml.cs
```

### `Models`

Contiene i modelli dell'applicazione.

`Giocatore.cs` rappresenta il singolo calciatore e contiene le proprietà corrispondenti alle informazioni presenti nel CSV.

### `Services`

Contiene la logica non direttamente collegata all'interfaccia grafica.

`CsvService.cs` gestisce:

- caricamento del dataset;
- conversione dei dati;
- creazione della copia di lavoro;
- salvataggio dello stato dell'asta.

### `Windows`

Contiene le finestre secondarie dell'applicazione.

`GiocatoreWindow` mostra il dettaglio del giocatore selezionato e permette di segnarlo come acquistato.

## Tecnologie

Il progetto utilizza:

- C#
- .NET
- WPF
- XAML
- LINQ
- Git
- GitHub

Lo sviluppo viene effettuato tramite Visual Studio Community.

## Architettura

Helper Asta è volutamente un'applicazione locale e semplice.

Non utilizza:

- database;
- server;
- API esterne;
- servizi cloud;
- autenticazione;
- connessione Internet.

Il dataset e lo stato dell'asta vengono gestiti tramite file CSV locali.

Questa scelta mantiene il progetto leggero e riduce le dipendenze durante l'utilizzo dal vivo.

## Stato del progetto

La prima versione dell'applicazione comprende le funzionalità necessarie per l'utilizzo durante un'asta Fantacalcio:

- caricamento del dataset;
- ricerca e filtri;
- consultazione dei giocatori;
- confronto delle alternative;
- gestione dei giocatori acquistati;
- persistenza e recupero della sessione.

## Possibili sviluppi futuri

Il progetto può essere esteso in futuro con funzionalità come:

- gestione del budget;
- composizione della propria rosa;
- numero di slot ancora disponibili per ruolo;
- prezzo di acquisto dei giocatori;
- storico degli acquisti;
- esportazione della rosa finale;
- dashboard riepilogativa dell'asta.

Queste funzionalità non sono necessarie per l'obiettivo della prima versione.

## Autore

**Enrico Maria Sardellini**
