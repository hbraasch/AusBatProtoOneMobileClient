using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinTools
{
    class DecisionTree
    {
        public class Classification
        {

        }
        public class Species: Classification
        {

            public List<CharacterBase> Characters = new List<CharacterBase>();
            public string speciesId;
            public string genusId;
            public Species(string name)
            {
                this.speciesId = name;
            }
        }

        public class Genus : Classification
        {
            public string name;

            public Genus(string name)
            {
                this.name = name;
            }
        }

        public class CharacterBase
        {
        }




        public abstract class Decision
        {
            public abstract void Evaluate(Species species);
        }
        public class DecisionQuery : Decision
        {
            public string Title { get; set; }
            public Decision Positive { get; set; }
            public Decision Negative { get; set; }
            public Func<Species, bool> Test { get; set; }

            public override void Evaluate(Species species)
            {
                bool result = this.Test(species);
                string resultAsString = result ? "yes" : "no";

                Console.WriteLine($"\t- {this.Title}? {resultAsString}");

                if (result) this.Positive.Evaluate(species);
                else this.Negative.Evaluate(species);
            }

            internal List<CharacterBase> GetCharacteristics()
            {
                throw new NotImplementedException();
            }
        }

        public class DecisionResult : Decision
        {
            public bool Result { get; set; }
            public override void Evaluate(Species species)
            {
                Console.WriteLine("\r\nOFFER A LOAN: {0}", Result ? "YES" : "NO");
            }
        }


        public interface IOptions { 
            
        }

        public class GularPoachPresentOption: Enumeration
        {
            public static GularPoachPresentOption Undefined = new(1, nameof(Undefined));
            public static GularPoachPresentOption IsPresent = new(2, nameof(IsPresent));
            public static GularPoachPresentOption IsNotPresent = new(3, nameof(IsNotPresent));

            public GularPoachPresentOption(int id, string name)
                : base(id, name)
            {
            }
        }

        private static DecisionQuery MainDecisionTree()
        {
            //Decision 4
            var familyBranch = new DecisionQuery
            {
                Title = "FamilyToSpeciesOrGenus",
                Test = (classification) => true,
                Positive = new DecisionResult { Result = true },
                Negative = new DecisionResult { Result = false }
            };
            return familyBranch;
        }

        public void Init()
        {
            List<Classification> classifications = new List<Classification>();

            var genus1 = new Genus("Genus1");
            var genus2 = new Genus("Genus2");

            var species1 = new Species("Species1")
            {
                genusId = genus1.name,
                Characters = 
                {
                new NumericCharacter("ForeArmLength", "Forearm length", 50, 60),
                new OptionsCharacter<GularPoachPresentOption>("GularPoachPresent", "Gular poach", GularPoachPresentOption.IsPresent)
                } 
            };
            var species2 = new Species("Species2")
            {
                genusId = genus2.name,
                Characters =
                {
                new NumericCharacter("ForeArmLength", "Forearm length", 40, 50),
                new OptionsCharacter<GularPoachPresentOption>("GularPoachPresent", "Gular poach", GularPoachPresentOption.IsNotPresent)
                }
            };
            var species3 = new Species("Species3")
            {
                genusId = genus2.name,
                Characters =
                {
                new NumericCharacter("ForeArmLength", "Forearm length", 50, 60),
                new OptionsCharacter<GularPoachPresentOption>("GularPoachPresent", "Gular poach", GularPoachPresentOption.IsPresent)
                }
            };
            var species4 = new Species("Species4")
            {
                genusId = genus2.name,
                Characters =
                {
                new NumericCharacter("ForeArmLength", "Forearm length", 40, 50),
                new OptionsCharacter<GularPoachPresentOption>("GularPoachPresent", "Gular poach", GularPoachPresentOption.IsNotPresent)
                }
            };

            classifications.AddRange(new List<Classification> { genus1, genus2, species1, species2, species3, species4});
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            var json = JsonConvert.SerializeObject(classifications, settings);
            classifications.Clear();
            classifications = JsonConvert.DeserializeObject< List<Classification>>(json, settings);

            var trunk = MainDecisionTree();

            List<CharacterBase> characteristics = trunk.GetCharacteristics();


        }
    }

    internal class NumericCharacter : DecisionTree.CharacterBase
    {
        private string name;
        private string prompt;
        private int minValue;
        private int maxValue;

        public NumericCharacter(string name, string prompt, int minValue, int maxValue)
        {
            this.name = name;
            this.prompt = prompt;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }

    internal class OptionsCharacter<T> : DecisionTree.CharacterBase
    {
        private string name;
        private string prompt;
        private DecisionTree.GularPoachPresentOption value;

        public OptionsCharacter(string name, string prompt, DecisionTree.GularPoachPresentOption value)
        {
            this.name = name;
            this.prompt = prompt;
            this.value = value;
        }
    }

    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !(left == right);
        }

        public static bool operator <(Enumeration left, Enumeration right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Enumeration left, Enumeration right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(Enumeration left, Enumeration right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Enumeration left, Enumeration right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }

        // Other utility methods ...
    }
}
