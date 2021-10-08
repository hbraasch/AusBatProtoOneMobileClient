using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AusBatProtoOneMobileClient.Models
{
    class Characters
    {
        #region *// Utility classes
        public class CharacterBase { }
        internal class NumericCharacter : CharacterBase
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
        internal class OptionsCharacter<T> : CharacterBase
        {
            private string name;
            private string prompt;
            private T value;

            public OptionsCharacter(string name, string prompt, T value)
            {
                this.name = name;
                this.prompt = prompt;
                this.value = value;
            }
        }
        public abstract class Enumeration : IComparable
        {
            public string Name { get; private set; }
            public List<string> VerboseDescriptions { get; set; } = new List<string>();
            public List<string> ImageSources { get; set; } = new List<string>();
            public int Id { get; private set; }

            protected Enumeration(int id, string name) => (Id, Name) = (id, name);
            public Enumeration(int id, string name, List<string> verboseDescriptions) => 
                (Id, Name, VerboseDescriptions) = (id, name, verboseDescriptions);
            public Enumeration(int id, string name, List<string> verboseDescriptions, List<string> imageSources) =>
                (Id, Name, VerboseDescriptions, ImageSources) = (id, name, verboseDescriptions, imageSources);


            public override string ToString() => Name;

            public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
                typeof(T).GetFields(BindingFlags.Public |
                                    BindingFlags.Static |
                                    BindingFlags.DeclaredOnly)
                         .Select(f => f.GetValue(null))
                         .Cast<T>();

            public override bool Equals(object obj)
            {
                if (!(obj is Enumeration))
                {
                    return false;
                }
                var otherValue = (Enumeration)obj;
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
        #endregion

        #region *// Application characters
        public class TailPresentCharacteristic : Enumeration
        {
            public static TailPresentCharacteristic Undefined = new TailPresentCharacteristic(1, nameof(Undefined));
            public static TailPresentCharacteristic IsAbsent = new TailPresentCharacteristic(2, nameof(IsAbsent));
            public static TailPresentCharacteristic IsPresent = new TailPresentCharacteristic(3, nameof(IsPresent));

            public TailPresentCharacteristic(int id, string name)
                : base(id, name) { }
        }
        #endregion
    }
}
