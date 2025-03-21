using System.Numerics;
using BanqueLib;

int random = Random.Shared.Next(100, 999);
int random2 = Random.Shared.Next(1, 99);
var compte = new Compte(random,"---",0,StatutCompte.Ok,false);
while (true)
{
    Console.Clear();
    Console.WriteLine(compte.Description());
    Console.WriteLine(
        $"""       
    
     TESTER COMPTE

     1 - Modifier détenteur
     2 - Peut déposer
     3 - Peut retirer
     4 - Peut retirer(Montant)
     5 - Déposer (montant)
     6 - Retirer (montant)
     7 - Vider
     8 - Geler
     9 - Dégeler
     a - Quitter
     b - reset
     

     Votre choix, stanislav Georgiev ?

    """);

    switch (Console.ReadKey(true).KeyChar)
    {
        case '1':
            compte.SetDétenteur($"Stanislav Georgiev {random2}");
            Console.WriteLine($"Détenteur modifié pour: {$"{compte.Détenteur}"} {random2}");
            break;
        case '2':
            if (compte.PeutDéposer())
                Console.WriteLine("** Peut déposer? Oui.");
            else
                Console.WriteLine("** Peut déposer? Non.");
            break;
        case '3':
            if (compte.PeutRetirer())
                Console.WriteLine("** Peut retirer? Oui.");
            else
                Console.WriteLine("** Peut retirer? Non.");
            break;
        case '4': // Déposer
            {
                decimal money = (decimal)Math.Round(Random.Shared.NextDouble() * 100, 2);

                if (money < compte.Solde)
                    Console.WriteLine($"** Peut retirer {money}? Oui.");
                else
                    Console.WriteLine($"** Peut retirer {money}? Non.");
            }
            break;
        case '5': // Retirer
            {
                decimal money = (decimal)Math.Round(Random.Shared.NextDouble() * 100, 2);
                compte.Déposer(money);
                Console.WriteLine($"** dépot de {money}$");
            }
            break;
        case '6': // Vider
            {
                decimal money = (decimal)Math.Round(Random.Shared.NextDouble() * 100, 2);
                compte.Retirer(money);
                Console.WriteLine($"** retrait de {money}$");
            }
            break;
        case '7': // Geler 
            {
                compte.Vider();
            }
            break;
        case '8': 
            {
                if (compte.Statut == StatutCompte.Gelé)
                {
                    Console.WriteLine("** Impossible de geler un compte deja geler");
                }
                else
                {
                    compte.Geler();
                }
            }
            break;
        case '9': // setters
            {
                if (compte.Statut == StatutCompte.Ok)
                {
                    Console.WriteLine("** Impossible de degeler un compte deja degeler");
                }
                else
                {
                    compte.Dégeler();
                }
            }
            break;
        case 'a':  // erreurs de constructions
            {
                (string, string?)[] erreurs =
                {
                    Utile.ExceptMsg(() => new Compte(0, "Han")),
                    Utile.ExceptMsg(() => new Compte(-1, "Han")),
                    Utile.ExceptMsg(() => new Compte(1, null!)),
                    Utile.ExceptMsg(() => new Compte(1, "")),
                    Utile.ExceptMsg(() => new Compte(1, "    ")),
                    Utile.ExceptMsg(() => new Compte(1, "Han", -1)),
                    Utile.ExceptMsg(() => new Compte(1, "Han", 0.001m)),
                };
                foreach (var (excep, message) in erreurs)
                    Console.WriteLine($"\n {excep}\n {message ?? ""}");
            }
            break;
        case 'b':  // erreurs de setter
            {
                var ok = new Compte(1009, "Obiwan", 7000);
                (string, string?)[] erreurs =
                {
                    Utile.ExceptMsg(() => ok.SetDétenteur(null!)),
                    Utile.ExceptMsg(() => ok.SetDétenteur("")),
                    Utile.ExceptMsg(() => ok.SetDétenteur("  ")),
                };
                foreach (var (excep, message) in erreurs)
                    Console.WriteLine($"\n {excep}\n {message ?? ""}");
            }
            break;
        case 'c':  // erreurs PeutRetirer, PeutDéposer
            {
                var ok = new Compte(1009, "Obiwan", 7000);
                (string, string?)[] erreurs =
                {
                    Utile.ExceptMsg(() => ok.PeutDéposer(-1)),
                    Utile.ExceptMsg(() => ok.PeutDéposer(1.001m)),
                    Utile.ExceptMsg(() => ok.PeutRetirer(-1)),
                    Utile.ExceptMsg(() => ok.PeutRetirer(1.001m)),
                };
                foreach (var (excep, message) in erreurs)
                    Console.WriteLine($"\n {excep}\n {message ?? ""}");
            }
            break;
        case 'd':  // erreurs de retrait et dépôt
            {
                var ok = new Compte(1009, "Obiwan", 7000);
                var gelé = new Compte(1009, "Obiwan", 7000, StatutCompte.Gelé);
                (string, string?)[] erreurs =
                {
                    Utile.ExceptMsg(() => ok.Déposer(1000.001m)),
                    Utile.ExceptMsg(() => gelé.Déposer(1000)),
                    Utile.ExceptMsg(() => ok.Retirer(8000)),
                    Utile.ExceptMsg(() => ok.Retirer(1000.001m)),
                    Utile.ExceptMsg(() => gelé.Retirer(1000)),
                };
                foreach (var (excep, message) in erreurs)
                    Console.WriteLine($"\n {excep}\n {message ?? ""}");
            }
            break;
        case 'e':  // erreurs pour geler, dégeler et vider
            {
                var ok = new Compte(1009, "Obiwan", 0);
                var gelé = new Compte(1009, "Obiwan", 10, StatutCompte.Gelé);
                (string, string?)[] erreurs =
                {
                    Utile.ExceptMsg(() => ok.Dégeler()),
                    Utile.ExceptMsg(() => gelé.Geler()),
                    Utile.ExceptMsg(() => ok.Vider()),
                    Utile.ExceptMsg(() => gelé.Vider()),
                };
                foreach (var (excep, message) in erreurs)
                    Console.WriteLine($"\n {excep}\n {message ?? ""}");
            }
            break;
        case 'q':
            Environment.Exit(0); break;
        default:
            Console.WriteLine(" Mauvais choix"); break;
    }
    Console.WriteLine("\n Appuyer sur ENTER pour continuer...");
    Console.ReadLine();
}

#pragma warning disable S3903 // Types should be defined in named namespaces
static class Utile
{
    public static (string, string?) ExceptMsg(Action action)
    {
        try
        {
            action();
            return ("EXCEPTION attendue", null);
        }
        catch (Exception ex)
        {
            return (ex.GetType().Name, ex.Message);
        }
    }
}
