using System;
using System.IO;
using System.Collections.Generic;

namespace MotsMystères
{
    /// <summary>
    /// Auteur: Michael Meilleur
    /// Description: Application de type console qui permet de jouer aux mots mystères.
    /// Date: 2021-11-25
    /// </summary>
    class Program
    {
        #region méthodes
        /// <summary>
        /// Auteur: Michael Meilleur
        /// Description: Demander un numéro de mot à l'utilisateur.
        /// Date: 2021-11-28
        /// </summary>
        /// <returns>iNum</returns>
        static private int DemanderNuméro()
        {
            //Variables locales.
            string sRéponse = "";
            bool bValide = false;
            int iNum = 0;

            //Demander le numéro de case.
            do
            {
                Console.WriteLine();
                Console.Write("Entrer le numéro du mot: ");
                sRéponse = Console.ReadLine();
                bValide = int.TryParse(sRéponse, out iNum) && iNum <= 68 && iNum >= 1;
                if (!bValide)
                {
                    Console.WriteLine("ERREUR: N'est pas un nombre ou n'est pas un nombre valide.");
                }
            } while (!bValide);

            return iNum;
        }

        /// <summary>
        /// Auteur: Michael Meilleur
        /// Description: Remplir un tableau 2D
        /// Date : 2021-11-28
        /// </summary>
        /// <param name="listeslettres"></param>
        /// <returns>acTableau</returns>
        static private char[,] RemplirTableau2D(List<string> listeslettres)
        {
            // Variables locales.
            int iLignes = listeslettres.Count;
            int iColonnes = 0;
            char cLettre = ' ';
            string sLigne = listeslettres[0];
            char[,] acTableau = null;

            // Trouver le nombre de colonnes.
            for (int iIndex = 0; iIndex < sLigne.Length; iIndex++)
            {
                cLettre = sLigne[iIndex];
                if (char.IsLetter(cLettre))
                {
                    iColonnes++;
                }
            }

            // Dimensions.
            acTableau = new char[iLignes, iColonnes];

            for (int iLigne = 0; iLigne < acTableau.GetLength(0); iLigne++)
            {
                byte byColonne = 0;
                byte byIndex = 0;
                sLigne = listeslettres[iLigne];
                for (int iColonne = 0; iColonne < sLigne.Length; iColonne++)
                {
                    cLettre = sLigne[byIndex];
                    if (char.IsLetter(cLettre))
                    {
                        acTableau[iLigne, byColonne] = cLettre;
                        byColonne++;
                    }

                    byIndex++;
                }

            }
            return acTableau;
        }

        /// <summary>
        /// Auteur: Michael Meilleur
        /// Description: Afficher un tableau 2D.
        /// Date: 2021-11-28
        /// </summary>
        /// <param name="acTableau"></param>
        static private void AfficherTableau2D(char[,] acTableau)
        {
            Console.Write("  ");
            // Afficher les index de colonnes.
            for (int iColonne = 0; iColonne < acTableau.GetLength(1); iColonne++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (iColonne < 10)
                    Console.Write(" 0" + iColonne);
                else
                    Console.Write(" " + iColonne);
                Console.ResetColor();
            }
            Console.WriteLine();


            // Afficher les lignes du tableau.
            for (int iLigne = 0; iLigne < acTableau.GetLength(0); iLigne++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (iLigne < 10)
                    Console.Write("0" + iLigne + " ");
                else
                    Console.Write(iLigne + " ");
                Console.ResetColor();
                for (int iColonne = 0; iColonne < acTableau.GetLength(1); iColonne++)
                {
                    if (char.IsUpper(acTableau[iLigne, iColonne]))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(acTableau[iLigne, iColonne] + "  ");
                        Console.ResetColor();
                    }
                    else
                        Console.Write(acTableau[iLigne, iColonne] + "  ");
                }
                Console.WriteLine();
            }

        }

        /// <summary>
        /// Auteur: Michael Meilleur
        /// Description: Afficher la liste de mots à trouver.
        /// Date: 2021-12-09
        /// </summary>
        /// <param name="iCptMot"></param>
        /// <param name="iMin"></param>
        /// <param name="iMax"></param>
        /// <param name="MotsTrouvés"></param>
        /// <param name="mots"></param>
        static private void AfficherMots(int iCptMot, int iMin, int iMax, List<string> MotsTrouvés, List<string> mots)
        {

            // Afficher les mots.
            for (int iIndex = iMin; iIndex <= iMax; iIndex = iIndex + 10)
            {
                if (iCptMot != 10)
                {
                    if (MotsTrouvés.Contains(mots[iIndex]))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(iCptMot + ") " + mots[iIndex].PadRight(14, ' ') + " ");
                        Console.ResetColor();
                        iCptMot += 10;
                    }
                    else
                    {
                        Console.Write(iCptMot + ") " + mots[iIndex].PadRight(14, ' ') + " ");
                        iCptMot += 10;
                    }
                }
                else
                {
                    if (MotsTrouvés.Contains(mots[iIndex]))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(iCptMot + ") " + mots[iIndex].PadRight(13, ' ') + " ");
                        Console.ResetColor();
                        iCptMot += 10;
                    }
                    else
                    {
                        Console.Write(iCptMot + ") " + mots[iIndex].PadRight(13, ' ') + " ");
                        iCptMot += 10;
                    }
                }

            }
            Console.WriteLine();
        }

        #endregion
        static void Main(string[] args)
        {
            // variables locales.
            StreamReader lettres = null;
            string sRéponse = "";
            string sLigne = "";
            string sMot = "";
            string sMotHor = "";
            string sMotVer = "";
            string sMotDiag = "";
            int iNum = 0;
            int iMotTrouvés = 0;
            int iCptIndex = 0;
            int iCptIndex2 = 0;
            int iCoord1 = 0;
            int iCoord2 = 0;
            int iCoord3 = 0;
            int iCoord4 = 0;
            bool bValide = false;
            bool bValide1 = false;
            bool bQuitter = false;
            List<string> listelignes = new List<string>();
            List<string> mots = new List<string>();
            List<string> MotsTrouvés = new List<string>();
            char[,] acLettre = null;


            // Demander le nom du fichier à ouvrir et remplir les listes.
            do
            {
                Console.Write("Entrer le nom du fichier: ");
                sRéponse = Console.ReadLine();
                if (File.Exists(sRéponse))
                {
                    lettres = new StreamReader(sRéponse);

                    while (!lettres.EndOfStream && !bValide && !bValide1)
                    {
                        do
                        {
                            sLigne = lettres.ReadLine();
                            if (sLigne == "*** MOTS À TROUVER: ***")
                                bValide = true;
                            else
                                listelignes.Add(sLigne); // remplir la liste de lignes.
                        }
                        while (!bValide);

                        while (!lettres.EndOfStream && !bValide1)
                        {
                            do
                            {
                                sLigne = lettres.ReadLine();
                                if (sLigne == "*** RÉPONSE ***")
                                    bValide1 = true;
                                else
                                    mots.Add(sLigne); // remplir la liste de mots.
                            }
                            while (!bValide1);
                        }
                    }
                    lettres.Close();
                    bQuitter = true;
                }
                else
                {
                    Console.WriteLine("Le fichier n'existe pas!");
                    bQuitter = false;
                }
            } while (!bQuitter);


            // Remplir le tableau.
            acLettre = RemplirTableau2D(listelignes);

            do
            {
                // Afficher les caractères et le menu.       
                Console.Clear();
                Console.WriteLine("========================= MOT MYSTÈRE =========================");
                AfficherTableau2D(acLettre);
                Console.WriteLine("=====================================");
                Console.WriteLine("Liste de mots à trouver (" + iMotTrouvés + " de 68):");
                Console.WriteLine("=====================================");

                // Afficher les mots.
                AfficherMots(1, 0, 60, MotsTrouvés, mots);
                AfficherMots(2, 1, 61, MotsTrouvés, mots);
                AfficherMots(3, 2, 62, MotsTrouvés, mots);
                AfficherMots(4, 3, 63, MotsTrouvés, mots);
                AfficherMots(5, 4, 64, MotsTrouvés, mots);
                AfficherMots(6, 5, 65, MotsTrouvés, mots);
                AfficherMots(7, 6, 66, MotsTrouvés, mots);
                AfficherMots(8, 7, 67, MotsTrouvés, mots);
                AfficherMots(9, 8, 58, MotsTrouvés, mots);
                AfficherMots(10, 9, 59, MotsTrouvés, mots);


                // Demander le numéro du mot.
                iNum = DemanderNuméro();
                sMot = mots[iNum - 1]; // garder le mot en mémoire.

                // Demander les coordonnées.
                do
                {
                    Console.Write("Entrer la coordonnée (ex:00,00) de la première lettre du mot " + sMot.ToUpper() + " : ");
                    sRéponse = Console.ReadLine();
                    string réponse = sRéponse;
                    string[] réponseliste = sRéponse.Split(",");
                    bValide = int.TryParse(réponseliste[0], out iCoord1) && int.TryParse(réponseliste[1], out iCoord2);
                    if (!bValide || iCoord1 > 19 || iCoord1 < 0 || iCoord2 > 19 || iCoord2 < 0)
                    {
                        Console.WriteLine("ERREUR: Coordonnées non valables!");
                    }
                } while (!bValide || iCoord1 > 19 || iCoord1 < 0 || iCoord2 > 19 || iCoord2 < 0);

                do
                {
                    Console.Write("Entrer la coordonnée (ex:00,00) de la dernière lettre du mot " + sMot.ToUpper() + " : ");
                    sRéponse = Console.ReadLine();
                    string réponse = sRéponse;
                    string[] réponseliste = sRéponse.Split(",");
                    bValide = int.TryParse(réponseliste[0], out iCoord3) && int.TryParse(réponseliste[1], out iCoord4);
                    if (!bValide || iCoord3 > 19 || iCoord3 < 0 || iCoord4 > 19 || iCoord4 < 0)
                    {
                        Console.WriteLine("ERREUR: Coordonnées non valides!");
                    }
                } while (!bValide || iCoord3 > 19 || iCoord3 < 0 || iCoord4 > 19 || iCoord4 < 0);

                // Comparer le mot choisis avec la grille de lettres.

                // Comparer le mot dans la grille à l'horizontale, à la verticale et en diagonale.
                if (iCoord1 == iCoord3)
                {
                    for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                    {
                        if (iCoord2 < iCoord4)
                        {
                            sMotHor += acLettre[iCoord1, iCoord2 + iCptIndex]; // Chercher à l'horizontale gauche-droite.                          
                            iCptIndex++;

                        }
                        else
                        {
                            sMotHor += acLettre[iCoord1, iCoord2 - iCptIndex]; // Chercher à l'horizontale droite-gauche.                                                 
                            iCptIndex++;
                        }

                    }

                    // Mettre les lettres en majuscules.
                    if (sMot == sMotHor.ToLower())
                    {
                        for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                        {
                            if (iCoord2 < iCoord4)
                            {
                                acLettre[iCoord1, iCoord2 + iCptIndex2] = char.ToUpper(acLettre[iCoord1, iCoord2 + iCptIndex2]);
                                iCptIndex2++;
                            }
                            else
                            {
                                acLettre[iCoord1, iCoord2 - iCptIndex2] = char.ToUpper(acLettre[iCoord1, iCoord2 - iCptIndex2]);
                                iCptIndex2++;
                            }
                        }
                    }
                }
                else if (iCoord2 == iCoord4)
                {

                    for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                    {
                        if (iCoord1 < iCoord3)
                        {
                            sMotVer += acLettre[iCoord1 + iCptIndex, iCoord2]; // Chercher à la verticale de haut en bas.
                            iCptIndex++;
                        }
                        else
                        {
                            sMotVer += acLettre[iCoord1 - iCptIndex, iCoord2]; // Chercher à la verticale de bas en haut.                                                      
                            iCptIndex++;
                        }
                    }

                    // Mettre les lettres en majuscules.
                    if (sMot == sMotVer.ToLower())
                    {
                        for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                        {
                            if (iCoord1 < iCoord3)
                            {
                                acLettre[iCoord1 + iCptIndex2, iCoord2] = char.ToUpper(acLettre[iCoord1 + iCptIndex2, iCoord2]);
                                iCptIndex2++;
                            }
                            else
                            {
                                acLettre[iCoord1 - iCptIndex2, iCoord2] = char.ToUpper(acLettre[iCoord1 - iCptIndex2, iCoord2]);
                                iCptIndex2++;
                            }
                        }
                    }
                }
                else if (iCoord1 > iCoord3 && iCoord2 < iCoord4)
                {
                    for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                    {
                        {
                            sMotDiag += acLettre[iCoord1 - iCptIndex, iCoord2 + iCptIndex]; // Chercher en diagonale de bas en haut et de gauche à droite.
                            iCptIndex++;
                        }
                    }
                    // Mettre les lettres en majuscules.
                    if (sMot == sMotDiag.ToLower())
                    {
                        for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                        {
                            acLettre[iCoord1 - iCptIndex2, iCoord2 + iCptIndex2] = char.ToUpper(acLettre[iCoord1 - iCptIndex2, iCoord2 + iCptIndex2]);
                            iCptIndex2++;
                        }
                    }
                }
                else if (iCoord1 > iCoord3 && iCoord2 > iCoord4)
                {
                    for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                    {
                        sMotDiag += acLettre[iCoord1 - iCptIndex, iCoord2 - iCptIndex]; // Chercher en diagonale de bas en haut et de droite à gauche.
                        iCptIndex++;
                    }
                    // Mettre les lettres en majuscules.
                    if (sMot == sMotDiag.ToLower())
                    {
                        for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                        {
                            acLettre[iCoord1 - iCptIndex2, iCoord2 - iCptIndex2] = char.ToUpper(acLettre[iCoord1 - iCptIndex2, iCoord2 - iCptIndex2]);
                            iCptIndex2++;
                        }
                    }
                }
                else if (iCoord1 < iCoord3 && iCoord2 < iCoord4)
                {
                    for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                    {
                        sMotDiag += acLettre[iCoord1 + iCptIndex, iCoord2 + iCptIndex]; // Chercher en diagonale de haut en bas et de gauche à droite.
                        iCptIndex++;
                    }
                    // Mettre les lettres en majuscules.
                    if (sMot == sMotDiag.ToLower())
                    {
                        for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                        {
                            acLettre[iCoord1 + iCptIndex2, iCoord2 + iCptIndex2] = char.ToUpper(acLettre[iCoord1 + iCptIndex2, iCoord2 + iCptIndex2]);
                            iCptIndex2++;
                        }
                    }
                }
                else
                {
                    for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                    {
                        sMotDiag += acLettre[iCoord1 + iCptIndex, iCoord2 - iCptIndex]; // Chercher en diagonale de haut en bas et de droite à gauche.
                        iCptIndex++;
                    }
                    // Mettre les lettres en majuscules.
                    if (sMot == sMotDiag.ToLower())
                    {
                        for (int iIndex = 0; iIndex < sMot.Length; iIndex++)
                        {
                            acLettre[iCoord1 + iCptIndex2, iCoord2 - iCptIndex2] = char.ToUpper(acLettre[iCoord1 + iCptIndex2, iCoord2 - iCptIndex2]);
                            iCptIndex2++;
                        }
                    }
                }

                if (MotsTrouvés.Contains(sMot))
                {
                    Console.WriteLine("Vous avez déjà trouvé ce mot!");
                    Console.WriteLine("Appuyer sur une touche pour continuer.");
                    Console.ReadKey();

                    // Remettre tout à null.
                    sMotHor = "";
                    sMotVer = "";
                    sMotDiag = "";
                    iCptIndex = 0;
                    iCptIndex2 = 0;
                }
                else
                {
                    if (sMot == sMotHor.ToLower() || sMot == sMotVer.ToLower() || sMot == sMotDiag.ToLower())
                    {
                        Console.WriteLine("Bravo!");
                        Console.WriteLine("Appuyer sur une touche pour continuer.");
                        Console.ReadKey();
                        MotsTrouvés.Add(sMot);

                        // Remettre tout à null.
                        sMotHor = "";
                        sMotVer = "";
                        sMotDiag = "";
                        iMotTrouvés++;
                        iCptIndex = 0;
                        iCptIndex2 = 0;
                    }
                    else
                    {
                        Console.WriteLine("ERREUR: Pas le bon mot!");
                        Console.WriteLine("Appuyer sur une touche pour continuer.");
                        Console.ReadKey();

                        // Remettre tout à null.
                        sMotHor = "";
                        sMotVer = "";
                        sMotDiag = "";
                        iCptIndex = 0;
                        iCptIndex2 = 0;
                    }
                }
            } while (iMotTrouvés != 68);
        }
    }
}









