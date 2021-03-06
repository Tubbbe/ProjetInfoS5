﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GuidePrenoms
{
    class GuidePrenomsApp
    {
        #region Enums
        /* Enum permettant de gérer l'appel des fonctionnalités depuis le menu */
        public enum MODE {
            TOP10ANNEE = '1',
            TOP10PERIODE = '2',
            NAISSANCEETORDREANNEE = '3',
            NAISSANCEETORDREPERIODE = '4',
            TENDANCE = '5',
            PRENOMPLUSMOINSDONNE = '6',
            MOTEURRECHERCHEPRENOM = '7',
            INFORMATIONPRENOM = '8',
            QUITTER = 'q'
        }
        #endregion
        #region Structures
        /* Structure qui va nous permettre de gérer nos données */
        public struct Prenom {
            public int annee { get; set; }
            public string prenom { get; set; }
            public int nombre { get; set; }
            public int ordre { get; set; }
        }
        #endregion
        #region Lecture du fichier
        /* Cette méthode sera appelée une seule fois dans le programme. Une fois les données stockées
         * dans un tableau, on manipulera le tableau à la place du fichier. */
        public static Prenom[] lectureFichier(string adresse, string nomFichier, ref int anneeMin, ref int anneeMax) {
            Prenom[] prenoms = null;
            StreamReader sr = null;
            string ligne = "", mot = "", anneeMinMax = "";

            try {   // Si le fichier n'existe pas, on passe dans le catch et sr1 est null
                sr = new StreamReader(adresse + "\\" + nomFichier + ".txt", Encoding.Unicode);
                ligne = sr.ReadLine(); 
            }
            catch {
                messageErreur("Impossible de lire le fichier.\nIl est introuvable dans le dossier spécifié, ou n'existe pas.");
            }

            /* On peut commencer à récupérer les années minimum et maximum, puis à stocker les données */
            if (sr != null && ligne != "") {
                int i = 0, j = 0, indexDonnees = 0; // Index pour parcourir les tableaux
                string[] donnees = new string[4];   // Va contenir le mot compris entre deux tabulations

                ligne = sr.ReadLine();  // On passe à la ligne 2 du fichier (la 1 ne nous intéresse pas)

                for (i = 0; i < 4; ++i)
                    anneeMinMax += ligne[i];

                try { anneeMax = int.Parse(anneeMinMax); }
                catch { messageErreur("Le format du fichier n'est pas valide."); }

                anneeMinMax = "";

                do
                    ligne = sr.ReadLine();
                while (!sr.EndOfStream);        // On va à la fin du fichier afin de récupérer l'année minimum

                for (i = 0; i < 4; ++i)
                    anneeMinMax += ligne[i];

                try { anneeMin = int.Parse(anneeMinMax); }
                catch { messageErreur("Le format du fichier n'est pas valide."); }

                sr.Close();                     // On ferme le fichier.

                prenoms = new Prenom[100 * (1 + anneeMax - anneeMin)];

                try {
                    sr = new StreamReader(adresse + "\\" + nomFichier + ".txt", Encoding.Unicode);
                    ligne = sr.ReadLine();
                }
                catch {
                    messageErreur("Impossible de lire le fichier.\nIl est introuvable dans le dossier spécifié, ou n'existe pas.");
                }

                i = 0;

                do { // Tant qu'on est pas à la fin du fichier, on boucle
                    ligne = sr.ReadLine();

                    /* Dans ce for, on va stocker dans donnees les 4 "mots" d'une ligne (année, prénom, ...) */
                    for (j = 0; j < ligne.Length; ++j)
                        if (!'\t'.Equals(ligne[j]))
                            mot += ligne[j];
                        else
                            if (indexDonnees < 4) {
                                donnees[indexDonnees] = mot;
                                ++indexDonnees;
                                mot = "";
                            }

                    /* On stocke le dernier mot dans donnees */
                    if (j == ligne.Length)
                        if (indexDonnees < 4) {
                            donnees[indexDonnees] = mot;
                            indexDonnees = 0;
                            mot = "";
                        }

                    donnees[1] = enleverLesAccents(donnees[1]);

                    /* On créé le prénom dans le tableau prenoms */
                    prenoms[i] = new Prenom() {
                        annee = int.Parse(donnees[0]),
                        prenom = donnees[1],
                        nombre = int.Parse(donnees[2]),
                        ordre = int.Parse(donnees[3])
                    };

                    ++i;
                }
                while (!sr.EndOfStream);

                /* On ferme le fichier */
                sr.Close();
            }

            return prenoms;
        }
        #endregion
        #region Affichage tableaux
        /* Méthode permettant d'afficher les prénoms du tableau sous la forme "TOP <valeur>" */
        public static void afficherPrenomsTop10(Prenom[] prenoms) {
            Console.WriteLine("\n");
            if (prenoms.Length >= 10)
                for (int i = 0; i < 10; ++i)
                    Console.WriteLine("TOP {0}\t: {1}", prenoms[i].ordre, prenoms[i].prenom);
        }
        #endregion
        #region Affichage console
        public static void debutProgramme() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=");
            Console.WriteLine("*               BIENVENUE DANS LE GUIDE DES PRENOMS DE BORDEAUX               *");
            Console.WriteLine("=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=");
            Console.WriteLine("*      Pour commencer, veuillez rentrer l'adresse de destination du fichier   *");
            Console.WriteLine("*                              au format OpenData                             *");
            Console.Write("*               Le fichier devra se nommer ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("prenoms_bordeaux.txt");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("               *");
            Console.WriteLine("=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=*=");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Exemple : C:\\Users\\Toto\\Desktop");
            Console.WriteLine();
        }

        /* Méthode gérant l'affichage du menu principal */
        public static void menuPrincipal() {
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
            Console.Write(@" Tendance d'un prénom sur une période donnée            -> {0}                 ",
                                (char)MODE.TENDANCE);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");

            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Prénom le plus/moins donné dans la période du fichier  -> {0}                 ",
                                (char)MODE.PRENOMPLUSMOINSDONNE);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");

            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Liste des prénoms commençant par...                    -> {0}                 ",
                                (char)MODE.MOTEURRECHERCHEPRENOM);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*");

            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(@" Plus d'informations sur un prénom                      -> {0}                 ",
                                (char)MODE.INFORMATIONPRENOM);
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

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void top10Affichage() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*                              TOP 10 DES PRENOMS                             *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void nbNaissanceEtOrdreAnneeAffichage() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*            NOMBRE DE NAISSANCE ET ORDRE D'UN PRENOM D'UNE ANNEE             *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void nbNaissanceEtOrdrePeriodeAffichage() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*           NOMBRE DE NAISSANCE ET ORDRE D'UN PRENOM D'UNE PERIODE            *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void tendancePrenomAffichage() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*                    TENDANCE D'UN PRENOM SUR UNE PERIODE                     *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void prenomPlusMoinsDonneAffichage() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*                  PRENOM LE PLUS/MOINS DONNE SUR LA PERIODE                  *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void moteurRecherchePrenomAffichage() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*                        MOTEUR DE RECHERCHE DE PRENOM                        *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nEntrez une lettre ou un début de prénom, pour obtenir la liste : ");
        }

        /* Méthode permettant de gérer l'affichage de la fonctionnalité */
        public static void informationPrenomAffichage() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===============================================================================");
            Console.WriteLine("*                        INFORMATIONS SUR UN PRENOM                           *");
            Console.WriteLine("===============================================================================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nEntrez une lettre ou un début de prénom, pour obtenir la liste : ");
        }

        /* Méthode permettant de gérer l'affichage d'un message d'erreur */
        public static void messageErreur(string err) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERREUR : " + err);
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion
        #region Fonctionnalités
        /* Fonction permettant de gérer le TOP X de naissances sur une année donnée, 
         * et qui retourne les résultats sous forme de tableau */
        public static Prenom[] topXNaissanceAnnee(Prenom[] prenoms, int top, bool top10, int anneeMin, int anneeMax) {
            int i = 0, indexResultat = 0, annee = -1;           // Index pour parcourir les tableaux et année
            Prenom[] resultat = new Prenom[top];    // Tableau de Prenom contenant les résultats

            Console.Clear();                        // On vide la console

            if (top10) {                            // On affiche le menu de la bonne fonctionnalité
                top10Affichage();
                Console.WriteLine("Rentrez l'année du top 10 ({0} - {1}) : ", anneeMin, anneeMax);
            }
            else {
                nbNaissanceEtOrdreAnneeAffichage();
                Console.WriteLine("Rentrez l'année ({0} - {1}) : ", anneeMin, anneeMax);
            }

            do
                annee = rentrerAnnee(anneeMin, anneeMax);             // On rentre l'année
            while (annee == -1);

            /* On stocke dans resultat les prénoms de l'année choisie */
            while (i < prenoms.Length && indexResultat < resultat.Length) {
                if (prenoms[i].annee == annee) {
                    resultat[indexResultat] = prenoms[i];
                    ++indexResultat;
                }
                ++i;
            }

            if (top10)
                afficherPrenomsTop10(resultat);

            return resultat;
        }


        /* Fonction permettant de gérer le TOP X de naissances sur une période donnée, 
         * et qui retourne les résultats sous forme de tableau */
        public static Prenom[] topXNaissancePeriode(Prenom[] prenoms, int top, bool periode, int anneeMin, int anneeMax) {
            Prenom[] resultat;                      // Tableau de Prenom contenant les résultats
            int anneeD = anneeMin, anneeF = anneeMax;

            Console.Clear();                        // On vide la console

            /* Si la fonctionnalité ne demande pas de période, les dates par défaut seront le 
             * minimum et le maximum du fichier passé */
            if (periode) {
                top10Affichage();
                Console.WriteLine("Rentrez l'année de début du top 10 ({0} - {1}) : ", anneeMin, anneeMax);

                do
                    anneeD = rentrerAnnee(anneeMin, anneeMax);             // On rentre l'année de début
                while (anneeD == -1);

                Console.WriteLine("Rentrez l'année de fin du top 10 ({0} - {1}) : ", anneeMin, anneeMax);

                do
                    anneeF = rentrerAnnee(anneeMin, anneeMax);             // On rentre l'année de fin
                while (anneeF == -1);

                if (anneeD > anneeF)
                    echanger(ref anneeD, ref anneeF);
            }

            /* S'il n'y a pas eu d'erreur, on continue la fonctionnalité. 
             * Les prénoms du top X sont stockés dans resultat, et renvoyés */
            if (anneeD != -1 && anneeF != -1) {
                resultat = recupererEtTrierPrenomsPeriode(prenoms, anneeD, anneeF);

                if (periode)
                    afficherPrenomsTop10(resultat);
            }
            else
                resultat = null;

            return resultat;
        }


        /* Méthode permettant de donner des informations sur un prénom dans l'année
         * souhaitée */
        public static void nbNaissanceEtOrdreAnnee(Prenom[] prenoms, int anneeMin, int anneeMax) {
            /* Ici, on a seulement besoin d'utiliser la recherche d'un prénom sur 
             * le top 100 d'une année */
            rechercherPrenom(topXNaissanceAnnee(prenoms, 100, false, anneeMin, anneeMax), false);
        }

        /* Méthode permettant de donner des informations sur un prénom dans la période
         * souhaitée */
        public static void nbNaissanceEtOrdrePeriode(Prenom[] prenoms, int anneeMin, int anneeMax) {
            nbNaissanceEtOrdrePeriodeAffichage();

            int anneeD, anneeF;

            Console.WriteLine("Date de début ({0} - {1}) : ", anneeMin, anneeMax);

            do
                anneeD = rentrerAnnee(anneeMin, anneeMax);             // On rentre l'année de début
            while (anneeD == -1);

            Console.WriteLine("Date de fin ({0} - {1}) : ", anneeMin, anneeMax);

            do
                anneeF = rentrerAnnee(anneeMin, anneeMax);             // On rentre l'année de fin
            while (anneeF == -1);

            if (anneeD > anneeF)
                echanger(ref anneeD, ref anneeF);

            if (anneeD != -1 && anneeF != -1)
                rechercherPrenom(recupererEtTrierPrenomsPeriode(prenoms, anneeD, anneeF), true); // On peut chercher un prénom dans ce tableau
        }

        /* Méthode permettant de donner la tendance d'un prénom en fonction d'une période donnée */
        public static void tendancePrenomPeriode(Prenom[] prenoms, int anneeMin, int anneeMax) {
            tendancePrenomAffichage();

            string prenom;
            int anneeD, anneeF, cptPeriodeAvant = 0, cptPeriode = 0;
            double moyennePeriodeAvant = 0, moyennePeriode = 0, variance = 0, ecartType = 0, ecartMoyenne = 0;

            Console.WriteLine("Date de début ({0} - {1}) : ", anneeMin, anneeMax);

            do
                anneeD = rentrerAnnee(anneeMin, anneeMax);             // On rentre l'année de début
            while (anneeD == -1);

            Console.WriteLine("Date de fin ({0} - {1}) : ", anneeMin, anneeMax);

            do
                anneeF = rentrerAnnee(anneeMin, anneeMax);             // On rentre l'année de fin
            while (anneeF == -1);

            if (anneeD > anneeF)
                echanger(ref anneeD, ref anneeF);

            Console.WriteLine("Rentrez le prénom souhaité : ");
            prenom = enleverLesAccents(Console.ReadLine().ToUpper());

            /* Calcul des moyennes et de la variance pour les deux périodes */
            foreach (Prenom p in prenoms)
                if (p.prenom != null && p.prenom.Equals(prenom))
                    if (p.annee < anneeD) {
                        moyennePeriodeAvant += p.nombre;
                        ++cptPeriodeAvant;
                        variance += Math.Pow(p.nombre, 2);
                    }
                    else 
                        if (p.annee >= anneeD && p.annee <= anneeF) {
                            moyennePeriode += p.nombre;
                            ++cptPeriode;
                        }

            if (moyennePeriodeAvant != 0)
                moyennePeriodeAvant /= cptPeriodeAvant;

            if (moyennePeriode != 0)
                moyennePeriode /= cptPeriode;

            variance = (variance - Math.Pow(moyennePeriodeAvant, 2) * cptPeriodeAvant) / cptPeriodeAvant;
            ecartType = Math.Sqrt(variance);

            if (moyennePeriodeAvant == 0)
                ecartType = 0;

            ecartMoyenne = moyennePeriode - moyennePeriodeAvant;

            if (ecartMoyenne <= -2 * ecartType)
                Console.WriteLine("Ce prénom est à l'abandon");
            else if (-2 * ecartType < ecartMoyenne && ecartMoyenne < -ecartType)
                Console.WriteLine("Ce prénom est désuet");
            else if (-ecartType <= ecartMoyenne && ecartMoyenne <= ecartType)
                Console.WriteLine("Ce prénom se maintient");
            else if (ecartType < ecartMoyenne && ecartMoyenne < 2 * ecartType)
                Console.WriteLine("Ce prénom est en vogue");
            else if (2 * ecartType <= ecartMoyenne)
                Console.WriteLine("Bravo, ce prénom explose !!!!! :D");
            else
                messageErreur("Aucune information n'est disponible pour ce prénom");
        }

        /* Méthode permettant d'afficher le prénom le plus donné, et le prénom le moins donné */
        public static void prenomPlusMoinsDonne(Prenom[] prenoms, int anneeMin, int anneeMax) {
            Prenom[] resultat = topXNaissancePeriode(prenoms, prenoms.Length, false, anneeMin, anneeMax); // On a besoin de tous les prénoms SANS doublon

            prenomPlusMoinsDonneAffichage();

            if (resultat != null) {
                Console.WriteLine("Le prénom le plus donné sur la période du fichier est {0},\nqui a été donné {1} fois !\n",
                                resultat[0].prenom, resultat[0].nombre);
                Console.WriteLine("Le prénom le moins donné sur la période du fichier est {0},\nqui a été donné {1} fois !\n",
                                resultat[resultat.Length - 1].prenom,
                                resultat[resultat.Length - 1].nombre);
            }
        }

        /* Méthode permettant à l'utilisateur de rechercher un prénom dans le tableau */
        public static void moteurRecherchePrenom(Prenom[] prenoms, int anneeMin, int anneeMax) {
            moteurRecherchePrenomAffichage();

            string debutPrenom = enleverLesAccents(Console.ReadLine().ToUpper());
            Prenom[] resultats = null;

            /* On récupère les prénoms qui commencent par la chaîne rentrée, parmi la liste de prénoms
             * compris entre la période mini et maxi */
            resultats = prenomCommencePar(recupererEtTrierPrenomsPeriode(prenoms, anneeMin, anneeMax), debutPrenom);

            if (resultats != null) {
                triAlphabetique(resultats);

                foreach (Prenom p in resultats)
                    Console.WriteLine(p.prenom);
            }
        }

        /* Méthode permettant de donner des informations sur un prénom donné */
        public static void informationPrenom(Prenom[] prenoms, int anneeMin, int anneeMax) {
            informationPrenomAffichage();

            bool easterEgg = true;
            int i = 0, cpt = 0;
            Prenom[] listeResultats = new Prenom[prenoms.Length];

            Console.WriteLine("Rentrez le prénom souhaité : ");
            string prenom = enleverLesAccents(Console.ReadLine().ToUpper());

            /* A chaque fois qu'on va rencontrer le prénom, on le rajoute dans le tableau */
            foreach (Prenom p in prenoms)
                if (p.prenom.Equals(prenom))
                    listeResultats[i++] = p;

            Console.WriteLine("\n" + prenom + " : \n");

            for (i = 0; i < listeResultats.Length; ++i) {
                #region Easter egg
                if (("EDWIGE".Equals(prenom) || "PIERRE-ALEXANDRE".Equals(prenom)) && easterEgg) {
                    Console.WriteLine("Ah que coucou <3");
                    easterEgg = false;
                    Console.WriteLine(@"   _______________          |*\_/*|________");
                    Console.WriteLine(@"  |  ___________  |        ||_/-\_|______  |");
                    Console.WriteLine(@"  | |           | |        | |           | |");
                    Console.WriteLine(@"  | |   0   0   | |        | |   0   0   | |");
                    Console.WriteLine(@"  | |     -     | |        | |     -     | |");
                    Console.WriteLine(@"  | |   \___/   | |        | |   \___/   | |");
                    Console.WriteLine(@"  | |___     ___| |        | |___________| |");
                    Console.WriteLine(@"  |_____|\_/|_____|        |_______________|");
                    Console.WriteLine(@"    _|__|/ \|_|_.............._|________|_");
                    Console.WriteLine(@"   / ********** \            / ********** \");
                    Console.WriteLine(@" /  ************  \        /  ************  \");
                    Console.WriteLine(@"--------------------      --------------------");
                }
                #endregion

                if (listeResultats[i].annee != 0) {
                    if (listeResultats[i].ordre < 11) {     // On va afficher la ligne si le prénom fait parti du top 10
                        if (listeResultats[i].ordre == 1)           // Sa couleur dépend de son classement (or/argent/bronze)
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        else if (listeResultats[i].ordre == 2)
                            Console.ForegroundColor = ConsoleColor.Gray;
                        else if (listeResultats[i].ordre == 3)
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        else
                            Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("- {0}e de l'année {1}, donné {2} fois",
                                            listeResultats[i].ordre, listeResultats[i].annee, listeResultats[i].nombre);
                    }
                    cpt += listeResultats[i].nombre;
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Il a été donné {0} fois sur la période de {1} à {2}\n\n", cpt, anneeMin, anneeMax);
        }
        #endregion
        #region Fonctions autres
        /* Permet de savoir si l'utilisateur veut recommencer la fonctionnalité
         * ou pas */
        public static bool recommencerFonctionnalité() {
            char c;
            Console.WriteLine("\nVoulez-vous faire une nouvelle recherche ? [y/n]");

            do
                c = Console.ReadKey().KeyChar;
            while (c != 'y' && c != 'n' && c != 'Y' && c != 'N');

            if (c == 'y' || c == 'Y')
                return true;
            else {
                Console.Clear();
                return false;
            }
        }

        /* Fonction permettant d'enlever les accents (ou caractères spéciaux) */
        public static string enleverLesAccents(string prenom) {
            string prenomSansAccent = "";
            for (int k = 0; k < prenom.Length; ++k)
                if ('é'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += 'E';
                else if ('è'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "E";
                else if ('ê'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "E";
                else if ('ï'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "I";
                else if ('î'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "I";
                else if ('ñ'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "N";
                else if ('à'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "A";
                else if ('â'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "A";
                else if ('ö'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "O";
                else if ('ô'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "O";
                else if ('ù'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "U";
                else if ('ç'.Equals(prenom.ToLower()[k]))
                    prenomSansAccent += "C";
                else
                    prenomSansAccent += prenom[k];
            return prenomSansAccent;
        }


        /* Fonction permettant de rentrer une année */
        public static int rentrerAnnee(int anneeMin, int anneeMax) {
            int annee;
            bool premiereValeur = true;

            /* Ici, on va essayer de rentrer une année (type int) */
            try {
                do {
                    if (premiereValeur)
                        premiereValeur = false;
                    else
                        messageErreur("La date n'est pas comprise entre " + anneeMin + " et " + anneeMax);

                    annee = int.Parse(Console.ReadLine());
                }
                while (annee > anneeMax || annee < anneeMin);
            }
            catch {
                messageErreur("Vous n'avez pas rentré un entier.");
                annee = -1;
            }

            return annee;
        }

        /* Ici, on va demander à l'utilisateur de rentrer un prénom,
         * afin d'avoir les informations sur celui-ci pour l'année/la période donnée.
         * Si celui-ci n'existe pas, on affiche la liste des prénoms
         * commençant par la chaîne rentrée */
        public static void rechercherPrenom(Prenom[] prenoms, bool periode) {
            Prenom p = new Prenom() { annee = 0, prenom = null, nombre = 0, ordre = 0 };
            bool premierPassage = true;
            Prenom[] commencerPar;
            string prenom = "";
            
            Console.WriteLine("Rentrez le prénom souhaité : ");

            do {
                if (!premierPassage) {
                    Console.Clear();
                    if (periode)
                        nbNaissanceEtOrdrePeriodeAffichage();
                    else
                        nbNaissanceEtOrdreAnneeAffichage();
                    Console.WriteLine("Ce prénom ne se trouve pas dans la liste.\n" +
                                        "Voici les prénoms commençant par {0} : ", prenom);

                    commencerPar = prenomCommencePar(prenoms, prenom);

                    if (commencerPar != null)
                        foreach (Prenom pr in commencerPar)
                            Console.Write("{0}, ", pr.prenom);

                    Console.WriteLine();
                    Console.WriteLine("\nVeuillez entrer un autre prénom : \n");
                }
                else
                    premierPassage = false;

                prenom = enleverLesAccents(Console.ReadLine().ToUpper());

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

        /* Réécriture du StartsWith(...), méthode disponible pour les types string
         * On ne savait pas si on avait le droit de l'utiliser */
        public static bool startsWith(string morceau, string chaineEntiere) {
            bool res = true;
            int i = 0;
            
            if (!(morceau == null || chaineEntiere == null)) {
                if (morceau.Length > chaineEntiere.Length)
                    return false;
                else
                    while (i < morceau.Length && res) {
                        if (morceau[i] != chaineEntiere[i])
                            res = false;
                        ++i;
                    }
                return res;
            }
            else
                return false;
        }

        /* Réécriture du Contains(...), méthode disponible pour les tableaux,
         * mais adaptée à notre cas
         * On ne savait pas si on avait le droit de l'utiliser */
        public static bool containsPrenom(Prenom[] prenoms, Prenom prenom) {
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
        public static Prenom[] prenomCommencePar(Prenom[] prenoms, string debutPrenom) {
            Prenom[] resultatTmp = new Prenom[prenoms.Length], resultat;
            int nb = -1;

            foreach (Prenom p in prenoms)
                if (startsWith(debutPrenom, p.prenom))
                    resultatTmp[++nb] = p;

            if (nb != -1) {
                resultat = new Prenom[nb + 1];

                for (int i = 0; i < nb + 1; ++i)
                    resultat[i] = resultatTmp[i];
            }
            else
                resultat = null;

            return resultat;
        }

        /* Méthode permettant d'échanger la valeur de deux entiers */
        public static void echanger(ref int v1, ref int v2) {
            int vT = v1;
            v1 = v2;
            v2 = vT;
        }

        /* Méthode permettant d'ajouter le nombre de fois qu'a été donné un prénom
         * d'une année au nombre de fois qu'à été donné un prénom dans une période donnée */
        public static void ajouterNombreAuPrenom(Prenom[] prenoms, string prenom, int nombre) {
            int i = 0;
            bool ok = false;

            while (i < prenoms.Length && !ok) {
                if (prenom.Equals(prenoms[i].prenom))
                    prenoms[i].nombre += nombre;
                ++i;
            }
        }

        /* Méthode permettant de trier les prénoms selon le nombre de fois
         * qu'ils ont été donné */
        public static void trierPrenomsParNombre(Prenom[] prenoms) {
            bool ok = false;

            /* Tri utilisé : tri à bulles */
            while (!ok) {
                ok = true;
                for (int i = 1; i < prenoms.Length; ++i)
                    if (prenoms[i - 1].nombre < prenoms[i].nombre) {
                        Prenom p = prenoms[i];
                        prenoms[i] = prenoms[i - 1];
                        prenoms[i - 1] = p;
                        ok = false;
                    }
            }
        }

        /* Méthode permettant de trier un tableau de Prenom par ordre alphabétique */
        public static void triAlphabetique(Prenom[] prenoms)
        {
            bool termine = false, echangeOk = false;
            int i = 1, indexString = 0;

            /* Tri utilisé : tri à bulles */
            while (!termine)
            {
                termine = true;
                while (i < prenoms.Length)
                {
                    echangeOk = false;
                    while (indexString < prenoms[i - 1].prenom.Length &&
                            indexString < prenoms[i].prenom.Length && !echangeOk)
                    {
                        if (prenoms[i - 1].prenom[indexString] > prenoms[i].prenom[indexString])
                        {
                            Prenom p = prenoms[i - 1];
                            prenoms[i - 1] = prenoms[i];
                            prenoms[i] = p;
                            echangeOk = true;
                            termine = false;
                        }
                        else
                            if (prenoms[i - 1].prenom[indexString] < prenoms[i].prenom[indexString])
                                echangeOk = true;

                        ++indexString;
                    }
                    ++i;
                    indexString = 0;
                }
                i = 1;
            }
        }

        /* Fonction permettant de récupérer les prénoms d'une période (triés par nombre) */
        public static Prenom[] recupererEtTrierPrenomsPeriode(Prenom[] prenoms, int anneeD, int anneeF) {
            Prenom[] prenomsPeriodeTmp, resultat;
            int cpt = 0;

            prenomsPeriodeTmp = new Prenom[(anneeF - anneeD + 1) * 100];

            /* Pour chaque Prenom du tableau, s'il n'existe pas dans le tableau
             * prenomsPeriodeTmp, on le rajoute. Sinon, on ajoute le nombre de fois
             * que le prénom a été donné une autre année */
            foreach (Prenom p in prenoms)
                if (p.annee >= anneeD && p.annee <= anneeF)
                    if (!containsPrenom(prenomsPeriodeTmp, p))
                        prenomsPeriodeTmp[cpt++] = p;
                    else
                        ajouterNombreAuPrenom(prenomsPeriodeTmp, p.prenom, p.nombre);

            resultat = new Prenom[cpt];   // On stocke dans un autre tableau pour enlever les cases inutiles

            trierPrenomsParNombre(prenomsPeriodeTmp);   // On trie le tableau par ordre croissant sur prenom.nombre

            /* On stocke les données dans le nouveau tableau, en redéfinissant l'ordre 
             * et l'année (0) */
            for (int i = 0; i < resultat.Length; ++i) {
                resultat[i] = prenomsPeriodeTmp[i];
                resultat[i].ordre = i + 1;
                resultat[i].annee = 0;
            }

            return resultat;
        }
        #endregion

        static void Main(string[] args) {
            Prenom[] prenoms = null;
            debutProgramme();

            string adresseFichier = Console.ReadLine();

            Console.WriteLine("Nom du fichier : (sans l'extension -> prenoms_bordeaux par exemple) : ");
            string nomFichier = Console.ReadLine();

            int anneeMin = 0, anneeMax = 0;

            // Les données du fichier texte
            prenoms = lectureFichier(adresseFichier, nomFichier, ref anneeMin, ref anneeMax);
            bool quitter = false;                                   // C'est lui qui va arrêter le programme
            char c;                                                 // Permet de sélectionner la fonctionnalité

            if (prenoms != null)
                Console.Clear();

            while (!quitter && prenoms != null) {
                menuPrincipal();
                Console.WriteLine("\nChoisissez ce que vous voulez faire :");
                c = char.ToLower(Console.ReadKey().KeyChar);

                /* On choisit la fonctionnalité en fonction du choix de l'utilisateur */
                switch (c) {
                    case (char) MODE.TOP10ANNEE:
                        do {
                            Console.Clear();
                            topXNaissanceAnnee(prenoms, 10, true, anneeMin, anneeMax);
                        }
                        while (recommencerFonctionnalité());
                        break;

                    case (char) MODE.TOP10PERIODE:
                        do {
                            Console.Clear();
                            topXNaissancePeriode(prenoms, 10, true, anneeMin, anneeMax);
                        }
                        while (recommencerFonctionnalité());
                        break;

                    case (char) MODE.NAISSANCEETORDREANNEE:
                        do {
                            Console.Clear();
                            nbNaissanceEtOrdreAnnee(prenoms, anneeMin, anneeMax);
                        }
                        while (recommencerFonctionnalité());
                        break;

                    case (char) MODE.NAISSANCEETORDREPERIODE:
                        do {
                            Console.Clear();
                            nbNaissanceEtOrdrePeriode(prenoms, anneeMin, anneeMax);
                        }
                        while (recommencerFonctionnalité());
                        break;

                    case (char) MODE.TENDANCE:
                        do {
                            Console.Clear();
                            tendancePrenomPeriode(prenoms, anneeMin, anneeMax);
                        }
                        while (recommencerFonctionnalité());
                        break;

                    case (char)MODE.PRENOMPLUSMOINSDONNE:
                        do {
                            Console.Clear();
                            prenomPlusMoinsDonne(prenoms, anneeMin, anneeMax);
                        }
                        while (recommencerFonctionnalité());
                        break;

                    case (char)MODE.MOTEURRECHERCHEPRENOM:
                        do {
                            Console.Clear();
                            moteurRecherchePrenom(prenoms, anneeMin, anneeMax);
                        }
                        while (recommencerFonctionnalité());
                        break;

                    case (char)MODE.INFORMATIONPRENOM:
                        do {
                            Console.Clear();
                            informationPrenom(prenoms, anneeMin, anneeMax);
                        }
                        while (recommencerFonctionnalité());
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
