using System.Windows;
using System.Windows.Input;
using HelperAsta.Models;
using HelperAsta.Services;
using HelperAsta.Windows;   
using Microsoft.Win32;
using System.IO;

namespace HelperAsta
{
    /// <summary>
    /// Finestra principale dell'applicazione.
    /// Gestisce il caricamento del dataset, i filtri di ricerca,
    /// la visualizzazione dei giocatori e l'apertura delle finestre di dettaglio.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CsvService _csvService = new CsvService();

        private List<Giocatore> _giocatori = new List<Giocatore>();

        private string? _percorsoCopiaLavoro;

        // Costruttore della finestra principale.
        public MainWindow()
        {
            InitializeComponent();

            InizializzaFiltri();
        }

        // Inizializza le ComboBox dei filtri con le opzioni disponibili.
        private void InizializzaFiltri()
        {
            CmbRuolo.Items.Add("Tutti");
            CmbRuolo.Items.Add("P");
            CmbRuolo.Items.Add("D");
            CmbRuolo.Items.Add("C");
            CmbRuolo.Items.Add("A");
            CmbRuolo.SelectedIndex = 0;

            CmbTitolarita.Items.Add("Tutte");
            CmbTitolarita.Items.Add("T");
            CmbTitolarita.Items.Add("R");
            CmbTitolarita.Items.Add("P");
            CmbTitolarita.SelectedIndex = 0;

            CmbObiettivo.Items.Add("Tutti");
            CmbObiettivo.Items.Add("Top");
            CmbObiettivo.Items.Add("Semitop");
            CmbObiettivo.Items.Add("3° fascia");
            CmbObiettivo.Items.Add("Titolare low cost");
            CmbObiettivo.Items.Add("Titolare");
            CmbObiettivo.Items.Add("1 credito");
            CmbObiettivo.Items.Add("Bug");
            CmbObiettivo.Items.Add("Scommessa");
            CmbObiettivo.Items.Add("Portiere affidabile");
            CmbObiettivo.Items.Add("Riserva");
            CmbObiettivo.Items.Add("Panchinaro");
            CmbObiettivo.SelectedIndex = 0;

            CmbSquadra.Items.Add("Tutte");
            CmbSquadra.SelectedIndex = 0;

        }

        // Event handler per il click sul pulsante "Carica CSV".
        private void BtnCaricaCsv_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Seleziona il file CSV",
                Filter = "File CSV (*.csv)|*.csv"
            };

            bool? risultato = openFileDialog.ShowDialog();

            if (risultato != true)
                return;

            // Carico il CSV selezionato e gestisco eventuali eccezioni.
            try
            {
                string percorsoOriginale = openFileDialog.FileName;

                string cartella = System.IO.Path.GetDirectoryName(percorsoOriginale)!;

                string nomeFile = System.IO.Path.GetFileNameWithoutExtension(percorsoOriginale);

                string estensione = System.IO.Path.GetExtension(percorsoOriginale);

                bool fileGiaDiAsta = nomeFile.EndsWith(
                    "_asta",
                    StringComparison.OrdinalIgnoreCase);

                if (fileGiaDiAsta)
                {
                    _percorsoCopiaLavoro = percorsoOriginale;
                }
                else
                {
                    string possibileCopia = System.IO.Path.Combine(cartella, $"{nomeFile}_asta{estensione}");

                    if (File.Exists(possibileCopia))
                    {
                        MessageBoxResult scelta = MessageBox.Show(
                            "È stata trovata una sessione d'asta precedente.\n\n" +
                            "Vuoi riprendere da quella situazione?\n\n" +
                            "Sì = riprendi l'asta\n" +
                            "No = ricomincia dal CSV originale",
                            "Sessione precedente trovata",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question);

                        if (scelta == MessageBoxResult.Yes)
                        {
                            _percorsoCopiaLavoro = possibileCopia;
                        }
                        else
                        {
                            _percorsoCopiaLavoro =
                                _csvService.CreaCopiaDiLavoro(
                                    percorsoOriginale,
                                    true);
                        }
                    }
                    else
                    {
                        _percorsoCopiaLavoro =
                            _csvService.CreaCopiaDiLavoro(
                                percorsoOriginale);
                    }
                }

                _giocatori =
                    _csvService.CaricaGiocatori(
                        _percorsoCopiaLavoro);

                CaricaFiltroSquadre();

                TxtFileCaricato.Text = System.IO.Path.GetFileName(percorsoOriginale);

                ApplicaFiltri();

                MessageBox.Show(
                    $"Caricati {_giocatori.Count} giocatori.",
                    "CSV caricato",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Errore durante il caricamento del CSV:\n\n{ex.Message}",
                    "Errore",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Event handler per il click sul pulsante "Cerca".
        private void BtnCerca_Click(object sender, RoutedEventArgs e)
        {
            ApplicaFiltri();
        }

        // Event handler per il click sul pulsante "Reset".
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            TxtNome.Text = string.Empty;

            CmbRuolo.SelectedIndex = 0;
            CmbTitolarita.SelectedIndex = 0;
            CmbObiettivo.SelectedIndex = 0;
            CmbSquadra.SelectedIndex = 0;

            ApplicaFiltri();
        }

        // Applica i filtri selezionati dall'utente e aggiorna la visualizzazione dei giocatori.
        private void ApplicaFiltri()
        {
            if (_giocatori.Count == 0)
            {
                DgGiocatori.ItemsSource = null;
                TxtContatore.Text = "Giocatori disponibili: 0";
                return;
            }

            IEnumerable<Giocatore> query =
                _giocatori.Where(g => g.Disponibile);

            string nome = TxtNome.Text.Trim();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(g =>
                    g.Nome.Contains(
                        nome,
                        StringComparison.OrdinalIgnoreCase));
            }

            string? ruolo = CmbRuolo.SelectedItem?.ToString();

            if (!string.IsNullOrWhiteSpace(ruolo) &&
                ruolo != "Tutti")
            {
                query = query.Where(g =>
                    g.Ruolo.Equals(
                        ruolo,
                        StringComparison.OrdinalIgnoreCase));
            }

            string? titolarita = CmbTitolarita.SelectedItem?.ToString();

            if (!string.IsNullOrWhiteSpace(titolarita) &&
                titolarita != "Tutte")
            {
                query = query.Where(g =>
                    g.Titolarita.Equals(
                        titolarita,
                        StringComparison.OrdinalIgnoreCase));
            }

            string? squadra = CmbSquadra.SelectedItem?.ToString();

            if (!string.IsNullOrWhiteSpace(squadra) &&
                squadra != "Tutte")
            {
                query = query.Where(g =>
                    g.Squadra.Equals(
                        squadra,
                        StringComparison.OrdinalIgnoreCase));
            }

            string? obiettivo = CmbObiettivo.SelectedItem?.ToString();

            if (!string.IsNullOrWhiteSpace(obiettivo) &&
                obiettivo != "Tutti")
            {
                query = query.Where(g =>
                    g.Obiettivo.Equals(
                        obiettivo,
                        StringComparison.OrdinalIgnoreCase));
            }

            List<Giocatore> risultati = query
                .OrderBy(g => g.Ruolo)
                .ThenByDescending(g => g.Obiettivo)
                .ThenByDescending(g => g.Fantamedia)
                .ThenBy(g => g.Nome)
                .ToList();

            DgGiocatori.ItemsSource = risultati;

            int disponibiliTotali = _giocatori.Count(g => g.Disponibile);

            TxtContatore.Text = $"Risultati: {risultati.Count} | Disponibili totali: {disponibiliTotali}";
        }

        // Event handler per la gestione del tasto "Invio".
        private void TxtNome_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplicaFiltri();
            }
        }

        // Event handler per il doppio click su un giocatore nella DataGrid.
        private void DgGiocatori_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgGiocatori.SelectedItem is not Giocatore giocatore)
                return;

            // Quando apro il giocatore mostro le alternative con stesso ruolo e stesso obiettivo.
            MostraAlternative(giocatore);

            int numeroAlternative = _giocatori.Count(g =>
                g.Disponibile &&
                g.Ruolo.Equals(
                    giocatore.Ruolo,
                    StringComparison.OrdinalIgnoreCase) &&
                g.Obiettivo.Equals(
                    giocatore.Obiettivo,
                    StringComparison.OrdinalIgnoreCase) &&
                !ReferenceEquals(g, giocatore));

            var finestra = new GiocatoreWindow(
                giocatore,
                numeroAlternative);

            finestra.Owner = this;

            // Se il giocatore viene preso, lo rimuovo.
            finestra.GiocatorePresoConfermato +=
                giocatorePreso =>
                {
                    SegnaComePreso(giocatorePreso);
                };

            // Quando la finestra viene chiusa mantengo soltanto il filtro del ruolo.
            finestra.Closed +=
                (s, args) =>
                {
                    MostraGiocatoriDelRuolo(giocatore);
                };

            finestra.Show();
        }

        // Mostra le alternative con lo stesso ruolo e lo stesso obiettivo del giocatore selezionato.
        private void MostraAlternative(Giocatore giocatore)
        {
            TxtNome.Text = string.Empty;

            CmbRuolo.SelectedItem = giocatore.Ruolo;

            CmbTitolarita.SelectedIndex = 0;

            CmbSquadra.SelectedIndex = 0;

            CmbObiettivo.SelectedItem = giocatore.Obiettivo;

            ApplicaFiltri();
        }

        // Segna il giocatore come preso, aggiornando la disponibilità e salvando le modifiche nel file CSV.
        private void SegnaComePreso(Giocatore giocatore)
        {
            giocatore.Disponibile = false;

            try
            {
                if (!string.IsNullOrWhiteSpace(_percorsoCopiaLavoro))
                {
                    _csvService.SalvaGiocatoriDisponibili(
                        _percorsoCopiaLavoro,
                        _giocatori);
                }

                ApplicaFiltri();
            }
            catch (Exception ex)
            {
                // Se il salvataggio fallisce, annulliamo anche la modifica in memoria.
                giocatore.Disponibile = true;

                MessageBox.Show(
                    $"Impossibile salvare la modifica.\n\n" +
                    $"Il giocatore NON è stato rimosso.\n\n" +
                    $"{ex.Message}",
                    "Errore di salvataggio",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                ApplicaFiltri();
            }
        }

        // Carica le squadre uniche dai giocatori disponibili e le aggiunge al filtro della ComboBox.
        private void CaricaFiltroSquadre()
        {
            CmbSquadra.Items.Clear();
            CmbSquadra.Items.Add("Tutte");

            var squadre = _giocatori
                .Select(g => g.Squadra)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s)
                .ToList();

            foreach (string squadra in squadre)
            {
                CmbSquadra.Items.Add(squadra);
            }

            CmbSquadra.SelectedIndex = 0;
        }

        // Mostra solo i giocatori dello stesso ruolo del giocatore selezionato, resettando gli altri filtri.
        private void MostraGiocatoriDelRuolo(Giocatore giocatore)
        {
            TxtNome.Text = string.Empty;

            CmbRuolo.SelectedItem = giocatore.Ruolo;
            CmbSquadra.SelectedIndex = 0;
            CmbTitolarita.SelectedIndex = 0;
            CmbObiettivo.SelectedIndex = 0;

            ApplicaFiltri();
        }
    }
}