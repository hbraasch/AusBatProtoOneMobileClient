using System;

namespace AusBatProtoOneMobileClient.Models
{
    #region *//DbaseVersion
    public class DbaseVersion
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }

        public DbaseVersion() { }

        public DbaseVersion(int major, int minor, int patch)
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
        }

        /// <summary>
        /// Expects string of format "<major>.<minor>.<patch>"
        /// </summary>
        /// <param name="version"></param>
        public DbaseVersion(string version)
        {
            var values = version.Split('.');
            if (values.Length != 3)
            {
                throw new ApplicationException("Version  string is incorrectly formatted");
            }
            try
            {
                this.Major = int.Parse(values[0]);
                this.Minor = int.Parse(values[1]);
                this.Patch = int.Parse(values[2]); ;
            }
            catch (Exception)
            {
                throw new ApplicationException("Version  string is incorrectly formatted");
            }
        }

        public override bool Equals(object obj)
        {
            if (Major == ((DbaseVersion)obj).Major && Minor == ((DbaseVersion)obj).Minor && Patch == ((DbaseVersion)obj).Patch) return true;
            return false;
        }

        public static bool operator ==(DbaseVersion a, DbaseVersion b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(DbaseVersion a, DbaseVersion b)
        {
            return !(a.Equals(b));
        }

        public static bool operator >(DbaseVersion a, DbaseVersion b)
        {
            if (a.Major > b.Major) return true;
            if (a.Minor > b.Minor) return true;
            if (a.Patch > b.Patch) return true;
            return false;
        }

        public static bool operator <(DbaseVersion a, DbaseVersion b)
        {
            if (b.Major > a.Major) return true;
            if (b.Minor > a.Minor) return true;
            if (b.Patch > a.Patch) return true;
            return false;
        }

        public static bool operator <=(DbaseVersion a, DbaseVersion b)
        {
            if (a == b) return true;
            return a < b;
        }

        public static bool operator >=(DbaseVersion a, DbaseVersion b)
        {
            if (a == b) return true;
            return a > b;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static DbaseVersion MinValue => new DbaseVersion(0, 0, 0);

    }
    #endregion
}
