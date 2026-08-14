using HelperAsta.Models;
using System.Globalization;
using System.IO;

namespace HelperAsta.Services
{
    /// <summary>
    /// Servizio per la gestione dei file CSV contenenti i dati dei giocatori.
    /// </summary>
    public class CsvService
    {
        /// Carica i giocatori da un file CSV e li restituisce come lista di oggetti Giocatore.
        public List<Giocatore> CaricaGiocatori(string percorsoFile)
        {
            var giocatori = new List<Giocatore>();

            if (!File.Exists(percorsoFile))
                throw new FileNotFoundException("Il file CSV selezionato non esiste.", percorsoFile);

            string[] righe = File.ReadAllLines(percorsoFile);

            if (righe.Length <= 1)
                return giocatori;

            // Partiamo da 1 perché la riga 0 contiene le intestazioni.
            for (int i = 1; i < righe.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(righe[i]))
                    continue;

                string[] campi = righe[i].Split(';');

                if (campi.Length < 18)
                    continue;

                var giocatore = new Giocatore
                {
                    Nome = campi[0].Trim(),
                    Squadra = campi[1].Trim(),
                    Ruolo = campi[2].Trim(),
                    Titolarita = campi[3].Trim(),

                    Partite = ConvertiIntero(campi[4]),

                    Media = ConvertiDouble(campi[5]),
                    Fantamedia = ConvertiDouble(campi[6]),

                    Obiettivo = campi[7].Trim(),

                    Gol = ConvertiIntero(campi[8]),
                    Assist = ConvertiIntero(campi[9]),

                    Rigori = ConvertiIntero(campi[10]),
                    Punizioni = ConvertiIntero(campi[11]),

                    Gialli = ConvertiIntero(campi[12]),
                    Rossi = ConvertiIntero(campi[13]),

                    Infortunio = campi[14].Trim(),
                    Estero = campi[15].Trim(),

                    Allenatore = campi[16].Trim(),
                    SituazioneAllenatore = campi[17].Trim(),

                    Disponibile = true
                };

                giocatori.Add(giocatore);
            }

            return giocatori;
        }

        // Genera una copia del file CSV originale con il suffisso "_asta" nel nome del file.
        public string CreaCopiaDiLavoro(string percorsoOriginale, bool sovrascrivi = false)
        {
            if (!File.Exists(percorsoOriginale))
                throw new FileNotFoundException(
                    "Il file CSV originale non esiste.",
                    percorsoOriginale);

            string cartella =
                Path.GetDirectoryName(percorsoOriginale)!;

            string nomeFile =
                Path.GetFileNameWithoutExtension(percorsoOriginale);

            string estensione =
                Path.GetExtension(percorsoOriginale);

            // Se per errore viene selezionata direttamente
            // una copia _asta, non creiamo _asta_asta.
            if (nomeFile.EndsWith(
                "_asta",
                StringComparison.OrdinalIgnoreCase))
            {
                return percorsoOriginale;
            }

            string percorsoCopia = Path.Combine(
                cartella,
                $"{nomeFile}_asta{estensione}");

            if (!File.Exists(percorsoCopia) || sovrascrivi)
            {
                File.Copy(
                    percorsoOriginale,
                    percorsoCopia,
                    true);
            }

            return percorsoCopia;
        }

        // Funzione di supporto per convertire una stringa in intero, restituendo 0 in caso di errore.
        private int ConvertiIntero(string valore)
        {
            if (string.IsNullOrWhiteSpace(valore))
                return 0;

            if (double.TryParse(
                valore.Trim(),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double numero))
            {
                return (int)numero;
            }

            return 0;
        }

        // Funzione di supporto per convertire una stringa in double, restituendo 0 in caso di errore.
        private double ConvertiDouble(string valore)
        {
            if (string.IsNullOrWhiteSpace(valore))
                return 0;

            if (double.TryParse(
                valore.Trim(),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double numero))
            {
                return numero;
            }

            return 0;
        }

        // Salva i giocatori disponibili in un file CSV, sovrascrivendo il file esistente se necessario.
        public void SalvaGiocatoriDisponibili(string percorsoFile, IEnumerable<Giocatore> giocatori)
        {
            string percorsoTemporaneo = percorsoFile + ".tmp";

            var righe = new List<string>
            {
                "nome;squadra;ruolo;titolarità;partite;media;fantamedia;obiettivo;gol;assist;rigori;punizioni;gialli;rossi;infortunio;estero;allenatore;situazione_allenatore"
            };

            // Aggiungiamo solo i giocatori disponibili al file CSV.
            foreach (Giocatore g in giocatori.Where(g => g.Disponibile))
            {
                string riga = string.Join(";",
                    g.Nome,
                    g.Squadra,
                    g.Ruolo,
                    g.Titolarita,
                    g.Partite.ToString(CultureInfo.InvariantCulture),
                    g.Media.ToString(CultureInfo.InvariantCulture),
                    g.Fantamedia.ToString(CultureInfo.InvariantCulture),
                    g.Obiettivo,
                    g.Gol.ToString(CultureInfo.InvariantCulture),
                    g.Assist.ToString(CultureInfo.InvariantCulture),
                    g.Rigori.ToString(CultureInfo.InvariantCulture),
                    g.Punizioni.ToString(CultureInfo.InvariantCulture),
                    g.Gialli.ToString(CultureInfo.InvariantCulture),
                    g.Rossi.ToString(CultureInfo.InvariantCulture),
                    g.Infortunio,
                    g.Estero,
                    g.Allenatore,
                    g.SituazioneAllenatore
                );

                righe.Add(riga);
            }

            File.WriteAllLines(percorsoTemporaneo, righe, new System.Text.UTF8Encoding(false));

            File.Move(percorsoTemporaneo, percorsoFile,true);
        }
    }
}
