using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GuidePrenoms
{
    class GuidePrenomsApp
    {
        public const int ANNEEMIN = 1900;
        public const int TOP100 = 100;
        public const int NBANNEES = 114;

        /* Mettez entre "" l'URL du fichier */
        public const string URL = @"C:\Users\Alexandre\Desktop\Programmation\C#\GuidePrenoms\GuidePrenoms\prenoms_bordeaux.txt";
        //public const string URL = @"C:\Users\amonge.ENSC\Desktop\Cours & TPs\S5\Info\GuidePrenoms\GuidePrenoms\prenoms_bordeaux.txt";

        #region Enums
        public enum MODE
        {
            AFFICHAGE = '1',
            TOP10 = '2',
            QUITTER = 'q'
        }
        #endregion
        #region Structures
        public struct Prenom
        {
            public int annee { get; set; }
            public string prenom { get; set; }
            public int nombre { get; set; }
            public int ordre { get; set; }
        }
        #endregion
        #region Lecture du fichier
        /* Cette méthode sera appelée une seule fois dans le programme. Une fois les données stockées
         * dans un tableau, on manipulera le tableau à la place du fichier. */
        public static Prenom[] lectureFichier()
        {
            Prenom[] prenoms = new Prenom[TOP100 * NBANNEES];   // Tableau contenant les données
            string fichier = URL;                               // URL du fichier texte

            try
            {
                StreamReader sr = new StreamReader(fichier, Encoding.Unicode);  // On créé notre lecteur
                string ligne = sr.ReadLine(), mot = "";
                int i = 0, j = 0, indexDonnees = 0;
                string[] donnees = new string[4];   // Va contenir le mot compris entre deux tabulations

                ligne = sr.ReadLine();  // On passe à la ligne 2 du fichier (la 1 ne nous intéresse pas)

                while (ligne != null)   // Tant qu'on est pas à la fin, on boucle
                {
                    /* Dans ce for, on va stocker dans donnees les 4 "mots" d'une ligne (année, prénom, ...) */
                    for (j = 0; j < ligne.Length; ++j)
                        if (!'\t'.Equals(ligne[j]))
                            mot += ligne[j];
                        else
                            if (indexDonnees < 4)
                            {
                                donnees[indexDonnees] = mot;
                                ++indexDonnees;
                                mot = "";
                            }

                    /* On stocke le dernier mot dans donnees */
                    if (j == ligne.Length)
                        if (indexDonnees < 4)
                        {
                            donnees[indexDonnees] = mot;
                            indexDonnees = 0;
                            mot = "";
                        }

                    /* On créé le prénom dans le tableau prenoms */
                    prenoms[i] = new Prenom()
                    {
                        annee = int.Parse(donnees[0]),
                        prenom = donnees[1],
                        nombre = int.Parse(donnees[2]),
                        ordre = int.Parse(donnees[3])
                    };

                    /* On passe à la ligne suivante */
                    ligne = sr.ReadLine();
                    ++i;
                }

                /* On ferme le fichier */
                sr.Close();
            }
            catch
            {
                messageErreur("Impossible de lire le fichier.\n" +
                                "Il est introuvable dans le dossier spécifié, ou n'existe pas.");
                prenoms = null;
            }

            return prenoms;
        }
        #endregion
        #region Affichage tableaux
        public static void afficherPrenoms(Prenom[] prenoms)
        {
            for (int i = 0; i < prenoms.Length; ++i)
                Console.WriteLine("{0} \t {1} \t\t {2} \t {3}",
                                prenoms[i].annee,
                                prenoms[i].prenom,
                                prenoms[i].nombre,
                                prenoms[i].ordre);
        }

        public static void afficherPrenomsTop10(Prenom[] prenoms)
        {
            for (int i = 0; i < prenoms.Length; ++i)
                Console.WriteLine("TOP {0}\t: {1}",
                                prenoms[i].ordre,
                                prenoms[i].prenom);
        }
        #endregion
        #region Affichage console
        public static void menuPrincipal()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*                              GUIDE DES PRENOMS                              *");
            Console.WriteLine("===============================================================================");
            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Afficher les prénoms           -> {0}                                         ",
                                (char)MODE.AFFICHAGE);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");
            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Afficher le top 10 d'une année -> {0}                                         ",
                                (char)MODE.TOP10);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");
            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Quitter                        -> {0}                                         ",
                                (char)MODE.QUITTER);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void finFonctionnalité()
        {
            Console.WriteLine("Appuyez sur une touche pour revenir au menu principal");
            Console.ReadKey();
            Console.Clear();
        }

        public static void top10Affichage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*                              TOP 10 DES PRENOMS                             *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Rentrez l'année du top 10 (1900 - {0}) : ", DateTime.Now.Year - 1);
        }

        public static void messageErreur(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERREUR : " + err);
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion
        #region Fonctionnalités
        public static void top10Naissance(Prenom[] prenoms)
        {
            int annee, i = 0, indexResultat = 0;    // Année et index pour parcourir les tableaux
            Prenom[] resultat = new Prenom[10];     // Tableau de Prenom contenant les résultats
            bool premiereValeur = true;             // Pour avoir un message d'erreur après avoir rentré une année non-valide

            Console.Clear();                        // On vide la console
            top10Affichage();                       // On affiche le menu de la fonctionnalité

            /* Ici, on va essayer de rentrer une année (type int) */
            try
            {
                do
                {
                    if (premiereValeur)
                        premiereValeur = false;
                    else
                    {
                        messageErreur("La date n'est pas comprise entre " + ANNEEMIN + " et " + (DateTime.Now.Year - 1));
                    }
                    annee = int.Parse(Console.ReadLine());
                }
                while (annee > DateTime.Now.Year - 1 || annee < ANNEEMIN);
            }
            catch
            {
                messageErreur("Vous n'avez pas rentré un entier.");
                annee = -1;
            }

            /* S'il n'y a pas eu d'erreur, on continue la fonctionnalité */
            if (annee != -1)
            {
                while (i < prenoms.Length && indexResultat < resultat.Length)
                {
                    if (prenoms[i].annee == annee)
                    {
                        resultat[indexResultat] = prenoms[i];
                        ++indexResultat;
                    }
                    ++i;
                }
                afficherPrenomsTop10(resultat);
            }
        }
        #endregion

        static void Main(string[] args)
        {
            Prenom[] prenoms = lectureFichier();    // Les données du fichier texte
            bool quitter = false;                   // C'est lui qui va arrêter le programme
            char c;                                 // Permet de sélectionner la fonctionnalité

            while (!quitter && prenoms != null)
            {
                menuPrincipal();
                Console.WriteLine("\nChoisissez ce que vous voulez faire :");
                c = char.ToLower(Console.ReadKey().KeyChar);
                Console.Clear();

                switch (c)
                {
                    case (char)MODE.AFFICHAGE:
                        afficherPrenoms(prenoms);
                        finFonctionnalité();
                        break;

                    case (char)MODE.TOP10:
                        top10Naissance(prenoms);
                        finFonctionnalité();
                        break;

                    case (char)MODE.QUITTER:
                        quitter = true;
                        break;

                    default:
                        messageErreur("Cette fonctionnalité n'existe pas.\n");
                        break;
                }
            }

            if (prenoms == null)
                Console.ReadLine();
        }
    }
}
