using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Donateurs.Models
{
    public class AnalyseurContributions
    {
        private List<Contribution> contributions;


        public AnalyseurContributions(string cheminFichierCsv) : this()
        {
            AjouterContributions(cheminFichierCsv);
        }


        public AnalyseurContributions()
        {
            this.contributions = new List<Contribution>();
        }


        public void AjouterContributions(string cheminFichierCsv)
        {
            if (File.Exists(cheminFichierCsv))
            {
                using (StreamReader sr = new StreamReader(cheminFichierCsv, Encoding.UTF8))
                {
                    string? ligne = sr.ReadLine();
                    while ((ligne = sr.ReadLine()) != null)
                    {
                        try
                        {
                            this.contributions.Add(new Contribution(ligne));
                        }
                        catch (Exception ex)
                        {
                            throw new FormatException($"{ex.Message} - {ligne}");
                        }
                    }
                }
            }
        }


        public List<Contribution> RechercherContributionsPossiblementIllegales()
        {
            List<Contribution> illegales = new List<Contribution>();
            Contributions.ForEach(contrib => { if (contrib.EstIllegale) illegales.Add(contrib); });
            return illegales;
        }


        public List<Contribution> Contributions => contributions;
    }
}
