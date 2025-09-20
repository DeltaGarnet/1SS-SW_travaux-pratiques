using System;
using System.Text;

namespace Donateurs.Models
{
    public class Contribution
    {
        private string type;
        private string nom;
        private string prenom;
        private decimal montant;
        private int nbVersements;
        private string municipalite;
        private string codePostal;
        private string parti;
        private string candidat;
        private DateTime? dateEvenement = null;
        private int anneeFinanciere;


        public Contribution(string ligneCsv)
        {
            string[] champs = ligneCsv.Split(';');

            if (champs.Length != 10)
                throw new ArgumentException($"Ligne non valide : {ligneCsv}");

            this.type = champs[0];

            string[] nomPrenom = champs[1].Split(',');
            this.nom = nomPrenom[0].Trim();
            this.prenom = nomPrenom[1].Trim();

            this.montant = Convert.ToDecimal(champs[2]);
            this.nbVersements = Convert.ToInt32(champs[3]);

            this.municipalite = champs[4];
            this.codePostal = champs[5];

            this.parti = champs[6];
            this.candidat = champs[7];

            if (champs[8] != "")
            {
                string[] dateSplit = champs[8].Split('-');
                this.dateEvenement = new DateTime(
                    Convert.ToInt32(dateSplit[0]),
                    Convert.ToInt32(dateSplit[1]),
                    Convert.ToInt32(dateSplit[2]));
            }

            this.anneeFinanciere = Convert.ToInt32(champs[9]);
        }


        public string ToCsv()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.Type);
            sb.Append(';');

            sb.Append(this.Nom);
            sb.Append(", ");
            sb.Append(this.Prenom);
            sb.Append(';');

            sb.Append(this.Montant);
            sb.Append(';');

            sb.Append(this.NbVersements);
            sb.Append(';');

            sb.Append(this.Municipalite);
            sb.Append(';');

            sb.Append(this.CodePostal);
            sb.Append(';');

            sb.Append(this.Parti);
            sb.Append(';');

            sb.Append(this.Candidat);
            sb.Append(';');

            sb.Append(this.DateEvenement?.ToString("yyyy-MM-dd") ?? "");
            sb.Append(';');

            sb.Append(this.AnneeFinanciere);

            return sb.ToString();
        }

        public bool EstIllegale
        {
            get
            {
                return Type is "Parti" or "Candidat" or "Député" or "Électeur" && Montant > 200 ||
                        Type == "Campagne" && Montant > 500;
            }
        }

        public string Type => type;

        public string Nom => nom;

        public string Prenom => prenom;

        public decimal Montant => montant;

        public int NbVersements => nbVersements;

        public string Municipalite => municipalite;

        public string CodePostal => codePostal;

        public string Parti => parti;

        public string Candidat => candidat;

        public DateTime? DateEvenement => dateEvenement;

        public int AnneeFinanciere => anneeFinanciere;

        public override string ToString()
        {
            return this.ToCsv();
        }
    }
}
