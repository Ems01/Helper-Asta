namespace HelperAsta.Models
{

    /// <summary>
    /// Rappresenta un giocatore di calcio con le sue statistiche e informazioni correlate.
    /// </summary>
    public class Giocatore
    {
        public string Nome { get; set; } = string.Empty;
        public string Squadra { get; set; } = string.Empty;
        public string Ruolo { get; set; } = string.Empty;
        public string Titolarita { get; set; } = string.Empty;
        public int Partite { get; set; }
        public double Media { get; set; }
        public double Fantamedia { get; set; }
        public string Obiettivo { get; set; } = string.Empty;
        public int Gol { get; set; }
        public int Assist { get; set; }
        public int Rigori { get; set; }
        public int Punizioni { get; set; }
        public int Gialli { get; set; }
        public int Rossi { get; set; }
        public string Infortunio { get; set; } = string.Empty;
        public string Estero { get; set; } = string.Empty;
        public string Allenatore { get; set; } = string.Empty;
        public string SituazioneAllenatore { get; set; } = string.Empty;
        public bool Disponibile { get; set; } = true;
    }
}
