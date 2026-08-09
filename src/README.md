# Helper Asta

Applicazione desktop WPF sviluppata in C# per supportare la gestione di un'asta Fantacalcio.

## Obiettivo

Helper Asta permette di caricare un dataset CSV contenente i calciatori disponibili e di consultarli rapidamente durante un'asta.

L'applicazione consente di:

- cercare un calciatore per nome;
- filtrare i giocatori per ruolo;
- filtrare per titolarità;
- filtrare per obiettivo;
- visualizzare statistiche e informazioni del calciatore;
- visualizzare le alternative disponibili appartenenti alla stessa fascia;
- segnare un calciatore come preso, rimuovendolo dalla lista dei disponibili;
- monitorare il numero di giocatori ancora disponibili.

## Ruoli

I giocatori sono suddivisi in:

- P - Portieri
- D - Difensori
- C - Centrocampisti
- A - Attaccanti

## Dataset

L'applicazione utilizza un file CSV con separatore `;`.

Il file originale non viene modificato: durante l'utilizzo viene creata una copia di lavoro sulla quale vengono registrate le modifiche effettuate durante l'asta.

## Tecnologie

- C#
- .NET
- WPF
- Visual Studio Community

## Stato del progetto

In sviluppo.