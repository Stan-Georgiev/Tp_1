using System.Data.SqlTypes;
using System.Reflection.Metadata;
using System.Text.Json;

namespace BanqueLib
{
    public enum StatutCompte { Ok, Gelé, Vide}
    public class Compte
    {

        #region ----- champs -----

        private readonly int _Numéro;
        private string _Détenteur;
        private decimal _Solde;
        private StatutCompte _Statut;
        private bool _EstGelé;

        #endregion
        #region ----- constructeurs -----
        public Compte(int Numéro = 1, string Détenteur = "", decimal Solde = 0, StatutCompte Statut = StatutCompte.Ok, bool EstGelé = false)
        {
            _Numéro = Numéro;
            _Détenteur = Détenteur;
            decimal.Round(Solde, 2);
            _Solde = Solde;
            _Statut = Statut;
           _EstGelé = EstGelé;

            var arrondi = decimal.Round(Solde, 2);

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Numéro);
            ArgumentException.ThrowIfNullOrWhiteSpace(Détenteur);
            ArgumentOutOfRangeException.ThrowIfNegative(Solde);

            
        }

        #endregion
        #region ----- getters -----
        public int Numéro => _Numéro;
        public decimal Solde => _Solde;
        public StatutCompte Statut => _Statut;
        public string Détenteur => _Détenteur;
        public bool EstGelé => _EstGelé;
        #endregion
        #region ----- setters -----
        public void SetDétenteur (string Détenteur = "") 
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(Détenteur);
            Détenteur = Détenteur.Trim();
            _Détenteur = Détenteur;
        }
        #endregion
        #region ----- Methodes -----
        public string Description()
        {
            string affiche = "";
            affiche += $"[SG] **********************************************************\n";
            affiche += $"[SG] *                                                        *\n";
            affiche += $"[SG] *  Compte: {Numéro,-12}                                  *\n";
            affiche += $"[SG] *  De: {Détenteur,-19}                               *\n";
            affiche += $"[SG] *  Solde: {Solde, -13:c}                                  *\n";
            affiche += $"[SG] *  Statut: {Statut, -13}                                 *\n";
            affiche += $"[SG] **********************************************************\n";
            return affiche;
        }
        
        public bool PeutDéposer (decimal montant = 1)
        {
            var arrondi = decimal.Round(montant, 2);

            if (_EstGelé == true)
            {
                Console.WriteLine($"** Impossible de déposer {arrondi}$");
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool PeutRetirer (decimal montant = 1)
        {
            decimal money = (decimal)Math.Round(Random.Shared.NextDouble() * 100, 2);



            if (_EstGelé == true || money >= _Solde)
            {
                Console.WriteLine($"** Impossible de retirer {money}$");
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
        #region ----- méthodes calculantes -----

        public decimal Déposer(decimal montant)
        {
            var arrondi = decimal.Round(montant, 2);

           

            if (PeutDéposer(montant))
            {
                _Solde += montant;
                return  _Solde;

            }
            else
            {
                return _Solde;
            }
        }

        public decimal Retirer(decimal Montant)
        {
            var arrondi = decimal.Round(Montant, 2);
            if (PeutRetirer(arrondi))
            {
                _Solde -= arrondi;
                Console.WriteLine("** Peut retirer? Oui");
                return _Solde;

            }
            else
            {
                Console.WriteLine("** Peut retirer? Non");
                return _Solde;
            }
        }

        #endregion
        #region ----- méthodes modifiantes -----

        public decimal Vider()
        {
            if (_Solde == 0|| _Statut == StatutCompte.Gelé)
            {
                Console.WriteLine("** Impossible de vider un compte vide ou geler");
                return Solde;
            }
            else
            {
                decimal montant = _Solde;
                _Solde -= montant;
                Console.WriteLine($"Retrait complet de {montant}$");
                return montant;
            }          
        }

        public void Geler()
        {
            if (_EstGelé == true || _Statut == StatutCompte.Gelé)
            {
                Console.WriteLine("Impossible de geler un compte deja geler");
            }
            else
            {
                Console.WriteLine("** Le compte a été gelé");
            }
            _Statut = StatutCompte.Gelé;
            _EstGelé = true;
            
        }

        public void Dégeler()
        {
            
            if (_EstGelé ==true)
            {
                Console.WriteLine("** Le compte a été dégelé");
            }
            _Statut = StatutCompte.Ok;
            _EstGelé = false;
        }
        #endregion

    }
}
