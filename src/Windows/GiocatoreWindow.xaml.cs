using HelperAsta.Models;
using System.Windows;

namespace HelperAsta.Windows
{
    /// <summary>
    /// Interazione logica per GiocatoreWindow.xaml
    /// </summary>
    public partial class GiocatoreWindow : Window
    {
        private readonly Giocatore _giocatore;

        public event Action<Giocatore>? GiocatorePresoConfermato;

        // Inizializza la finestra con i dati del giocatore e il numero di alternative disponibili.
        public GiocatoreWindow(Giocatore giocatore, int numeroAlternative)
        {
            InitializeComponent();

            _giocatore = giocatore;

            CaricaDati(numeroAlternative);
        }

        // Carica i dati del giocatore nella finestra.
        private void CaricaDati(int numeroAlternative)
        {
            TxtNome.Text = _giocatore.Nome;
            TxtSquadra.Text = _giocatore.Squadra;

            TxtCategoria.Text =
                $"{_giocatore.Ruolo} • {_giocatore.Obiettivo}";

            TxtAlternative.Text =
                numeroAlternative.ToString();

            TxtPartite.Text =
                $"Partite: {_giocatore.Partite}";

            TxtMedia.Text =
                $"Media: {_giocatore.Media:0.00}";

            TxtFantamedia.Text =
                $"Fantamedia: {_giocatore.Fantamedia:0.00}";

            if (_giocatore.Ruolo == "P")
            {
                TxtGol.Text =
                    $"Gol subiti: {_giocatore.Gol}";
            }
            else
            {
                TxtGol.Text =
                    $"Gol: {_giocatore.Gol}";
            }

            TxtAssist.Text =
                $"Assist: {_giocatore.Assist}";

            TxtGialli.Text =
                $"Gialli: {_giocatore.Gialli}";

            TxtRossi.Text =
                $"Rossi: {_giocatore.Rossi}";

            TxtTitolarita.Text =
                $"Titolarità: {_giocatore.Titolarita}";

            TxtObiettivo.Text =
                $"Obiettivo: {_giocatore.Obiettivo}";

            TxtRigori.Text =
                $"Rigori: {DescriviGerarchia(_giocatore.Rigori)}";

            TxtPunizioni.Text =
                $"Punizioni: {DescriviGerarchia(_giocatore.Punizioni)}";

            TxtInfortunio.Text =
                $"Infortunio: {_giocatore.Infortunio}";

            TxtEstero.Text =
                $"Estero: {_giocatore.Estero}";

            TxtAllenatore.Text =
                $"Allenatore: {_giocatore.Allenatore}";

            TxtSituazioneAllenatore.Text =
                $"Situazione allenatore: {_giocatore.SituazioneAllenatore}";
        }

        // Descrive la gerarchia di un valore numerico in base a regole specifiche.
        private string DescriviGerarchia(int valore)
        {
            return valore switch
            {
                1 => "1° scelta",
                2 => "2° scelta",
                3 => "3° scelta",
                _ => "No"
            };
        }

        // Gestisce l'evento di click sul pulsante "Annulla", chiudendo la finestra.
        private void BtnAnnulla_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        // Gestisce l'evento di click sul pulsante "Preso", invocando l'evento GiocatorePresoConfermato e chiudendo la finestra.
        private void BtnPreso_Click(object sender, RoutedEventArgs e)
        {
            GiocatorePresoConfermato?.Invoke(_giocatore);
            Close();
        }
    }
}