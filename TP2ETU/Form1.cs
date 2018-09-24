/*
 * Cette application consiste à montrer des mots avec des images au joueur 
 * et il doit ecrire les mots qu'il se rappel.
 * Auteur: Kevin R. Leclaire
 * 
 * */

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TP2ETU.Properties;
using WMPLib;
namespace TP2ETU
{
    public partial class frmMemoryGameMain : Form
    {
        #region Propriétés /  variables partagées par toutes les méthodes.
        /// <summary>
        /// variable pour la musique
        /// </summary>
        WindowsMediaPlayer mediaPlayer = new WindowsMediaPlayer();
        /// <summary>
        /// Tableau contenant tous les pictures box utilisées. Ce tableau est construit lors du chargement de la
        /// fenêtre (méthode Load)
        /// </summary>
        PictureBox[] tousLesPicturesBox = null;

        /// <summary>
        /// Tableau contenant toutes les images possibles pour le jeu.
        /// </summary>
        Bitmap[] toutesLesImagesAffichees = new Bitmap[] { Resources.Meteor_Logo_Art, Resources.auron1, Resources.yuna1, Resources.tidus1, Resources.lulu1, Resources.wakka1,Resources.rikku1, Resources.kimahri1, Resources.seymour1, Resources.aeris, Resources.cloud, Resources.barret, Resources.Tifa, Resources.cait, Resources.vincent, Resources.rouge, Resources.yuffie, Resources.sephiroth, Resources.Cid};

        /// <summary>
        /// Tableau contenant les mots associés aux images du tableau toutesLesImagesAffichees.
        /// </summary>
        string[] tousLesTextesAssociesAuxImages = new string[] { "Cachée", "auron", "yuna", "tidus", "lulu", "wakka", "rikku", "kimahri", "seymour", "aeris", "clad", "barret", "tifa", "cait", "vincent", "rougexiii", "yuffie", "sephirot", "cid" };
        /// <summary>
        /// Nombre de mots a trouvés
        /// </summary>
        int nombreDeMot = 2;
        /// <summary>
        /// Tableau qui va contenir tous les indexes des mots choisis au hasard et qui seront placé 
        /// dans une des indexes de ce tableau au hasard pour assigner les images au picturesbox facilement 
        /// </summary>
        int[] indexMotChoisi = new int[32] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        /// <summary>
        /// Nombre de mots trouvés
        /// </summary>
        int motTrouvé = 0;

        Random rnd = new Random();
        #endregion

        /// <summary>
        /// Appelée au chargement de l'application pour constituer le tableau des picturebox
        /// et initialiser correctement les valeurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMemoryGameMain_Load(object sender, EventArgs e)
        {
            // Création du tableau des picturebox
            tousLesPicturesBox = new PictureBox[]
            {
        
        pbImg8,pbImg9,pbImg10,
        pbImg16,pbImg17,pbImg18,pbImg19,pbImg20,pbImg21,pbImg22,
        pbImg24,pbImg25,pbImg26,pbImg27,pbImg28,pbImg29,pbImg30,pbImg31
            };


            // A COMPLETER AU BESOIN

        }

  #region Méthodes de gestion des événements et/ou appelées automatiquement
        public frmMemoryGameMain()
        {
            InitializeComponent();
        }



        #endregion

  #region Bouton
        /// <summary>
        /// Cette fonction débute une partie"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DebuterPartie_Click(object sender, EventArgs e)
        {           
            SelectionnerMotAUtiliser();
            AssignerImagesDesMotsAuxPicturesBox();
            timerMasquerImage.Enabled = true;
            DebuterPartie.Enabled = false;
        }
        /// <summary>
        /// Cette fonction sert à valider la partie en comparant les mots entrés par 
        /// le joueur et ceux qu’il devait trouver.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValiderPartie_Click(object sender, EventArgs e)
        {
            ValiderPartie.Enabled = false;
            string motsAAfficher = "";
            string[] motsEntrés = new string[nombreDeMot];
            string[] motsRecherchés = new string[nombreDeMot];
            motsRecherchés = ObtenirMotsRecherchés();
            motsEntrés = ExtraireMotsEntrésParJoueur();
            for (int compteur = 0; compteur < motsRecherchés.Length; compteur++)
            {
                if (motsEntrés.Contains(motsRecherchés[compteur]))
                {
                    motTrouvé++;
                }
                else
                {
                    motsAAfficher += "  " + motsRecherchés[compteur] + "  ";
                }
            }

            groupBox1.Text = "mots trouvés : " + motTrouvé.ToString() + "\nmots non trouvés : " + (nombreDeMot - motTrouvé) +"\n"+ motsAAfficher;
            if(motTrouvé == nombreDeMot)
            {
                Form2 Excellence = new Form2();
                Excellence.Show();

            }
            // Arrête la musique
            mediaPlayer.controls.stop();
        }
        /// <summary>
        /// Ce bouton sert a recommencer le jeux  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Recommencer_Click(object sender, EventArgs e)
        {
            ReinitialiserJeu();
        }
        #endregion

        /// <summary>
        /// Cette fonction choisie au hasard les mots à trouver et les place dans un index
        /// aléatoire dans le tableau partagé
        /// </summary>
        /// 
        void SelectionnerMotAUtiliser()
        {
            int indexMot;
            
            for (int compteur = 0; compteur < nombreDeMot; compteur++)
            {
                do
                {
                     indexMot = rnd.Next(1, toutesLesImagesAffichees.Length);
                } while (indexMotChoisi.Contains(indexMot));
                indexMotChoisi[ChoisirIndexNonUtilisee(indexMotChoisi)] = indexMot;
            }
        }
        /// <summary>
        /// Cette fonction choisie un index aléatoirement
        /// dans un tableau d’entiers en s’assurant qu’il n’est pas utilisé par un autre mot.
        /// </summary>
        /// <param name="indexMotChoisi"></param>
        /// <returns>un entier qui représente un index non utilisé dans le tableau reçu en paramètre. </returns>
        int ChoisirIndexNonUtilisee(int[] indexMotChoisi)
        {
            int indexChoisi;
            Random rnd2 = new Random();
            do
            {
                indexChoisi = rnd2.Next(0, tousLesPicturesBox.Length);
            } while (indexMotChoisi[indexChoisi] != -1);
            return indexChoisi;
        }
        /// <summary>
        /// Cette fonction assigne un image qui correspond aux mots choisie aléatoirement dans une picturebox
        /// choisie aléatoirement.
        /// </summary>
        void AssignerImagesDesMotsAuxPicturesBox()
        {
            for (int compteur = 0; compteur <= indexMotChoisi.Length - 1; compteur++)
            {
                if (indexMotChoisi[compteur] != -1)
                {
                    tousLesPicturesBox[compteur].BackgroundImage = toutesLesImagesAffichees[indexMotChoisi[compteur]];
                }
            }
        }
        /// <summary>
        /// Cette fonction remet tous les picturesbox a l'image de base
        /// </summary>
        void MasquerImage()
        {
            for (int compteur = 0; compteur < tousLesPicturesBox.Length; compteur++)
            {
                tousLesPicturesBox[compteur].BackgroundImage = toutesLesImagesAffichees[0];
            }
            timerMasquerImage.Enabled = false;

        }
        /// <summary>
        /// Cette fonction obtient les mots recherchés qui ont été choisi aléatoirement.
        /// </summary>
        /// <returns>retourne un tableau de chaine de caractères avec tous les mot recherchés.</returns>
        string[] ObtenirMotsRecherchés()
        {
            int compteur2 = 0;
            string[] motsRecherchés = new string[1000];
            for (int compteur = 0; compteur < indexMotChoisi.Length; compteur++)
            {
                if (indexMotChoisi[compteur] != -1)
                {
                    motsRecherchés[compteur2] = tousLesTextesAssociesAuxImages[indexMotChoisi[compteur]];
                    compteur2++;
                }

            }
            return motsRecherchés;
        }
        /// <summary>
        /// Cette fonction extrait les mots entrés par le joueur dans une boite de texte.
        /// </summary>
        /// <returns>retourne un tableau de chaine de caractères avec tous les mots entrés par le joueur. </returns>
        string[] ExtraireMotsEntrésParJoueur()
        {
            
            string[] motsEntrés = new string[nombreDeMot];
            motsEntrés = textBox1.Text.ToLower().Split(' ');
            motsEntrés = ExtraireÉlémentsUniques(motsEntrés);
            return motsEntrés;
        }
        /// <summary>
        /// Cette fonction enlève les éléments 
        /// qui sont écrit plusieurs fois pour les garder une seule fois.
        /// </summary>
        /// <param name="motsEntrés"></param>
        /// <returns></returns>
        string[] ExtraireÉlémentsUniques(string[] motsEntrés)
        {
            for(int compteur = 0;compteur < motsEntrés.Length;compteur++)
            {
                for(int compteur2 = 0;compteur2 < motsEntrés.Length;compteur2++)
                {
                    if(motsEntrés[compteur] == motsEntrés[compteur2] && compteur != compteur2)
                    {
                        motsEntrés[compteur2] = "";
                    }
                }
            }
            return motsEntrés;
        }
        /// <summary>
        /// Cette fonctiom remet tous les variables partagées a leur valeur de base et 
        /// aussi toutes les sortes de boite avec du text.
        /// </summary>
        void ReinitialiserJeu()
        {
            for(int compteur = 0;compteur < indexMotChoisi.Length;compteur++ )
            {
                indexMotChoisi[compteur] = -1;               
            }
            motTrouvé = 0;

            ValiderPartie.Enabled = false;
            DebuterPartie.Enabled = true;
            textBox1.Text = "";
            groupBox1.Text = "Résultats";
            textBox1.Enabled = false;
            MasquerImage();

        }
        /// <summary>
        /// Cette fonction sert a choisir le nombre de mots à afficher et elle recommence la 
        /// partie si le nombre de mot est changé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
          nombreDeMot  = Convert.ToInt32(numericUpDown1.Value);
            ReinitialiserJeu();
        }     
        /// <summary>
        /// Cette fonction change l'image de fond à chaque 10 secondes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlternerImageDeFond_Tick(object sender, EventArgs e)
        {
            Bitmap[] toutesLesImagesDeFond = new Bitmap[] { Resources.FFVII_BackGround, Resources.FFVII_BackGroundVincent, Resources.FFX_BackGround2, Resources.FFX_BackGroundAuron };
            Random rnd = new Random();
            int imageAleatoire = 0;

            imageAleatoire = rnd.Next(0, 4);
            BackgroundImage = toutesLesImagesDeFond[imageAleatoire];

        }
  #region Menu

        #region TimerMasquerImage
        //Timer qui sert à masquer les images après un certain temps décider par le joueur
        private void timerMasquerImage_Tick(object sender, EventArgs e)
        {
            MasquerImage();
            ValiderPartie.Enabled = true;
            textBox1.Enabled = true;
        }
        //bouton dans le menu pour changer le nombre de seconde pour afficher les images
        private void secondeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerMasquerImage.Interval = 1000;
        }
        private void secToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerMasquerImage.Interval = 2000;
        }
        private void secToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timerMasquerImage.Interval = 3000;
        }
        private void secToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            timerMasquerImage.Interval = 4000;
        }
        private void secToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            timerMasquerImage.Interval = 5000;
        }
        private void secToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            timerMasquerImage.Interval = 6000;
        }
        #endregion

        // bouton dans le menu pour recommencer la partie
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReinitialiserJeu();
        }
        //bouton dans le menu pour quitter l'application
        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

  #region Musique
        private void battleThemeFFXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer.URL = "Art/BattleFFX.mp3";
            mediaPlayer.controls.play(); 
        }

        private void battleThemeFFVIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer.URL = "Art/BattleFFVII.mp3";
            mediaPlayer.controls.play();
        }

        private void toZanarkandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer.URL = "Art/ToZanarkand.mp3";
            mediaPlayer.controls.play();
        }

        private void fFVIIMainThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer.URL = "Art/FFVIIMAINTHEME.mp3";
            mediaPlayer.controls.play();
        }

        private void sTOPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer.controls.stop();
        }

        #endregion

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ValiderPartie.PerformClick();
            }
        }
    }
}

