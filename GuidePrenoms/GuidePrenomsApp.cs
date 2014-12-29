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
        // TODO : Enlever cette constante, et créer une 2e variable pour l'année la plus élevée du fichier
        public const int ANNEEMIN = 1900;

        /* Mettez entre "" l'URL du fichier */
        public const string URL = @"C:\Users\Alexandre\Desktop\Programmation\C#\GuidePrenoms\GuidePrenoms\prenoms_bordeaux.txt";
        //public const string URL = @"C:\Users\amonge.ENSC\Desktop\Cours & TPs\S5\Info\GuidePrenoms\GuidePrenoms\prenoms_bordeaux.txt";

        #region Enums
        public enum MODE
        {
            TOP10ANNEE = '1',
            TOP10PERIODE = '2',
            NAISSANCEETORDREANNEE = '3',
            NAISSANCEETORDREPERIODE = '4',
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
            Prenom[] prenoms = new Prenom[100 * 114];           // Tableau contenant les données
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
        /* Méthode permettant d'afficher les prénoms du tableau sous la forme "TOP X" */
        public static void afficherPrenomsTop10(Prenom[] prenoms)
        {
            if (prenoms.Length >= 10)
                for (int i = 0; i < 10; ++i)
                    Console.WriteLine("TOP {0}\t: {1}",
                                    prenoms[i].ordre,
                                    prenoms[i].prenom);
        }
        #endregion
        #region Affichage console
        /* Méthode gérant l'affichage du menu principal */
        public static void menuPrincipal()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*                        GUIDE DES PRENOMS DE BORDEAUX                        *");
            Console.WriteLine("===============================================================================");

            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Afficher le top 10 d'une année                         -> {0}                 ",
                                (char)MODE.TOP10ANNEE);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");

            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Afficher le top 10 d'une période                       -> {0}                 ",
                                (char)MODE.TOP10PERIODE);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");

            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Nombre de naissance et ordre d'un prénom d'une année   -> {0}                 ",
                                (char)MODE.NAISSANCEETORDREANNEE);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");

            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Nombre de naissance et ordre d'un prénom d'une période -> {0}                 ",
                                (char)MODE.NAISSANCEETORDREPERIODE);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");

            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Quitter                                                -> {0}                 ",
                                (char)MODE.QUITTER);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");

            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* Méthode permettant de gérer la fin d'une fonctionnalité */
        public static void finFonctionnalité()
        {
            Console.WriteLine("Appuyez sur une touche pour revenir au menu principal");
            Console.ReadKey();
            Console.Clear();
        }

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void top10Affichage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*                              TOP 10 DES PRENOMS                             *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void nbNaissanceEtOrdreAnneeAffichage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*            NOMBRE DE NAISSANCE ET ORDRE D'UN PRENOM D'UNE ANNEE             *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void nbNaissanceEtOrdrePeriodeAffichage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*           NOMBRE DE NAISSANCE ET ORDRE D'UN PRENOM D'UNE PERIODE            *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* Méthode permettant de gérer l'affichage d'un message d'erreur */
        public static void messageErreur(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERREUR : " + err);
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion
        #region Fonctionnalités
        /* Fonction permettant de gérer le TOP X de naissances sur une année donnée, 
         * et qui retourne les résultats sous forme de tableau */
        public static Prenom[] topXNaissanceAnnee(Prenom[] prenoms, int top, bool top10)
        {
            int i = 0, indexResultat = 0;           // Index pour parcourir les tableaux
            Prenom[] resultat = new Prenom[top];    // Tableau de Prenom contenant les résultats

            Console.Clear();                        // On vide la console

            if (top10)                              // On affiche le menu de la bonne fonctionnalité
            {
                top10Affichage();
                Console.WriteLine("Rentrez l'année du top 10 (1900 - 2013) : ");
            }
            else
            {
                nbNaissanceEtOrdreAnneeAffichage();
                Console.WriteLine("Rentrez l'année (1900 - 2013) : ");
            }

            int annee = rentrerAnnee();             // On rentre l'année

            /* S'il n'y a pas eu d'erreur, on continue la fonctionnalité. 
             * Les prénoms du top X sont stockés dans resultat, et renvoyés */
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
                if (top10)
                    afficherPrenomsTop10(resultat);
            }

            return resultat;
        }


        /* Fonction permettant de gérer le TOP X de naissances sur une période donnée, 
         * et qui retourne les résultats sous forme de tableau */
        public static Prenom[] topXNaissancePeriode(Prenom[] prenoms, int top)
        {
            Prenom[] resultat;                      // Tableau de Prenom contenant les résultats

            Console.Clear();                        // On vide la console

            top10Affichage();
            Console.WriteLine("Rentrez l'année de début du top 10 (1900 - 2013) : ");

            int anneeD = rentrerAnnee();             // On rentre l'année de début

            Console.WriteLine("Rentrez l'année de fin du top 10 (1900 - 2013) : ");

            int anneeF = rentrerAnnee();             // On rentre l'année

            if (anneeD > anneeF)
                echanger(ref anneeD, ref anneeF);

            /* S'il n'y a pas eu d'erreur, on continue la fonctionnalité. 
             * Les prénoms du top X sont stockés dans resultat, et renvoyés */
            if (anneeD != -1 && anneeF != -1)
            {
                resultat = recupererEtTrierPrenomsPeriode(prenoms, anneeD, anneeF);
                afficherPrenomsTop10(resultat);
            }
            else
                resultat = null;

            return resultat;
        }


        /* Méthode permettant de donner des informations sur un prénom dans l'année
         * souhaitée */
        public static void nbNaissanceEtOrdreAnnee(Prenom[] prenoms)
        {
            /* Ici, on a seulement besoin d'utiliser la recherche d'un prénom sur 
             * le top 100 d'une année */
            rechercherPrenom(topXNaissanceAnnee(prenoms, 100, false), false);
        }

        /* Méthode permettant de donner des informations sur un prénom dans la période
         * souhaitée */
        public static void nbNaissanceEtOrdrePeriode(Prenom[] prenoms)
        {
            nbNaissanceEtOrdrePeriodeAffichage();

            Prenom[] prenomsPeriode;
            int anneeD, anneeF;

            Console.WriteLine("Date de début :");
            anneeD = rentrerAnnee();

            Console.WriteLine("Date de fin : ");
            anneeF = rentrerAnnee();

            if (anneeD > anneeF)
                echanger(ref anneeD, ref anneeF);

            if (anneeD != -1 && anneeF != -1)
            {
                prenomsPeriode = recupererEtTrierPrenomsPeriode(prenoms, anneeD, anneeF);
                rechercherPrenom(prenomsPeriode, true);     // On peut chercher un prénom dans ce tableau
            }
        }
        #endregion
        #region Fonctions autres
        /* Permet de savoir si l'utilisateur veut recommencer la fonctionnalité
         * ou pas */
        public static bool recommencerFonctionnalité()
        {
            char c;
            Console.WriteLine("Voulez-vous faire une nouvelle recherche ? [y/n]");

            do
                c = Console.ReadKey().KeyChar;
            while (c != 'y' && c != 'n' && c != 'Y' && c != 'N');

            if (c == 'y' || c == 'Y')
                return true;
            else
                return false;
        }


        /* Fonction permettant de rentrer une année */
        public static int rentrerAnnee()
        {
            int annee;
            bool premiereValeur = true;

            /* Ici, on va essayer de rentrer une année (type int) */
            try
            {
                do
                {
                    if (premiereValeur)
                        premiereValeur = false;
                    else
                    {
                        messageErreur("La date n'est pas comprise entre " + ANNEEMIN + " et " + 2013);
                    }
                    annee = int.Parse(Console.ReadLine());
                }
                while (annee > 2013 || annee < ANNEEMIN);
            }
            catch
            {
                messageErreur("Vous n'avez pas rentré un entier.");
                annee = -1;
            }

            return annee;
        }

        /* Ici, on va demander à l'utilisateur de rentrer un prénom,
         * afin d'avoir les informations sur celui-ci pour l'année donnée.
         * Si celui-ci n'existe pas, on affiche la liste des prénoms
         * commençant par la chaîne rentrée */
        public static void rechercherPrenom(Prenom[] prenoms, bool periode)
        {
            Prenom p = new Prenom() { annee = 0, prenom = null, nombre = 0, ordre = 0 };
            bool premierPassage = true;
            Prenom[] commencerPar;
            string prenom = "";

            Console.WriteLine("Rentrez le prénom souhaité : ");

            do
            {
                if (!premierPassage)
                {
                    Console.Clear();
                    nbNaissanceEtOrdreAnneeAffichage();
                    Console.WriteLine("Ce prénom ne se trouve pas dans la liste.\n" +
                                        "Voici les prénoms commençant par {0} : ", prenom);

                    commencerPar = prenomCommencePar(prenoms, prenom);

                    if (commencerPar != null)
                        foreach (Prenom pr in commencerPar)
                            Console.Write("{0}, ", pr.prenom);

                    Console.WriteLine();
                }
                else
                    premierPassage = false;

                prenom = Console.ReadLine().ToUpper();

                for (int i = 0; i < prenoms.Length; ++i)
                    if (prenoms[i].prenom.Equals(prenom))
                        p = prenoms[i];
            }
            while (p.prenom == null);

            if (periode)
                Console.WriteLine("\n\n{0} est le top {1} de la période donnée !\n" +
                                    "Il a été donné {3} fois.\n", p.prenom, p.ordre,
                                                                            p.annee, p.nombre);
            else
                Console.WriteLine("\n\n{0} est le top {1} de l'année {2} !\n" +
                                    "Il a été donné {3} fois cette année-là.\n", p.prenom, p.ordre,
                                                                            p.annee, p.nombre);
        }

        /* Réécriture du StartsWith(string), méthode disponible pour les types string
         * str1 : morceau de chaîne à comparer
         * str2 : chaîne complète */
        public static bool startsWith(string morceau, string chaineEntiere)
        {
            bool res = true;
            int i = 0;

            if (!(morceau == null || chaineEntiere == null))
            {
                if (morceau.Length > chaineEntiere.Length)
                    return false;
                else
                    while (i < morceau.Length && res)
                    {
                        if (morceau[i] != chaineEntiere[i])
                            res = false;
                        ++i;
                    }
                return res;
            }
            else
                return false;
        }

        /* Réécriture du Contains(string), méthode disponible pour les tableaux de string,
         * mais adaptée à notre cas
         * prenoms : tableau contenant les prénoms
         * prenom : prénom */
        public static bool containsPrenom(Prenom[] prenoms, Prenom prenom)
        {
            bool res = false;
            int i = 0;
            
            if (prenom.prenom != null)
                while (i < prenoms.Length && !res)
                    if (prenom.prenom.Equals(prenoms[i++].prenom))
                        res = true;
            return res;
        }

        /* Fonction qui renvoie la liste des prénoms qui commencent
         * par chaîne de caractère donnée */
        public static Prenom[] prenomCommencePar(Prenom[] prenoms, string str)
        {
            Prenom[] resultatTmp = new Prenom[1000], resultat;
            int nb = -1;

            foreach (Prenom p in prenoms)
            {
                if (startsWith(str, p.prenom))   //p.prenom.StartsWith(str)
                {
                    resultatTmp[++nb] = p;
                }
            }

            if (nb != -1)
            {
                resultat = new Prenom[nb + 1];

                for (int i = 0; i < nb + 1; ++i)
                {
                    resultat[i] = resultatTmp[i];
                }
            }
            else
                resultat = null;

            return resultat;
        }

        /* Méthode permettant d'échanger la valeur de deux entiers */
        public static void echanger(ref int v1, ref int v2)
        {
            int vT = v1;
            v1 = v2;
            v2 = vT;
        }

        /* Méthode permettant d'ajouter le nombre de fois qu'a été donné un prénom
         * d'une année au nombre de fois qu'à été donné un prénom dans une période donnée */
        public static void ajouterNombreAuPrenom(Prenom[] prenoms, string prenom, int nombre)
        {
            int i = 0;
            bool ok = false;

            while (i < prenoms.Length && !ok)
            {
                if (prenom.Equals(prenoms[i].prenom))
                    prenoms[i].nombre += nombre;
                ++i;
            }
        }

        /* Méthode permettant de trier les prénoms selon le nombre de fois
         * qu'ils ont été donné */
        public static void trierPrenomsParNombre(Prenom[] prenoms)
        {
            bool ok = false;

            while (!ok)
            {
                ok = true;
                for (int i = 1; i < prenoms.Length; ++i)
                {
                    if (prenoms[i - 1].nombre < prenoms[i].nombre)
                    {
                        Prenom p = prenoms[i];
                        prenoms[i] = prenoms[i - 1];
                        prenoms[i - 1] = p;
                        ok = false;
                    }
                }
            }
        }

        /* Fonction permettant de récupérer les prénoms d'une période, et de les trier par nombre */
        public static Prenom[] recupererEtTrierPrenomsPeriode(Prenom[] prenoms, int anneeD, int anneeF)
        {
            Prenom[] prenomsPeriodeTmp, resultat;
            int cpt = 0;

            prenomsPeriodeTmp = new Prenom[(anneeF - anneeD + 1) * 100];

            /* Pour chaque Prenom du tableau, s'il n'existe pas dans le tableau
             * prenomsPeriodeTmp, on le rajoute. Sinon, on ajoute le nombre de fois
             * que le prénom a été donné une autre année */
            foreach (Prenom p in prenoms)
            {
                if (p.annee >= anneeD && p.annee <= anneeF)
                    if (!containsPrenom(prenomsPeriodeTmp, p))
                        prenomsPeriodeTmp[cpt++] = p;
                    else
                        ajouterNombreAuPrenom(prenomsPeriodeTmp, p.prenom, p.nombre);
            }

            resultat = new Prenom[cpt];   // On stocke dans un autre tableau pour enlever les cases inutiles

            trierPrenomsParNombre(prenomsPeriodeTmp);   // On trie le tableau par ordre croissant sur prenom.nombre

            /* On stocke les données dans le nouveau tableau, en redéfinissant l'ordre 
             * et l'année (0) */
            for (int i = 0; i < resultat.Length; ++i)
            {
                resultat[i] = prenomsPeriodeTmp[i];
                resultat[i].ordre = i + 1;
                resultat[i].annee = 0;
            }

            return resultat;
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

                switch (c)
                {
                    case (char) MODE.TOP10ANNEE:
                        do
                        {
                            Console.Clear();
                            topXNaissanceAnnee(prenoms, 10, true);
                        }
                        while (recommencerFonctionnalité());
                        finFonctionnalité();
                        break;

                    case (char) MODE.TOP10PERIODE:
                        do
                        {
                            Console.Clear();
                            topXNaissancePeriode(prenoms, 10);
                        }
                        while (recommencerFonctionnalité());
                        finFonctionnalité();
                        break;

                    case (char) MODE.NAISSANCEETORDREANNEE:
                        do
                        {
                            Console.Clear();
                            nbNaissanceEtOrdreAnnee(prenoms);
                        }
                        while (recommencerFonctionnalité());
                        finFonctionnalité();
                        break;

                    case (char) MODE.NAISSANCEETORDREPERIODE:
                        do
                        {
                            Console.Clear();
                            nbNaissanceEtOrdrePeriode(prenoms);
                        }
                        while (recommencerFonctionnalité());
                        finFonctionnalité();
                        break;

                    case (char) MODE.QUITTER:
                        quitter = true;
                        break;

                    default:
                        Console.Clear();
                        messageErreur("Cette fonctionnalité n'existe pas.\n");
                        break;
                }
            }

            if (prenoms == null)
                Console.ReadLine();
        }
    }
}
