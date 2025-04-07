using System.Numerics;
using System.Text.Json;
using BanqueLib;

int random = Random.Shared.Next(100, 999);
int random2 = Random.Shared.Next(1, 99);
var compte = new Compte(random,"---",0,StatutCompte.Ok,false);
Déserialize();



void Déserialize()
{
    const string datafile = "Compte.json";
    string contenuJson = File.ReadAllText(datafile);

    compte = JsonSerializer.Deserialize<Compte>(contenuJson);
}


while (true)
{

     void Serializer(Compte compte)
     {
        const string datafile = "Compte.json";

        File.WriteAllText(datafile, JsonSerializer.Serialize(compte, new JsonSerializerOptions { WriteIndented = true }));
     }

    

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
        case 'a':
            Serializer(compte);
            Environment.Exit(1000);
            break;

        case 'b':
            compte = new Compte(random, "---", 0, StatutCompte.Ok, false);
            
            Console.WriteLine("Un nouveau compte a été créé");
            break;
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



