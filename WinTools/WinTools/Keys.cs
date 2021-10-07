using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WinTools.Keys;

namespace WinTools
{
    public class Keys
    {
        public class KeyManager
        {
            internal KeyManagerFamily FromClassification(string v)
            {
                throw new NotImplementedException();
            }

            internal List<ClassificationBase> Filter(List<CharacterBase> characters)
            {
                throw new NotImplementedException();
            }
        }

        public class KeyManagerFamily
        {

            internal KeyManagerGenus ToGenus(string v)
            {
                throw new NotImplementedException();
            }

            internal List<CharacterBase> GetCharacters()
            {
                throw new NotImplementedException();
            }
        }

        public class KeyManagerGenus
        {
            internal KeyManagerGenus FromGenus(string v)
            {
                throw new NotImplementedException();
            }
            internal KeyManagerSpecies ToSpecies(string v, string v1)
            {
                throw new NotImplementedException();
            }

            internal KeyManagerGenus UseCharacter(CharacterBase key)
            {
                throw new NotImplementedException();
            }
        }

        public class KeyManagerSpecies
        {
            internal KeyManagerSpecies UseCharacter(CharacterBase key)
            {
                throw new NotImplementedException();
            }

            internal KeyManagerGenus ToGenus(string v)
            {
                throw new NotImplementedException();
            }

            internal KeyManagerFamily FromFamily(string v)
            {
                throw new NotImplementedException();
            }

            internal KeyManagerSpecies ToSpecies(string v)
            {
                throw new NotImplementedException();
            }
        }
        public class CharacterBase
        {
            public string Prompt { get; set; }
        }

        public class NumericCharacter : CharacterBase
        {
            private string v1;
            private int v2;
            private int v3;

            public NumericCharacter(string v1, int v2, int v3)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.v3 = v3;
            }

            public float Min { get; set; }
            public float Max { get; set; }
            public float SelectedValue { get; set; }

        }

        public class OptionsCharacter: CharacterBase
        {
            private string v1;
            private List<string> list;
            private string v2;

            public OptionsCharacter(string v1, List<string> list, string v2)
            {
                this.v1 = v1;
                this.list = list;
                this.v2 = v2;
            }

            public List<string> Options { get; set; }
            public string SelectedValue { get; set; }
        }


        public class ClassificationBase {
            public string Id { get; set; }
        }

        public class Genus : ClassificationBase { }
        public class Species : ClassificationBase { }


    }
}
