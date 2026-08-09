using HelperAsta.Models;
using System.Windows;

namespace HelperAsta.Windows
{
    public partial class GiocatoreWindow : Window
    {
        private readonly Giocatore _giocatore;

        public event Action<Giocatore>? GiocatorePresoConfermato;

        public GiocatoreWindow(Giocatore giocatore, int numeroAlternative)
        {
            InitializeComponent();

            _giocatore = giocatore;

            CaricaDati(numeroAlternative);
        }

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

        private void BtnAnnulla_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnPreso_Click(object sender, RoutedEventArgs e)
        {
            GiocatorePresoConfermato?.Invoke(_giocatore);
            Close();
        }
    }
}