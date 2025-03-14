using System.Reflection.Metadata;

namespace BanqueLib
{
    public enum StatutCompte { Ok, Gele, Vide}
    public class Compte
    {

        #region ----- champs -----

        private readonly int _numero;
        private string _detenteur;
        private decimal _solde;
        private StatutCompte _statut;
        private bool _estGele;

        #endregion
        #region ----- constructeurs -----
        public Compte(int numero, string detenteur, decimal solde, StatutCompte statut, bool estGele)
        {
            _numero = numero;
            decimal.Round(solde, 2);
            _solde = solde;
            _statut = statut;
           _estGele = estGele;
        }

        #endregion
        #region ----- getters -----
        public int numero => _numero;
        public decimal solde => _solde;
        public StatutCompte statut => _statut;
        public string detenteur => _detenteur;
        #endregion


        public void SetNomDetenteur (string detenteur) 
        {
            if (String.IsNullOrWhiteSpace(detenteur))
            {
                throw new ArgumentNullException();
            }
            detenteur = detenteur.Trim();
            _detenteur = detenteur;
        }

        

        public string Description()
        {
            string affiche = "";
            affiche += $"[SG] ************************************************************************\n";
            affiche += $"[SG] *                                                                      *\n";
            affiche += $"[SG] *               Compte({numero})                                       *\n";
            affiche += $"[SG] *                De({detenteur})                                       *\n";
            affiche += $"[SG] *                Solde({solde})                                        *\n";
            affiche += $"[SG] *                Statut({statut})                                      *\n";
            affiche += $"[SG] ************************************************************************\n";
            return affiche;
        }
        
        public bool PeutDéposer (decimal montant)
        {
            var arrondi = decimal.Round(montant, 2);

            if (montant!= arrondi || montant <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }        

            if (_estGele = true)
            {
                return false;
            }

            return true;
        }

        public bool PeutRetirer (decimal montant)
        {
            var arrondi = decimal.Round(montant, 2);

            if (montant <= 0 || montant != arrondi)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (!_estGele)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region ----- méthodes calculantes -----

        public decimal Déposer(decimal montant)
        {
            var arrondi = decimal.Round(montant, 2);

            if (montant <= 0 || montant != arrondi)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (PeutDéposer(montant))
            {
                _solde += montant;
                return  _solde;

            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public decimal Retirer(decimal Montant)
        {
            var arrondi = decimal.Round(Montant, 2);

            if (Montant <= 0 || Montant != arrondi)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (PeutDéposer(Montant))
            {
                _solde -= Montant;
                return _solde;

            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        #endregion
        #region ----- méthodes modifiantes -----

        public decimal Vider()
        {
            if (_statut != StatutCompte.Vide || _statut == StatutCompte.Gele)
            {
                throw new InvalidOperationException();
            }
            else
            {
                decimal montant = _solde;
                _solde -= montant;
                _statut = StatutCompte.Vide;
                return montant;
            }          
        }

        public void Geler()
        {
            if (_statut == StatutCompte.Gele)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _statut = StatutCompte.Gele;
            }
        }

        public void Dégeler()
        {
                _statut = StatutCompte.Gele;        
        }


        #endregion



    }
}
