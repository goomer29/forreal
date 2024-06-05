using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forreal.ViewModels
{
    public class LoadingPageViewModel:ViewModel
    {
        public ObservableCollection<string> Facts { get; set; }
        public string Fact { get; set; }
        public LoadingPageViewModel()
        {
            Facts=GetFacts();
            Fact = GetRandomFact(Facts);
        }
        private string GetRandomFact(ObservableCollection<string> facts)
        {
            Random rnd = new Random();
            int num = rnd.Next(0, facts.Count);
            return facts[num];
        }
        private ObservableCollection<string> GetFacts()
        {
            ObservableCollection<string> facts= new ObservableCollection<string>();
            facts.Add("did you know ? ילד כותב בתוך דלי is a polindrom!");
            facts.Add("did you know? רק פושטק עלוב בולע קטשופ קר is a polindrom!");
            facts.Add("did you know? in the 40's, boy's color was pink and girl's color was blue!");
            facts.Add("did you know? some people are born without fingerprints!");
            facts.Add("did you know? identical twins don’t have the same fingerprints!");
            facts.Add("did you know? identical twins don’t have the same fingerprints!");
            facts.Add("did you know? wearing a tie can reduce blood flow to the brain by 7.5%");
            facts.Add("did you know? the world’s oldest dog lived to 29.5 years old!");
            facts.Add("did you know? football teams wearing red kits play better");
            facts.Add("did you know? a horse normally has more than one horsepower");
            facts.Add("did you know? deaf people are known to use sign language in their sleep");
            facts.Add("did you know? a horse normally has more than one horsepower");
            facts.Add("did you know? LEGO bricks withstand compression better than concrete");
            facts.Add("did you know? flamingoes aren’t born pink");
            return facts;
        }
    }
}
